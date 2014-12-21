using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Canal.Unity.Modes
{
    public class GameMode : Behavior
    {
        public List<Object> featureList = new List<Object>();

        private GameMode parent;
        private ModeConfiguration configuration;
        private Dictionary<string, Object> loadedFeatures = new Dictionary<string, Object>();
        private Transform featureContainer;

        public void Initialize(ModeConfiguration configuration, GameMode parent = null)
        {
            this.parent = parent;
            this.configuration = configuration;
            this.featureContainer = new GameObject().transform;

            name = configuration.name;
            featureContainer.name = "Features";
            featureContainer.parent = transform;
        }

        public void LoadRequiredFeatures()
        {
            if (parent != null)
            {
                parent.LoadRequiredFeatures();
            }

            foreach (string feature in configuration.RequiredFeatures)
            {
                GetFeature<Transform>(feature);
            }
        }

        public T GetFeature<T>(string id)
        {
            Object loadedFeature = null;
            if (loadedFeatures.TryGetValue(id, out loadedFeature))
            {
                return GetFeature<T>(loadedFeature);
            }

            loadedFeature = LoadFeature(id);
            if (loadedFeature != null)
            {
                return GetFeature<T>(loadedFeature);
            }

            if (parent != null)
            {
                return parent.GetFeature<T>(id);
            }

            return default(T);
        }

        private Object LoadFeature(string id)
        {
            // Load the feature if it's supported
            ModeConfiguration.FeatureMap featureInfo = configuration.SupportedFeatures.FirstOrDefault(x => x.Id == id);
            if (featureInfo.Id != id)
            {
                return null;
            }

            Object asset = Resources.Load(featureInfo.AssetPath);
            if (asset == null)
            {
                return null;
            }
            if (asset is GameObject)
            {
                GameObject instance = GameObject.Instantiate(asset) as GameObject;
                instance.transform.parent = featureContainer;
                asset = instance;
            }
            loadedFeatures[featureInfo.Id] = asset;
            featureList.Add(asset);
            return asset;
        }

        private T GetFeature<T>(Object featureSource)
        {
            if (featureSource == null)
            {
                return default(T);
            }
            else if (featureSource is GameObject)
            {
                return (T)(object)((GameObject)featureSource).GetComponent(typeof(T));
            }
            else if (featureSource is T)
            {
                return (T)(object)featureSource;
            }
            return default(T);
        }
    }
}

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Canal.Unity.Modes
{
    public class GameMode : Behavior
    {
        public List<Object> featureList = new List<Object>();

        public GameMode Parent { get; private set; }
        public ModeConfiguration Configuration { get; private set; }

        private Dictionary<string, Object> loadedFeatures = new Dictionary<string, Object>();
        private Transform featureContainer;

        public void Initialize(ModeConfiguration configuration, GameMode parent = null)
        {
            this.Parent = parent;
            this.Configuration = configuration;
            this.featureContainer = new GameObject().transform;

            name = string.Format("Mode: {0}", configuration.name);
            featureContainer.name = "Features";
            featureContainer.parent = transform;
        }

        public void LoadRequiredFeatures()
        {
            if (Parent != null)
            {
                Parent.LoadRequiredFeatures();
            }

            foreach (string feature in Configuration.RequiredFeatures)
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

            if (Parent != null)
            {
                return Parent.GetFeature<T>(id);
            }

            return default(T);
        }

        private Object LoadFeature(string id)
        {
            // Load the feature if it's supported
            ModeConfiguration.FeatureMap featureInfo = Configuration.SupportedFeatures.FirstOrDefault(x => x.Id == id);
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

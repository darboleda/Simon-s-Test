using UnityEngine;
using System.Collections.Generic;

namespace Canal.Unity
{
    public class FeatureRegistry : Behavior
    {
        public const string Tag = "Features";

        [SerializeField]
        private FeatureList list;

        [SerializeField]
        private string[] requiredFeatures; 

        private Dictionary<string, Object> loadedManagers = new Dictionary<string, Object>();

        public void Awake()
        {
            if (list == null)
            {
                list = Resources.Load("Config/DefaultFeatures") as FeatureList;
            }

            if (requiredFeatures != null)
            {
                for (int i = 0, count = requiredFeatures.Length; i < count; ++i)
                {
                    LoadFeature<Component>(requiredFeatures[i]);
                }
            }
        }

        public T LoadFeature<T>(string managerId)
        {
            Object featureContainer = null;
            if (loadedManagers.ContainsKey(managerId))
            {
                featureContainer = loadedManagers[managerId];
            }
            else
            {
                FeatureList.FeatureMapping mapping = list[managerId];
                Object featureAsset = Resources.Load(mapping.FeaturePath);

                if (featureAsset is GameObject)
                {
                    GameObject featurePrefab = featureAsset as GameObject;
                    GameObject featureInstance = GameObject.Instantiate(featurePrefab) as GameObject;
                    featureInstance.name = featurePrefab.name;
                    featureInstance.ParentAndResetTransform(transform);
                    featureContainer = featureInstance;
                    loadedManagers[mapping.FeatureId] = featureContainer;
                }
                else if (featureAsset is ScriptableObject)
                {
                    featureContainer = featureAsset;
                    loadedManagers[mapping.FeatureId] = featureContainer;
                }
            }

            if (featureContainer is GameObject)
            {
                return (T)(object)(((GameObject)featureContainer).GetComponent(typeof(T)));
            }
            if (featureContainer is T)
            {
                return (T)(object)featureContainer;
            }
            return default(T);
        }
    }
}

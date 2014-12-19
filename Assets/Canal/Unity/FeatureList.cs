using System.Collections.Generic;

using UnityEngine;

namespace Canal.Unity
{
    public class FeatureList : ScriptableObject
    {
        [System.Serializable]
        public struct FeatureMapping
        {
            public string FeatureId;
            public string FeaturePath;
        }

        [SerializeField]
        private FeatureMapping[] features;
        public string[] RequiredFeatures;

        public FeatureMapping this[string managerId]
        {
            get
            {
                for (int i = 0, count = features.Length; i < count; ++i)
                {
                    if (features[i].FeatureId == managerId)
                    {
                        return features[i];
                    }
                }
                return new FeatureMapping();
            }
        }
    }
}

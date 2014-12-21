using System;

using UnityEngine;

namespace Canal.Unity.Modes
{
    public class ModeConfiguration : ScriptableObject
    {
        [Serializable]
        public struct FeatureMap
        {
            public string Id;
            public string AssetPath;
        }

        public string ParentAssetPath;
        public FeatureMap[] SupportedFeatures;
        public string[] RequiredFeatures;
    }
}

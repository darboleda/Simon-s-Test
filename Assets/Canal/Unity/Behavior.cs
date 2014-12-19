using UnityEngine;

namespace Canal.Unity
{
    public class Behavior : MonoBehaviour
    {
        private FeatureRegistry internalFeatureRegistry;
        private FeatureRegistry featureRegistry
        {
            get
            {
                if (internalFeatureRegistry == null)
                {
                    GameObject registryContainer = GameObject.FindWithTag(FeatureRegistry.Tag);
                    if (registryContainer == null)
                    {
                        internalFeatureRegistry = new GameObject().AddComponent<FeatureRegistry>();
                        internalFeatureRegistry.gameObject.tag = FeatureRegistry.Tag;
                    }
                    else
                    {
                        internalFeatureRegistry = registryContainer.GetComponent<FeatureRegistry>();
                    }
                }
                return internalFeatureRegistry;
            }
        }

        public T RequestFeature<T>(string id)
        {
            return featureRegistry.LoadFeature<T>(id);
        }
    }
}

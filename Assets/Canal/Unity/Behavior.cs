using UnityEngine;

using Canal.Unity.Modes;
namespace Canal.Unity
{
    public class Behavior : MonoBehaviour
    {
        private GameModeManager internalGmm;
        private GameModeManager Gmm
        {
            get
            {
                if (internalGmm == null)
                {
                    internalGmm = GameObject.FindObjectOfType<GameModeManager>();
                }
                return internalGmm;
            }
        }

        public T RequestFeature<T>(string id)
        {
            return Gmm.GetFeature<T>(id);
        }
    }
}

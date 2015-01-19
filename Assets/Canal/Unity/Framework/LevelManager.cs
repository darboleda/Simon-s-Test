using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Canal.Unity.Framework
{
    public class LevelManager : Behavior
    {
        [SerializeField]
        private string LevelRootTag;

        [SerializeField]
        private LevelMapping levels;
        public LevelMapping Levels
        {
            get { return levels; }
        }

        private Level currentLevel;

        public void LoadLevelAdditive<T>(int index, System.Action<T> callback = null) where T : Level
        {
            LoadLevelAdditive<T>(levels.GetKeyAtIndex(index), callback);
        }

        public void LoadLevelAdditive<T>(string key, System.Action<T> callback = null) where T : Level
        {
            string levelName = levels[key];

            if (currentLevel != null)
            {
                currentLevel.OnUnloaded();
                currentLevel.gameObject.SetActive(false);
            }
            Application.LoadLevelAdditive(levelName);
            this.StartCoroutine(WaitForLevelLoad<T>(callback));
        }

        private IEnumerator WaitForLevelLoad<T>(System.Action<T> callback = null) where T : Level
        {
            yield return null;

            if (currentLevel != null)
            {
                Object.Destroy(currentLevel.gameObject);
            }
            yield return null;
            currentLevel = GameObject.FindGameObjectsWithTag(this.LevelRootTag).SelectMany(x => x.GetComponents<Level>()).FirstOrDefault();
            if (currentLevel != null)
            {
                currentLevel.OnLoaded();
            }
            if (callback != null) callback(currentLevel as T);
        }

        public IEnumerator UnloadUnusedAssets()
        {
            yield return Resources.UnloadUnusedAssets();
        }
    }
}

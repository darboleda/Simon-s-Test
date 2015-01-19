using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Canal.Unity.Framework
{
    public class LevelManager : Behavior
    {
        public class LevelLoader
        {
            private LevelManager manager;
            private string levelToLoad;
            private Level level;

            public T GetLoadedLevel<T>() where T : Level
            {
                return level as T;
            }

            public LevelLoader(LevelManager manager, string levelToLoad)
            {
                this.manager = manager;
                this.levelToLoad = levelToLoad;
            }

            public Coroutine Load()
            {
                return manager.StartCoroutine(BeginLoad());
            }

            private IEnumerator BeginLoad()
            {
                if (manager.currentLevel != null)
                {
                    manager.currentLevel.OnUnloaded();
                    manager.currentLevel.gameObject.SetActive(false);
                }
                Application.LoadLevelAdditive(levelToLoad);

                yield return null;

                if (manager.currentLevel != null)
                {
                    Object.Destroy(manager.currentLevel.gameObject);
                }
                yield return null;
                manager.currentLevel = GameObject.FindGameObjectsWithTag(manager.LevelRootTag).SelectMany(x => x.GetComponents<Level>()).FirstOrDefault();
                if (manager.currentLevel != null)
                {
                    manager.currentLevel.OnLoaded();
                }
                level = manager.currentLevel;
            }
        }

        [SerializeField]
        private string LevelRootTag;

        [SerializeField]
        private LevelMapping levels;
        public LevelMapping Levels
        {
            get { return levels; }
        }

        private Level currentLevel;

        public LevelLoader GetAdditiveLevelLoader(int index)
        {
            return GetAdditiveLevelLoader(levels.GetKeyAtIndex(index));
        }

        public LevelLoader GetAdditiveLevelLoader(string key)
        {
            string levelName = levels[key];
            return new LevelLoader(this, levelName);
        }

        public IEnumerator UnloadUnusedAssets()
        {
            yield return Resources.UnloadUnusedAssets();
        }
    }
}

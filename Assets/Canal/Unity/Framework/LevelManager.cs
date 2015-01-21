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
            private string levelKey;
            private Level level;

            public T GetLoadedLevel<T>() where T : Level
            {
                return level as T;
            }

            public LevelLoader(LevelManager manager, string levelKey)
            {
                this.manager = manager;
                this.levelKey = levelKey;
            }

            public Coroutine Load()
            {
                return manager.StartCoroutine(BeginLoad());
            }

            private IEnumerator BeginLoad()
            {
                string levelToLoad = manager.levels[levelKey];
                if (manager.currentLevel != null)
                {
                    manager.currentLevel.OnUnloaded();
                }

                if (manager.currentLevel != null)
                {
                    Object.Destroy(manager.currentLevel.gameObject);
                }
                yield return null;
                yield return manager.UnloadUnusedAssets();
                Application.LoadLevelAdditive(levelToLoad);
                yield return null;
                manager.currentLevel = GameObject.FindGameObjectsWithTag(manager.LevelRootTag).SelectMany(x => x.GetComponents<Level>()).FirstOrDefault();
                if (manager.currentLevel != null)
                {
                    manager.currentLevel.OnLoaded();
                }
                level = manager.currentLevel;
                manager.LastLoadedLevelKey = levelKey;
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

        public string LastLoadedLevelKey { get; private set; }

        public LevelLoader GetAdditiveLevelLoader(int index)
        {
            return GetAdditiveLevelLoader(levels.GetKeyAtIndex(index));
        }

        public LevelLoader GetAdditiveLevelLoader(string key)
        {
            return new LevelLoader(this, key);
        }

        public AsyncOperation UnloadUnusedAssets()
        {
            return Resources.UnloadUnusedAssets();
        }
    }
}

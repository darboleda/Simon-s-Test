using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Canal.Unity.Framework
{
    public class LevelMapping : ScriptableObject
    {
        [Serializable]
        public class Map
        {
            public string Key;
            public string Value;
        }

        #if UNITY_EDITOR
        public List<Map> GetDefaultLevels(System.Func<UnityEditor.EditorBuildSettingsScene, bool> sceneTest)
        {
            if (sceneTest == null) sceneTest = x => true;
            List<Map> mappedLevels = new List<Map>();
            foreach (string scene in UnityEditor.EditorBuildSettings.scenes.Where(sceneTest).Select(x => Path.GetFileNameWithoutExtension(x.path)))
            {
                mappedLevels.Add(new Map() { Key = scene, Value = scene });
            }
            return mappedLevels;
        }

        public void SetDefaultLevels(System.Func<UnityEditor.EditorBuildSettingsScene, bool> sceneTest)
        {
            levelMappings = GetDefaultLevels(sceneTest);
        }
        #endif

        [SerializeField]
        public List<Map> levelMappings = new List<Map>();

        public string this[string key]
        {
            get { return levelMappings.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault(); }
        }

        public int LevelCount
        {
            get { return levelMappings.Count; }
        }

        public string GetKeyAtIndex(int index)
        {
            return levelMappings.Select(x => x.Key).ElementAtOrDefault(index);
        }
    }
}

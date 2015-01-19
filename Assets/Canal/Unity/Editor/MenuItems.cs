using System.IO;

using UnityEditor;
using UnityEngine;

using Canal.Unity;
using Canal.Unity.Editor;

public class MenuItems
{
    [MenuItem("Assets/Create/Level Mapping")]
    private static void CreateLevelMapping()
    {
        CanalEditor.CreateAsset<Canal.Unity.Framework.LevelMapping>("New Level Mapping").SetDefaultLevels(x => x.enabled);
    }
}

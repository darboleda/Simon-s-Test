using System.IO;

using UnityEditor;
using UnityEngine;

using Canal.Unity;
using Canal.Unity.Modes;

using Canal.Unity.Editor;

public class MenuItems
{
    [MenuItem("Assets/Create/Room List")]
    private static void CreateRoomList()
    {
        AssetCreator.CreateScriptableObject<RoomList>();
    }

    [MenuItem("Assets/Create/Game Mode Configuration")]
    private static void CreateGameModeConfiguration()
    {
        AssetCreator.CreateScriptableObject<ModeConfiguration>();
    }

    [MenuItem("Assets/Create/Level Mapping")]
    private static void CreateLevelMapping()
    {
        CanalEditor.CreateAsset<Canal.Unity.Framework.LevelMapping>("New Level Mapping").SetDefaultLevels(x => x.enabled);
    }
}

using System.IO;

using UnityEditor;
using UnityEngine;

using Canal.Unity;
using Canal.Unity.Modes;

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
}

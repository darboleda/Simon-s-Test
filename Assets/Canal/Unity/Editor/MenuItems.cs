using System.IO;

using UnityEditor;
using UnityEngine;

using Canal.Unity;

public class MenuItems
{
    [MenuItem("Assets/Create/Room List")]
    private static void CreateRoomList()
    {
        AssetCreator.CreateScriptableObject<RoomList>();
    }

    [MenuItem("Assets/Create/Feature List")]
    private static void CreateFeatureList()
    {
        AssetCreator.CreateScriptableObject<FeatureList>();
    }
}

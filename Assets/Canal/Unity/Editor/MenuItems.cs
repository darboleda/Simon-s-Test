using System.IO;

using UnityEditor;
using UnityEngine;

using Canal.Unity;

public class MenuItems
{
    [MenuItem("Assets/Create/Room List")]
    private static void CreateRoomList()
    {
        RoomList roomList = ScriptableObject.CreateInstance<RoomList>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        string assetPathName = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/New {1}.asset", path, typeof(RoomList).ToString()));
        AssetDatabase.CreateAsset(roomList, assetPathName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeObject = roomList;
    }

}

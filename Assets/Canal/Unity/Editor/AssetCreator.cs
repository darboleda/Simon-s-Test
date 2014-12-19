using System.IO;

using UnityEditor;
using UnityEngine;

using Canal.Unity;

public static class AssetCreator
{
	public static void CreateScriptableObject<T>()  where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        string assetPathName = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/New {1}.asset", path, typeof(T).ToString()));
        AssetDatabase.CreateAsset(asset, assetPathName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeObject = asset;
    }

}

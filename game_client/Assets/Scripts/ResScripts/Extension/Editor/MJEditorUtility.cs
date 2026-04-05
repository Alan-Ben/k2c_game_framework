using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


static class MJEditorUtility
{
    const string g_UserAssetDirectory = "Assets/Resources/EditorConfig/";

    /// <summary>
    /// Fetch all assets of type `T`
    /// </summary>
    /// <typeparam name="T">the type to retrieve, must inherit from ScriptableObject and implement IHasDefault and ICustomSettings</typeparam>
    /// <returns>All the assets of that type on the project</returns>
    internal static List<T> GetAll<T>() where T: ScriptableObject
    {
        var tGuids = AssetDatabase.FindAssets("t:" + typeof(T));
        var Ts = new List<T>();
        foreach (var guid in tGuids)
            Ts.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
        return Ts;
    }
    
    /// <summary>
    /// Fetch a default asset from path relative to the product folder. If not found, a new one is created.
    /// </summary>
    /// <typeparam name="T">>the type to retrieve, must inherit from ScriptableObject and implement IHasDefault and ICustomSettings</typeparam>
    /// <returns>The first asset of the type T found inside the project (order set by AssetDataBase.FindAssets function. If none returns a new object T with default values set by IHasDefault.</returns>
    internal static T GetFirstOrNew<T>() where T : ScriptableObject
    {
        string[] all = AssetDatabase.FindAssets("t:" + typeof(T));

        T asset = all.Length > 0 ? AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(all.First())) : null;

        if(asset == null)
        {
            return CreateNew<T>();
        }

        return asset;
    }
    
    /// <summary>
    /// Create a new asset at path relative to the product folder.
    /// </summary>
    /// <typeparam name="T">>the type to create, must inherit from ScriptableObject and implement IHasDefault and ICustomSettings</typeparam>
    /// <returns>A new object T with default values set by IHasDefault.</returns>
    internal static T CreateNew<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        EditorUtility.SetDirty(asset);

        string folder = g_UserAssetDirectory;

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);


        string assetPath = AssetDatabase.GenerateUniqueAssetPath(folder + typeof(T).Name + "-Default.asset");

        if (string.IsNullOrEmpty(assetPath))
        {
            return null;
        }
        AssetDatabase.CreateAsset(asset, assetPath);
        return asset;
    }
}
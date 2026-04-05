using UnityEditor;
using UnityEngine;

/// <summary>
/// 用于Editor下重新序列化资源
/// https://answers.unity.com/questions/1625900/why-am-i-getting-assetimporter-is-referencing-an-a.html?_ga=2.37416169.858563537.1585015286-1010379941.1527752000
/// </summary>
[CreateAssetMenu()]
public class PrefabReferenceFixer : ScriptableObject
{
    [MenuItem("Assets/Force Reserialize")]
    private static void ForceReserialize()
    {
        GameObject[] selection = Selection.gameObjects;
        string[] objectPaths = new string[selection.Length];

        for (int i = 0; i < selection.Length; ++i)
        {
            objectPaths[i] = AssetDatabase.GetAssetPath(selection[i]);
        }

        AssetDatabase.ForceReserializeAssets(objectPaths);
    }
    
    [MenuItem("Assets/Force Reserialize All")]
    public static void forceReserializeAll()
    {
        if (EditorUtility.DisplayDialog("确认全部重新序列化", "确认全部重新序列化？耗时大搞1-2小时", "确定"))
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}
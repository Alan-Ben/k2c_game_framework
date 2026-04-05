
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightEnvironmentMono))]
public class LightEnvironmentMonoInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mono = target as LightEnvironmentMono;
        if (GUILayout.Button("保存当前环境颜色值"))
        {
            mono.setSphericalHarmonicsL2();
            EditorUtility.SetDirty(mono);
        }
        if (GUILayout.Button("保存当前场景的LightProbe到此灯光配置处"))
        {
            string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromOriginalSource(mono));
            path = path.Replace(".prefab", "_probe.asset");
            AssetDatabase.CreateAsset(Instantiate(LightmapSettings.lightProbes), path);
            
            mono.lightProbes = AssetDatabase.LoadAssetAtPath<LightProbes>(path);
            EditorUtility.SetDirty(mono);
        }
    }
}

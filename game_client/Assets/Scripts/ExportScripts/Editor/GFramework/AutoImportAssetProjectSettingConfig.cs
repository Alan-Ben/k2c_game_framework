using System.Collections;
using System.Collections.Generic;
using AssetPreprocessor.Scripts.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class AutoImportAssetProjectSettingConfig : ScriptableObject
{
    [Header("项目资源配置")] public AutoImportAssetConfig autoImportAssetConfig = null;
    
    //在ProjectSettings中配置资源导入自动设置，将本配置文件放置ProjectSettings文件夹下
    public const string k_AutoImportAssetProjectSettingConfigPath = "ProjectSettings/AutoImportAssetProjectSetting.asset";
    private static AutoImportAssetProjectSettingConfig _autoImportAssetProjectSettingConfig;

    //获取ProjectSettings文件夹下的本配置文件
    internal static AutoImportAssetProjectSettingConfig GetOrCreateSettings()
    {
        var files=InternalEditorUtility.LoadSerializedFileAndForget(k_AutoImportAssetProjectSettingConfigPath);
        if (files.Length != 0)
        {
            _autoImportAssetProjectSettingConfig = (AutoImportAssetProjectSettingConfig) files[0];
        }
        if (!_autoImportAssetProjectSettingConfig)
        {
            Debug.Log("创建导入资产自动设置配置文件");
            _autoImportAssetProjectSettingConfig = ScriptableObject.CreateInstance<AutoImportAssetProjectSettingConfig>();
            EditorUtility.SetDirty(_autoImportAssetProjectSettingConfig);
            Save();
        }
        return _autoImportAssetProjectSettingConfig;
    }

    public static void Save()
    {
        InternalEditorUtility.SaveToSerializedFileAndForget(new []{_autoImportAssetProjectSettingConfig},k_AutoImportAssetProjectSettingConfigPath,true);
    }
    
    internal static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }
    
}
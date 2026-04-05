using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


static class AutoImportAssetProjectSetting
{
    //ProjectSettings窗口编写参考资料：
    //https://yomunchan.moe/Post/309
    //https://docs.unity.cn/cn/2019.4/ScriptReference/SettingsProvider.html
    
    private static SerializedObject mAutoImportAssetProjectSettingConfig;
    private static SerializedProperty mAutoImportAssetConfig;
    private static SerializedProperty mTexturePreprocessorConfigs;
    private static SerializedProperty mAudioPreprocessorConfigs;
    private static SerializedProperty mModelPreprocessorConfigs;
    
    //在ProjectSettings窗口下显示和配置 资源导入自动设置
    [SettingsProvider]
    public static SettingsProvider ProjectSetting_ImportAsset()
    {
        var provider = new SettingsProvider("Project/ImportAssetAutoSetting", SettingsScope.Project)
        {
            label = "资源导入自动设置",
            //初始化
            activateHandler = (searchContext, rootElement) =>
            {
                mAutoImportAssetProjectSettingConfig = AutoImportAssetProjectSettingConfig.GetSerializedSettings();
                mAutoImportAssetConfig = mAutoImportAssetProjectSettingConfig.FindProperty("autoImportAssetConfig");
            },
            //OnGUI 刷新
            guiHandler = (searchContext) =>
            {
                EditorGUILayout.PropertyField(mAutoImportAssetConfig, new GUIContent("项目资源导入配置"));

                mAutoImportAssetProjectSettingConfig.ApplyModifiedProperties();
            },
            //关闭后
            deactivateHandler = () =>
            {
                AutoImportAssetProjectSettingConfig.Save();
            }
        };
        return provider;
    }
    
}

using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace AssetPreprocessor.Scripts.Editor
{
    public class AudioPreprocessor : AssetPostprocessor
    {
        /// <summary>
        /// https://docs.unity3d.com/ScriptReference/AssetPostprocessor.OnPreprocessAudio.html
        /// 
        /// IMPORTANT:
        /// Use OnPostprocessAudio() hook instead of OnPreprocessAudio() since OnPostprocessAudio gives a reference to
        /// the AudioClip, which is needed for AudioClip.length.
        /// </summary>
        private void OnPostprocessAudio(AudioClip audioClip)
        {
            var audioImporter = assetImporter as AudioImporter;
            if (audioImporter == null) return;
            var assetPath = audioImporter.assetPath;
            var assetName = AssetPreprocessorUtils.GetAssetNameFromPath(audioImporter.assetPath);
            SerializedObject serializedObject = new SerializedObject(audioImporter);
            SerializedProperty normalize = serializedObject.FindProperty("m_Normalize");
            
            if (audioClip == null)
            {
                Debug.LogError($"{typeof(AudioClip)} is null. Path: {assetPath}");
                return;
            }
            
            //获取音频导入设置配置文件，通过ProjectSetting中的配置获取
            var configs = AutoImportAssetProjectSettingConfig.GetOrCreateSettings().autoImportAssetConfig?.AudioPreprocessorConfigs;
            //var configs = AssetPreprocessorUtils.GetScriptableObjectsOfType<AudioPreprocessorConfig>();
            if (configs==null || configs.Count == 0)
            {
                return;
            }
            configs = configs
                .Where(conf => conf.ShouldUseConfigForAssetImporter(assetImporter))
                .ToList();
            configs.Sort((config1, config2) => config1.ConfigSortOrder.CompareTo(config2.ConfigSortOrder));
            AudioPreprocessorConfig config = null;
            for (var i = 0; i < configs.Count; i++)
            {
                var configToTest = configs[i];
                if (audioClip.length > configToTest.MaxClipLengthInSeconds) continue;
                //运用第一个匹配配置
                config = configToTest;
                break;
            }
            if (config == null) return; //如果没有匹配配置则跳过预处理
            
            
            //如果有关闭Normalize选择则跳过预处理
            //判断是否强制处理
            string isForce="";
            if (normalize.boolValue==false)
            {
                if (!config.ForcePreprocess)
                {
                    //二次改动Quality时，进行检查提示
                    float qua = audioImporter.defaultSampleSettings.quality * 100f;
                    if (qua - config.Quality * 100 > 1)
                        Debug.LogError($"{assetPath}, 新设置Quality:{qua}与配置规范设置:{config.Quality * 100}不一致，请确认",
                            AssetDatabase.LoadMainAssetAtPath(assetPath));

                    isForce = "配置文件开启了强制规范：";
                    return;
                }
            }
            
            
            //工具配置文件对音频进行导入预处理
            Debug.Log(isForce+$"音频导入自动设置--音频文件名: {assetName},导入设置配置文件: {config.name}", config);
            audioImporter.forceToMono = config.ForceToMono;
            audioImporter.loadInBackground = config.LoadInBackground;
            audioImporter.ambisonic = config.Ambisonic;
            audioImporter.preloadAudioData = config.PreloadAudioData;
            //设置平台格式
            var sampleSettings = audioImporter.defaultSampleSettings;
            sampleSettings.loadType = config.AudioClipLoadType;
            sampleSettings.compressionFormat = config.AudioCompressionFormat;
            sampleSettings.quality = config.Quality;
            sampleSettings.sampleRateSetting = config.AudioSampleRateSetting;
            if (config.AudioSampleRateSetting == AudioSampleRateSetting.OverrideSampleRate)
            {
                sampleSettings.sampleRateOverride = config.AudioSampleRateOverride;
            }
            //将平台格式设置应用到所有平台
            normalize.boolValue = false;
            serializedObject.ApplyModifiedProperties();
            audioImporter.defaultSampleSettings = sampleSettings;
            EditorUtility.SetDirty(audioClip);
            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();
        }
    }
}



using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 音频文件自动处理
/// </summary>
public class SoundAutoProc : AssetPostprocessor
{
    /// <summary>
    /// 音频类文件导入自动检测与处理
    /// </summary>
    /// <param name="_importedAssets"></param>
    /// <param name="_deletedAssets"></param>
    /// <param name="_movedAssets"></param>
    /// <param name="_movedFromAssetPaths"></param>
    static void OnPostprocessAllAssets(string[] _importedAssets, string[] _deletedAssets, string[] _movedAssets, string[] _movedFromAssetPaths)
    {
        List<string> soundPathes = new List<string>();
        for(int i = 0; i<_importedAssets.Length; ++i)
        {
            string path = _importedAssets[i];
            if (path.Contains("sounds"))
            {
                soundPathes.Add(path);
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                if (clip)
                {
                    AudioImporter auImporter = (AudioImporter)AssetImporter.GetAtPath(path);
                    var setting = auImporter.defaultSampleSettings;
                    setting.quality = 0.4f;
                    setting.sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate;
                    setting.sampleRateOverride = 32000;
                    auImporter.defaultSampleSettings = setting;
                }
            }
        }
        if(soundPathes.Count > 0)
        {
            RefreshAudioPrefab(soundPathes.ToArray());
        }
    }
    
    #region 音频预制体生成相关

    public static void RefreshAudioPrefab(string[] _pathes)
    {
        AudioMixer mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>("Assets/Resources/AudioMix.mixer");
        AudioMixerGroup effectGroup = mixer.FindMatchingGroups("Master/Effect")[0];
        AudioMixerGroup bgmGroup = mixer.FindMatchingGroups("Master/Music")[0];
    
        foreach (string path in _pathes)
        {
            string audioFilePath = path;
            int firstIndex = audioFilePath.LastIndexOf('/');
            int lastIndex = audioFilePath.IndexOf('.');
            string audioName = audioFilePath.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
            string dirPath = audioFilePath.Substring(0, firstIndex + 1);
            string prefabFilePath = dirPath + audioName + ".prefab";
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioFilePath);
    
            if (clip)
            {
                GameObject clipPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabFilePath);
                if (clipPrefab)
                {
                    Debug.LogWarning($"{prefabFilePath}已存在，不重复生成");
                    continue;
                }
    
                clipPrefab = new GameObject(audioName);
                AudioSource audioSource = clipPrefab.AddComponent<AudioSource>();
                audioSource.clip = clip;
                // TODO
                GameObject.DestroyImmediate(clipPrefab);
            }
        }
    }

    [MenuItem("Assets/音频相关/生成预制体", priority = 0)]
    static void TryGenerateAudioPrefab()
    {
        string[] guids = Selection.assetGUIDs;
        string[] pathes = new string[guids.Length];
    
        for(int i = 0; i < pathes.Length; ++i)
        {
            pathes[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
        }
        RefreshAudioPrefab(pathes);
    }
    #endregion
}
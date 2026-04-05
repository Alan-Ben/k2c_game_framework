using System.Collections;
using System.Collections.Generic;
using AssetPreprocessor.Scripts.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AssetPreprocessor.Scripts.Editor
{
    [CreateAssetMenu(menuName="ScriptableObject/AssetPreprocessor/AutoImportAssetConfig")]
    public class AutoImportAssetConfig : ScriptableObject
    {
        [Header("贴图导入配置")]
        public List<TexturePreprocessorConfig> TexturePreprocessorConfigs = new List<TexturePreprocessorConfig>();

        [Header("音频导入配置")]
        public List<AudioPreprocessorConfig> AudioPreprocessorConfigs = new List<AudioPreprocessorConfig>();

        [Header("模型导入配置")]
        public List<ModelPreprocessorConfig> ModelPreprocessorConfigs = new List<ModelPreprocessorConfig>();
    }
}

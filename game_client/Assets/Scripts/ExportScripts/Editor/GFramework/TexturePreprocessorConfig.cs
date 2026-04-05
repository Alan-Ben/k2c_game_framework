using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using TextureCompressionQuality = UnityEditor.TextureCompressionQuality;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace AssetPreprocessor.Scripts.Editor
{
    [Serializable]
    public class CompressionSetting
    {
        [Tooltip("Format used if the texture does NOT have an alpha channel.")]
        public TextureImporterFormat RGBFormat = TextureImporterFormat.Automatic;
        [Tooltip("Format used if the texture has an alpha channel.")]
        public TextureImporterFormat RGBAFormat = TextureImporterFormat.Automatic;
        public TextureCompressionQuality TextureCompressionQuality = TextureCompressionQuality.Normal;
    }
    [CreateAssetMenu(menuName="ScriptableObject/AssetPreprocessor/TexturePreprocessorConfig")]
    public class TexturePreprocessorConfig : BasePreprocessorConfig
    {
        [HideInInspector]
        [Header("Platforms")]
            public List<string> PlatformsRegexList = new List<string>
            {
                "Android",
                "iOS",
                "Standalone",
            };
            
        [Header("Texture Settings")]
            [Tooltip("勾选此选项则覆盖TextureImporterType")]
            public TextureImporterType TextureImporterType=TextureImporterType.Default;
            public TextureImporterShape TextureImporterShape = TextureImporterShape.Texture2D;
            public bool sRGB;
            public bool alphaIsTransparency = true;
            [Tooltip("勾选此选项则会按照下面的值设置MaxTextureSize")]
            public bool forceSetMaxTextureSize = false;
            [Tooltip("Absolute max size allowed for textures. The texture's native size will be used if it is smaller than the max texture size.")]
            public int MaxTextureSize = 2048;
            public bool EnableReadWrite;
            public bool GenerateMipMaps = true;
            public bool EnableMipMapStreaming;
            [Tooltip("自定以MipMap的功能，需要指定搜索MipMap图的路径,存放mip图的路径为 ：”文件名“+.mip。mip贴图名字为：”文件名“+.mip+”mip等级“")]
            public bool CustomMipMap = false;
            public string CustomMipMapSearchPath = "";
            [Tooltip("HDR图转为RGBM格式，需要指定搜索原始HDR图的路径，匹配规则为：”文件名“+ _hdr")]
            public bool HDRCubeMapToRGBM = false;
            public string HDRCubeMapSearchPath = "";

            [Tooltip("Decides how to round when a texture's size is NOT a power of two.")]
            public TextureImporterNPOTScale NPOTScale = TextureImporterNPOTScale.ToNearest;

        [Header("Filtering Settings")]
            public TextureWrapMode TextureWrapMode=TextureWrapMode.Clamp;
            public FilterMode FilterMode = FilterMode.Bilinear;
            public int AnisoLevel = 1;

        [Header("Compression Settings")]
            public CompressionSetting winCompressSetting;
            public CompressionSetting androidCompressSetting;
            public CompressionSetting iosCompressSetting;

       
    }
}


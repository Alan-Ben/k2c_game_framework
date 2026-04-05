using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AssetPreprocessor.Scripts.Editor
{
    class TexturePreprocessor : AssetPostprocessor
    {
        private bool m_isReadable; // 当前处理的图片设置是否为可读, 用于后续改图可以维持图片的读写和之前一致
        private TexturePreprocessorConfig m_Config; // 当前图片匹配的配置
        /// <summary>
        /// https://docs.unity3d.com/ScriptReference/AssetPostprocessor.OnPreprocessTexture.html
        /// </summary>
        private void OnPreprocessTexture()
        {
            var textureImporter = (TextureImporter) assetImporter;
            var assetPath = textureImporter.assetPath;
            
            
            //获取贴图导入设置配置文件
            var configs = AutoImportAssetProjectSettingConfig.GetOrCreateSettings().autoImportAssetConfig?.TexturePreprocessorConfigs;
            //var configs = AssetPreprocessorUtils.GetScriptableObjectsOfType<TexturePreprocessorConfig>();//指定路径可以大幅度提高速度
            if (configs==null || configs.Count == 0)
            {
                return;
            }
            configs = configs
                .Where(conf => conf.ShouldUseConfigForAssetImporter(assetImporter))
                .ToList();
            configs.Sort((config1, config2) => config1.ConfigSortOrder.CompareTo(config2.ConfigSortOrder)); // 根据配置优先级排序
            TexturePreprocessorConfig config = null;
            for (var i = 0; i < configs.Count;)
            {
                var configToTest = configs[i];
                //运用第一个匹配配置
                config = configToTest;
                break;
            }
            if(config == null) return;  //如果没有匹配配置则跳过预处理
            
            m_Config = config;
            m_isReadable = textureImporter.isReadable;
            
            //如果有平台覆盖则跳过预处理
            //判断是否强制处理
            int maxSize;
            TextureImporterFormat format;
            string isForce = "";
            if (textureImporter.GetPlatformTextureSettings("iPhone", out maxSize, out format)||textureImporter.GetPlatformTextureSettings("Android", out maxSize, out format)
                ||textureImporter.GetPlatformTextureSettings("Standalone", out maxSize, out format))
            {
                if (!config.ForcePreprocess)
                {
                    return; 
                }
                isForce = "配置文件开启了强制规范：";
            }

            //根据配置文件对贴图进行导入预处理
            var textureName = AssetPreprocessorUtils.GetAssetNameFromPath(textureImporter.assetPath);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            var hasAlpha = textureImporter.DoesSourceTextureHaveAlpha();
            var originalTextureSize = GetOriginalTextureSize(textureImporter);
            var nativeSize = Mathf.NextPowerOfTwo(Mathf.Max(originalTextureSize.width, originalTextureSize.height));
            var textureSize = config.MaxTextureSize;
            //Log预处理信息，单击获取贴图导入配置文件
            UnityEngine.Debug.Log(isForce+ $"贴图导入自动设置--贴图文件名: {textureName}，导入设置配置文件:{config.name}", config);
            // Handle when native size for texture is too small. Happens for baked light maps and reflection maps.
            // Need to reimport asset once to get correct native texture size.
            if(nativeSize <= 4)
            {
                AssetDatabase.Refresh();
                return;
            }
            textureImporter.textureType = config.TextureImporterType;   //设置贴图类型
            textureImporter.textureShape = config.TextureImporterShape;  
            textureImporter.sRGBTexture = config.sRGB;  //设置贴图sRGB(Color Texture)
            textureImporter.alphaIsTransparency = hasAlpha && config.alphaIsTransparency; //设置Alpha是否透明
            textureImporter.isReadable = config.EnableReadWrite;    //设置贴图 Read/Write Enabled
            m_isReadable = textureImporter.isReadable;
            textureImporter.mipmapEnabled = config.GenerateMipMaps; //设置贴图MipMap
            textureImporter.streamingMipmaps = config.EnableMipMapStreaming;
            if(textureImporter.textureType !=TextureImporterType.Sprite)  //非2次幂的贴图缩放模式
                textureImporter.npotScale = config.NPOTScale;
            if (config.forceSetMaxTextureSize)      //设置贴图最大尺寸
                textureImporter.maxTextureSize = textureSize;
            //设置Filter Mode
            textureImporter.wrapMode = config.TextureWrapMode;
            textureImporter.anisoLevel = config.AnisoLevel;
            textureImporter.filterMode = config.FilterMode;
            //设置贴图平台格式
            SetTextureImporterPlatformSetting(config, textureImporter, texture, textureName, textureSize, hasAlpha, originalTextureSize);
        }

        private static bool _isCanBeMultipleOf4(int _num)
        {
            return _num % 4 == 0;
        }

        private static void SetTextureImporterPlatformSetting(
            TexturePreprocessorConfig config,
            TextureImporter textureImporter,
            Texture texture,
            string textureName,
            int textureSize,
            bool hasAlpha,
            Size originalTextureSize
        )
        {
            int winMaxTextureSize = 0;
            int iosMaxTextureSize = 0;
            int androidMaxTextureSize = 0;
            
            config.PlatformsRegexList.ForEach(platformRegexString =>
            {
                var oldSet = textureImporter.GetPlatformTextureSettings(platformRegexString);
                
                int curPtTexSize = textureSize;
                if(!config.forceSetMaxTextureSize && oldSet != null && oldSet.overridden)
                {
                    curPtTexSize = oldSet.maxTextureSize;
                }
              
                CompressionSetting formatSet = config.winCompressSetting;
                
                if(platformRegexString == "Standalone")
                {
                    formatSet = config.winCompressSetting;
                }
                else if(platformRegexString == "Android")
                {
                    formatSet = config.androidCompressSetting;
                }
                else if(platformRegexString == "iOS")
                {
                    formatSet = config.iosCompressSetting;
                }
                
                //精灵格式的图片导入时需要进行检查图片大小操作，而非精灵格式的图片大小会自动缩放为2的幂次大小
                if(textureImporter.textureType == TextureImporterType.Sprite)
                {
                    int max = originalTextureSize.width;
                    int min = originalTextureSize.height;
                    if(max < min)
                    {
                        max = originalTextureSize.height;
                        min = originalTextureSize.width;
                    }
                    
                    if(curPtTexSize >= max)
                    {
                        if (!(_isCanBeMultipleOf4(max) && _isCanBeMultipleOf4(min)))
                        {
                            //UnityEngine.Debug.LogError(
                                //$"{textureName}[{platformRegexString}]图片的分辨率({originalTextureSize.width}, {originalTextureSize.height})不是4的倍数，不能被压缩，需要调整(打包图集或修改图片大小)！",
                                //AssetDatabase.LoadMainAssetAtPath(textureImporter.assetPath));
                        }
                    }
                    else
                    {
                        int nmax = curPtTexSize;
                        int nmin = curPtTexSize / max * min;
                        if (!(_isCanBeMultipleOf4(nmax) && _isCanBeMultipleOf4(nmin)))
                        {
                            UnityEngine.Debug.LogError($"{textureName}[{platformRegexString}]图片的分辨率({originalTextureSize.width}, {originalTextureSize.height})不是4的倍数，不能被压缩，需要调整(打包图集或修改图片大小)！", 
                                AssetDatabase.LoadMainAssetAtPath(textureImporter.assetPath));
                        }
                    }
                }
                
                var format = hasAlpha ? formatSet.RGBAFormat : formatSet.RGBFormat;
                if(oldSet == null || oldSet.format != format || oldSet.compressionQuality != (int) formatSet.TextureCompressionQuality || oldSet.maxTextureSize != curPtTexSize)
                {
                    textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings
                    {
                        overridden = true,
                        name = platformRegexString,
                        maxTextureSize = curPtTexSize,
                        format = format,
                        compressionQuality = (int) formatSet.TextureCompressionQuality,
                        allowsAlphaSplitting = false
                    });
                }
            });
            
            
        }

        /// <summary>
        /// Hacky way to get the native texture size via the TextureImporter.
        /// https://forum.unity.com/threads/getting-original-size-of-texture-asset-in-pixels.165295/
        /// </summary>
        private static Size GetOriginalTextureSize(TextureImporter importer)
        {
            if (_getImageSizeDelegate == null) {
                var method = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                _getImageSizeDelegate = Delegate.CreateDelegate(typeof(GetImageSize), null, method) as GetImageSize;
            }
 
            var size = new Size();
            
            _getImageSizeDelegate(importer, ref size.width, ref size.height);
 
            return size;
        }
		
        private delegate void GetImageSize(TextureImporter importer, ref int width, ref int height);
        private static GetImageSize _getImageSizeDelegate;

        private struct Size {
            public int width;
            public int height;
        }
        
        private string GetCustomMipMapDir(string searchFolder, string path)
        {
            var filenameWithoutExtention = Path.GetFileNameWithoutExtension(path);
            var directoryName = Path.GetDirectoryName(path);
            return Path.Combine(searchFolder, filenameWithoutExtention + ".mip");
        }
        
        private string GetCustomMipmapFilenamePattern(string searchFolder,string path)
        {
            var filenameWithoutExtention = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            return Path.Combine(GetCustomMipMapDir(searchFolder, path), filenameWithoutExtention + ".mip{0}" + extension);
        }
        
        private string GetHDRSourcePath(string searchFolder, string path)
        {
            var filenameWithoutExtention = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            var directoryName = Path.GetDirectoryName(path);
            return Path.Combine(searchFolder, filenameWithoutExtention + "_hdr" + extension);
        }
        
        /// <summary>
        ///  将hdr颜色转为RGBM
        /// </summary>
        /// <param name="hdr"></param>
        /// <param name="MaxValue"></param>
        /// <returns></returns>
        private static Color RGBMEncode(Color hdr, float MaxValue)
        {
            Color c = new Color();
            c.r = Mathf.Sqrt(hdr.r);
            c.g = Mathf.Sqrt(hdr.g);
            c.b = Mathf.Sqrt(hdr.b);
            c *= 1.0f / MaxValue;

            Color rgbm;
            rgbm.a = Mathf.Clamp01(c.maxColorComponent);
            rgbm.a = Mathf.Ceil(rgbm.a * 255.0f) / 255.0f;
            rgbm.r = c.r / rgbm.a;
            rgbm.g = c.g / rgbm.a;
            rgbm.b = c.b / rgbm.a;
            return rgbm;
        }

        
        private void OnPostprocessCubemap(Cubemap texture)
        {
            if (m_Config != null && m_Config.CustomMipMap)
            {
                var dir = GetCustomMipMapDir(m_Config.CustomMipMapSearchPath, assetPath);
                if (Directory.Exists(dir))
                {
                    string pattern = GetCustomMipmapFilenamePattern(m_Config.CustomMipMapSearchPath, assetPath);
                    for (int m = 0; m < texture.mipmapCount; m++)
                    {

                        string path = string.Format(pattern, m);
                        var mipmapTexture = AssetDatabase.LoadAssetAtPath<Cubemap>(path);

                        if (mipmapTexture != null)
                        {
                            if (mipmapTexture.isReadable)
                            {
                                for (int i = 0; i < 6; i++)
                                {
                                    Color[] c = mipmapTexture.GetPixels((CubemapFace) i, 0);
                                    texture.SetPixels(c, (CubemapFace) i, m);
                                }

                            }
                            else
                                UnityEngine.Debug.LogError($"图片未开启读写，请开启后，重新导入: {path},");
                        }
                    }

                    texture.Apply(false, !m_isReadable);
                    UnityEngine.Debug.Log($"自定义MipMap成功{texture.name}：{assetPath}", texture);
                }
            }

            // HDR图转为RGBM的功能
            if (m_Config != null && m_Config.HDRCubeMapToRGBM)
            {
                string path = GetHDRSourcePath(m_Config.HDRCubeMapSearchPath, assetPath);
                var sourceTex = AssetDatabase.LoadAssetAtPath<Cubemap>(path);
                if (sourceTex != null)
                {
                    for (int k = 0; k < texture.mipmapCount; k++)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            var colors = sourceTex.GetPixels((CubemapFace)i, k);
                            for (int j = 0; j < colors.Length; j++)
                            {
                                colors[j] = RGBMEncode(colors[j], 16);
                            }
                            texture.SetPixels(colors, (CubemapFace)i, k);
                        }
                    }
              
                    texture.Apply(false, !m_isReadable);
                }
            }
        }
        
        void OnPostprocessTexture(Texture2D texture)
        {
            if (m_Config != null && m_Config.CustomMipMap)
            {
                var dir = GetCustomMipMapDir(m_Config.CustomMipMapSearchPath, assetPath);
                if (Directory.Exists(dir))
                {
                    string pattern = GetCustomMipmapFilenamePattern(m_Config.CustomMipMapSearchPath, assetPath);
                    for (int m = 0; m < texture.mipmapCount; m++)
                    {
                
                        string path = string.Format(pattern, m);
                        var mipmapTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    
                        if(mipmapTexture != null)
                        {
                            if (mipmapTexture.isReadable)
                            {
                                Color[] c = mipmapTexture.GetPixels(0);
                                texture.SetPixels(c, m);
                            }
                            else
                                UnityEngine.Debug.LogError($"图片未开启读写，请开启后，重新导入: {path},");
                        }
                    }
                    texture.Apply(false, !m_isReadable);
                    UnityEngine.Debug.Log($"自定义MipMap成功{texture.name}：{assetPath}", texture);
                }
            }
        }
    }
}

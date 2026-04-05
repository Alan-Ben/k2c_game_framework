using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using GOE;
using UnityEditor.U2D;
using UnityEngine.U2D;
using UnityEngine.UI;
using System.Linq;
using AssetPreprocessor.Scripts.Editor;
using Object = UnityEngine.Object;

public class TextureCheck
{
    static string pcPlat = "Standalone";
    static string iosPlat = "iPhone";
    static string androidPlat = "Android";
    static string webPlat = "Web";
    static MethodInfo method_GetWidthAndHeight = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
    
    
    public static void GetTextureOriginalSize(TextureImporter ti, out int width, out int height)
    {
        if (ti == null)
        {
            width = 0;
            height = 0;
            return;
        }
  
        object[] args = new object[2] { 0, 0 };
        method_GetWidthAndHeight.Invoke(ti, args);
  
        width = (int)args[0];
        height = (int)args[1];
    }
    
    [MenuItem("NPAssets/资源规范/检查IOS平台格式改动",false,100)]
    [MenuItem("Assets/Texture/检查IOS平台格式改动",false,100)]
     public static void CheckIOSPlat()
     { 
        Object[] selectionObjects = Selection.objects;
        foreach (var objects in selectionObjects)
        {
            string path = AssetDatabase.GetAssetPath(objects);
            //如果选择对象是文件夹
            if (Directory.Exists(path))
            {
                //获取文件夹路径string[]
                var folders = new List<string>();
                folders.Add(path);
                string[] filePath = folders.ToArray();

                //获取文件夹下的贴图和图集
                foreach (var guid in AssetDatabase.FindAssets("t:Texture", filePath))
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                    if (textureImporter == null)
                    {
                        //Debug.Log("此贴图的贴图导入文件不可获取" , AssetImporter.GetAtPath(assetPath));
                        continue;
                    }
                    CheckIOSByTextureImporter(textureImporter);
                }
                continue;
            }
            //如果选择对象是贴图或图集
            if (objects is Texture)
            {
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                CheckIOSByTextureImporter(textureImporter);
                continue;
            }
        }
        Debug.Log("贴图IOS平台格式改动检查完毕");
     }

     private static void CheckIOSByTextureImporter(TextureImporter textureImporter)
     {
         TextureImporterPlatformSettings iosTextureImporterPlatformSettings =
             textureImporter.GetPlatformTextureSettings("iPhone");

         //获取配置的平台格式
         //获取贴图导入设置配置文件
         var configs = AutoImportAssetProjectSettingConfig.GetOrCreateSettings().autoImportAssetConfig?.TexturePreprocessorConfigs;
         if (configs==null || configs.Count == 0)
         {
             return;
         }
         configs = configs
             .Where(conf => conf.ShouldUseConfigForAssetImporter(textureImporter))
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
         if (config == null)
         {
             //Debug.Log("此贴图不存在匹配的自动设置格式", textureImporter);
             return;
         } 
                    
         //判断配置文件是否和当前设置相同
         if (config.iosCompressSetting.RGBFormat != iosTextureImporterPlatformSettings.format &&
             config.iosCompressSetting.RGBAFormat != iosTextureImporterPlatformSettings.format)
         {
             Debug.Log("此贴图IOS平台格式有改动" + "\n现设置："+ iosTextureImporterPlatformSettings.format + "\nRGBFormat设置：" + config.iosCompressSetting.RGBFormat + "\nRGBAFormat设置：" + config.iosCompressSetting.RGBAFormat, textureImporter);
         }
     }
     
     [MenuItem("NPAssets/资源规范/自动设置安卓平台格式为IOS格式", false, 101)]
     [MenuItem("Assets/Texture/自动设置安卓平台格式为IOS格式",false,101)]
     public static void AutoAndroidPlatByIOS()
     {
         Object[] selectionObjects = Selection.objects;
         AssetDatabase.StartAssetEditing();
         foreach (var objects in selectionObjects)
         {
             string path = AssetDatabase.GetAssetPath(objects);
             //如果选择对象是文件夹
             if (Directory.Exists(path))
             {
                 //获取文件夹路径string[]
                 var folders = new List<string>();
                 folders.Add(path);
                 string[] filePath = folders.ToArray();

                 //获取文件夹下的贴图和图集
                 foreach (var guid in AssetDatabase.FindAssets("t:Texture", filePath))
                 {
                     var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                     TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                     if (textureImporter == null)
                     {
                         //Debug.Log("此贴图的贴图导入文件不可获取" , AssetImporter.GetAtPath(assetPath));
                         continue;
                     }
                     AutoAndroidByIOSTextureImporter(textureImporter);
                     AssetImporter.GetAtPath(assetPath).SaveAndReimport();
                 }
                 continue;
             }
             //如果选择对象是贴图或图集
             if (objects is Texture)
             {
                 TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                 AutoAndroidByIOSTextureImporter(textureImporter);
                 AssetImporter.GetAtPath(path).SaveAndReimport();
                 continue;
             }
         }
         AssetDatabase.StopAssetEditing();
         AssetDatabase.Refresh();
         Debug.Log("贴图IOS平台格式改动检查完毕");
     }
     private static void AutoAndroidByIOSTextureImporter(TextureImporter textureImporter)
     {
         TextureImporterPlatformSettings iosTextureImporterPlatformSettings =
             textureImporter.GetPlatformTextureSettings("iPhone");
        
         TextureImporterPlatformSettings androidTextureImporterPlatformSettings =
             textureImporter.GetPlatformTextureSettings("Android");

         androidTextureImporterPlatformSettings.format = iosTextureImporterPlatformSettings.format;
         textureImporter.SetPlatformTextureSettings(androidTextureImporterPlatformSettings);
     }
     
    [MenuItem("NPAssets/资源规范/文件贴图规范窗口",false,50)]
    [MenuItem("Assets/Texture/文件贴图规范窗口")]
    static void EditorTextureCheck()
    {
        Object[] selectionObjects = Selection.objects;
        List<Object> objects = new List<Object>(selectionObjects);
        TextureCheckEditorWindow.selectionObject = objects;
        FileCheckIsStandard(objects);
    }
    public static void FileCheckIsStandard(List<Object> _objects)
    {
        AtlasCollect.instance.init();
        List<TexPathAndCount> texPathAndCountList = new List<TexPathAndCount>();
        List<string> errorList = new List<string>();
        List<TextureErrorType> textureErrorTypesList = new List<TextureErrorType>();
        List<int> pathIndexList = new List<int>();
        AssetDatabase.StartAssetEditing();
        //Object[] selectionObject = _objects;
        //Debug.Log(selectionObject[0].name);
        foreach (var objects in _objects)
        {
            string path = AssetDatabase.GetAssetPath(objects);
            //如果选择对象是文件夹
            if (Directory.Exists(path))
            {
                //获取文件夹路径string[]
                var folders = new List<string>();
                folders.Add(path);
                string[] filePath = folders.ToArray();

                //获取文件夹下的贴图和图集
                foreach (var guid in AssetDatabase.FindAssets("t:Texture t:SpriteAtlas", filePath))
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    int index = errorList.Count;
                    //通过路径检查贴图是否规范
                    PathCheckTex(errorList, textureErrorTypesList,assetPath);
                    if (index != errorList.Count)
                    {
                        TexPathAndCount texPathAndCount;
                        texPathAndCount.path = assetPath;
                        texPathAndCount.count = errorList.Count - index;
                        for (int i = 0; i < texPathAndCount.count; i++)
                        {
                            pathIndexList.Add(texPathAndCountList.Count);
                        }
                        texPathAndCountList.Add(texPathAndCount);
                    }
                }

                continue;
            }

            //如果选择对象是贴图或图集
            if (objects is Texture || objects is SpriteAtlas)
            {
                //通过路径检查贴图是否规范
                int index = errorList.Count;
                PathCheckTex(errorList,textureErrorTypesList, path);
                if (index != errorList.Count)
                {
                    TexPathAndCount texPathAndCount;
                    texPathAndCount.path = path;
                    texPathAndCount.count = errorList.Count - index;
                    for (int i = 0; i < texPathAndCount.count; i++)
                    {
                        pathIndexList.Add(texPathAndCountList.Count);
                    }
                    texPathAndCountList.Add(texPathAndCount);
                }
                continue;
            }

        }
        TextureCheckEditorWindow.pathIndexList = pathIndexList;
        TextureCheckEditorWindow.texPathAndCountList = texPathAndCountList;
        TextureCheckEditorWindow.errorList = errorList;
        TextureCheckEditorWindow.textureErrorTypesList = textureErrorTypesList;
        TextureCheckEditorWindow.ShowWindow();

        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }
    
    /// <summary>
    /// 检查贴图资源是否符合贴图规范
    /// </summary>
    /// <param name="imageList">UI图片列表</param>
    /// <param name="errorList">UI错误信息列表</param>
    /// <param name="assetPath">资源路径</param>
    private static void PathCheckTex(
        List<string> errorList,List<TextureErrorType> textureErrorTypesList,string assetPath)
    {
        SpriteAtlas spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
        if (null != spriteAtlas)
        {
            //UnityEngine.Debug.LogError(spriteAtlas.ToString()+"是精灵图集");
            _CheckSpriteAtlas(spriteAtlas,errorList,textureErrorTypesList,assetPath);
            return;
        }

        TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (textureImporter == null)
        {
            //UnityEngine.Debug.LogError(assetPath + " 获取图片格式信息错误!");
            return;
        }
        
        
        Texture texObj = AssetDatabase.LoadAssetAtPath<Texture>(textureImporter.assetPath);
        if (null == texObj)
        {
            //UnityEngine.Debug.LogError(textureImporter.assetPath + " 文件不是图片!");
            return;
        }
        
        //TextureCheck_TexMipmap检查
        if (TextureCheckEditorWindow.texMipmap)
        {
            //判断需要检查Mipmap开启还是关闭
            if (TextureCheckEditorWindow.texMipmapOpen)
            {
                //检查mipmap错误开启
                if (textureImporter.mipmapEnabled)
                {
                    errorList.Add(",该图片开启了mipmap");
                    textureErrorTypesList.Add(TextureErrorType.TextureGenerateMipmapOpen);
                }
            }
            else
            {
                //检查mipmap错误关闭
                if (!textureImporter.mipmapEnabled)
                {
                    errorList.Add(",该图片关闭了mipmap");
                    textureErrorTypesList.Add(TextureErrorType.TextureGenerateMipmapClose);
                }
            }
            
        }
        
        //TextureCheck_TexReadEnabled检查
        if (TextureCheckEditorWindow.texReadEnabled)
        {
            //查看是否开启了readwriteable
            if (textureImporter.isReadable)
            {
                errorList.Add(",该图片开启了readwriteable");
                textureErrorTypesList.Add(TextureErrorType.TextureReadable);
            }
        }
        
        //TextureCheck_TexSize检查
        if (TextureCheckEditorWindow.texSize)
        {
            //检查图片类型是否是Sprite，且贴图边长是否符合4的倍数
            //判断边长是否符合4的倍数
            if (texObj.width%4!=0||texObj.height%4!=0)
            {
                //判断贴图是否是精灵类型
                if (textureImporter.textureType == TextureImporterType.Sprite)
                {
                    //判断精灵是否已经打包图集
                    if (AtlasCollect.instance.getAtlas(assetPath)=="None")
                    {
                        errorList.Add("该精灵尚未打包,且图片边长不为4的倍数，请手动修改");
                        textureErrorTypesList.Add(TextureErrorType.SpriteSizeAndNoIsAtlas);
                    }
                }
                //不是精灵类型
                else
                {
                    errorList.Add(",该图片的分辨率不是4的倍数，且未进行POT处理，请手动修改");
                    textureErrorTypesList.Add(TextureErrorType.TextureSize);
                }
            }
        }
        
        /* 不对alphaIsTransparency进行检查
        //TextureCheck_TexAlpha检查
        if (TextureCheckEditorWindow.texAlpha)
        {
            
            //查看贴图是否透明
            if(textureImporter.DoesSourceTextureHaveAlpha())
            {
                if(!textureImporter.alphaIsTransparency)
                {
                    errorList.Add(",该图片具有Alpha通道，AlphaIsTransparency应该为ture");
                    imageList.Add(assetPath);
                    textureErrorTypesList.Add(TextureErrorType.TextureAlpha);
                }
            }
            else
            {
                if (textureImporter.alphaIsTransparency)
                {
                    errorList.Add(",该图片不具有Alpha通道，AlphaIsTransparency应该为false");
                    imageList.Add(assetPath);
                    textureErrorTypesList.Add(TextureErrorType.TextureAlpha);
                }
            }
        }
        */
        
        //TextureCheck_TexPlatform检查
        if (TextureCheckEditorWindow.texPlatform)
        {
            //检查平台设置最大尺寸是否相同
            if (!_checkPlatformSize(textureImporter))
            {
                errorList.Add(",该图片不同平台设置的最大尺寸不同，自动修改将统一改为2048");
                textureErrorTypesList.Add(TextureErrorType.MaxSizeDifferent);
                
            }
        
            //各平台进行检测
            _checkIos(textureImporter, texObj, errorList,textureErrorTypesList);
            _checkAndroid(textureImporter, texObj,errorList,textureErrorTypesList);
            _checkPc(textureImporter, texObj,errorList,textureErrorTypesList);
            //_checkWeb(textureImporter, texObj, imageList, errorList);
        }
    }
    
    /// <summary>
    /// 检查图集是否符合贴图规范
    /// </summary>
    protected static void _CheckSpriteAtlas(SpriteAtlas spriteAtlas,
        List<string> errorList,List<TextureErrorType> textureErrorTypesList,string assetPath)
    {
        //检查图集是否关闭mipmap和readEnabled
        SpriteAtlasTextureSettings spriteAtlasTextureSettings = spriteAtlas.GetTextureSettings();
        //TextureCheck_AtlasMipmap检查
        if (TextureCheckEditorWindow.atlasMipmap)
        {
            //判断需要检查Mipmap开启还是关闭
            if (TextureCheckEditorWindow.atlasMipmapOpen)
            {
                if(spriteAtlasTextureSettings.generateMipMaps == true)
                {
                    errorList.Add(",该精灵图集开启了mipmap");
                    textureErrorTypesList.Add(TextureErrorType.SpriteAtlasGenerateMipMapOpen);
                }
            }
            else
            {
                if(spriteAtlasTextureSettings.generateMipMaps == false)
                {
                    errorList.Add(",该精灵图集开关闭了mipmap");
                    textureErrorTypesList.Add(TextureErrorType.SpriteAtlasGenerateMipMapClose);
                }
            }
            
        }

        //TextureCheck_AtlasReadEnabled检查
        if (TextureCheckEditorWindow.atlasReadEnabled)
        {
            if (spriteAtlasTextureSettings.readable == true)
            {
                errorList.Add(",该精灵图集开启Read/WriteEnabled");
                textureErrorTypesList.Add(TextureErrorType.SpriteAtlasReadable);
            }
        }

        TextureImporterPlatformSettings textureImporterPlatformSettings;
        //TextureCheck_AtlasPlatform检查
        if (TextureCheckEditorWindow.atlasPlatform)
        {
            //检查IOS平台的设置
            textureImporterPlatformSettings =spriteAtlas.GetPlatformSettings(iosPlat);
            if ((textureImporterPlatformSettings.format != TextureImporterFormat.ASTC_6x6 && textureImporterPlatformSettings.format != TextureImporterFormat.ASTC_4x4)|| textureImporterPlatformSettings.overridden==false )
            {
                //如果定义了AtlasRGBA是正确的且平台设置确实符合RGBA16或RGBA32
                if (TextureCheckEditorWindow.atlasRGBA && (textureImporterPlatformSettings.format == TextureImporterFormat.RGBA16 ||
                    textureImporterPlatformSettings.format == TextureImporterFormat.RGBA32))
                {
                    //不输出错误
                }
                else
                {
                    errorList.Add("是精灵图集，IOS平台格式设置错误!自动改正将其设置为TextureImporterFormat.ASTC_6x6");
                    textureErrorTypesList.Add(TextureErrorType.IOS);
                }
            }
                
        
            //检查Android平台的设置
            textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings(androidPlat);
            if (textureImporterPlatformSettings.format != TextureImporterFormat.ETC2_RGBA8 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB16 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB32)
            {
                //如果定义了AtlasRGBA是正确的且平台设置确实符合RGBA
                if (TextureCheckEditorWindow.atlasRGBA && (textureImporterPlatformSettings.format == TextureImporterFormat.RGBA16 || 
                                                                      textureImporterPlatformSettings.format == TextureImporterFormat.RGBA32))
                {
                    //不输出错误
                }
                else
                {
                    errorList.Add("是精灵图集，Android平台格式设置错误!自动改正将其设置为TextureImporterFormat.ETC2_RGBA8");
                    textureErrorTypesList.Add(TextureErrorType.Android);
                }
            }
            
            //检查Pc平台的设置
            textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings(pcPlat);
            if (textureImporterPlatformSettings.format != TextureImporterFormat.DXT5 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB16 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB32)
            {
                //如果定义了AtlasRGBA是正确的且平台设置确实符合RGBA
                if (TextureCheckEditorWindow.atlasRGBA && (textureImporterPlatformSettings.format == TextureImporterFormat.RGBA16 || 
                                                                      textureImporterPlatformSettings.format == TextureImporterFormat.RGBA32))
                {
                    //不输出错误
                }
                else
                {
                    errorList.Add("是精灵图集，pc平台格式设置错误!自动改正将其设置为TextureImporterFormat.DXT5");
                    textureErrorTypesList.Add(TextureErrorType.PC);
                }
            }
        }
        
        //TextureCheck_AtlasSize检查
        if (TextureCheckEditorWindow.atlasSize)
        {
            //判断平台尺寸是否相同
            if (!_checkAtlasPlatformSize(spriteAtlas))
            {
                errorList.Add("是精灵图集，平台设置的尺寸不同或小于精灵尺寸，自动修改将统一改为2048" );
                textureErrorTypesList.Add(TextureErrorType.MaxSizeDifferent);
            }
        }

    }
    

    /**************
     * 检测图片格式
     **/
    #region 图片格式检查和改正
    //检查安卓平台规范
    protected static void _checkAndroid(TextureImporter _texture, Texture _texObj, List<string> errorList,List<TextureErrorType> textureErrorTypesList)
    {
        int maxSize;
        TextureImporterFormat format;
        if (!_texture.GetPlatformTextureSettings(androidPlat, out maxSize, out format))
        {
            errorList.Add("无法获取到Android格式！没有平台覆盖");
            textureErrorTypesList.Add(TextureErrorType.Android);
            return;
        }
        //如果定义了TexRGBA是正确的且平台设置确实符合RGBA16或RGBA32
        if (TextureCheckEditorWindow.texRGBA && (format == TextureImporterFormat.RGBA16 ||format == TextureImporterFormat.RGBA32))
        {
            //不输出错误
            return;
        }
        if (_texObj.name.Substring((_texObj.name.Length - 2)>=0?(_texObj.name.Length - 2):0) == "_a")
        {

            //判断格式
            if (format != TextureImporterFormat.Alpha8)
            {
                errorList.Add("alpha通道图片，Android格式不是alpha8！");
                textureErrorTypesList.Add(TextureErrorType.Android);
                return;
            }
            return;;
        }
        
        if (_texture.textureType == TextureImporterType.NormalMap)
        {
            if (format != TextureImporterFormat.ETC2_RGBA8)
            {
                errorList.Add("Android法线贴图，格式不是ETC2_RGBA8!");
                textureErrorTypesList.Add(TextureErrorType.Android);
                return;
            }
            return;
        }
        //判断平台格式是否符合规范
        if (_texture.DoesSourceTextureHaveAlpha())
        {
            if (format != TextureImporterFormat.ETC2_RGBA8)
            {
                errorList.Add("Android正常有透明通道贴图，格式不是ETC2_RGBA8！");
                textureErrorTypesList.Add(TextureErrorType.Android);
            }
        }
        else
        {
            if (format != TextureImporterFormat.ETC_RGB4)
            {
                errorList.Add("Android正常无透明通道贴图，格式不是ETC_RGB4！");
                textureErrorTypesList.Add(TextureErrorType.Android);
            }
        }
    }
    
    //设置安卓平台规范
    protected static void _AutoAndroid(TextureImporter _texture, Texture _texObj,List<string> errorList)
    {
        int platformMaxTextureSize = 0;
        TextureImporterFormat platformTextureFmt;
        int platformCompressionQuality = 0;
        bool platformAllowsAlphaSplit = false;
        
        //获取设置值
        _texture.GetPlatformTextureSettings(androidPlat, out platformMaxTextureSize,
            out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
        
        //若为_a透明通道贴图，设置为Alpha8
        if (_texObj.name.Substring((_texObj.name.Length - 2)>=0?(_texObj.name.Length - 2):0) == "_a")
        {
            _texture.SetPlatformTextureSettings(androidPlat,platformMaxTextureSize,TextureImporterFormat.Alpha8,platformCompressionQuality,platformAllowsAlphaSplit);
            return;
        }
        
        //若为法线贴图
        if (_texture.textureType == TextureImporterType.NormalMap)
        {
            _texture.SetPlatformTextureSettings(androidPlat,platformMaxTextureSize,TextureImporterFormat.ETC2_RGBA8,platformCompressionQuality,platformAllowsAlphaSplit);
            return;;
        }
        
        //若含有透明通道平台格式设置为ETC2_RGBA8，反之设置为ETC_RGB4
        if (_texture.DoesSourceTextureHaveAlpha())
        {
            _texture.SetPlatformTextureSettings(androidPlat,platformMaxTextureSize,TextureImporterFormat.ETC2_RGBA8,platformCompressionQuality,platformAllowsAlphaSplit);
            return;
        }
        else
        {
            _texture.SetPlatformTextureSettings(androidPlat,platformMaxTextureSize,TextureImporterFormat.ETC_RGB4,platformCompressionQuality,platformAllowsAlphaSplit);
            return;
        }
        
        
    }
    
    //检查Pc平台规范
    protected static void _checkPc(TextureImporter _texture, Texture _texObj, List<string> errorList,List<TextureErrorType> textureErrorTypesList)
    {
        int maxSize;
        TextureImporterFormat format;
        if (!_texture.GetPlatformTextureSettings(pcPlat, out maxSize, out format))
        {
            errorList.Add("无法获取到PC格式！没有平台覆盖");
            textureErrorTypesList.Add(TextureErrorType.PC);
            return;
        }
        //如果定义了TexRGBA是正确的且平台设置确实符合RGBA16或RGBA32
        if (TextureCheckEditorWindow.texRGBA && (format == TextureImporterFormat.RGBA16 ||format == TextureImporterFormat.RGBA32))
        {
            //不输出错误
            return;
        }

        //判断尺寸和大小
        //判断是否纯通道贴图
        if (_texObj.name.Substring((_texObj.name.Length - 2)>=0?(_texObj.name.Length - 2):0) == "_a")
        {
            //判断格式
            if (format != TextureImporterFormat.Alpha8)
            {
                errorList.Add("alpha通道图片，Pc格式不是alpha8！");
                textureErrorTypesList.Add(TextureErrorType.PC);
                return;;
            }
            return;
        }

        if (_texture.textureType == TextureImporterType.NormalMap)
        {
            if (format != TextureImporterFormat.DXT5)
            {
                errorList.Add("PC法线贴图，格式不是DXT5!");
                textureErrorTypesList.Add(TextureErrorType.PC);
                return;
            }
            return;
        }

        //判断透明
        if (_texture.DoesSourceTextureHaveAlpha())
        {
            if (format != TextureImporterFormat.DXT5)
            {
                errorList.Add("PC正常有透明通道贴图，格式不是DXT5!");
                textureErrorTypesList.Add(TextureErrorType.PC);
            }
        }
        else
        {
            if (format != TextureImporterFormat.DXT1)
            {
                errorList.Add("PC正常无透明通道贴图，格式不是DXT1!");
                textureErrorTypesList.Add(TextureErrorType.PC);
            }
        }
        
    }
    
    //设置Pc平台规范
    protected static void _AutoPc(TextureImporter _texture, Texture _texObj,List<string> errorList)
    {
        int platformMaxTextureSize = 0;
        TextureImporterFormat platformTextureFmt;
        int platformCompressionQuality = 0;
        bool platformAllowsAlphaSplit = false;
        
        //获取设置值
        _texture.GetPlatformTextureSettings(pcPlat, out platformMaxTextureSize,
            out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
        
        //若为_a透明通道贴图，设置为Alpha8
        if (_texObj.name.Substring((_texObj.name.Length - 2)>=0?(_texObj.name.Length - 2):0) == "_a")
        {
            _texture.SetPlatformTextureSettings(pcPlat,platformMaxTextureSize,TextureImporterFormat.Alpha8,platformCompressionQuality,platformAllowsAlphaSplit);
            return;
        }
        
        //若为法线贴图
        if (_texture.textureType == TextureImporterType.NormalMap)
        {
            _texture.SetPlatformTextureSettings(pcPlat,platformMaxTextureSize,TextureImporterFormat.DXT5,platformCompressionQuality,platformAllowsAlphaSplit);
            return;;
        }
        
        //若含有透明通道平台格式设置为DXT5，反之设置为DXT5
        if (_texture.DoesSourceTextureHaveAlpha())
        {
            _texture.SetPlatformTextureSettings(pcPlat,platformMaxTextureSize,TextureImporterFormat.DXT5,platformCompressionQuality,platformAllowsAlphaSplit);
            return;
        }
        else
        {
            _texture.SetPlatformTextureSettings(pcPlat,platformMaxTextureSize,TextureImporterFormat.DXT1,platformCompressionQuality,platformAllowsAlphaSplit);
            return;
        }
    }
    protected static void _checkWeb(TextureImporter _texture, Texture _texObj, List<string> imageList, List<string> errorList)
    {
        int maxSize;
        TextureImporterFormat format;
        if (!_texture.GetPlatformTextureSettings(webPlat, out maxSize, out format))
        {
            UnityEngine.Debug.LogError(_texture.assetPath + " 获取图片格式信息错误!");
            return;
        }

        //判断尺寸和大小
        //判断是否纯通道贴图
        if (_texObj.name.Substring(_texObj.name.Length - 2) == "_a")
        {
            //判断尺寸和大小
            if (maxSize > 32 && maxSize >= _texObj.width && maxSize >= _texObj.height)
            {
                UnityEngine.Debug.LogError(_texture.assetPath + " Web平台alpha通道图片分辨率设置过大!");
                return;
            }

            //判断格式
            if (format != TextureImporterFormat.Alpha8)
            {
                imageList.Add(_texture.assetPath);
                errorList.Add("alpha通道图片，Web格式不是alpha8！");
            }
        }
        else
        {
            //判断尺寸和大小
            if (maxSize > 32 && maxSize > _texObj.width && maxSize > _texObj.height)
            {
                UnityEngine.Debug.LogError(_texture.assetPath + " Web平台分辨率设置过大!");
                return;
            }

            //判断其他格式
            if (_texture.DoesSourceTextureHaveAlpha() && _texture.textureType != TextureImporterType.Sprite)
            {
                if (format != TextureImporterFormat.DXT5)
                {
                    imageList.Add(_texture.assetPath);
                    errorList.Add("图片有透明通道，Web格式不是DXT5！");
                }
            }
            else if (_texture.textureType == TextureImporterType.Sprite)
            {
                if (format != TextureImporterFormat.DXT1)
                {
                    imageList.Add(_texture.assetPath);
                    errorList.Add("图片为sprite，Web格式不是DXT1！");
                }
            }
            else
            {
                if (format != TextureImporterFormat.DXT1)
                {
                    imageList.Add(_texture.assetPath);
                    errorList.Add("图片无透明通道，Web格式不是DXT1！");
                }
            }
        }
    }
    
    protected static TextureImporterFormat __IOSFormat = TextureImporterFormat.ASTC_6x6;
    protected static TextureImporterFormat __IOSHeightFormat = TextureImporterFormat.ASTC_4x4;
    protected static TextureImporterFormat __IOSFormat_a = TextureImporterFormat.Alpha8;
    
    //检查IOS平台规范
    protected static void _checkIos(TextureImporter _texture, Texture _texObj, List<string> errorList,List<TextureErrorType> textureErrorTypesList)
    {
        int maxSize;
        TextureImporterFormat format;
        if (!_texture.GetPlatformTextureSettings(iosPlat, out maxSize, out format))
        {
            errorList.Add("无法获取到IOS格式！没有平台覆盖");
            textureErrorTypesList.Add(TextureErrorType.IOS);
            return;
        }
        
        //如果定义了TexRGBA是正确的且平台设置确实符合RGBA16或RGBA32
        if (TextureCheckEditorWindow.texRGBA && (format == TextureImporterFormat.RGBA16 ||format == TextureImporterFormat.RGBA32))
        {
            //不输出错误
            return;
        }
        //判断是否纯通道贴图
        if (_texObj.name.Substring((_texObj.name.Length - 2)>=0?(_texObj.name.Length - 2):0) == "_a")
        {
            //判断格式
            if (format != __IOSFormat_a)
            {
                errorList.Add("ios alpha通道图片，格式不是" + __IOSFormat_a + "！");
                textureErrorTypesList.Add(TextureErrorType.IOS);
                return;
            }
            return;
        }
        
        //如果是法线贴图
        if (_texture.textureType == TextureImporterType.NormalMap)
        {
            if (format != __IOSHeightFormat)
            {
                errorList.Add("Ios 法线贴图,格式不是" +__IOSHeightFormat + "！");
                textureErrorTypesList.Add(TextureErrorType.IOS);
                return;
            }
            return;;
        }

        if (format != __IOSFormat && format != __IOSHeightFormat)
        {
            errorList.Add("Ios的平台格式格式不是" + __IOSFormat + " 或 " + __IOSHeightFormat + "！");
            textureErrorTypesList.Add(TextureErrorType.IOS);
        }
        
    }
    
    //设置IOS平台规范
    protected static void _AutoIos(TextureImporter _texture, Texture _texObj,  List<string> errorList)
    {
        int platformMaxTextureSize = 0;
        TextureImporterFormat platformTextureFmt;
        int platformCompressionQuality = 0;
        bool platformAllowsAlphaSplit = false;
        
        //获取设置值
        _texture.GetPlatformTextureSettings(iosPlat, out platformMaxTextureSize,
            out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
        
        //若为_a透明通道贴图，设置为__IOSFormat_a
        if (_texObj.name.Substring((_texObj.name.Length - 2)>=0?(_texObj.name.Length - 2):0) == "_a")
        {
            _texture.SetPlatformTextureSettings(iosPlat,platformMaxTextureSize,__IOSFormat_a,platformCompressionQuality,platformAllowsAlphaSplit);
            return;
        }
        //若为法线贴图，设置为__IOSHeightFormat
        if (_texture.textureType == TextureImporterType.NormalMap)
        {
            _texture.SetPlatformTextureSettings(iosPlat,platformMaxTextureSize,__IOSHeightFormat,platformCompressionQuality,platformAllowsAlphaSplit);
            return;
        }
        //反之为正常情况，设置为__IOSFormat
        _texture.SetPlatformTextureSettings(iosPlat,platformMaxTextureSize,__IOSFormat,platformCompressionQuality,platformAllowsAlphaSplit);
        return;
        
    }
    #endregion

    
    /**************
     * 已将添加和改正功能集成至检查编辑器界面，将其注释
     **/
    #region
    /*此方法已集成至贴图规范检测图形界面
    [MenuItem("NPAssets/贴图资源规范/添加选择至检查文件贴图规范列表")]
    [MenuItem("Assets/Texture/添加选择至检查文件贴图规范列表")]
    static void AddTextureCheck()
    {
        Object[] selectionObjects = Selection.objects;
        List<Object> objects = new List<Object>(selectionObjects);
        TextureCheckEditorWindow.selectionObject.AddRange(objects);
        FileCheckIsStandard(TextureCheckEditorWindow.selectionObject);
    }
    */
    
    /// <summary>
    /// 此自动设置贴图格式方法已不匹配项目贴图规范
    /// </summary>
    //[MenuItem("NPAssets/资源规范/自动设置贴图格式",false,1)]
    //[MenuItem("Assets/Texture/自动设置贴图格式")]
    static void EditorTextureSet()
    {
        Object[] selectionObjects = Selection.objects;
        List<Object> objects = new List<Object>(selectionObjects);
        TextureCheckEditorWindow.selectionObject = objects;
        FileAutoSetStandard(selectionObjects);
        
    }
    public static void FileAutoSetStandard(Object[] _objects)
    {
        AtlasCollect.instance.init();
        List<TexPathAndCount> texPathAndCountList = new List<TexPathAndCount>();
        List<string> errorList = new List<string>();
        List<TextureErrorType> textureErrorTypesList = new List<TextureErrorType>();
        List<int> pathIndexList = new List<int>();
        AssetDatabase.StartAssetEditing();
        foreach (var objects in _objects)
        {
            string path = AssetDatabase.GetAssetPath(objects);
            //如果选择对象是文件夹
            if (Directory.Exists(path))
            {
                //获取文件夹路径string[]
                var folders = new List<string>();
                folders.Add(path);
                string[] filePath = folders.ToArray();
                //获取文件夹下的贴图和图集
                foreach (var guid in AssetDatabase.FindAssets("t:Texture t:SpriteAtlas", filePath))
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    int index = errorList.Count;
                    //通过路径自动改正贴图至规范
                    PathAutoTex(errorList,textureErrorTypesList,assetPath);
                    AssetImporter.GetAtPath(assetPath).SaveAndReimport();
                    if (index != errorList.Count)
                    {
                        TexPathAndCount texPathAndCount;
                        texPathAndCount.path = assetPath;
                        texPathAndCount.count = errorList.Count - index;
                        for (int i = 0; i < texPathAndCount.count; i++)
                        {
                            pathIndexList.Add(texPathAndCountList.Count);
                        }
                        texPathAndCountList.Add(texPathAndCount);
                    }
                }
                
                continue;
            }
            
            //如果选择对象是贴图或图集
            if (objects is Texture || objects is SpriteAtlas)
            {
                int index = errorList.Count;
                //通过路径检查贴图是否规范
                PathAutoTex(errorList,textureErrorTypesList,path);
                AssetImporter.GetAtPath(path).SaveAndReimport();
                if (index != errorList.Count)
                {
                    TexPathAndCount texPathAndCount;
                    texPathAndCount.path = path;
                    texPathAndCount.count = errorList.Count - index;
                    for (int i = 0; i < texPathAndCount.count; i++)
                    {
                        pathIndexList.Add(texPathAndCountList.Count);
                    }
                    texPathAndCountList.Add(texPathAndCount);
                }
                continue;
            }
            
        }
        if (errorList.Count <= 0)
        {
            Debug.Log("设置文件贴图规范结果：改正完成，所有图片格式已符合标准！");
            TextureCheckEditorWindow.pathIndexList = pathIndexList;
            TextureCheckEditorWindow.texPathAndCountList = texPathAndCountList;
            TextureCheckEditorWindow.errorList = errorList;
            TextureCheckEditorWindow.textureErrorTypesList = textureErrorTypesList;
        }
        else
        {
            TextureCheckEditorWindow.pathIndexList = pathIndexList;
            TextureCheckEditorWindow.texPathAndCountList = texPathAndCountList;
            TextureCheckEditorWindow.errorList = errorList;
            TextureCheckEditorWindow.textureErrorTypesList = textureErrorTypesList;
            TextureCheckEditorWindow.ShowWindow();
        }

        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
        
    }
    
    /// <summary>
    /// 将贴图资源设置至贴图规范
    /// </summary>
    /// <param name="imageList">UI图片列表</param>
    /// <param name="errorList">UI错误信息列表</param>
    /// <param name="assetPath">资源路径</param>
    private static void PathAutoTex(
        List<string> errorList,List<TextureErrorType> textureErrorTypesList, string assetPath)
    {
        SpriteAtlas spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
        if (null != spriteAtlas)
        {
            //UnityEngine.Debug.LogError(spriteAtlas.ToString()+"是精灵图集");
            _AutoSetSpriteAtlas(spriteAtlas,errorList,assetPath);
            return;
        }

        TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (textureImporter == null)
        {
            //UnityEngine.Debug.LogError(assetPath + " 获取图片格式信息错误!");
            return;
        }
        
        
        Texture texObj = AssetDatabase.LoadAssetAtPath<Texture>(textureImporter.assetPath);
        if (null == texObj)
        {
            //UnityEngine.Debug.LogError(textureImporter.assetPath + " 文件不是图片!");
            return;
        }
       
        //查看是否开启了minmap
        if (textureImporter.mipmapEnabled)
        {
            textureImporter.mipmapEnabled = false;
        }
        
        //查看是否开启了readwriteable
        if (textureImporter.isReadable)
        {
            textureImporter.isReadable = false;
        }
        
        //检查图片类型是否是Sprite，且贴图边长是否符合4的倍数
        //判断边长是否符合4的倍数
        if (texObj.width%4!=0||texObj.height%4!=0)
        {
            //判断贴图是否是精灵类型
            if (textureImporter.textureType == TextureImporterType.Sprite)
            {
                //判断精灵是否已经打包图集
                if (AtlasCollect.instance.getAtlas(assetPath)=="None")
                {
                    errorList.Add("该精灵尚未打包,且图片边长不为4的倍数，请手动修改");
                    textureErrorTypesList.Add(TextureErrorType.SpriteSizeAndNoIsAtlas);
                }
            }
            //不是精灵类型
            else
            {
                errorList.Add(",该图片的分辨率不是4的倍数，请手动修改");
                textureErrorTypesList.Add(TextureErrorType.TextureSize);
            }
        }
        
        
        //检查平台设置最大尺寸是否相同
        if (!_checkPlatformSize(textureImporter))
        {
            _AutoPlatformSize(textureImporter);
        }
        
        
        //各平台的规范设置
        _AutoIos(textureImporter, texObj,  errorList);
        _AutoAndroid(textureImporter, texObj, errorList); 
        _AutoPc(textureImporter, texObj,  errorList);
            
    }
    
    /// <summary>
    /// 设置图集至贴图规范
    /// </summary>
    protected static void _AutoSetSpriteAtlas(SpriteAtlas spriteAtlas, 
        List<string> errorList,string assetPath)
    {
        //检查图集的mipmap和readable
        SpriteAtlasTextureSettings spriteAtlasTextureSettings = spriteAtlas.GetTextureSettings();
        if (spriteAtlasTextureSettings.generateMipMaps )
        {
            spriteAtlasTextureSettings.generateMipMaps = false;
        }
        if (spriteAtlasTextureSettings.readable)
        {
            spriteAtlasTextureSettings.readable = false;
        }
        //将改正进行更新
        spriteAtlas.SetTextureSettings(spriteAtlasTextureSettings);

        bool sizeIsDifferent = _checkAtlasPlatformSize(spriteAtlas);
        //进行IOS平台的设置
        TextureImporterPlatformSettings textureImporterPlatformSettings =spriteAtlas.GetPlatformSettings(iosPlat);
        if ((textureImporterPlatformSettings.format != TextureImporterFormat.ASTC_6x6 && textureImporterPlatformSettings.format != TextureImporterFormat.ASTC_4x4 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB16 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB32)|| textureImporterPlatformSettings.overridden==false )
        {
            textureImporterPlatformSettings.format = TextureImporterFormat.ASTC_6x6;
            if (sizeIsDifferent)
                textureImporterPlatformSettings.maxTextureSize = 2048;
            textureImporterPlatformSettings.overridden = true;
            //将改正进行更新
            spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);
        }
                
        //进行Android平台的设置
        textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings(androidPlat);
        if (textureImporterPlatformSettings.format != TextureImporterFormat.ETC2_RGBA8 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB16 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB32)
        {
            textureImporterPlatformSettings.format = TextureImporterFormat.ETC2_RGBA8;
            if (sizeIsDifferent)
                textureImporterPlatformSettings.maxTextureSize = 2048;
            textureImporterPlatformSettings.overridden = true;
            //将改正进行更新
            spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);
        }
                
        //进行Pc平台的设置
        textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings(pcPlat);
        if (textureImporterPlatformSettings.format != TextureImporterFormat.DXT5 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB16 && textureImporterPlatformSettings.format!=TextureImporterFormat.ARGB32)
        {
            textureImporterPlatformSettings.format = TextureImporterFormat.DXT5;
            if (sizeIsDifferent)
                textureImporterPlatformSettings.maxTextureSize = 2048;
            textureImporterPlatformSettings.overridden = true;
            //将改正进行更新
            spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);
        }
    }
    
    #endregion
    
    public static void FileCheckSprite(List<Object> _selectionObject)
    {
        AtlasCollect.instance.init();
        List<TexPathAndCount> texPathAndCountList = new List<TexPathAndCount>();
        List<string> errorList = new List<string>();
        List<TextureErrorType> textureErrorTypesList = new List<TextureErrorType>();
        List<int> pathIndexList = new List<int>();
        AssetDatabase.StartAssetEditing();
        foreach (Object objects in _selectionObject)
        {
            string path = AssetDatabase.GetAssetPath(objects);
            //如果选择对象是文件夹
            if (Directory.Exists(path))
            {
                //获取文件夹路径string[]
                var folders = new List<string>();
                folders.Add(path);
                string[] filePath = folders.ToArray();
                //获取文件夹下的贴图和图集
                foreach (var guid in AssetDatabase.FindAssets("t:Texture", filePath))
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    int index = errorList.Count;
                    //通过路径自动设置贴图至规范
                    PathCheckSprite(errorList,textureErrorTypesList,assetPath);
                    if (index != errorList.Count)
                    {
                        TexPathAndCount texPathAndCount;
                        texPathAndCount.path = assetPath;
                        texPathAndCount.count = errorList.Count - index;
                        pathIndexList.Add(index);
                        texPathAndCountList.Add(texPathAndCount);
                    }
                }
                continue;
            }
            
            //如果选择对象是贴图或图集
            if (objects is Texture || objects is SpriteAtlas)
            {
                int index = errorList.Count;
                //通过路径自动设置贴图至规范
                PathCheckSprite(errorList,textureErrorTypesList,path);
                if (index != errorList.Count)
                {
                    TexPathAndCount texPathAndCount;
                    texPathAndCount.path = path;
                    texPathAndCount.count = errorList.Count - index;
                    pathIndexList.Add(index);
                    texPathAndCountList.Add(texPathAndCount);
                }
                continue;
            }
            
        }
        if (errorList.Count <= 0)
        {
            EditorUtility.DisplayDialog("检查未打包图集的精灵结果","所有精灵都已打包图集","完成");
            TextureCheckEditorWindow.pathIndexList = pathIndexList;
            TextureCheckEditorWindow.texPathAndCountList = texPathAndCountList;
            TextureCheckEditorWindow.errorList = errorList;
            TextureCheckEditorWindow.textureErrorTypesList = textureErrorTypesList;
            TextureCheckEditorWindow.textureErrorTypesList = textureErrorTypesList;
           
        }
        else
        {
            TextureCheckEditorWindow.pathIndexList = pathIndexList;
            TextureCheckEditorWindow.texPathAndCountList = texPathAndCountList;
            TextureCheckEditorWindow.errorList = errorList;
            TextureCheckEditorWindow.textureErrorTypesList = textureErrorTypesList;
            TextureCheckEditorWindow.ShowWindow();
        }

        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }
    
    /// <summary>
    /// 检查精灵是否已经打包图集
    /// </summary>
    /// <param name="imageList">UI图片列表</param>
    /// <param name="errorList">UI错误信息列表</param>
    /// <param name="assetPath">资源路径</param>
    private static void PathCheckSprite(
        List<string> errorList,List<TextureErrorType> textureErrorTypesList,string assetPath)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (textureImporter == null)
        {
            //UnityEngine.Debug.LogError(assetPath + " 获取图片格式信息错误!");
            return;
        }

        Texture texObj = AssetDatabase.LoadAssetAtPath<Texture>(textureImporter.assetPath);
        if (null == texObj)
        {
            //UnityEngine.Debug.LogError(textureImporter.assetPath + " 文件不是图片!");
            return;
        }
        
        //判断是否是精灵类型，且为单个模式
        if (textureImporter.textureType == TextureImporterType.Sprite)
        {
            //判断精灵是否未打包
            if (AtlasCollect.instance.getAtlas(assetPath)=="None")
            {
                if (texObj.width % 4 != 0 || texObj.height % 4 != 0)
                {
                    errorList.Add("该精灵尚未打包,且图片边长不为4的倍数，请手动修改");
                    textureErrorTypesList.Add(TextureErrorType.SpriteSizeAndNoIsAtlas);
                }
                else
                {
                    errorList.Add("该精灵尚未打包");
                    textureErrorTypesList.Add(TextureErrorType.SpriteNoIsAtlas);
                }
            }
        }
    }
    
    /// <summary>
    /// 检查平台尺寸是否设置相同
    /// </summary>
    /// <param name="_texture"></param>
    /// <returns></returns>
    static bool _checkPlatformSize(TextureImporter _texture)
    {
        int baseMaxSize;
        int maxSize;
        TextureImporterFormat format;
        
        //获取ios平台最大尺寸
        _texture.GetPlatformTextureSettings(iosPlat, out maxSize, out format);
        baseMaxSize = maxSize;

        //判断Android平台最大尺寸是否相同
        _texture.GetPlatformTextureSettings(androidPlat, out maxSize, out format);
        if (baseMaxSize != maxSize)
        {
            return false;
        }
        
        //判断pc平台最大尺寸是否相同
        _texture.GetPlatformTextureSettings(pcPlat, out maxSize, out format);
        if (baseMaxSize != maxSize)
        {
            return false;
        }
        
        //平台尺寸相同返回true
        return true;
    }
    
    /// <summary>
    /// 自动设置平台尺寸为2048
    /// </summary>
    /// <param name="_texture"></param>
    static void _AutoPlatformSize(TextureImporter _texture)
    {
        int baseMaxSize;
        int maxSize;
        TextureImporterFormat format;
        //设置ios平台最大尺寸为2048
        _texture.GetPlatformTextureSettings(iosPlat, out maxSize, out format);
        _texture.SetPlatformTextureSettings(iosPlat,2048,format);
        
        //设置Android平台最大尺寸为2048
        _texture.GetPlatformTextureSettings(androidPlat, out maxSize, out format);
        _texture.SetPlatformTextureSettings(androidPlat,2048,format);
        
        //设置pc平台最大尺寸为2048
        _texture.GetPlatformTextureSettings(pcPlat, out maxSize, out format);
        _texture.SetPlatformTextureSettings(pcPlat,2048,format);

        return;
    }
    
    /// <summary>
    /// 检查图集的平台设置尺寸是否相同
    /// </summary>
    /// <param name="_texture"></param>
    /// <returns></returns>
    static bool _checkAtlasPlatformSize(SpriteAtlas spriteAtlas)
    {
        int baseMaxSize;
        //获取ios平台最大尺寸
        TextureImporterPlatformSettings spriteAltalsImport =spriteAtlas.GetPlatformSettings(iosPlat);
        Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(sprites);
        foreach (Sprite sprite in sprites)
        {
            if (sprite == null)
            {
                Debug.LogError(AssetDatabase.GetAssetPath(spriteAtlas.GetInstanceID())+"这张图集里的贴图存在问题!!!!",spriteAtlas);
                return false;
            }
            if (spriteAltalsImport.maxTextureSize < sprite.texture.height ||
                spriteAltalsImport.maxTextureSize < sprite.texture.width)
            {
                //Debug.Log(assetPath+"是图集，其最大平台设置尺寸小于包含精灵尺寸，请将其最大尺寸改正为2048",AssetDatabase.LoadMainAssetAtPath(assetPath));
                //spriteAltalsImport.maxTextureSize = 2048;
                return false;
            }
        }
        baseMaxSize = spriteAltalsImport.maxTextureSize;
        
        //判断Android平台最大尺寸是否相同
        spriteAltalsImport =spriteAtlas.GetPlatformSettings(androidPlat);
        if (baseMaxSize != spriteAltalsImport.maxTextureSize)
        {
            return false;
        }

        //判断pc平台最大尺寸是否相同
        spriteAltalsImport =spriteAtlas.GetPlatformSettings(pcPlat);
        if (baseMaxSize != spriteAltalsImport.maxTextureSize)
        {
            return false;
        }
        
        //平台尺寸相同返回true
        return true;
    }

    
    [MenuItem("Assets/Texture/重置更改贴图格式")]
    static void ResetTexture()
    {
        Object[] selectionObjects = Selection.objects;
        AssetDatabase.StartAssetEditing();
        foreach (var objects in selectionObjects)
        {
            string path = AssetDatabase.GetAssetPath(objects);
            //如果选择对象是文件夹
            if (Directory.Exists(path))
            {
                //获取文件夹路径string[]
                var folders = new List<string>();
                folders.Add(path);
                string[] filePath = folders.ToArray();

                //获取文件夹下的贴图和图集
                foreach (var guid in AssetDatabase.FindAssets("t:Texture", filePath))
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    //通过路径清楚贴图的平台覆盖
                    TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                    if (textureImporter != null)
                    {
                        textureImporter.ClearPlatformTextureSettings("Standalone");
                        textureImporter.ClearPlatformTextureSettings("iPhone");
                        textureImporter.ClearPlatformTextureSettings("Android");
                        AssetImporter.GetAtPath(assetPath).SaveAndReimport();
                    }
                }
                continue;
            }
            //如果选择对象是贴图或图集
            if (objects is Texture || objects is SpriteAtlas)
            {
                //通过路径清楚贴图的平台覆盖
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                if (textureImporter != null)
                {
                    textureImporter.ClearPlatformTextureSettings("Standalone");
                    textureImporter.ClearPlatformTextureSettings("iPhone");
                    textureImporter.ClearPlatformTextureSettings("Android");
                    AssetImporter.GetAtPath(path).SaveAndReimport();
                }
                continue;
            }
        }
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }
    
    [MenuItem("NPAssets/资源规范/对项目进行更改/更新项目贴图serializedVersion")]
    public static void UpDateTexVersion()
    {
        List<string> path = new List<string>();
        //获取贴图路径
        foreach (var guid in AssetDatabase.FindAssets("t:Texture", new[] {"Assets/Resources"}))
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            path.Add(assetPath);
        }
        IEnumerable<string> IEpath = path;
        //更新路线下的serializedVersion
        AssetDatabase.ForceReserializeAssets(IEpath);
        Debug.Log("更新贴图serializedVersion完毕");
    }
     public static void FileGetDependencies(List<Object> _objects)
    {
        AtlasCollect.instance.init();
        List<TexPathAndCount> texPathAndCountList = new List<TexPathAndCount>();
        List<string> errorList = new List<string>();
        List<TextureErrorType> textureErrorTypesList = new List<TextureErrorType>();
        List<int> pathIndexList = new List<int>();
        AssetDatabase.StartAssetEditing();
        foreach (var objects in _objects)
        {
            string path = AssetDatabase.GetAssetPath(objects);
            //如果选择对象是文件夹
            if (Directory.Exists(path))
            {
                var folders = new List<string>();
                folders.Add(path);
                string[] filePath = folders.ToArray();

                //获取文件夹下的贴图和图集
                foreach (var guid in AssetDatabase.FindAssets("t:SpriteAtlas", filePath))
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    //获取图集的AssetBundleName
                    string atlasABName = AssetDatabase.GetImplicitAssetBundleName(assetPath);
                    if (atlasABName== "")
                    {
                        //错误信息列表
                        errorList.Add("该图集的AssetBundle尚未设置");
                        textureErrorTypesList.Add(TextureErrorType.AssetBundleError);
                        TexPathAndCount texPathAndCount;
                        texPathAndCount.path = assetPath;
                        texPathAndCount.count = 1;
                        pathIndexList.Add(texPathAndCountList.Count);
                        texPathAndCountList.Add(texPathAndCount);
                    }
                    else
                    {
                        //获取图集下贴图的AssetBundleName
                        var depences = AssetDatabase.GetDependencies(assetPath);
                        foreach (var spritePath in depences)
                        {
                            string spriteABName = AssetDatabase.GetImplicitAssetBundleName(spritePath);
                            if (spriteABName == "")
                            {
                                //错误信息列表
                                errorList.Add("该贴图的ABName为空，他的图集ABName为："+atlasABName);
                                textureErrorTypesList.Add(TextureErrorType.AssetBundleError);
                                TexPathAndCount texPathAndCount;
                                texPathAndCount.path = spritePath;
                                texPathAndCount.count = 1;
                                pathIndexList.Add(texPathAndCountList.Count);
                                texPathAndCountList.Add(texPathAndCount);
                            }
                            if (spriteABName != atlasABName)
                            {
                                //错误信息列表
                                errorList.Add("该贴图的ABName为："+ spriteABName+"；不等于他的图集ABName为："+atlasABName);
                                textureErrorTypesList.Add(TextureErrorType.AssetBundleError);
                                TexPathAndCount texPathAndCount;
                                texPathAndCount.path = spritePath;
                                texPathAndCount.count = 1;
                                pathIndexList.Add(texPathAndCountList.Count);
                                texPathAndCountList.Add(texPathAndCount);
                            }
                        }
                    }
                }
                continue;
            }
            //如果选择对象是贴图或图集
            if (objects is SpriteAtlas)
            {
                //获取图集的AssetBundleName
                string atlasABName = AssetDatabase.GetImplicitAssetBundleName(path);
                if (atlasABName== "")
                {
                    //错误信息列表
                    errorList.Add("该图集的AssetBundle尚未设置");
                    textureErrorTypesList.Add(TextureErrorType.AssetBundleError);
                    TexPathAndCount texPathAndCount;
                    texPathAndCount.path = path;
                    texPathAndCount.count = 1;
                    pathIndexList.Add(texPathAndCountList.Count);
                    texPathAndCountList.Add(texPathAndCount);
                }
                else
                {
                    //获取图集下贴图的AssetBundleName
                    var depences = AssetDatabase.GetDependencies(path);
                    foreach (var spritePath in depences)
                    {
                        string spriteABName = AssetDatabase.GetImplicitAssetBundleName(spritePath);
                        if (spriteABName == "")
                        {
                            //错误信息列表
                            errorList.Add("该贴图的ABName为空，他的图集ABName为："+atlasABName);
                            textureErrorTypesList.Add(TextureErrorType.AssetBundleError);
                            TexPathAndCount texPathAndCount;
                            texPathAndCount.path = spritePath;
                            texPathAndCount.count = 1;
                            pathIndexList.Add(texPathAndCountList.Count);
                            texPathAndCountList.Add(texPathAndCount);
                        }
                        if (spriteABName != atlasABName)
                        {
                            //错误信息列表
                            errorList.Add("该贴图的ABName为："+ spriteABName+",不等于他的图集ABName为："+atlasABName);
                            textureErrorTypesList.Add(TextureErrorType.AssetBundleError);
                            TexPathAndCount texPathAndCount;
                            texPathAndCount.path = spritePath;
                            texPathAndCount.count = 1;
                            pathIndexList.Add(texPathAndCountList.Count);
                            texPathAndCountList.Add(texPathAndCount);
                        }
                    }
                }
            }

        }
        TextureCheckEditorWindow.pathIndexList = pathIndexList;
        TextureCheckEditorWindow.texPathAndCountList = texPathAndCountList;
        TextureCheckEditorWindow.errorList = errorList;
        TextureCheckEditorWindow.textureErrorTypesList = textureErrorTypesList;
        TextureCheckEditorWindow.ShowWindow();

        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }


     [MenuItem("NPAssets/资源规范/对项目进行更改/更改UI贴图的WrapMode为Clamp")]
     public static void UpdateSpriteWrapMode()
     {
         AssetDatabase.StartAssetEditing();
         //搜索场景中的Prefab，对UI组件上的贴图更改WrapMode为Clamp，对用于Tile模式的贴图更改WrapMode为Repeat
         UIWrapModeChangeToClamp();
         AssetDatabase.StopAssetEditing();
         AssetDatabase.Refresh();
     }
     
     public static void UIWrapModeChangeToClamp()
     {
         string[] guids=AssetDatabase.FindAssets("t:Prefab", new[] {"Assets/Resources"});
         foreach (var guid in guids)
         {
             string assetPath=AssetDatabase.GUIDToAssetPath(guid);
             var pre = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
             if (pre == null)
             {
                 continue;
             }
             //获取预制体中的Image组件和RawImage组件
             Image[] images = pre.GetComponentsInChildren<Image>();
             RawImage[] rawImages = pre.GetComponentsInChildren<RawImage>();
             
             //对Image组件进行处理
             foreach (var image in images)
             {
                 if (image.type == Image.Type.Tiled)
                 {
                     string path = AssetDatabase.GetAssetPath(image.sprite.GetInstanceID());
                     ChangTexWarpMode(path,TextureWrapMode.Repeat);
                 }
                 else
                 {
                     string path = AssetDatabase.GetAssetPath(image.sprite.GetInstanceID());
                     ChangTexWarpMode(path,TextureWrapMode.Clamp);
                 }
             }
             //对RawImage组件进行处理
             foreach (var rawImage in rawImages)
             {
                 //判断RawImage的UVRect属性是否有修改,有则需要Repeat模式
                 if (rawImage.uvRect.x != 0 || rawImage.uvRect.y != 0 || rawImage.uvRect.width != 1 ||
                     rawImage.uvRect.height != 1)
                 {
                     string path = AssetDatabase.GetAssetPath(rawImage.texture.GetInstanceID());
                     ChangTexWarpMode(path,TextureWrapMode.Repeat);
                 }
                 else
                 {
                     string path = AssetDatabase.GetAssetPath(rawImage.texture.GetInstanceID());
                     ChangTexWarpMode(path,TextureWrapMode.Clamp);
                 }
             }
         }
         
     }

     public static void ChangTexWarpMode(string path, TextureWrapMode textureWrapMode)
     {
         TextureImporter textureImporter=AssetImporter.GetAtPath(path) as TextureImporter;
         if (textureImporter == null)
         {
             return;
         }
         if (textureImporter.wrapMode != textureWrapMode)
         {
             textureImporter.wrapMode = textureWrapMode;
             Debug.Log(path,AssetDatabase.LoadMainAssetAtPath(path));
             AssetImporter.GetAtPath(path).SaveAndReimport();
         }
     }
     
     [MenuItem("NPAssets/资源规范/查找特效上使用，未开启读写的Mesh",false,1)]
     public static void debugAssetParticleUsedMeshSetting()
     {
         EditorUtil.checkAssetParticleUsedMeshSetting(out string kk);
     }
     [MenuItem("NPAssets/资源规范/检查AssetBundle 是否有同名的" , false, 1)]
     public static void checkAssetBundleSameName()
     {
         //检查Bundle错误
         List<string> errorBundleList = EditorUtil.checkAssetBundleVariant();
         List<string> errorSameNameList = EditorUtil.checkSameNameResource();
         if((errorBundleList != null && errorBundleList.Count > 0) || (errorSameNameList != null && errorSameNameList.Count > 0))
         {
             //有Bundle错误
             if(errorBundleList != null && errorBundleList.Count > 0)
                 Debug.LogError("有Bundle的variant没有设置：" + errorBundleList.ToStringList());
             if(errorSameNameList != null && errorSameNameList.Count > 0)
                 Debug.LogError("存在同一个Bundle里面有同名资源：" + errorSameNameList.ToStringList());
             return;
         }
         AssetDatabase.Refresh();
         Debug.Log("检查AssetBundle 是否有同名的 完成");
     }
     
     [MenuItem("NPAssets/资源规范/对项目进行更改/项目图集格式规范处理")]
     public static void AutoSpriteAtlasAndroidSetting()
     {
         AssetDatabase.StartAssetEditing();
         string[] guids=AssetDatabase.FindAssets("t:SpriteAtlas");
         foreach (var guid in guids)
         {
             string path = AssetDatabase.GUIDToAssetPath(guid);
             object[] toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
             foreach (var obj in toCheck)
             {
                 SpriteAtlas spriteAtlas=obj as SpriteAtlas;

                 //设置andriod的平台设置
                 var android = spriteAtlas.GetPlatformSettings("Android");
                 if (!android.overridden || android.format == TextureImporterFormat.Automatic || android.format == TextureImporterFormat.ETC2_RGBA8  )
                 {
                     android.overridden = true;
                     android.format = TextureImporterFormat.ASTC_6x6;
                     spriteAtlas.SetPlatformSettings(android);
                     UnityEngine.Debug.Log(spriteAtlas.name + "图集的安卓格式由ETC2转为ASTC" ,spriteAtlas);
                 }
                 
                 //设置ios图集平台格式
                 var ios=spriteAtlas.GetPlatformSettings("iPhone");

                 if (!ios.overridden || ios.format == TextureImporterFormat.Automatic)
                 {
                     ios.overridden = true;
                     ios.format = TextureImporterFormat.ASTC_6x6;
                     spriteAtlas.SetPlatformSettings(ios);
                     UnityEngine.Debug.Log(spriteAtlas.name + "图集的IOS格式由默认转为ASTC" ,spriteAtlas);
                 }
                 
                 //设置pc的平台格式
                 var pc = spriteAtlas.GetPlatformSettings("Standalone");
                 if (!pc.overridden || pc.format == TextureImporterFormat.Automatic)
                 {
                     pc.overridden = true;
                     pc.format = TextureImporterFormat.DXT5;
                     spriteAtlas.SetPlatformSettings(pc);
                     UnityEngine.Debug.Log(spriteAtlas.name + "图集的pc格式由默认转为DXT5" ,spriteAtlas);
                 }

             }
         }
         UnityEngine.Debug.Log("项目图集安卓格式ETC2转为ASTC完毕");
         AssetDatabase.StopAssetEditing();
         AssetDatabase.SaveAssets();
         AssetDatabase.Refresh();
     }
     
     [MenuItem("NPAssets/资源规范/检查项目中的图集格式",false,1)]
     public static void CheckSpriteAtlasSetting()
     {
         UnityEngine.Debug.Log("检查项目中的图集格式 完毕");
         string[] guids=AssetDatabase.FindAssets("t:SpriteAtlas");
         foreach (var guid in guids)
         {
             string path = AssetDatabase.GUIDToAssetPath(guid);
             object[] toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
             foreach (var obj in toCheck)
             {
                 SpriteAtlas spriteAtlas=obj as SpriteAtlas;
                 
                 //设置ios的平台设置
                 TextureImporterPlatformSettings ios=spriteAtlas.GetPlatformSettings("iPhone");
                 if (!ios.overridden||!AtlasIOSPlatNeed(ios))
                 {
                     UnityEngine.Debug.Log(path+"\nios平台设置为："+ios.format+"  项目规范为：ASTC6x6",spriteAtlas);
                 }
                 
                 //设置andriod的平台设置
                 TextureImporterPlatformSettings android = spriteAtlas.GetPlatformSettings("Android");
                 if (!android.overridden||!AtlasAndroidPlatNeed(android))
                 {
                     UnityEngine.Debug.Log(path+"\nandroid平台设置为："+android.format+"  项目规范为：ASTC6x6",spriteAtlas);
                 }
                 
                 //设置pc的平台设置
                 TextureImporterPlatformSettings pc = spriteAtlas.GetPlatformSettings("Standalone");
                 if (!pc.overridden || !AtlasPCPlatNeed(pc))
                 {
                     UnityEngine.Debug.Log(path+"\npc平台设置为："+pc.format+"  项目规范为：DXT5",spriteAtlas);
                 }
                 
                 //检查ui图集的TightPacking选项
                 if (path.Contains("Assets/Resources/GUI"))
                 {
                     var packing = spriteAtlas.GetPackingSettings();
                     if (packing.enableTightPacking)
                     {
                         UnityEngine.Debug.Log(path+"\n是ui图集，但错误开启了TightPacking",spriteAtlas);
                     }
                     
                 }
                 //获取ios平台最大尺寸
                 Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
                 spriteAtlas.GetSprites(sprites);
                 foreach (Sprite sprite in sprites)
                 {
                     if (sprite == null)
                     {
                         Debug.LogError(AssetDatabase.GetAssetPath(spriteAtlas.GetInstanceID())+"这张图集里的贴图存在问题!!!!",spriteAtlas);
                     }
                     if (ios.maxTextureSize < sprite.texture.height ||
                         ios.maxTextureSize < sprite.texture.width ||
                         android.maxTextureSize < sprite.texture.height ||
                         android.maxTextureSize < sprite.texture.width ||
                         pc.maxTextureSize < sprite.texture.height ||
                         pc.maxTextureSize < sprite.texture.width)
                     {
                         Debug.LogError(AssetDatabase.GetAssetPath(spriteAtlas.GetInstanceID())+"这张图集爆了!!!!",spriteAtlas);
                     }
                 }
             }
         }
         AssetDatabase.SaveAssets();
     }

     private static bool AtlasIOSPlatNeed(TextureImporterPlatformSettings textureImporterPlatformSettings)
     {
         if (textureImporterPlatformSettings.format == TextureImporterFormat.ASTC_4x4 ||
             textureImporterPlatformSettings.format == TextureImporterFormat.ASTC_5x5 ||
             textureImporterPlatformSettings.format == TextureImporterFormat.ASTC_6x6 ||
             textureImporterPlatformSettings.format == TextureImporterFormat.RGBA32
         )
         {
             return true;
         }

         return false;
     }
     private static bool AtlasAndroidPlatNeed(TextureImporterPlatformSettings textureImporterPlatformSettings)
     {
         if (textureImporterPlatformSettings.format == TextureImporterFormat.ASTC_4x4 ||
             textureImporterPlatformSettings.format == TextureImporterFormat.ASTC_5x5 ||
             textureImporterPlatformSettings.format == TextureImporterFormat.ASTC_6x6 ||
             textureImporterPlatformSettings.format == TextureImporterFormat.RGBA32
         )
         {
             return true;
         }

         return false;
     }
     private static bool AtlasPCPlatNeed(TextureImporterPlatformSettings textureImporterPlatformSettings)
     {
         if (textureImporterPlatformSettings.format == TextureImporterFormat.DXT5 ||
             textureImporterPlatformSettings.format == TextureImporterFormat.RGBA32
         )
         {
             return true;
         }

         return false;
     }

     
     [MenuItem("Assets/Create/2D/Sprite Atlas_MJ" ,false ,19)]
     public static void CreateSpriteAtlas()
     {
         SpriteAtlas spriteAtlas = new SpriteAtlas();
         //设置ios图集平台格式
         var ios=spriteAtlas.GetPlatformSettings("iPhone");
         ios.overridden = true;
         ios.format = TextureImporterFormat.ASTC_6x6;
         spriteAtlas.SetPlatformSettings(ios);
         
         //设置android的平台格式
         var android = spriteAtlas.GetPlatformSettings("Android");
         android.overridden = true;
         android.format = TextureImporterFormat.ASTC_6x6;
         spriteAtlas.SetPlatformSettings(android);
         
         //设置pc的平台格式
         var pc = spriteAtlas.GetPlatformSettings("Standalone");
         pc.overridden = true;
         pc.format = TextureImporterFormat.DXT5;
         spriteAtlas.SetPlatformSettings(pc);
         
         //判断是否是ui文件夹，是则关闭Tightpacking选项
         string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
         if (path.Contains("Assets/Resources/GUI"))
         {
             var packing = spriteAtlas.GetPackingSettings();
             packing.enableTightPacking = false;
             spriteAtlas.SetPackingSettings(packing);
         }
         
         ProjectWindowUtil.CreateAsset(spriteAtlas,"New Sprite Atlas.spriteatlas");
     }
     
     [MenuItem("Assets/Create/2D/Sprite Atlas" ,true ,20)]
     private static bool HideUnitySpriteAtlas()
     {
         // 返回false将菜单项置为不可用（或隐藏）
         return false;
     }

     /// <summary>
     /// 检查是否使用了系统默认shader的材质 - 会重复打包
     /// </summary>
     [MenuItem("NPAssets/资源规范/检查错误的材质", false, 1)]
     static void CheckErrorMaterial()
     {
         string[] searchPath = new string[] {"Assets/Resources/"};
         CheckErrorMaterialAtPath(searchPath);
     }
     
     /// <summary>
     /// 检查是否使用了系统默认shader的材质 - 会重复打包
     /// </summary>
     [MenuItem("Assets/检查工具/材质规范窗口")]
     static void MaterialCheckWindow()
     {
         Object[] selectionObjects = Selection.objects;
         List<Object> objectList = new List<Object>();
         //选择文件夹对象
         foreach (var selectionObject in selectionObjects)
         {
             string path = AssetDatabase.GetAssetPath(selectionObject);
             if(Directory.Exists(path))
                 objectList.Add(selectionObject);
         }
         MaterialCheckEditorWindow.selectionObjectList = objectList;
         MaterialCheckEditorWindow.ShowWindow();
     }

    public static void CheckErrorMaterialAtPath(string[] _searchPath , bool _miss_debug = true ,bool _system_lit_debug = true,bool _system_Unlit_debug = true, bool _urp_debug = true,bool _error_debug = true, bool _other_debug = true)
    {
        //测试文件夹排除检查
        string ignorePath = "Assets/Resources/GameRes/go/go_1abcd_player/go_99999_test/";
        
        //如果搜索路径为空，指定搜索Resources路径
        if(_searchPath == null || _searchPath.Length == 0)
            _searchPath = new string[] {"Assets/Resources/"};
        
        //检查模型文件
        CheckModelMaterialsAtPath(_searchPath,ignorePath,_miss_debug);
        
        //检查材质文件
        CheckBaseMaterialsAtPath(_searchPath,ignorePath,_system_lit_debug,_system_Unlit_debug,_urp_debug,_error_debug,_other_debug);
        
        Debug.Log("检查错误的材质完毕");
    }

    //检查模型文件，检查内容：材质丢失
    private static void CheckModelMaterialsAtPath(string[] _searchPath ,string _ignorePath , bool _miss_debug = true)
    {
        string[] modelGuids = AssetDatabase.FindAssets("t:Model",_searchPath);
        
        List<GameObject> miss_debug = new List<GameObject>();
        
        foreach (string guid in modelGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if(path.Contains(_ignorePath))
                continue;
            GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        
            if (model != null)
            {
                Renderer[] renderers = model.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                {
                    foreach (var material in renderer.sharedMaterials)
                    {
                        if (material == null)
                        {
                            miss_debug.Add(model);
                            continue;
                        }
                
                        // if (material.shader.name == "Universal Render Pipeline/Lit")
                        //     URPLit = true;
                        //
                        // if (material.shader.name == "Universal Render Pipeline/UnLit")
                        //     URPUnlit = true;
                    }
                }

                // if (miss)
                //     Debug.Log(string.Format("<color=#ff0000>{0}</color>", "材质丢失") + "\t" + model.name,model);
                // if(URPLit)
                //     Debug.Log(string.Format("<color=#7B68EE>{0}</color>", "系统Lit材质") + "\t" + model.name,model);
                // if (URPUnlit)
                //     Debug.Log(string.Format("<color=#FFA500>{0}</color>", "系统Unlit材质") + "\t" + model.name,model);
            }
        }

        if (_miss_debug)
            foreach (var model in miss_debug)
                Debug.Log(string.Format("<color=#00E5EE>{0}</color>", "材质丢失") + "\t" + model.name,model);
    }
    
    //检查材质文件
    private static void CheckBaseMaterialsAtPath(string[] _searchPath ,string _ignorePath,bool _system_lit_debug = true,bool _system_Unlit_debug = true, bool _urp_debug = true,bool _error_debug = true, bool _other_debug = true)
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material",_searchPath);

        //按类型输出
        List<Material> system_lit_debug = new List<Material>();
        List<Material> system_Unlit_debug = new List<Material>();
        List<Material> urp_debug = new List<Material>();
        List<Material> error_debug = new List<Material>();
        List<Material> other_debug = new List<Material>();
        
        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if(path.Contains(_ignorePath))
                continue;
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null)
            {
                //白名单
                if (material.shader.name.IndexOf("MJ/") != 0 
                    && material.shader.name.IndexOf("URPEx/") != 0
                    && material.shader.name.IndexOf("Spine/") != 0
                    && material.shader.name.IndexOf("Hide/") != 0
                    && material.shader.name.IndexOf("TextMeshPro/") != 0
                    && material.shader.name.IndexOf("GUI/") != 0
                    && material.shader.name.IndexOf("Skybox/") != 0
                    )
                {
                    //黑名单
                    //使用系统lit材质
                    if (material.shader.name == "Universal Render Pipeline/Lit")
                    {
                        system_lit_debug.Add(material);
                        continue;
                    }
                    
                    //使用系统unlit材质
                    if (material.shader.name == "Universal Render Pipeline/Unlit" || material.shader.name.IndexOf("Unlit/") == 0)
                    {
                        system_Unlit_debug.Add(material);
                        continue;
                    }
                    
                    //使用系统其他URP材质
                    if (material.shader.name.IndexOf("Universal Render Pipeline/") == 0)
                    {
                        urp_debug.Add(material);
                        continue;
                    }
                    
                    //不存在的材质
                    if (material.shader.name == "Hidden/InternalErrorShader")
                    {
                        error_debug.Add(material);
                        continue;
                    }
                    
                    //剩下非项目规范的材质
                    other_debug.Add(material);
                }
            }
        }

        if (_system_lit_debug)
            foreach (var material in system_lit_debug)
                Debug.Log(string.Format("<color=#00EE76>{0}</color>", "系统lit材质") + "\t" + material.name,material);

        if(_system_Unlit_debug)
            foreach (var material in system_Unlit_debug)
                Debug.Log(string.Format("<color=#FFF68F>{0}</color>", "系统Unlit材质") + "\t" + material.name,material);

        if(_urp_debug)
            foreach (var material in urp_debug)
                Debug.Log(string.Format("<color=#8B658B>{0}</color>", "系统其他URP材质") + "\t" + material.name,material);
        
        if(_error_debug)
            foreach (var material in error_debug)
                Debug.Log(string.Format("<color=#FF0000>{0}</color>", "不存在的材质") + "\t" + material.name,material);
        
        if(_other_debug)
            foreach (var material in other_debug)
                Debug.Log(string.Format("<color=#C71585>{0}</color>", "非项目规范的材质") + "\t" + material.name,material);
    }

    
     [MenuItem("NPAssets/资源规范/项目打包一键检查，并生成变体↓" ,false ,0)]
     private static void PackCheck()
     {
         //查找特效上使用，未开启读写的Mesh
         debugAssetParticleUsedMeshSetting();
         
         //检查AssetBundle 是否有同名的
         checkAssetBundleSameName();

         //检查项目中的图集格式
         CheckSpriteAtlasSetting();

         //检查错误的材质信息
         CheckErrorMaterial();
         
         //生成shader变体
         ShaderVariantCollector.CollectShaders();
     }
     
}

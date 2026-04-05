using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.U2D;
using UnityEngine.U2D;

public class TextureCheckEditorWindow : EditorWindow
{
    //错误图片路径列表
    public static List<TexPathAndCount> texPathAndCountList = new List<TexPathAndCount>();
    //路径索引列表
    public static List<int> pathIndexList = new List<int>();
    //错误信息列表
    public static List<string> errorList = new List<string>();
    //错误类型列表
    public static List<TextureErrorType> textureErrorTypesList = new List<TextureErrorType>();
    //查询范围
    public static List<Object> selectionObject = new List<Object>();
    public Vector2 scrollPosition1 = new Vector2(0, 0);
    public Vector2 scrollPosition2 = new Vector2(0, 0);

    //初始化选项框
    public static bool texMipmap = false;
    public static bool texMipmapOpen = true;
    public static bool texReadEnabled = true;
    public static bool texAlpha = true;
    public static bool texSize = true;
    public static bool texPlatform = true;
    public static bool atlasMipmap = false;
    public static bool atlasMipmapOpen = true;
    public static bool atlasReadEnabled = true;
    public static bool atlasSize = true;
    public static bool atlasPlatform = true;
    public static bool atlasRGBA = true;
    public static bool texRGBA = true;
    //GameObject gameObject;
    //Editor gameObjectEditor;

    [MenuItem("Window/TextureCheck Editor")]
    public static void ShowWindow()
    {
        GetWindow<TextureCheckEditorWindow>("贴图资源规范");
    }
    
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1,  true,true,GUILayout.Width(450), GUILayout.Height(300));
        GUILayout.Label("------------------当前选择的检查对象------------");
        if (selectionObject.Count == 0)
        {
            GUILayout.Label("当前选择的文件数量为空，请添加文件选择");
        }
        else
        {
            for(int i=0;i<selectionObject.Count;i++)
            {
                GUILayout.BeginHorizontal();
                string path = AssetDatabase.GetAssetPath(selectionObject[i]);
                //取消文件按钮
                if(GUILayout.Button("取消",GUILayout.Width(40)))
                {
                    selectionObject.RemoveAt(i);
                    TextureCheck.FileCheckIsStandard(selectionObject);
                }
                GUILayout.Label(path);
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
        
        GUILayout.BeginVertical();
        GUILayout.Label("------------------需要检查的错误类型------------");
        GUILayout.Label("---贴图检查---");
        EditorGUILayout.BeginHorizontal();
        
        if(texReadEnabled != GUILayout.Toggle(texReadEnabled,"开启ReadEnabled",GUILayout.Width(200)))
        {
            texReadEnabled = !texReadEnabled;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }
        
        if (texMipmap != GUILayout.Toggle(texMipmap, "检查贴图Mipmap", GUILayout.Width(150)))
        {
            texMipmap = !texMipmap;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }
        if (texMipmap)
        {
            if (texMipmapOpen != GUILayout.Toggle(texMipmapOpen, "勾选检查开启，不勾选检查关闭", GUILayout.Width(200)))
            {
                texMipmapOpen = !texMipmapOpen;
                TextureCheck.FileCheckIsStandard(selectionObject);
            }
        }
        /*不会对AlphaIsTransparency选项进行检查，根据使用场景进行开启
        if (texAlpha != GUILayout.Toggle(texAlpha, "错误的AlphaIsTransparency设置", GUILayout.Width(200)))
        {
            texAlpha = !texAlpha;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }
        */
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if(texSize != GUILayout.Toggle(texSize,"错误的贴图尺寸大小",GUILayout.Width(200)))
        {
            texSize = !texSize;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }

        if (texPlatform != GUILayout.Toggle(texPlatform, "错误的贴图平台格式", GUILayout.Width(150)))
        {
            texPlatform = !texPlatform;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }
        if (texPlatform)
        {
            if (texRGBA != GUILayout.Toggle(texRGBA, "不检查贴图RGBA16和32", GUILayout.Width(200)))
            {
                texRGBA = !texRGBA;
                TextureCheck.FileCheckIsStandard(selectionObject);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(15);
        GUILayout.Label("---图集检查---");
        EditorGUILayout.BeginHorizontal();
        if (atlasReadEnabled != GUILayout.Toggle(atlasReadEnabled, "开启ReadEnabled", GUILayout.Width(200)))
        {
            atlasReadEnabled = !atlasReadEnabled;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }
        
        if (atlasMipmap != GUILayout.Toggle(atlasMipmap, "检查图集Mipmap", GUILayout.Width(150)))
        {
            atlasMipmap = !atlasMipmap;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }
        if (atlasMipmap)
        {
            if (atlasMipmapOpen != GUILayout.Toggle(atlasMipmapOpen, "勾选检查开启，不勾选检查关闭", GUILayout.Width(200)))
            {
                atlasMipmapOpen = !atlasMipmapOpen;
                TextureCheck.FileCheckIsStandard(selectionObject);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if (atlasSize != GUILayout.Toggle(atlasSize, "错误的图集尺寸大小", GUILayout.Width(200)))
        {
            atlasSize = !atlasSize;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }

        if (atlasPlatform != GUILayout.Toggle(atlasPlatform, "错误的图集平台格式", GUILayout.Width(150)))
        {
            atlasPlatform = !atlasPlatform;
            TextureCheck.FileCheckIsStandard(selectionObject);
        }

        if (atlasPlatform)
        {
            if (atlasRGBA != GUILayout.Toggle(atlasRGBA, "不检查图集RGBA16和32", GUILayout.Width(200)))
            {
                atlasRGBA = !atlasRGBA;
                TextureCheck.FileCheckIsStandard(selectionObject);
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(15);
        GUILayout.Label("---快捷操作---");
        if(GUILayout.Button("全部改正",GUILayout.Width(200)))
        {
            //TextureCheck.FileAutoSetStandard(selectionObject);   忽略的列表也会改正
            //对全部错误类型进行改正
            for (int i = 0; i < errorList.Count;)
            {
                //判断错误是否可改正
                if (CorrectionError(textureErrorTypesList[i],texPathAndCountList[pathIndexList[i]].path))
                {
                    TexPathAndCount texPathAndCount;
                    texPathAndCount.count = texPathAndCountList[pathIndexList[i]].count-1;
                    texPathAndCount.path = texPathAndCountList[pathIndexList[i]].path;
                    texPathAndCountList[pathIndexList[i]] = texPathAndCount;
                    pathIndexList.RemoveAt(i);
                    errorList.RemoveAt(i);
                    textureErrorTypesList.RemoveAt(i);
                }
                //不可改正将进行提升
                else
                {
                    i++;
                }
            }
            foreach (TexPathAndCount texPathAndCount in texPathAndCountList)
            {
                AssetDatabase.ImportAsset(texPathAndCount.path);
            }
            AssetDatabase.Refresh();
        }
        if(GUILayout.Button("刷新列表",GUILayout.Width(200)))
        {
            TextureCheck.FileCheckIsStandard(selectionObject);
            Debug.Log("错误的贴图数量为："+texPathAndCountList.Count+"  错误的类型数量为:"+errorList.Count);
        }
        if(GUILayout.Button("检查文件中未打包图集的精灵",GUILayout.Width(200)))
        {
            TextureCheck.FileCheckSprite(selectionObject);
        }
        if(GUILayout.Button("检查图集里贴图AssetBundle",GUILayout.Width(200)))
        {
            TextureCheck.FileGetDependencies(selectionObject);
        }
        if(GUILayout.Button("将选择文件添加至列表",GUILayout.Width(200)))
        {
            Object[] selectionObjects = Selection.objects;
            List<Object> objects = new List<Object>(selectionObjects);
            TextureCheckEditorWindow.selectionObject.AddRange(objects);
            TextureCheck.FileCheckIsStandard(selectionObject);
        }
        if(GUILayout.Button("检查Assets/Resources文件夹",GUILayout.Width(200)))
        {
            selectionObject = new List<Object>();
            selectionObject.Add(AssetDatabase.LoadMainAssetAtPath("Assets/Resources"));
            TextureCheck.FileCheckIsStandard(selectionObject);
        }
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(15);
        GUILayout.Label("----------------错误信息显示-------------         错误数量："+errorList.Count);
        GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
        guiStyle.alignment = TextAnchor.MiddleLeft;
        scrollPosition2 = EditorGUILayout.BeginScrollView(scrollPosition2, true,true,GUILayout.Width(1200), GUILayout.Height(600));
        
        if(errorList.Count==0)
            GUILayout.Label("当前选择下，所有贴图已符合规范");
        for (int i = 0; i < errorList.Count; i++)
        {
            GUILayout.BeginHorizontal();
            //忽略错误按钮
            if(GUILayout.Button("忽略",GUILayout.Width(50)))
            {
                TexPathAndCount texPathAndCount;
                texPathAndCount.count = texPathAndCountList[pathIndexList[i]].count-1;
                texPathAndCount.path = texPathAndCountList[pathIndexList[i]].path;
                texPathAndCountList[pathIndexList[i]] = texPathAndCount;
                pathIndexList.RemoveAt(i);
                errorList.RemoveAt(i);
                textureErrorTypesList.RemoveAt(i);
            }
            
            //错误改正按钮
            if(GUILayout.Button("改正",GUILayout.Width(50)))
            {
                //判断错误是否可改正
                if (CorrectionError(textureErrorTypesList[i],texPathAndCountList[pathIndexList[i]].path))
                {
                    AssetImporter.GetAtPath(texPathAndCountList[pathIndexList[i]].path).SaveAndReimport();
                    TexPathAndCount texPathAndCount;
                    texPathAndCount.count = texPathAndCountList[pathIndexList[i]].count-1;
                    texPathAndCount.path = texPathAndCountList[pathIndexList[i]].path;
                    texPathAndCountList[pathIndexList[i]] = texPathAndCount;
                    pathIndexList.RemoveAt(i);
                    errorList.RemoveAt(i);
                    textureErrorTypesList.RemoveAt(i);
                    //将修改进行导入
                }
                //不可改正将进行提升
                else
                {
                    EditorUtility.DisplayDialog("改正结果","该错误类型无法自动改正，请手动改正","确定");
                }
            }
            
            //错误信息按钮
            if (i < errorList.Count)
            {
                if (GUILayout.Button(new GUIContent(texPathAndCountList[pathIndexList[i]].path + "," + errorList[i], AssetDatabase.GetCachedIcon(texPathAndCountList[pathIndexList[i]].path)),guiStyle ))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<Object>(texPathAndCountList[pathIndexList[i]].path);
                    Selection.activeObject = asset;
                }
            }
            GUILayout.EndHorizontal();
            
        }
        EditorGUILayout.EndScrollView();
    }
    
    //根据错误类型进行改正
    bool CorrectionError(TextureErrorType textureErrorType,string assetPath)
    {
        SpriteAtlas spriteAtlas;
        SpriteAtlasTextureSettings spriteAtlasTextureSettings;
        TextureImporter textureImporter;
        Texture texObj;
        TextureImporterPlatformSettings textureImporterPlatformSettings;
        int platformMaxTextureSize = 0;
        TextureImporterFormat platformTextureFmt;
        int platformCompressionQuality = 0;
        bool platformAllowsAlphaSplit = false;
        bool pass = false;
        switch (textureErrorType)
        {
            //图集开启mipmap错误改正
            case TextureErrorType.SpriteAtlasGenerateMipMapOpen:
                spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath); 
                spriteAtlasTextureSettings = spriteAtlas.GetTextureSettings();
                spriteAtlasTextureSettings.generateMipMaps = false;
                spriteAtlas.SetTextureSettings(spriteAtlasTextureSettings);
                pass = true;
                break;
            case TextureErrorType.SpriteAtlasGenerateMipMapClose:
                spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath); 
                spriteAtlasTextureSettings = spriteAtlas.GetTextureSettings();
                spriteAtlasTextureSettings.generateMipMaps = true;
                spriteAtlas.SetTextureSettings(spriteAtlasTextureSettings);
                pass = true;
                break;
            //图集开启Readable错误改正
            case TextureErrorType.SpriteAtlasReadable:
                spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath); 
                spriteAtlasTextureSettings = spriteAtlas.GetTextureSettings();
                spriteAtlasTextureSettings.readable = false;
                spriteAtlas.SetTextureSettings(spriteAtlasTextureSettings);
                pass = true;
                break;
            //IOS平台设置错误改正
            case TextureErrorType.IOS:
                spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath); 
                if (null != spriteAtlas)
                {
                    textureImporterPlatformSettings =spriteAtlas.GetPlatformSettings("iPhone");
                    textureImporterPlatformSettings.format = TextureImporterFormat.ASTC_6x6;
                    textureImporterPlatformSettings.overridden = true;
                    //将改正进行更新
                    spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);
                    pass = true;
                    break;
                }
                textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                texObj = AssetDatabase.LoadAssetAtPath<Texture>(textureImporter.assetPath);
                //获取设置值
                textureImporter.GetPlatformTextureSettings("iPhone", out platformMaxTextureSize,
                    out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
                //若为_a透明通道贴图，设置为__IOSFormat_a
                if (texObj.name.Substring((texObj.name.Length - 2)>=0?(texObj.name.Length - 2):0) == "_a")
                {
                    textureImporter.SetPlatformTextureSettings("iPhone",platformMaxTextureSize,TextureImporterFormat.Alpha8,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
                //若为法线贴图，设置为__IOSHeightFormat
                if (textureImporter.textureType == TextureImporterType.NormalMap)
                {
                    textureImporter.SetPlatformTextureSettings("iPhone",platformMaxTextureSize,TextureImporterFormat.ASTC_4x4,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
                //反之为正常情况，设置为__IOSFormat
                textureImporter.SetPlatformTextureSettings("iPhone",platformMaxTextureSize,TextureImporterFormat.ASTC_6x6,platformCompressionQuality,platformAllowsAlphaSplit);
                pass = true;
                break;
            //Android平台设置错误改正
            case TextureErrorType.Android:
                spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath); 
                if (null != spriteAtlas)
                {
                    textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings("Android");
                    textureImporterPlatformSettings.format = TextureImporterFormat.ETC2_RGBA8;
                    textureImporterPlatformSettings.overridden = true;
                    //将改正进行更新
                    spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);
                    pass = true;
                    break;
                }
                textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                texObj = AssetDatabase.LoadAssetAtPath<Texture>(textureImporter.assetPath);
                //获取设置值
                textureImporter.GetPlatformTextureSettings("Android", out platformMaxTextureSize,
                    out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
                //若为_a透明通道贴图，设置为Alpha8
                if (texObj.name.Substring((texObj.name.Length - 2)>=0?(texObj.name.Length - 2):0) == "_a")
                {
                    textureImporter.SetPlatformTextureSettings("Android",platformMaxTextureSize,TextureImporterFormat.Alpha8,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
                //若为法线贴图，设置为ETC2_RGBA8
                if (textureImporter.textureType == TextureImporterType.NormalMap)
                {
                    textureImporter.SetPlatformTextureSettings("Android",platformMaxTextureSize,TextureImporterFormat.ETC2_RGBA8,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
                
                //若含有透明通道平台格式设置为ETC2_RGBA8，反之设置为ETC_RGB4
                if (textureImporter.DoesSourceTextureHaveAlpha())
                {
                    textureImporter.SetPlatformTextureSettings("Android",platformMaxTextureSize,TextureImporterFormat.ETC2_RGBA8,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
                else
                {
                    textureImporter.SetPlatformTextureSettings("Android",platformMaxTextureSize,TextureImporterFormat.ETC_RGB4,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
            //PC平台设置错误改正
            case TextureErrorType.PC:
                spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath); 
                if (null != spriteAtlas)
                {
                    textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings("Standalone");
                    textureImporterPlatformSettings.format = TextureImporterFormat.DXT5;
                    textureImporterPlatformSettings.overridden = true;
                    //将改正进行更新
                    spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);
                    pass = true;
                    break;
                }
                textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                texObj = AssetDatabase.LoadAssetAtPath<Texture>(textureImporter.assetPath);
                //获取设置值
                textureImporter.GetPlatformTextureSettings("Standalone", out platformMaxTextureSize,
                    out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
                
                //若为_a透明通道贴图，设置为Alpha8
                if (texObj.name.Substring((texObj.name.Length - 2)>=0?(texObj.name.Length - 2):0) == "_a")
                {
                    textureImporter.SetPlatformTextureSettings("Standalone",platformMaxTextureSize,TextureImporterFormat.Alpha8,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
                
                //若为法线贴图，设置为ETC2_RGBA8
                if (textureImporter.textureType == TextureImporterType.NormalMap)
                {
                    textureImporter.SetPlatformTextureSettings("Standalone",platformMaxTextureSize,TextureImporterFormat.DXT5,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
                
                //若含有透明通道平台格式设置为DXT5，反之设置为DXT5
                if (textureImporter.DoesSourceTextureHaveAlpha())
                {
                    textureImporter.SetPlatformTextureSettings("Standalone",platformMaxTextureSize,TextureImporterFormat.DXT5,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
                else
                {
                    textureImporter.SetPlatformTextureSettings("Standalone",platformMaxTextureSize,TextureImporterFormat.DXT1,platformCompressionQuality,platformAllowsAlphaSplit);
                    pass = true;
                    break;
                }
            //贴图创建mipmap错误改正
            case TextureErrorType.TextureGenerateMipmapOpen:
                textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;;
                textureImporter.mipmapEnabled = false;
                pass = true;
                break;
            case TextureErrorType.TextureGenerateMipmapClose:
                textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;;
                textureImporter.mipmapEnabled = true;
                pass = true;
                break;
            //贴图开启Readable错误改正
            case TextureErrorType.TextureReadable:
                textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;;
                textureImporter.isReadable = false;
                pass = true;
                break;
            //贴图大小错误问题，无法自动改正
            case TextureErrorType.TextureSize:
                pass = false;
                break;
            case TextureErrorType.SpriteSizeAndNoIsAtlas:
                pass = false;
                break;
            case TextureErrorType.SpriteNoIsAtlas:
                pass = false;
                break;
            case TextureErrorType.MaxSizeDifferent:
                spriteAtlas=AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath); 
                if (null != spriteAtlas)
                {
                    textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings("Standalone");
                    textureImporterPlatformSettings.maxTextureSize = 2048;
                    //将改正pc进行更新
                    spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);
                    
                    textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings("Android");
                    textureImporterPlatformSettings.maxTextureSize = 2048;
                    //将改正pc进行更新
                    spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);
                    
                    textureImporterPlatformSettings = spriteAtlas.GetPlatformSettings("iPhone");
                    textureImporterPlatformSettings.maxTextureSize = 2048;
                    //将改正pc进行更新
                    spriteAtlas.SetPlatformSettings(textureImporterPlatformSettings);

                    pass = true;
                    break;
                }
                textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                //更改ios最大尺寸为2048
                textureImporter.GetPlatformTextureSettings("iPhone", out platformMaxTextureSize,
                    out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
                textureImporter.SetPlatformTextureSettings("iPhone",2048,platformTextureFmt,platformCompressionQuality,platformAllowsAlphaSplit);
                //更改Android最大尺寸为2048
                textureImporter.GetPlatformTextureSettings("Android", out platformMaxTextureSize,
                    out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
                textureImporter.SetPlatformTextureSettings("Android",2048,platformTextureFmt,platformCompressionQuality,platformAllowsAlphaSplit);
                //更改pc最大尺寸为2048
                textureImporter.GetPlatformTextureSettings("Standalone", out platformMaxTextureSize,
                    out platformTextureFmt, out platformCompressionQuality, out platformAllowsAlphaSplit);
                textureImporter.SetPlatformTextureSettings("Standalone",2048,platformTextureFmt,platformCompressionQuality,platformAllowsAlphaSplit);
                pass = true;
                break;
            case TextureErrorType.AssetBundleError:
                pass = false;
                break;;
            default:
                pass = false;
                break;
        }
        return pass;
    }
    
    void OnFocus()
    {
    }
    
    void OnDestroy()
    {
        texMipmap = false;
        texMipmapOpen = true;
        texReadEnabled = true;
        texAlpha = true;
        texSize = true;
        texPlatform = true;
        atlasMipmap = false;
        atlasMipmapOpen = true;
        atlasReadEnabled = true;
        atlasSize = true;
        atlasPlatform = true;
        atlasRGBA = true;
        texRGBA = true;
    }
}

//错误类型枚举
public enum TextureErrorType
{
    TextureGenerateMipmapOpen,     //贴图开启了mipmap
    TextureGenerateMipmapClose,     //贴图关闭了mipmap
    TextureReadable,        //贴图开启了Readable
    TextureAlpha,   //贴图的Alpha设置错误
    SpriteAtlasGenerateMipMapOpen,  //精灵图集创建了Mipmap
    SpriteAtlasGenerateMipMapClose,  //精灵图集创建了Mipmap
    SpriteAtlasReadable,     //精灵图集开启了Readable
    IOS,    //Ios平台设置错误
    Android,    //Android平台设置错误
    PC,     //pc平台设置错误
    TextureSize,    //贴图大小错误
    SpriteSizeAndNoIsAtlas,    //精灵边长不为4的倍数且未打包错误
    SpriteNoIsAtlas,
    MaxSizeDifferent,   //平台最大尺寸不同错误
    AssetBundleError
}

public struct TexPathAndCount
{
    public string path;
    public int count;
}

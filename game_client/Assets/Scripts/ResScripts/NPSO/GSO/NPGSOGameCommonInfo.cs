using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[System.Serializable]
public class LanguageFontTypeAssetPath
{
    public ENPLanguage language;
    public List<FontTypeAssetPath> fontAssetPathList;
    [ALHeader("在fontAssetPathList列表中找不到对应的字体类型时, 使用的TMP_FontAsset")]
    public NPCommonAssetPathInfo defaultFontAssetPath;
    
    public static LanguageFontTypeAssetPath FindLanguageFontTypeAssetPath(List<LanguageFontTypeAssetPath> fontTypeAssetPathList, ENPLanguage language)
    {
        if (fontTypeAssetPathList == null)
            return null;
        
        foreach (var fontTypeAssetPath in fontTypeAssetPathList)
        {
            if (fontTypeAssetPath != null && fontTypeAssetPath.language == language)
            {
                return fontTypeAssetPath;
            }
        }

        return null;
    }
}

[Serializable]
public class FontTypeAssetPath
{
    public EFontType fontType;
    public NPCommonAssetPathInfo fontAssetPath;
    
    public static FontTypeAssetPath FindFontTypeAssetPath(List<FontTypeAssetPath> fontTypeAssetPathList, EFontType fontType)
    {
        if (fontTypeAssetPathList == null)
            return null;
        
        foreach (var fontTypeAssetPath in fontTypeAssetPathList)
        {
            if (fontTypeAssetPath != null && fontTypeAssetPath.fontType == fontType)
            {
                return fontTypeAssetPath;
            }
        }

        return null;
    }
}

/*******************
 * 平台部分登录相关信息存储对象
 **/
[System.Serializable]
public class NPGSOGameCommonInfo : ScriptableObject
{
    /** 默认场景索引信息 */
    public NPGSceneIndex mainSceneIndex;

    /** 单区域提示对象网格mono **/
    public NPMonoGridView prefab_Go;

    /** 需要加载的通用Asset路径队列，配置在这里是为了方便扩展配置*/
    public List<string> commonAssetBundlePathList;

    // Shader的置灰的默认参数
    [ALHeader("置灰变黑参数")]
    [Range(0,1)]public float grayGlobalScale = 1;
    
    [ALHeader("置灰半透明参数")]
    [Range(0,1)]public float grayGlobalAlphaScale = 0.7f;

    // Shader的置灰的默认参数
    [ALHeader("置灰去色参数")]
    [Range(0,1)]public float grayGlobalColorScale = 0.4f;
    
    // Shader的模型对话变暗的默认参数
    [ALHeader("模型对话变暗参数")]
    [Range(0,1)]public float darkGlobalScale = 0.45f;

    /** GUI Image 灰度图材质 */
    public Material guiGrayMat;

    /** GTD Sprite Render 灰图材质 */
    public Material gtdGrayMat;

    /** GTD Sprite Render 默认材质 */
    public Material gtdDefaultMat;
    
    public WCGTeamSkillPromptMono team_skill_prompt;//指挥官技能提示对象

    /** 可放置区域的材质 */
    public Material canPutMat;
    /** 不可放置区域的材质 */
    public Material cannotPutMat;
    
    public Material guiVideoMat;

    /** 默认展示的替换大图资源 */
    public Texture2D defaultTexture;

    public Color friendHitColor = Color.blue;
    public Color enemyHitColor = Color.red;
    public float hitColorChgTime = 0.05f;

    public string initFontTextStr;//初始化字体的文本内容

    [ALHeader("全局通用星的图标,数组第一个图为1阶星，移次到5阶星，共5个图")]
    public List<Sprite> starSpriteList;

    [ALHeader("如果需要替换随包的URP设置才开启这个开关")]
    public bool openReplaceUrpSetting = false;
    [ALHeader("URP替换设置")]
    public NPURPSetting urpSetting;
    [ALHeader("默认的一个值变化曲线")]
    public EaseType defaultValueEaseType = EaseType.InOutQuad;
    [ALHeader("默认的平滑阻尼时间")]
    public float defaultSmoothDampTime = 0.3f;

    [ALHeader("TextMeshPro字体资源路径列表(默认ENPLanguage.NONE的配置为默认字体资源配置)")]
    public List<LanguageFontTypeAssetPath> TMPFontTypeAssetPathList;
    
    [ALHeader("Text字体资源路径列表(默认ENPLanguage.NONE的配置为默认字体资源配置)")]
    public List<LanguageFontTypeAssetPath> textFontTypeAssetPath;

    public NPGGoIndex business_building_base_res_index;
    public NPGGoIndex farming_building_base_res_index;

    #region 第一版TextMeshPro处理方案(修改FallBack, 弃用)
    // [Header("不同语言对应的需要添加到fallback中TMPFont")]
    // public List<LanguageFont> languageAddToFallbackTMPFonts;
    #endregion
    

#if UNITY_EDITOR
    
    private Dictionary<EFontType, Font> _m_dTextFontLocalAssetDic = new Dictionary<EFontType, Font>();

    private bool _m_bIsLocalFontDicInit = false;

    private void _initLocalFontDic()
    {
        if (_m_bIsLocalFontDicInit)
            return;
        
        _m_bIsLocalFontDicInit = true;
        LanguageFontTypeAssetPath fontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(textFontTypeAssetPath, ENPLanguage.NONE);

        foreach (EFontType enumFontType in Enum.GetValues(typeof(EFontType)))
        {
            if (fontTypeAssetPath == null) continue;
            FontTypeAssetPath typePath =
                FontTypeAssetPath.FindFontTypeAssetPath(fontTypeAssetPath.fontAssetPathList, enumFontType);
            if (typePath == null || typePath.fontAssetPath == null) continue;
            Font font = NPResUtil.loadLocalRes<Font>(typePath.fontAssetPath.asset_path, System.IO.Path.GetFileNameWithoutExtension(typePath.fontAssetPath.obj_name),System.IO.Path.GetExtension(typePath.fontAssetPath.obj_name));
            if (_m_dTextFontLocalAssetDic != null) _m_dTextFontLocalAssetDic[enumFontType] = font;
        }
    }
    /// <summary>
    ///  获取通用配置上配置的None使用的字体
    /// </summary>
    /// <param name="_fontType"></param>
    /// <param name="_font"></param>
    /// <returns></returns>
    public bool tryGetNoneSetLocalFontByType(EFontType _fontType, out Font _font)
    {
        _font = null;
        _initLocalFontDic();
        if (_m_dTextFontLocalAssetDic != null) 
            return _m_dTextFontLocalAssetDic.TryGetValue(_fontType, out _font);
        return false;
    }

    /// <summary>
    /// 参数有变化的时候刷新一下全局参数，方便预览
    /// </summary>
    private void OnValidate()
    {
        _m_bIsLocalFontDicInit = false;
        Shader.SetGlobalFloat(ShaderPropertyMgr.g_GrayScale, grayGlobalScale);
        Shader.SetGlobalFloat(ShaderPropertyMgr.g_GrayColorScale, grayGlobalColorScale);
        Shader.SetGlobalFloat(ShaderPropertyMgr.g_GrayAlphaScale, grayGlobalAlphaScale);
        // Shader.SetGlobalFloat(ShaderPropertyMgr.g_DarkScale, darkGlobalScale);
    }
#endif
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "settings/common_info.unity3d"; } }
    public static string objName { get { return "game_common_info"; } }
}

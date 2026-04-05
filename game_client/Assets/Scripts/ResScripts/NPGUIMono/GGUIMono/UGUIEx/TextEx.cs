using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

using ALPackage;
using JetBrains.Annotations;
using GOE;


public interface _ITextMono
{
    /// <summary>
    /// 字体类型
    /// </summary>
    public EFontType fontType { get; }

    public void setFontAsset(Font _font);

    public Font fontAsset { get; }
     
    public MonoBehaviour mono { get; }
}

public class TextEx : Text, ILayoutSelfController, _ITextMono
{
    protected static readonly Regex RichTextRegex = new Regex("(<b>)|(</b>)|(<i>)|(</i>)|(<size.+?>)|(</size>)|(<color.+?>)|(</color>)|(<u.*?>)|(</u>)|(<a.+?>)|(</a>)");
    [NotNull] protected static readonly string clColorBeforeLabel = "<cl>";
    [NotNull] protected static readonly string clColorAfterLabel = "</cl>";

    public static ENPLanguage g_Lang = ENPLanguage.NONE;
    
    //用于存储翻译的文本
    private string _m_sMainText = null;
    //保存的语言
    private ENPLanguage _m_eLanguage = ENPLanguage.NONE;

    [ALHeader("是否强制单行居中对齐，多行左对齐")] 
    public bool oneLineMiddleOtherLeft = false;
    [ALHeader("是否使用BestFit扩展功能，优先按最大大小排布，超框以后再缩小")] 
    public bool isUseBestFitEx = false;

    [ALHeader("自定义颜色<cl>标签是否生效")]
    public bool cl_colorEnable;
    [ALHeader("<cl>自定义颜色")]
    public Color cl_color;
    
    [SerializeField] [HideInInspector] [ALHeader("字体类型")]
    private EFontType m_fontType;

    private string _m_sReplaceClColorBeforeLabel = "";
    private string _m_sReplaceClColorAfterLabel = "";
    
    /// <summary>
    /// 获取显示的文本，如果有子类要修改显示逻辑，重写这个类
    /// </summary>
    /// <param name="_baseString">原始文本</param>
    /// <returns></returns>
    protected virtual string _getShowText(string _baseString)
    {
        return _replaceClColor(TextTranslate.instance.getLanguage(_baseString));
    } 
    
    public override string text
    {
        get
        {
            //Prefab模式，不处理
            if (NPResUtil.isInPrefabStage())
            {
                _refreshClColorLabel();//刷新cl标签信息
                return _replaceClColor(base.text);
            }

            //非运行中不做额外处理
            if (!Application.isPlaying)
                return base.text;
            
            // 非Prefab模式, 直接连同Inspector中的值一起修改
            base.m_Text = _replaceClColor(base.text);
            
#if (UNITY_EDITOR || UNITY_STANDALONE) && NP_GAME // 只在Editor 或 pc包判断
            // 不需要翻译key, 直接返回
            if (!CharacterDetermineMgr.instance.needTranslateKeyKey)
                return string.IsNullOrEmpty(_m_sMainText) ? base.text : _m_sMainText;//返回翻译前文本
#endif

            if (_m_eLanguage == g_Lang)
            {
                //不需要翻译
                return base.text;
            }
            else
            {
                //需要翻译
                if (null == _m_sMainText)
                    _m_sMainText = base.text;

                //设置语言
                _m_eLanguage = g_Lang;

                if (string.IsNullOrEmpty(_m_sMainText))
                    return base.text;
                
                base.m_Text = _getShowText(_m_sMainText);
                
                return base.text;
            }
        }
        set
        {
            //存的这个str不改上面不会重新翻译
            _m_sMainText = value;
            base.text = value;
            _setOneLineToCenter();
        }
    }

    protected override void Awake()
    {
#if NP_GAME
        GameLanguageMgr.instance.onTextMonoAwake(this);
#endif
        
        base.Awake();

        //Prefab模式，不处理
        if (NPResUtil.isInPrefabStage())
        {
            return;
        }
        
        _refreshClColorLabel();

        //运行中不修改
        if (!Application.isPlaying)
            return;

        if (_m_eLanguage != g_Lang)
        {
            //需要翻译
            if (null == _m_sMainText)
                _m_sMainText = base.text;

            //设置语言
            _m_eLanguage = g_Lang;

            if (string.IsNullOrEmpty(_m_sMainText))
                return ;

            //开始处理翻译
            base.text = _getShowText(_m_sMainText);
        }
    }

    protected override void OnDestroy()
    {
#if NP_GAME
        GameLanguageMgr.instance.onTextMonoDestroy(this);
#endif
        
        base.OnDestroy();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _setOneLineToCenter();
    }
    
    public void SetLayoutHorizontal()
    {
        _setOneLineToCenter();
    }

    public void SetLayoutVertical()
    {
        _setOneLineToCenter();
    }
    
    public override float preferredWidth
    {
        get
        {
            var settings = GetGenerationSettings(Vector2.zero);
            return cachedTextGenerator.GetPreferredWidth(m_Text, settings) / pixelsPerUnit;
        }
    }
    public override float preferredHeight
    {
        get
        {
            var settings = GetGenerationSettings(new Vector2(GetPixelAdjustedRect().size.x, 0.0f));
            //资源勾选BestFit不处理，并且勾选扩展功能，preferredHeight单独处理
            if (isUseBestFitEx && resizeTextForBestFit)
            {
                TextGenerationSettings bestFitSettings = GetGenerationSettings(rectTransform.rect.size);
                //勾选了BestFit扩展功能，算出最合适字体大小后，再用这个字体大小计算preferredHeight
                _populateMesh_BestFitEx(bestFitSettings, false);
                
                settings.resizeTextForBestFit = false;
                settings.fontSize = _m_CurUseBestFitExFontSize;
            }
            return cachedTextGenerator.GetPreferredHeight(m_Text, settings) / pixelsPerUnit;
        }
    }
    
    /// <summary>
    /// 根据文本长度设置对齐方式
    /// </summary>
    private void _setOneLineToCenter()
    {
        if (oneLineMiddleOtherLeft)
        {
            var set = GetGenerationSettings(rectTransform.rect.size);
            //设置值的时候会带参数，这里要重新取一下实际文本
            string basStr = _getShowText(base.text);
            cachedTextGenerator.Populate(basStr, set);
            //小于1行居中对其
            if(cachedTextGenerator.lineCount <= 1)
            {
                switch (alignment)
                {
                    case TextAnchor.UpperLeft:
                    case TextAnchor.UpperCenter:
                    case TextAnchor.UpperRight:
                        alignment = TextAnchor.UpperCenter;
                        break;
                    case TextAnchor.MiddleLeft:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.MiddleRight:
                        alignment = TextAnchor.MiddleCenter;
                        break;
                    case TextAnchor.LowerLeft:
                    case TextAnchor.LowerCenter:
                    case TextAnchor.LowerRight:
                        alignment = TextAnchor.LowerCenter;
                        break;
                }
            }
            //大于1行左对齐
            else
            {
                switch (alignment)
                {
                    case TextAnchor.UpperLeft:
                    case TextAnchor.UpperCenter:
                    case TextAnchor.UpperRight:
                        alignment = TextAnchor.UpperLeft;
                        break;
                    case TextAnchor.MiddleLeft:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.MiddleRight:
                        alignment = TextAnchor.MiddleLeft;
                        break;
                    case TextAnchor.LowerLeft:
                    case TextAnchor.LowerCenter:
                    case TextAnchor.LowerRight:
                        alignment = TextAnchor.LowerLeft;
                        break;
                }
            }
        }
    }

    public override void SetVerticesDirty()
    {
        _m_bIsBestFitExDirty = true;
        base.SetVerticesDirty();
    }
    
    #region 设置BestFit特殊逻辑，超宽以后再缩小

    private bool _m_bIsBestFitExDirty = false; // 是否需要重新计算BestFitEx
    private int _m_CurUseBestFitExFontSize = 0; // 当前使用的BestFitEx计算出最合适的字体大小
    
    /// <summary>
    /// 计算BestFitEx最合适的字体大小，并生成Mesh
    /// </summary>
    /// <param name="settings"></param>
    private void _populateMesh_BestFitExAlways(TextGenerationSettings settings)
    {
        //关闭BestFit
        settings.resizeTextForBestFit = false;
        //BestFit的最小字体大小
        int minSize = resizeTextMinSize;
        //文字长度
        int txtLen = text.Length;
        //倒叙遍历按字体大小从大大小依次尝试设置
        for (int i = resizeTextMaxSize; i >= minSize; --i)
        {
            settings.fontSize = i;
            cachedTextGenerator.PopulateWithErrors(text, settings, gameObject);
            //已生成并包含在可见行中的字符数 == 原本的文字长度，就说明没被裁剪，用这个字体大小显示
            if (cachedTextGenerator.characterCountVisible == txtLen)
            {
                _m_CurUseBestFitExFontSize = i;
                break;
            }
        }
    }
    
    /// <summary>
    /// 判断是否需要重新计算BestFitEx，如果需要则重新计算，根据参数_alwaysPopulate决定是否始终重新生成Mesh
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="_alwaysPopulate">如果为ture，则始终重新生成Mesh</param>
    private void _populateMesh_BestFitEx(TextGenerationSettings settings, bool _alwaysPopulate = true)
    {
        if (!_m_bIsBestFitExDirty)
        {
            if (_alwaysPopulate)
            {
                settings.fontSize = _m_CurUseBestFitExFontSize;
                settings.resizeTextForBestFit = false;
                cachedTextGenerator.PopulateWithErrors(text, settings, gameObject);
            }
            return;
        }
        _populateMesh_BestFitExAlways(settings);
        _m_bIsBestFitExDirty = false;
    }
    
    #endregion
    
    [NotNull]private readonly UIVertex[] _m_tmpVerts = new UIVertex[4];
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        //资源没勾选BestFit不处理，没勾选扩展功能也不处理，按基类UGUI源码逻辑处理
        if (!resizeTextForBestFit || !isUseBestFitEx)
        {
            base.OnPopulateMesh(toFill);
            return;
        }
        
        if (null == font || null == rectTransform) 
            return;

        m_DisableFontTextureRebuiltCallback = true;

        #region 设置BestFit特殊逻辑，超宽以后再缩小
        
        TextGenerationSettings settings = GetGenerationSettings(rectTransform.rect.size);
        //资源没勾选BestFit不处理
        if (!resizeTextForBestFit)
        {
            cachedTextGenerator.PopulateWithErrors(text, settings, gameObject);
            return;
        }

        _populateMesh_BestFitEx(settings);

        #endregion

        //TODO 下面代码的逻辑参考unity2020.3.42f1版本UGUI源码修改，后续如果版本升级ugui源码更新后可能会异常，需要同步维护修改
        
        // Apply the offset to the vertices
        IList<UIVertex> verts = cachedTextGenerator.verts;
        float unitsPerPixel = 1 / pixelsPerUnit;
        int vertCount = verts.Count;

        // We have no verts to process just return (case 1037923)
        if (vertCount <= 0)
        {
            toFill.Clear();
            return;
        }

        Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
        roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
        toFill.Clear();
        if (roundingOffset != Vector2.zero)
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                _m_tmpVerts[tempVertsIndex] = verts[i];
                _m_tmpVerts[tempVertsIndex].position *= unitsPerPixel;
                _m_tmpVerts[tempVertsIndex].position.x += roundingOffset.x;
                _m_tmpVerts[tempVertsIndex].position.y += roundingOffset.y;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(_m_tmpVerts);
            }
        }
        else
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                _m_tmpVerts[tempVertsIndex] = verts[i];
                _m_tmpVerts[tempVertsIndex].position *= unitsPerPixel;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(_m_tmpVerts);
            }
        }

        m_DisableFontTextureRebuiltCallback = false;
    }

    private void _refreshClColorLabel()
    {
        _m_sReplaceClColorBeforeLabel = cl_colorEnable ? $"<color=#{ColorUtility.ToHtmlStringRGBA(cl_color)}>" : "";
        _m_sReplaceClColorAfterLabel = cl_colorEnable ? "</color>" : "";
    }
    
    /// <summary>
    /// 替换<cl>标签
    /// </summary>
    /// <param name="_text"></param>
    /// <returns></returns>
    protected string _replaceClColor(string _text)
    {
        if (string.IsNullOrEmpty(_text))
            return _text;
        
        _text = _text.Replace(clColorBeforeLabel, _m_sReplaceClColorBeforeLabel);
        _text = _text.Replace(clColorAfterLabel, _m_sReplaceClColorAfterLabel);

        return _text;
    }

    public EFontType fontType
    {
        get
        {
            return m_fontType;
        }
        // set
        // {
        //     m_fontType = value;
        //     m_havePropertiesChanged = true; SetVerticesDirty();
        // }
    }
    public void setFontAsset(Font _font)
    {
        font = _font;
    }

    public Font fontAsset { get => font; }
    public MonoBehaviour mono { get => this; }
}
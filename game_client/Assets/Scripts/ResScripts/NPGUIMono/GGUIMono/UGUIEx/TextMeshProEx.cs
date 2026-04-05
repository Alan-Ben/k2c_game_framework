
using System;
using ALPackage;
using GOE;
using TMPro;
using UnityEngine;

public interface _ITextMeshProMono
{
    /// <summary>
    /// 字体类型
    /// </summary>
    public EFontType fontType { get; }

    /// <summary>
    /// 设置TMP_FontAsset
    /// </summary>
    /// <param name="_fontAsset"></param>
    public void setTMP_FontAsset(TMP_FontAsset _fontAsset, Action<string, Action<Material>> _getFontMaterial);
        
    public TMP_FontAsset TMPFontAsset { get; }

    public MonoBehaviour mono { get; }
}

public class TextMeshProEx : TextMeshPro, _ITextMeshProMono
{
    //用于存储翻译的文本
    private string _m_sMainText = null;
    //保存的语言
    private ENPLanguage _m_eLanguage = ENPLanguage.NONE;
    
    [SerializeField]
    private string _m_sInitMaterialTag;//初始化时的材质tag(这个一般来说是由配置的FontType决定的, 切换语言不会导致所需材质变化, 所以在游戏运行时是不会改变的)

    public new string text
    {
        get
        {
            //使用本地资源加载不修改文本，否则会导致资源错乱
            if (ALLocalResLoaderMgr.instance.isLoadUIFromLocal)
                return base.text;

            //没有运行的时候，不翻译
            if(!Application.isPlaying)
            {
                return base.text;
            }
            
#if (UNITY_EDITOR || UNITY_STANDALONE) && NP_GAME // 只在Editor 或 pc包判断
            // 不需要翻译key, 直接返回
            if (!CharacterDetermineMgr.instance.needTranslateKeyKey)
                return string.IsNullOrEmpty(_m_sMainText) ? base.text : _m_sMainText;//返回翻译前文本
#endif
        
            if (_m_eLanguage == TextEx.g_Lang)
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
                _m_eLanguage = TextEx.g_Lang;

                if (string.IsNullOrEmpty(_m_sMainText))
                    return base.text;

                //开始处理翻译, '#1'
                if (_m_sMainText.Length >= 2 && _m_sMainText[0].Equals('#'))
                {
                    base.m_text = TextTranslate.instance.getLanguage(_m_sMainText);
                }

                return base.text;
            }
        }
        set
        {
            base.text = value;
        }
    }
    
    [SerializeField] private EFontType _m_fontType;
    public EFontType fontType
    {
        get
        {
            return _m_fontType;
        }
        // set
        // {
        //     _m_fontType = value;
        //     m_havePropertiesChanged = true; SetVerticesDirty();
        // }
    }

    protected override void Awake()
    {
#if NP_GAME
        GameLanguageMgr.instance.onTextMeshProAwake(this);
#endif

        base.Awake();

        onLanguageChange();
    }

    protected override void OnDestroy()
    {
#if NP_GAME
        GameLanguageMgr.instance.onTextMeshProDestroy(this);
#endif
        base.OnDestroy();
    }

    public void onLanguageChange()
    {
        //使用本地资源加载不修改文本，否则会导致资源错乱
//        if (ALLocalResLoaderMgr.instance.isLoadUIFromLocal)
//            return;

        //Prefab模式，不处理
        if(NPResUtil.isInPrefabStage())
        {
            return;
        }
  
        //没有运行的时候，不翻译
        if(!Application.isPlaying)
        {
            return;
        }
        
        if (_m_eLanguage != TextEx.g_Lang)
        {
            //需要翻译
            if (null == _m_sMainText)
                _m_sMainText = base.text;

            //设置语言
            _m_eLanguage = TextEx.g_Lang;

            if (string.IsNullOrEmpty(_m_sMainText))
                return ;

            //开始处理翻译
            if(_m_sMainText.Length >= 2 && _m_sMainText[0].Equals('#'))
            {
                base.text = TextTranslate.instance.getLanguage(_m_sMainText);
            }
        }
    }

    /// <summary>
    /// 设置TMP_FontAsset
    /// </summary>
    /// <param name="_fontAsset"></param>
    public void setTMP_FontAsset(TMP_FontAsset _fontAsset, Action<string, Action<Material>> _getFontMaterial)
    {
        font = _fontAsset;
        
        //设置_fontAsset时会导致Material的变化, 所以这里需要重新设置一下
        _getFontMaterial?.Invoke(_m_sInitMaterialTag, (_material) =>
        {
            if (_material != null)
            {
                fontMaterial = _material;//直接使用fontMaterial设置材质, 不会创建新的材质实例
            }
        });
        
        // TextMeshPro的ReBuild方法和Text不太一样, 在ReBuild中不会主动调用text的get方法重新获取文本, 所以这里需要手动调用一下
        onLanguageChange();
    }

    public TMP_FontAsset TMPFontAsset => font;

    public MonoBehaviour mono => this;
}

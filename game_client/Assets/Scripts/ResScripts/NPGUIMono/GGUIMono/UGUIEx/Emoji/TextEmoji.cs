using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CEmoji;
using GOE;


public class TextEmoji : Text, ILayoutSelfController, _ITextMono
{
    #region Emoji参数
    [SerializeField]
    private EmojiAtlas m_emojiAtlas;
        
    private bool m_bNeedTranslateEmoji = true;

    private string m_EmojiText;      // 原始的emoji的文本
    private string m_strTransText;      // emoji转换成privateCode之后的串
    
    public EmojiAtlas emojiAtlals
    {
        get
        {
            return m_emojiAtlas;
        }
        set
        {
            m_emojiAtlas = value;
            if (m_emojiAtlas != null)
            {
                m_emojiAtlas.init();
            }
        }
    }
    
    private EmojiImage m_emojiImage;

    #endregion
    
    [SerializeField] [HideInInspector] [ALHeader("字体类型")]
    private EFontType m_fontType;

    protected override void Awake()
    {
#if NP_GAME
        GameLanguageMgr.instance.onTextMonoAwake(this);
#endif
        base.Awake();
        
        // EmojiImage改为运行时创建，Editor下不显示
        if (Application.isPlaying && m_emojiImage == null)
        {
            Transform imgTrans = transform.Find("EmojiImage");
            if (imgTrans == null)
            {
                GameObject go = new GameObject("EmojiImage", new []{typeof(RectTransform), typeof(CanvasRenderer), typeof(EmojiImage)});
                imgTrans = go.transform;
                imgTrans.SetParent(transform, false);
            }
            m_emojiImage = imgTrans.GetComponent<EmojiImage>();
            if (m_emojiImage == null)
            {
                m_emojiImage = imgTrans.gameObject.AddComponent<EmojiImage>();
            }

            if (m_emojiImage != null && m_emojiImage.emojiAtlals != m_emojiAtlas)
            {
                m_emojiImage.emojiAtlals = m_emojiAtlas;
                m_emojiImage.maskable = maskable;
            }

            if (m_emojiImage != null)
            {
                m_emojiImage.rectTransform.pivot = rectTransform.pivot;
                m_emojiImage.rectTransform.anchorMin = Vector2.zero;
                m_emojiImage.rectTransform.anchorMax = Vector2.one;
            }
        }
    }

    protected override void OnDestroy()
    {
#if NP_GAME
        GameLanguageMgr.instance.onTextMonoDestroy(this);
#endif
        base.OnDestroy();
        
        if (m_emojiImage != null)
        {
            DestroyImmediate(m_emojiImage.gameObject);
            m_emojiImage = null;
        }
    }

    public override string text
    {
        get
        {
            // 这里因为外部会通过这个接口获取字符串，改为直接返回m_Emoji文本，不用考虑对应关系问题
            return m_EmojiText;
        }
        set
        {
            string str = value;

            //emoji
            if(m_emojiAtlas != null)
            {
                if(m_bNeedTranslateEmoji)
                {
                    m_EmojiText = str;
                    m_strTransText = m_emojiAtlas.Translate(m_EmojiText);
                    str = m_strTransText;
                }
                //m_Text存的是转为Quad后的字符串
                //m_strTransText存的是翻译成privateCode后的字符串
                str = EmojiAtlas.ReplaceEmojiToSpace(str);
            }

            base.text = str;
        }
    }

    #region Emoji相关处理

    
    // 提供给InputField使用，InputField内部会做emoji解析，因此直接将结果传过来，节省计算
    public void SetEmojiText(string emojiText, string strTranslatedText, string strReplaced)
    {
        m_EmojiText = emojiText;
        m_strTransText = strTranslatedText;
        
        m_bNeedTranslateEmoji = false;
        text = strReplaced;
        m_bNeedTranslateEmoji = true;
    }


    #endregion

    readonly UIVertex[] m_TempVerts = new UIVertex[4];
    readonly UIVertex[] m_TempEmojiVerts = new UIVertex[4];
        
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if (font == null)
        {
            if (m_emojiImage != null)
            {
                m_emojiImage.startMeshGeneration();
                m_emojiImage.endMeshGeneration();
            }
            return;
        }

        // We don't care if we the font Texture changes while we are doing our Update.
        // The end result of cachedTextGenerator will be valid for this instance.
        // Otherwise we can get issues like Case 619238.
        m_DisableFontTextureRebuiltCallback = true;

        Vector2 extents = rectTransform.rect.size;

        var settings = GetGenerationSettings(extents);
        cachedTextGenerator.PopulateWithErrors(m_Text, settings, gameObject);

        // Apply the offset to the vertices
        IList<UIVertex> verts = cachedTextGenerator.verts;
        float unitsPerPixel = 1 / pixelsPerUnit;
        int vertCount = verts.Count;

        // We have no verts to process just return (case 1037923)
        if (vertCount <= 0)
        {
            toFill.Clear();
            if (m_emojiImage != null)
            {
                m_emojiImage.startMeshGeneration();
                m_emojiImage.endMeshGeneration();
            }
            return;
        }


        Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
        roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
        toFill.Clear();
        
        if (m_emojiImage != null)
        {
            m_emojiImage.startMeshGeneration();
        }
        if (roundingOffset != Vector2.zero)
        {
            for (int i = 0; i < vertCount; ++i)
            {
                int tempVertsIndex = i & 3;
                m_TempVerts[tempVertsIndex] = verts[i];
                m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                if (tempVertsIndex == 3)
                    toFill.AddUIVertexQuad(m_TempVerts);
            }
        }
        else
        {
            // EMJSPACE = '\u2001'这个字符相比中文最下边距要高一点，手动往上偏移，0.11f这个值是自己测的
            float yOffset = fontSize * 0.11f;
            // emoji和隔壁字符留一点空隙 不要太挤
            float emjOffset = fontSize * 0.03f;
            float emojiSize = fontSize - emjOffset * 2;
            // 2是'\u2001'这个字符生成四边形的固定大小
            float quadSize = 2;

            // -quadSize是为了归一坐标，用emojiSize来计算四边形位置
            Vector4 posOffset = new Vector4(emjOffset, emjOffset -yOffset, emojiSize -quadSize, emojiSize - yOffset -quadSize);
            
            for (int i = 0; i < vertCount; i += 4)
            {
                int characterIndex = i / 4;
                
                if (m_emojiAtlas!=null && m_emojiAtlas.TryGetEmojiUV(m_strTransText, characterIndex, out Vector2[] emojiUV))
                {
                    m_TempEmojiVerts[0] = verts[i + 0];
                    m_TempEmojiVerts[1] = verts[i + 1];
                    m_TempEmojiVerts[2] = verts[i + 2];
                    m_TempEmojiVerts[3] = verts[i + 3];

                 
                    m_TempEmojiVerts[0].position += new Vector3(posOffset.x, posOffset.w, 0);
                    m_TempEmojiVerts[1].position += new Vector3(posOffset.z, posOffset.w, 0);
                    m_TempEmojiVerts[2].position += new Vector3(posOffset.z, posOffset.y, 0);
                    m_TempEmojiVerts[3].position += new Vector3(posOffset.x, posOffset.y, 0);
                    
                    m_TempEmojiVerts[0].position *= unitsPerPixel;
                    m_TempEmojiVerts[1].position *= unitsPerPixel;
                    m_TempEmojiVerts[2].position *= unitsPerPixel;
                    m_TempEmojiVerts[3].position *= unitsPerPixel;
                            
                    m_TempEmojiVerts[0].color = Color.white;
                    m_TempEmojiVerts[1].color = Color.white;
                    m_TempEmojiVerts[2].color = Color.white;
                    m_TempEmojiVerts[3].color = Color.white;

                    m_TempEmojiVerts[0].uv0 = emojiUV[0];
                    m_TempEmojiVerts[1].uv0 = emojiUV[1];
                    m_TempEmojiVerts[2].uv0 = emojiUV[2];
                    m_TempEmojiVerts[3].uv0 = emojiUV[3];
                    if (m_emojiImage != null) m_emojiImage.addUIVertexQuad(m_TempEmojiVerts);
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_TempVerts[j] = verts[i + j];
                        m_TempVerts[j].position *= unitsPerPixel;
                    }
                
                    toFill.AddUIVertexQuad(m_TempVerts);
                }
            }
        }
        if (m_emojiImage != null) m_emojiImage.endMeshGeneration();

        m_DisableFontTextureRebuiltCallback = false;
    }


    public EFontType fontType => m_fontType;

    public void setFontAsset(Font _font)
    {
        font = _font;
    }

    public Font fontAsset { get => font; }
    public MonoBehaviour mono { get => this; }
    
    //补充ILayoutSelfController接口，参考TextEx的情况，避免Enable的时候判断TextEmoji没有ILayoutController便不刷新LayerOut, TextSizeFixMonoV2的enable后触发，所以有这个脚本也没用
    public void SetLayoutHorizontal()
    {
    }

    public void SetLayoutVertical()
    {
    }
}
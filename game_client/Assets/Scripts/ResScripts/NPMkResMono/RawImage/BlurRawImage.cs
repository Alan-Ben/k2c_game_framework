
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class BlurRawImage : RawImage
{
    public static readonly int g_DownSampleValue = Shader.PropertyToID("_DownSampleValue");
    public static readonly int g_BlurColor = Shader.PropertyToID("_BlurColor");
    
    //模糊扩散度
    [SerializeField, Range(0.0f, 20.0f), Header("[模糊扩散度]进行高斯模糊时，相邻像素点的间隔。此值越大相邻像素间隔越远，图像越模糊。但过大的值会导致失真。")]
    public float blurSpreadSize = 1f;

    [SerializeField, Range(1, 8), Header("[迭代次数]此值越大,则模糊操作的迭代次数越多，模糊效果越好，但消耗越大。")]
    public int blurIterations = 2;

    [SerializeField, Header("模糊材质")]
    public Material blurMaterial;

    [SerializeField, Header("是否要开启降采样，如果是RenderTexture直接变小的就不需要，直接相机则需要")]
    public bool needDownSample = true;

    [SerializeField, Range(0, 6), Header("[降采样次数]向下采样的次数。此值越大,则采样间隔越大,需要处理的像素点越少,运行速度越快。")]
    public int downSampleNum = 3;
    
    public Color blurColor = Color.white;
    
    [SerializeField]
    private bool _m_bOpenBlur = false;

    private bool _m_bHasOpenBlur = false;
    private RenderTexture _m_rtBlurTexture = null;
    
    public bool openBlur
    {
        get { return _m_bOpenBlur; }
        set
        {
            _m_bOpenBlur = value;
            _openBlur();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _openBlur();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _closeBlur();
    }

    public void refreshBlur()
    {
        _openBlur();
    }
    
    private void _openBlur()
    {
        _closeBlur();
        if (_m_bOpenBlur)
        {
            _m_bHasOpenBlur = true;
            if (texture != null && blurMaterial != null)
            {
                int renderWidth = needDownSample ? texture.width >> downSampleNum: texture.width;
                int renderHeight = needDownSample ? texture.height >> downSampleNum:texture.height;
                RenderTextureDescriptor rtDesc = new RenderTextureDescriptor(renderWidth, renderHeight);
            
                float widthMod = 1.0f / (1.0f * (needDownSample ? 1 << downSampleNum: 1 ));
                //Shader的降采样参数赋值
                blurMaterial.SetFloat(g_DownSampleValue, blurSpreadSize * widthMod);
                blurMaterial.SetColor(g_BlurColor, blurColor);
            
                _m_rtBlurTexture = RenderTexture.GetTemporary(rtDesc);
                Graphics.Blit(texture, _m_rtBlurTexture, blurMaterial, 0);

                RenderTexture blurTexture = RenderTexture.GetTemporary(rtDesc);

                for (int i = 0; i < blurIterations; i++)
                {
                    //迭代偏移量参数
                    float iterationOffs = (i * 1.0f);

                    //Shader的降采样参数赋值
                    blurMaterial.SetFloat(g_DownSampleValue, blurSpreadSize * widthMod + iterationOffs);
                    Graphics.Blit(_m_rtBlurTexture, blurTexture, blurMaterial, 1);

                    Graphics.Blit(blurTexture, _m_rtBlurTexture, blurMaterial, 2);
                }

                RenderTexture.ReleaseTemporary(blurTexture);
                UpdateMaterial();
            }
        }
    }

    private void _closeBlur()
    {
        _m_bHasOpenBlur = false;
        if (_m_rtBlurTexture != null)
        {
            RenderTexture.ReleaseTemporary(_m_rtBlurTexture);
            _m_rtBlurTexture = null;
        }
        UpdateMaterial();
    }
    
    /// <summary>
    /// Returns the texture used to draw this Graphic.
    /// </summary>
    public override Texture mainTexture
    {
        get
        {
            if(_m_bHasOpenBlur)
                return _m_rtBlurTexture;
            return base.mainTexture;
        }
    }
}

using UnityEngine;


public class UIScreenEffectParamSet : MonoBehaviour
{
    public static readonly int _Distortion_Params1 = Shader.PropertyToID("_Distortion_Params1");
    public static readonly int _Distortion_Params2 = Shader.PropertyToID("_Distortion_Params2");
    public static readonly int _SkewAmount = Shader.PropertyToID("_SkewAmount");
    public static readonly int _SkewCenter = Shader.PropertyToID("_SkewCenter");
    public static readonly int _MaskIntensity = Shader.PropertyToID("_MaskIntensity");
    public static readonly int _RadialBlurIntensityID = Shader.PropertyToID("_RadialBlurIntensity");
    public static readonly int _RadialBlurCenterID = Shader.PropertyToID("_RadialBlurCenter");
    public static readonly int _RadialBlurRadiusID = Shader.PropertyToID("_RadialBlurRadius");
    public static readonly int _RadialBlurIterationID = Shader.PropertyToID("_RadialBlurIteration");
    public static readonly int _RadialBlurMaskScaleID = Shader.PropertyToID("_RadialBlurMaskScale");
    public static readonly int _RadialBlurMaskHardnessID = Shader.PropertyToID("_RadialBlurMaskHardness");
    public static readonly int _RadialBlurMaskPowerID = Shader.PropertyToID("_RadialBlurMaskPower");

    [Header("镜头畸变强度")]
    [Tooltip("Total distortion amount.")]
    [Range(-1f, 1f)]
    public float intensity = 0f;

    [Tooltip("Intensity multiplier on X axis. Set it to 0 to disable distortion on this axis.")]
    [Range(0f, 1f)]
    public float xMultiplier = 1f;

    [Tooltip("Intensity multiplier on Y axis. Set it to 0 to disable distortion on this axis.")]
    [Range(0f, 1f)]
    public float yMultiplier = 1f;

    [Tooltip("Distortion center point. 0.5,0.5 is center of the screen")]
    public Vector2 center = new Vector2(0.5f, 0.5f);

    [Tooltip("Controls global screen scaling for the distortion effect. Use this to hide screen borders when using high \"Intensity.\"")]
    [Range(0.01f, 5f)]
    public float scale = 1f;

    [Header("透视斜切")]
    [Tooltip("透视斜切强度。> 0 右侧收窄，< 0 左侧收窄，0 为关闭。")]
    public float skewAmount = 0f;

    [Tooltip("斜切基准中心点 (X: 水平, Y: 垂直)，范围 0~1，默认 (0.5, 0.5)")]
    public Vector2 skewCenter = new Vector2(0.5f, 0.5f);

    [Header("速度线遮罩强度")]
    [Tooltip("速度线遮罩强度，0 为完全透明，1 为完全不透明")]
    [Range(0f, 1f)]
    public float maskIntensity = 0f;

    [Header("径向模糊")]
    [Tooltip("径向模糊强度，0 为关闭，1 为最强")]
    [Range(0f, 1f)]
    public float radialBlurIntensity = 0f;

    [Tooltip("径向模糊中心点 (0~1)，默认 (0.5, 0.5) 为屏幕中心")]
    public Vector2 radialBlurCenter = new Vector2(0.5f, 0.5f);

    [Tooltip("径向模糊半径")]
    [Range(0f, 1f)]
    public float radialBlurRadius = 0.1f;

    [Tooltip("径向模糊迭代次数，值越大越平滑但越耗性能")]
    [Range(1f, 12f)]
    public float radialBlurIteration = 10f;

    [Tooltip("径向模糊遮罩范围")]
    [Range(0f, 2f)]
    public float radialBlurMaskScale = 1f;

    [Tooltip("径向模糊遮罩硬度")]
    [Range(0f, 1f)]
    public float radialBlurMaskHardness = 0f;

    [Tooltip("径向模糊遮罩对比度")]
    public float radialBlurMaskPower = 5f;

    public Material material;
    public UnityEngine.UI.Graphic graphic;
    private Material _m_material;

    // Start is called before the first frame update
    void Start()
    {
        if (material != null)
        {
            _m_material = Instantiate(material);
        }
        if (graphic != null)
        {
            graphic.material = _m_material;
        }
    }

    private void OnDestroy()
    {
        if (_m_material != null)
        {
            Destroy(_m_material);
            _m_material = null;
        }
    }

    private void OnValidate()
    {
        SetupLensDistortion(_m_material == null ? material : _m_material);
        SetupSkew(_m_material == null ? material : _m_material);
        SetupMaskIntensity(_m_material == null ? material : _m_material);
        SetupRadialBlur(_m_material == null ? material : _m_material);
    }

    // Update is called once per frame
    void Update()
    {
        SetupLensDistortion(_m_material);
        SetupSkew(_m_material);
        SetupMaskIntensity(_m_material);
        SetupRadialBlur(_m_material);
    }

    void SetupLensDistortion(Material _mat)
    {
        if(_mat == null)
            return;
        if (intensity == 0f)
        {
            _mat.DisableKeyword("_DISTORTION");
            return;
        }
        _mat.EnableKeyword("_DISTORTION");
        float amount = 1.6f * Mathf.Max(Mathf.Abs(intensity * 100f), 1f);
        float theta = Mathf.Deg2Rad * Mathf.Min(160f, amount);
        float sigma = 2f * Mathf.Tan(theta * 0.5f);
        var cen = this.center * 2f - Vector2.one;
        var p1 = new Vector4(
            cen.x,
            cen.y,
            Mathf.Max(xMultiplier, 1e-4f),
            Mathf.Max(yMultiplier, 1e-4f)
        );
        var p2 = new Vector4(
            intensity >= 0f ? theta : 1f / theta,
            sigma,
            1f / scale,
            intensity * 100f
        );

        _mat.SetVector(_Distortion_Params1, p1);
        _mat.SetVector(_Distortion_Params2, p2);
    }

    void SetupSkew(Material _mat)
    {
        if (_mat == null)
            return;
        if (skewAmount == 0f)
        {
            _mat.DisableKeyword("_SKEW");
            return;
        }
        _mat.EnableKeyword("_SKEW");
        _mat.SetFloat(_SkewAmount, skewAmount);
        _mat.SetVector(_SkewCenter, new Vector4(skewCenter.x, skewCenter.y, 0f, 0f));
    }

    void SetupMaskIntensity(Material _mat)
    {
        if (_mat == null)
            return;
        _mat.SetFloat(_MaskIntensity, maskIntensity);
    }

    void SetupRadialBlur(Material _mat)
    {
        if (_mat == null)
            return;
        if (radialBlurIntensity == 0f)
        {
            _mat.DisableKeyword("_RADIAL_BLUR");
            return;
        }
        _mat.EnableKeyword("_RADIAL_BLUR");
        _mat.SetFloat(_RadialBlurIntensityID, radialBlurIntensity);
        _mat.SetVector(_RadialBlurCenterID, new Vector4(radialBlurCenter.x, radialBlurCenter.y, 0f, 0f));
        _mat.SetFloat(_RadialBlurRadiusID, radialBlurRadius);
        _mat.SetFloat(_RadialBlurIterationID, radialBlurIteration);
        _mat.SetFloat(_RadialBlurMaskScaleID, radialBlurMaskScale);
        _mat.SetFloat(_RadialBlurMaskHardnessID, radialBlurMaskHardness);
        _mat.SetFloat(_RadialBlurMaskPowerID, radialBlurMaskPower);
    }
}

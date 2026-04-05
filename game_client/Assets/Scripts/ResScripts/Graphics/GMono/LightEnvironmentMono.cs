using System;
using GOE;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LightEnvironmentMono: MonoBehaviour
{
    private static readonly int _g_gSkinShadowStrength = Shader.PropertyToID("_GSkinShadowStrength");
    private static readonly int _g_gEnvironmentReflectionContrast = Shader.PropertyToID("_GEnvironmentReflectionContrast");
    private static readonly int _g_EnvironmentColor = Shader.PropertyToID("_EnvironmentColor");
    private static readonly int _g_gDitherSize = Shader.PropertyToID("_DitherSize");

    private static readonly int _g_gShadowDitherMap = Shader.PropertyToID("_ShadowDitherMap");
    
    private static readonly int _g_customLutParams = Shader.PropertyToID("_Custom_Lut_Params");
    private static readonly int _g_customLut = Shader.PropertyToID("_CustomLut");
    
    public bool autoUpdateEnvironment = false;
    public bool autoUpdateAmbient = false;

    [ALHeader("场景灯光环境的配置脚本")]
    public AmbientMode ambientMode = AmbientMode.Skybox;
    // [ColorUsageAttribute(true, true)]public Color skyColor = new Color(253f/255, 206f/255,210f/255);
    // [ColorUsageAttribute(true, true)]public Color equatorColor = new Color(183f/255, 194f/255,216f/255);
    // [ColorUsageAttribute(true, true)]public Color groundColor;

    [ColorUsageAttribute(true, true)]public Color envColor = new Color(250f/255,250f/255,250f/255);
    public Texture skybox;
    public Texture shadowDitherMap;
    public Material skyboxMaterial;
    
    [Range(0,3)]
    public float ambientIntensity = 1f;
    [Range(0,3)]
    public float reflectionIntensity = 1f;
    [Range(0,20)]
    public float envContrast = 2.0f;

    [Range(0,1)]
    public float skinShadowStrength = 0;
    
    public float maxShadowDistance = 0;

    public bool openCustomGradingLut = false;
    public Texture colorGradingLut;

    public bool openPlanarReflect = false;
    [Header("是否开启平面阴影")]
    public bool openPlanarShadow = false;
    [Header("是否开启阴影范围优化")]
    public bool openShadowClipOverride = false;

    // [Range(0,2)]
    // public float ditherSize = 1;
    
    public SphericalHarmonicsL2 ambientProbe;
    
    public LightProbes lightProbes;
    
    private void OnEnable()
    {
        if(autoUpdateEnvironment)
            setEnvironment();
    }

    private void OnDisable()
    {
    }
    
    private void OnValidate()
    {
        if(autoUpdateEnvironment)
            setEnvironment();
        Shader.SetGlobalFloat(ShaderPropertyMgr.g_DarkScale, 1);

        if (RenderSettings.skybox != null) 
            RenderSettings.skybox.SetColor(_g_EnvironmentColor, envColor);
        // DynamicGI.UpdateEnvironment(); // 不实时，会反复，有点怪
    }
    
    public void setSphericalHarmonicsL2()
    {
        ambientProbe = RenderSettings.ambientProbe;
    }

    private void setEnvironment()
    {
        // Shader.SetGlobalColor("_GEnvironmentDiffuseColor", diffuseEnvColor);

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
        RenderSettings.customReflection = skybox;
        if(skyboxMaterial!=null)
            RenderSettings.skybox = skyboxMaterial;
        if (autoUpdateAmbient)
        {
            RenderSettings.ambientProbe = ambientProbe;
            RenderSettings.ambientMode = ambientMode;
            LightmapSettings.lightProbes = lightProbes;
        }
      
        Shader.SetGlobalColor(_g_EnvironmentColor,envColor);
        RenderSettings.ambientIntensity = ambientIntensity;
        RenderSettings.reflectionIntensity = reflectionIntensity;
        Shader.SetGlobalFloat(_g_gEnvironmentReflectionContrast,envContrast);
        // Shader.SetGlobalFloat(_g_gSkinShadowStrength, skinShadowStrength);
        // Shader.SetGlobalFloat(_g_gDitherSize, ditherSize);

        Shader.SetGlobalTexture(_g_gShadowDitherMap,shadowDitherMap);
        // DynamicGI.UpdateEnvironment();
        
#if NP_GAME
        // 镜面反射开关
        // MJUniversalRenderAPI.g_OpenPlanarReflect = openPlanarReflect;
        // MJUniversalRenderAPI.g_OpenPlanarShadow = openPlanarShadow;
        // MJUniversalRenderAPI.g_OpenShadowClipOverride = openShadowClipOverride;
#endif
        
        if(openCustomGradingLut && colorGradingLut != null)
        {
#if NP_GAME
            // MJUniversalRenderAPI.g_IsHdrEnabled = false;
#endif
        }
        else
        {
#if NP_GAME
            // MJUniversalRenderAPI.g_IsHdrEnabled = true;
#endif
        }
    }
}
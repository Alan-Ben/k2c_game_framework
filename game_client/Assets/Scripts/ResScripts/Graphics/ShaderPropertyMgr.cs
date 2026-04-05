
using UnityEngine;


namespace GOE
{
	/// <summary>
	/// 此类存放所有ShaderPropertyId，方便统一管理
	/// </summary>
	public class ShaderPropertyMgr
	{
		//基础属性
		public static readonly int g_BaseMap = Shader.PropertyToID("_BaseMap");
		
		public static int g_IVideoTextureId = Shader.PropertyToID("_VideoTex");
		public static int g_IVideoTextureST = Shader.PropertyToID("_VideoTex_ST");
		
		public static readonly int g_DownSampleValue = Shader.PropertyToID("_DownSampleValue");

		//风的相关的属性
	    public static readonly int g_WindSpeed = Shader.PropertyToID("_WindSpeed");
	    public static readonly int g_WindParams = Shader.PropertyToID("_WindParams");
	    public static readonly int g_WindStrength = Shader.PropertyToID("_WindStrength");
	    public static readonly int g_WindDir = Shader.PropertyToID("_WindDir");
	    public static readonly int g_WindTex = Shader.PropertyToID("_WindTex");
    
	    // 平面面片光
	    public static readonly int g_ScreenLightsTex = Shader.PropertyToID("_ScreenLightsTex");
	    public static readonly int g_ScreenLightsIntensity = Shader.PropertyToID("_ScreenLightsIntensity");

	    // 用于环境反射效果的MatCap
	    public static readonly int g_MatCapMap = Shader.PropertyToID("_MatCapMap");
	    public static readonly int g_MatCapIntensity = Shader.PropertyToID("_MatCapIntensity");
    
	    // 菲涅尔效果参数
	    public static readonly int g_FresnelIntensity = Shader.PropertyToID("_FresnelIntensity");
	    public static readonly int g_FresnelColor = Shader.PropertyToID("_FresnelColor");
	    
	    public static readonly int g_BlurTexture       = Shader.PropertyToID("_BlurTexture");
	    public static readonly int g_BlurTexture2       = Shader.PropertyToID("_BlurTexture2");
    
    
	    public static readonly int g_GrayScale = Shader.PropertyToID("_GrayScale");
	    public static readonly int g_GrayColorScale = Shader.PropertyToID("_GrayColorScale");
	    public static readonly int g_GrayAlphaScale = Shader.PropertyToID("_GrayAlphaScale");
	    public static readonly int g_DarkScale = Shader.PropertyToID("_DarkScale");

	    public static readonly int g_ShadowWorldPos = Shader.PropertyToID("_ShadowWorldPos");
	    public static readonly int g_PlanarShadowIntensity = Shader.PropertyToID("_PlanarShadowIntensity");
	    public static readonly int g_ShadowPlane = Shader.PropertyToID("_ShadowPlane");
	    
	    public static readonly int g_PlaneShadowMap = Shader.PropertyToID("_PlaneShadowMap");
	    public static readonly int g_PlaneShadowOffset = Shader.PropertyToID("_PlaneShadowOffset");
	    public static readonly int g_PlaneShadowScale = Shader.PropertyToID("_PlaneShadowScale");
	    public static readonly int g_DiffuseType = Shader.PropertyToID("_DiffuseType");
	    
	    public static readonly int g_Smoothness = Shader.PropertyToID("_Smoothness");
	    public static readonly int g_SkinColor = Shader.PropertyToID("_SkinColor");
	    public static readonly int g_BaseColor = Shader.PropertyToID("_BaseColor");

        public static readonly int g_OpenDark = Shader.PropertyToID("_OpenDark");

        public static readonly int g_TargetColor = Shader.PropertyToID("_TargetColor");
        public static readonly int g_TargetColorIntensity = Shader.PropertyToID("_TargetColorIntensity");

        public static readonly int g_DepthCutoff = Shader.PropertyToID("_DepthCutoff");

		// Fog
		public static readonly int g_UIFogOffset = Shader.PropertyToID("_UIFogOffset");
	    // UI
	    public static readonly int g_ClipRect = Shader.PropertyToID("_ClipRect");

	    /// <summary>
	    /// 开启指定材质的深度图写入
	    /// </summary>
	    /// <param name="_mat"></param>
	    public static void openDepthOnlyPass(Material _mat)
	    {
		    if (_mat != null)
			    _mat.SetShaderPassEnabled("DepthOnly", true);
	    }
	    /// <summary>
	    /// 关闭指定材质的深度图写入
	    /// </summary>
	    /// <param name="_mat"></param>
	    public static void closeDepthOnlyPass(Material _mat)
	    {
		    if (_mat != null)
			    _mat.SetShaderPassEnabled("DepthOnly", false);
	    }

	    /// <summary>
	    /// 开启变黑效果，传入的材质最好是实例material，而不是sharedMaterial，避免影响其他环境下的效果，或者确保退出的时候有还原设置
	    /// 需要把模型所有材质都设置一遍，如果有材质没有相应功能让Ben加
	    /// </summary>
	    /// <param name="_mat"></param>
	    /// <param name="_isOpenMask">开关</param>
	    public static void openMaskShow(Material _mat, bool _isOpenMask)
	    {
		    if(_isOpenMask)
			    openMaskShow(_mat);
		    else
			    closeMaskShow(_mat);
	    }
	    /// <summary>
	    /// 开启变黑效果，传入的材质最好是实例material，而不是sharedMaterial，避免影响其他环境下的效果，或者确保退出的时候有还原设置
	    /// 需要把模型所有材质都设置一遍，如果有材质没有相应功能让Ben加
	    /// </summary>
	    /// <param name="_mat"></param>
	    public static void openMaskShow(Material _mat)
	    {
		    if (_mat != null)
		    {
#if UNITY_EDITOR
			    _mat.SetInt("_OpenMaskShow", 1);
#endif
			    _mat.EnableKeyword("_OPEN_MASKSHOW");
		    }
	    }
	    
	    /// <summary>
	    /// 关闭变黑效果
	    /// </summary>
	    /// <param name="_mat"></param>
	    public static void closeMaskShow(Material _mat)
	    {
		    if (_mat != null)
		    {
#if UNITY_EDITOR
			    _mat.SetInt("_OpenMaskShow", 0);
#endif
			    _mat.DisableKeyword("_OPEN_MASKSHOW");
		    }
	    }
	    /// <summary>
	    /// 开启置灰效果，传入的材质最好是实例material，而不是sharedMaterial，避免影响其他环境下的效果，或者确保退出的时候有还原设置
	    /// 需要把模型所有材质都设置一遍，如果有材质没有相应功能让Ben加
	    /// </summary>
	    /// <param name="_mat"></param>
	    /// <param name="_isDark">开关</param>
	    public static void openDark(Material _mat, bool _isDark)
	    {
		    if(_isDark)
			    openDark(_mat);
		    else
			    closeDark(_mat);
	    }
	    /// <summary>
	    /// 开启置灰效果，传入的材质最好是实例material，而不是sharedMaterial，避免影响其他环境下的效果，或者确保退出的时候有还原设置
	    /// 需要把模型所有材质都设置一遍，如果有材质没有相应功能让Ben加
	    /// </summary>
	    /// <param name="_mat"></param>
	    public static void openDark(Material _mat)
	    {
            if (_mat != null)
            {
                _mat.SetFloat(g_OpenDark, 1);
#if NP_GAME
                _mat.SetFloat(g_DarkScale, GGameCommonInfo.instance.obj.darkGlobalScale);
#endif
				_mat.EnableKeyword("_OPEN_DARK");
            }
	    }

	    /// <summary>
	    /// 关闭置灰效果
	    /// </summary>
	    /// <param name="_mat"></param>
	    public static void closeDark(Material _mat)
	    {
		    if (_mat != null)
		    {
                _mat.SetFloat(g_OpenDark, 0);
                _mat.SetFloat(g_DarkScale, 1);
                _mat.DisableKeyword("_OPEN_DARK");
		    }
	    }

	    /// <summary>
	    /// 设置基础颜色贴图
	    /// </summary>
	    /// <param name="_mat"></param>
	    /// <param name="_tex"></param>
	    public static void setBaseMap(Material _mat, Texture _tex)
	    {
		    if(_mat != null)
			    _mat.SetTexture(g_BaseMap, _tex);
	    }

		/// <summary>
		/// 设置叠加颜色及强度
		/// </summary>
		/// <param name="_mat"></param>
		/// <param name="_color"></param>
		/// <param name="_value"></param>
        public static void setAddColorAndIntensity(Material _mat,Color _color, float _value)
        {
            if (_mat != null)
            {
                _mat.SetColor(g_TargetColor, _color);
				_mat.SetFloat(g_TargetColorIntensity, _value);
			}
		}

		/// <summary>
		/// 设置DepthAlphaCutoff
		/// </summary>
		/// <param name="_mat"></param>
		/// <param name="_value"></param>
		public static void setDepthAlphaCutoff(Material _mat, float _value)
        {
            if (_mat != null)
            {
                _mat.SetFloat(g_DepthCutoff, _value);
            }
        }
	}
}
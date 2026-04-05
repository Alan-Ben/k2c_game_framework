
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ToneMappingTestMono : MonoBehaviour
{
    public static readonly int _CustomToneCurve = Shader.PropertyToID("_CustomToneCurve");
    public static readonly int _ToeSegmentA = Shader.PropertyToID("_ToeSegmentA");
    public static readonly int _ToeSegmentB = Shader.PropertyToID("_ToeSegmentB");
    public static readonly int _MidSegmentA = Shader.PropertyToID("_MidSegmentA");
    public static readonly int _MidSegmentB = Shader.PropertyToID("_MidSegmentB");
    public static readonly int _ShoSegmentA = Shader.PropertyToID("_ShoSegmentA");
    public static readonly int _ShoSegmentB = Shader.PropertyToID("_ShoSegmentB");
    public static readonly int _Variants = Shader.PropertyToID("_Variants");
    
    internal static readonly int FilmSlope = Shader.PropertyToID("_FilmSlope");
    internal static readonly int FilmToe = Shader.PropertyToID("_FilmToe");
    internal static readonly int FilmShoulder = Shader.PropertyToID("_FilmShoulder");
    internal static readonly int FilmBlackClip = Shader.PropertyToID("_FilmBlackClip");
    internal static readonly int FilmWhiteClip = Shader.PropertyToID("_FilmWhiteClip");
    
    
    
    public float adapted_lum = 1.0f;
    public int toneMappingMethod =  0;
    
    [Range(0,1)]public float _m_FilmSlope = 0.91f;
    [Range(0,1)]public float _m_FilmToe = 0.53f;
    [Range(0,1)]public float _m_FilmShoulder = 0.23f;
    [Range(0,1)]public float _m_FilmBlackClip = 0f;
    [Range(0,1)]public float _m_FilmWhiteClip = 0.035f;

    [Range(0,1)]public float m_ToeStrength = 0;
    [Range(0,1)]public float m_ToeLength = 0.5f;
    [Range(0,1)]public float m_ShoulderStrength = 0;
    [Range(0,20)]public float m_ShoulderLength = 0.5f;
    [Range(0,1)]public float m_ShoulderAngle = 0;
    [Range(0.01f,5)]public float m_Gamma = 1;

    public HableCurve m_HableCurve = new HableCurve();

    private void OnValidate()
    {
           
        Shader.SetGlobalFloat("_ACES_AdaptedLum",adapted_lum);
        Shader.SetGlobalFloat("_ToneMapping_Method",toneMappingMethod);

        Shader.SetGlobalFloat(FilmSlope, _m_FilmSlope);
        Shader.SetGlobalFloat(FilmToe, _m_FilmToe);
        Shader.SetGlobalFloat(FilmShoulder, _m_FilmShoulder);
        Shader.SetGlobalFloat(FilmBlackClip, _m_FilmBlackClip);
        Shader.SetGlobalFloat(FilmWhiteClip, _m_FilmWhiteClip);
        m_HableCurve.Init(
            m_ToeStrength,
            m_ToeLength,
            m_ShoulderStrength,
            m_ShoulderLength,
            m_ShoulderAngle,
            m_Gamma
        );
        Shader.SetGlobalVector(ToneMappingTestMono._CustomToneCurve, m_HableCurve.uniforms.curve);
        Shader.SetGlobalVector(ToneMappingTestMono._ToeSegmentA, m_HableCurve.uniforms.toeSegmentA);
        Shader.SetGlobalVector(ToneMappingTestMono._ToeSegmentB, m_HableCurve.uniforms.toeSegmentB);
        Shader.SetGlobalVector(ToneMappingTestMono._MidSegmentA, m_HableCurve.uniforms.midSegmentA);
        Shader.SetGlobalVector(ToneMappingTestMono._MidSegmentB, m_HableCurve.uniforms.midSegmentB);
        Shader.SetGlobalVector(ToneMappingTestMono._ShoSegmentA, m_HableCurve.uniforms.shoSegmentA);
        Shader.SetGlobalVector(ToneMappingTestMono._ShoSegmentB, m_HableCurve.uniforms.shoSegmentB);
    }
}

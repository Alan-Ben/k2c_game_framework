using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnvironmentConfig : MonoBehaviour
{
    [ColorUsageAttribute(true, true)]public Color skyColor;
    [ColorUsageAttribute(true, true)]public Color equatorColor;
    [ColorUsageAttribute(true, true)]public Color GroundColor;

    [ColorUsageAttribute(true, true)]public Color diffuseEnvColor;
    public Texture skybox;

    [Range(0,1)]
    public float skinShadowStrength = 0;

    private static readonly int _g_gSkinShadowStrength = Shader.PropertyToID("_GSkinShadowStrength");

    private void OnValidate()
    {
        setEnvironment();
    }

    void setEnvironment()
    {
        // Shader.SetGlobalColor("_GEnvironmentDiffuseColor", diffuseEnvColor);
        RenderSettings.ambientMode = AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = skyColor * diffuseEnvColor;
        RenderSettings.ambientEquatorColor = equatorColor * diffuseEnvColor;
        RenderSettings.ambientGroundColor = GroundColor * diffuseEnvColor;
        Shader.SetGlobalFloat(_g_gSkinShadowStrength, skinShadowStrength);
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
        RenderSettings.customReflection = skybox;
    }

    private void OnEnable()
    {
        setEnvironment();
    }
}
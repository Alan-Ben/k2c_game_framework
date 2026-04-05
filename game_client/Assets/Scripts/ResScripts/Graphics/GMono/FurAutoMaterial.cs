
using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

public class FurAutoMaterial : MonoBehaviour
{
    private static readonly int _g_furOffset = Shader.PropertyToID("_FurOffset");
    private static readonly int _g_zWriteMode = Shader.PropertyToID("_ZWriteMode");
    
    private List<Material> furMaterials = new List<Material>();
    private int _m_curFurCount = 0;
    private Material _m_furMat;

    private void OnEnable()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            return;
        if (_m_furMat == null)
            _m_furMat = renderer.sharedMaterial;
        
        if (_m_furMat == null)
            return;
        int furCount = 15;
#if NP_GAME
        furCount = QualityMgr.instance.furPassCount;
#endif
        
        if(furCount == _m_curFurCount)
            return;
        
        furMaterials.Clear();
        _m_curFurCount = furCount;
        Material alphaTestMat = Instantiate(_m_furMat);
#if UNITY_EDITOR
        alphaTestMat.name = _m_furMat.name + "_alphaTest";
#endif
        alphaTestMat.SetFloat(_g_furOffset, 0);
        alphaTestMat.SetFloat(_g_zWriteMode, 1);
        alphaTestMat.renderQueue = 2500;
        furMaterials.Add(alphaTestMat);
        for (int i = 0; i < _m_curFurCount; i++)
        {
            Material iMat = Instantiate(_m_furMat);
#if UNITY_EDITOR
            iMat.name = _m_furMat.name + "_fur" + i;
#endif
            iMat.SetFloat(_g_furOffset, (i + 1)/(float)_m_curFurCount);
            iMat.SetShaderPassEnabled("ShadowCaster", false);
            iMat.SetShaderPassEnabled("PlanarShadow", false);
            iMat.SetShaderPassEnabled("PlanarReflect", false);
            furMaterials.Add(iMat);
        }
        renderer.materials = furMaterials.ToArray();
    }
}

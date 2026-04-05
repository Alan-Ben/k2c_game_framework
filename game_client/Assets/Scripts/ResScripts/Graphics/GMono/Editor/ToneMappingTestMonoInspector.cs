
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

[CustomEditor(typeof(ToneMappingTestMono))]
public class ToneMappingTestMonoInspector : Editor
{
  


    SerializedProperty m_ToeStrength;
    SerializedProperty m_ToeLength;
    SerializedProperty m_ShoulderStrength;
    SerializedProperty m_ShoulderLength;
    SerializedProperty m_ShoulderAngle;
    SerializedProperty m_Gamma;
    
    readonly HableCurve m_HableCurve = new HableCurve();

    Rect m_CurveRect;
    Material m_Material;
    RenderTexture m_CurveTex;


    private void OnEnable()
    {
        m_ToeStrength = serializedObject.FindProperty("m_ToeStrength");
        m_ToeLength = serializedObject.FindProperty("m_ToeLength");
        m_ShoulderStrength = serializedObject.FindProperty("m_ShoulderStrength");
        m_ShoulderLength = serializedObject.FindProperty("m_ShoulderLength");
        m_ShoulderAngle = serializedObject.FindProperty("m_ShoulderAngle");
        m_Gamma = serializedObject.FindProperty("m_Gamma");
        m_Material = new Material(Shader.Find("Hidden/HD PostProcessing/Editor/Custom Tonemapper Curve"));

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mono = target as ToneMappingTestMono;
        {
            
            m_CurveRect = GUILayoutUtility.GetRect(128, 80);
            m_CurveRect.xMin += EditorGUI.indentLevel * 15f;
            float toeStrength = m_ToeStrength.floatValue;
            float toeLength = m_ToeLength.floatValue;
            float shoulderStrength = m_ShoulderStrength.floatValue;
            float shoulderLength = m_ShoulderLength.floatValue;
            float shoulderAngle = m_ShoulderAngle.floatValue;
            float gamma = m_Gamma.floatValue;
            m_HableCurve.Init(
                toeStrength,
                toeLength,
                shoulderStrength,
                shoulderLength,
                shoulderAngle,
                gamma
            );
            
            float alpha = GUI.enabled ? 1f : 0.5f;

            m_Material.SetVector(ToneMappingTestMono._CustomToneCurve, m_HableCurve.uniforms.curve);
            m_Material.SetVector(ToneMappingTestMono._ToeSegmentA, m_HableCurve.uniforms.toeSegmentA);
            m_Material.SetVector(ToneMappingTestMono._ToeSegmentB, m_HableCurve.uniforms.toeSegmentB);
            m_Material.SetVector(ToneMappingTestMono._MidSegmentA, m_HableCurve.uniforms.midSegmentA);
            m_Material.SetVector(ToneMappingTestMono._MidSegmentB, m_HableCurve.uniforms.midSegmentB);
            m_Material.SetVector(ToneMappingTestMono._ShoSegmentA, m_HableCurve.uniforms.shoSegmentA);
            m_Material.SetVector(ToneMappingTestMono._ShoSegmentB, m_HableCurve.uniforms.shoSegmentB);
            m_Material.SetVector(ToneMappingTestMono._Variants, new Vector4(alpha, m_HableCurve.whitePoint, 0f, 0f));
            
            CheckCurveRT((int)m_CurveRect.width, (int)m_CurveRect.height);

            var oldRt = RenderTexture.active;
            Graphics.Blit(null, m_CurveTex, m_Material, EditorGUIUtility.isProSkin ? 0 : 1);
            RenderTexture.active = oldRt;

            GUI.DrawTexture(m_CurveRect, m_CurveTex);
            void CheckCurveRT(int width, int height)
            {
                if (m_CurveTex == null || !m_CurveTex.IsCreated() || m_CurveTex.width != width || m_CurveTex.height != height)
                {
                    CoreUtils.Destroy(m_CurveTex);
                    m_CurveTex = new RenderTexture(width, height, 0, GraphicsFormat.R8G8B8A8_SRGB);
                    m_CurveTex.hideFlags = HideFlags.HideAndDontSave;
                }
            }
        }
    }
}

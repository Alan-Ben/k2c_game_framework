using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(BlurRawImage), true)]
    [CanEditMultipleObjects]
    /// <summary>
    /// Custom editor for BlurRawImage.
    /// Extend this class to write a custom editor for a component derived from RawImage.
    /// </summary>
    public class BlurRawImageEditor : RawImageEditor
    {
        SerializedProperty m_blurSpreadSize;
        SerializedProperty m_blurIterations;
        SerializedProperty m_blurMaterial;
        SerializedProperty m_needDownSample;
        SerializedProperty m_downSampleNum;
        SerializedProperty m_blurColor;
        SerializedProperty m__m_bOpenBlur;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            m_blurSpreadSize = serializedObject.FindProperty("blurSpreadSize");
            m_blurIterations = serializedObject.FindProperty("blurIterations");
            m_blurMaterial = serializedObject.FindProperty("blurMaterial");
            BlurRawImage blurRawImage = target as BlurRawImage;
            if (blurRawImage != null && blurRawImage.blurMaterial == null)
            {
                Material defaultMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/GameRes/common/shaders/urp_ex/Util/RapidBlur.mat");
                blurRawImage.blurMaterial = defaultMat;
                EditorUtility.SetDirty(blurRawImage);
            }
            m_needDownSample = serializedObject.FindProperty("needDownSample");
            m_downSampleNum = serializedObject.FindProperty("downSampleNum");
            m_blurColor = serializedObject.FindProperty("blurColor");
            m__m_bOpenBlur = serializedObject.FindProperty("_m_bOpenBlur");
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
        
            EditorGUILayout.PropertyField(m_blurSpreadSize);
            EditorGUILayout.PropertyField(m_blurIterations);
            EditorGUILayout.PropertyField(m_blurMaterial);
            EditorGUILayout.PropertyField(m_needDownSample);
            EditorGUILayout.PropertyField(m_downSampleNum);
            EditorGUILayout.PropertyField(m_blurColor);
            EditorGUILayout.PropertyField(m__m_bOpenBlur);
            bool hasModified = serializedObject.hasModifiedProperties;
            serializedObject.ApplyModifiedProperties();
            if (hasModified)
            {
                BlurRawImage blurRawImage = target as BlurRawImage;
                if (blurRawImage != null) blurRawImage.refreshBlur();
            }
        }
      
    }
}

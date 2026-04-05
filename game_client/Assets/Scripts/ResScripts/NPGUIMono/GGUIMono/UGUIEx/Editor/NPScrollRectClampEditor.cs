using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace GOE
{
    [CustomEditor(typeof(NPScrollRectClamp), true)]
    [CanEditMultipleObjects]
    public class NPScrollRectClampEditor : ScrollRectEditor
    {       
        SerializedProperty clampNormalizePos;
        SerializedProperty m_PaddingClamp;
        // SerializedProperty rightBottomClampOffset;
        

        protected override void OnEnable()
        {
            base.OnEnable();
            clampNormalizePos              = serializedObject.FindProperty("clampNormalizePos");
            m_PaddingClamp              = serializedObject.FindProperty("m_PaddingClamp");
            // rightBottomClampOffset              = serializedObject.FindProperty("rightBottomClampOffset");

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();    
            serializedObject.Update();
            EditorGUILayout.PropertyField(clampNormalizePos);
            EditorGUILayout.PropertyField(m_PaddingClamp);
            // EditorGUILayout.PropertyField(rightBottomClampOffset);
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}
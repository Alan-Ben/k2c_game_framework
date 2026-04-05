using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using GOE;

[CanEditMultipleObjects]
[CustomEditor(typeof(ImageMatGray))]
public class ImageMatGrayInspector : ImageEditor
{
    SerializedProperty m_grayMaterial;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_grayMaterial = serializedObject.FindProperty("_grayMaterial");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_grayMaterial);
        serializedObject.ApplyModifiedProperties();
    }
}
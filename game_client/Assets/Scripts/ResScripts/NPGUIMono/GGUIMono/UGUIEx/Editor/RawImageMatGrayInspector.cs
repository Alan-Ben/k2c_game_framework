using GOE;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(RawImageMatGray))]
public class RawImageMatGrayInspector : RawImageEditor
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
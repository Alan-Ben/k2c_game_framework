using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;


[CustomEditor(typeof(RawImageMesh),true)]
[CanEditMultipleObjects]
public class RawImageMeshEditor : RawImageEditor
{
    private SerializedProperty _mesh;
    private SerializedProperty _overrideScale;
    protected override void OnEnable()
    {
        base.OnEnable();
        _mesh = serializedObject.FindProperty("m_mesh");
        _overrideScale=serializedObject.FindProperty("m_overrideScale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_mesh);
        EditorGUILayout.PropertyField(_overrideScale);
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();  
        
    }
}

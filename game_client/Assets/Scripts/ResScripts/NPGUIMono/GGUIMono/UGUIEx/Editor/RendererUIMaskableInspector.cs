using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomEditor(typeof(RendererUIMaskable))]
    public class RendererUIMaskableInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("renders"));
            if (GUILayout.Button("自动赋值"))
            {
                var mono = target as RendererUIMaskable;
                if (mono != null)
                {
                    mono.renders.Clear();
                    mono.renders = new List<Renderer>(mono.GetComponentsInChildren<Renderer>());
                    EditorUtility.SetDirty(mono);
                }
            }
        }
    }
}
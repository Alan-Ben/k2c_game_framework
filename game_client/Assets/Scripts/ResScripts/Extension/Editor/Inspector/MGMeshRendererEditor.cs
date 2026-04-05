using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MG
{
    [CustomEditor(typeof(MeshRenderer))]
    [CanEditMultipleObjects]
    public class MeshRendererEditor : DecoratorEditor
    {

        public MeshRendererEditor() : base("MeshRendererEditor")
        {
        }
        
        private SerializedProperty m_SortingOrder;
        private SerializedProperty m_SortingLayerID;
        public void OnEnable()
        {
            m_SortingOrder = serializedObject.FindProperty("m_SortingOrder");
            m_SortingLayerID = serializedObject.FindProperty("m_SortingLayerID");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            serializedObject.Update();
            InternalEditorBridge.RenderSortingLayerFields(m_SortingOrder, m_SortingLayerID);
            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(SkinnedMeshRenderer))]
    [CanEditMultipleObjects]
    public class SkinnedMeshRendererEditor : DecoratorEditor
    {
        Renderer _m_renderer;

        public SkinnedMeshRendererEditor() : base("SkinnedMeshRendererEditor")
        {
        }
        private SerializedProperty m_SortingOrder;
        private SerializedProperty m_SortingLayerID;
        public void OnEnable()
        {
            m_SortingOrder = serializedObject.FindProperty("m_SortingOrder");
            m_SortingLayerID = serializedObject.FindProperty("m_SortingLayerID");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            serializedObject.Update();
            InternalEditorBridge.RenderSortingLayerFields(m_SortingOrder, m_SortingLayerID);
            serializedObject.ApplyModifiedProperties();
        }

        // 如果有需要继承的类有此方法参照这个方法去调用
        public void OnSceneGUI()
        {
        	CallInspectorMethod("OnSceneGUI");
        }
    }
}
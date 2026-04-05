using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(SpriteRenderer))]
    [CanEditMultipleObjects]
    internal class SpriteRendererEditorEx : SpriteRendererEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            m_Lighting.RenderSettings(false);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
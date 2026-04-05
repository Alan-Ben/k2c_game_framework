
using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomEditor(typeof(GTDMonoFakePerspectiveItem))]
    public class GTDMonoFakePerspectiveItemInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button(new GUIContent("Set Current Position As OriginPos", "自动把当前 transform 的 position 填到 originPos 中")))
            {
                (target as GTDMonoFakePerspectiveItem)?.setCurrentPositionAsOriginPos();
            }
        }
    }
}
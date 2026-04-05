
using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomEditor(typeof(GTDMonoFakePerspectiveScene))]
    public class GTDMonoFakePerspectiveSceneInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button(new GUIContent("Auto Collect Items", "自动把这个场景里的所有 GTDMonoFakePerspectiveItem 脚本添加到 perspectiveItems 列表中")))
            {
                (target as GTDMonoFakePerspectiveScene)?.autoCollectItems();
            }

            if (GUILayout.Button(new GUIContent("Set Current Camera Pos As Origin Pos", "把现在 sceneCamera 的坐标值自动填到 cameraOriginPos 上")))
            {
                (target as GTDMonoFakePerspectiveScene)?.setCurrentCameraPosAsOriginPos();
            }
        }
    }
}
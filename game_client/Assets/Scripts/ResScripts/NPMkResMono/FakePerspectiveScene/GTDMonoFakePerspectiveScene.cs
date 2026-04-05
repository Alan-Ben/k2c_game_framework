
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [ExecuteInEditMode]
    public class GTDMonoFakePerspectiveScene : MonoBehaviour
    {
        [ALInfo("目标相机在游戏运行起来后，会自动被替换为游戏的主相机，在编辑时可以随意指定任意相机")]
        [ALHeader("这个场景的相机")]
        public Camera sceneCamera;
        [ALInfo("目前只支持这三种移动模式，感觉也够用，不够用可以联系程序修改，改动不大，但是配置会变得更复杂")]
        [ALHeader("这个相机有效的移动模式")]
        public ESceneMoveType sceneMoveMode;

        [ALInfo("这个坐标的意义是：相机在这个坐标看向所有的 item 时，这些 item 都正好在它们的原始位置上")]
        [ALHeader("相机的标准位置")]
        public Vector3 cameraOriginPos;

        [ALInfo("相机距离标准位置多远内算是有效的，一般情况不用改动这个值，如果发现相机到一定距离后 item 不动了，就适当调大这个值")]
        [ALHeader("相机的有效范围")]
        public float validCameraRange = 50f;

        [ALHeader("所有模拟透视效果的 item ")]
        public List<GTDMonoFakePerspectiveItem> perspectiveItems;

        private float _m_sqrValidCameraRange;
        private _ALogicPlane2DPosGetter _m_logicPosGetter;

        private void Awake()
        {
#if NP_GAME
            if (!Application.isPlaying)
                return;

            sceneCamera = CameraController.instance.controlCamera;  
#endif

            _m_sqrValidCameraRange = validCameraRange * validCameraRange;
            _m_logicPosGetter = sceneMoveMode switch
            {
                ESceneMoveType.XY => new LogicPlane2DPosGetterXY(Vector3.zero),
                ESceneMoveType.XZ => new LogicPlane2DPosGetterXZ(Vector3.zero),
                ESceneMoveType.YZ => new LogicPlane2DPosGetterZY(Vector3.zero),
                _ => _m_logicPosGetter
            };
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _m_sqrValidCameraRange = validCameraRange * validCameraRange;
            _m_logicPosGetter = sceneMoveMode switch
            {
                ESceneMoveType.XY => new LogicPlane2DPosGetterXY(Vector3.zero),
                ESceneMoveType.XZ => new LogicPlane2DPosGetterXZ(Vector3.zero),
                ESceneMoveType.YZ => new LogicPlane2DPosGetterZY(Vector3.zero),
                _ => _m_logicPosGetter
            };
        }
#endif

        private void LateUpdate()
        {
            if (sceneCamera == null || _m_logicPosGetter == null)
                return;

            // 计算相机偏移了标准位置多少
            Vector3 cameraOffsetToOrigin = sceneCamera.transform.position - cameraOriginPos;
            // 如果偏移超过允许的范围后面就不处理了
            if (cameraOffsetToOrigin.sqrMagnitude > _m_sqrValidCameraRange)
                return;

            // 剔除掉非场景移动方向的移动值
            Vector2 logicOffset = _m_logicPosGetter.getLogicPos(cameraOffsetToOrigin);
            foreach (GTDMonoFakePerspectiveItem item in perspectiveItems)
            {
                if (item == null)
                    continue;

                Vector2 scaledLogicOffset = item.scaleLogicOffset(logicOffset);
                Vector3 worldOffsetOnSceneMovePlane = Vector3.zero;
                _m_logicPosGetter.setLogicXPos(ref worldOffsetOnSceneMovePlane, scaledLogicOffset.x);
                _m_logicPosGetter.setLogicYPos(ref worldOffsetOnSceneMovePlane, scaledLogicOffset.y);
            
                // 把剔除过的移动值再映射到相机的移动方向上
                Transform cameraTrans = sceneCamera.transform;
                Vector3 cameraRight = cameraTrans.right;
                Vector3 cameraUp = cameraTrans.up;
                Vector3 itemOffset = cameraRight * Vector3.Dot(cameraRight, worldOffsetOnSceneMovePlane) +
                                     cameraUp * Vector3.Dot(cameraUp, worldOffsetOnSceneMovePlane);
                
                item.setOffset(itemOffset);
            }
            
#if UNITY_EDITOR
            if (useDebugDraw)
            {
                debugDraw();

                foreach (GTDMonoFakePerspectiveItem item in perspectiveItems)
                {
                    if (item == null)
                        return;

                    item.debugDraw(debugDrawSize);
                }
            }
#endif
        }

#if UNITY_EDITOR
        [ALHeader("调试用的参数")]
        public float debugDrawSize = 1;
        public bool useDebugDraw = true;
        public void debugDraw()
        {
            if (sceneCamera == null)
                return;
            
            DebugPlus.DrawSphere(cameraOriginPos, debugDrawSize, Color.gray);
            DebugPlus.DrawArrow2(cameraOriginPos, sceneCamera.transform.position, Color.gray);
            DebugPlus.DrawSphere(sceneCamera.transform.position, debugDrawSize, Color.blue);
        }

        [ContextMenu("Auto Collect Items")]
        public void autoCollectItems()
        {
            if (perspectiveItems == null)
                return;
            
            perspectiveItems.Clear();
            
            GameObject[] rootObjects = gameObject.scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                GTDMonoFakePerspectiveItem[] items = rootObject.GetComponentsInChildren<GTDMonoFakePerspectiveItem>(true);
                if (items == null)
                    continue;
                
                perspectiveItems.AddRange(items);
            }
        }

        [ContextMenu("Set Current Camera Pos As Origin Pos")]
        public void setCurrentCameraPosAsOriginPos()
        {
            if (sceneCamera == null)
                return;

            cameraOriginPos = sceneCamera.transform.position;
        }
#endif
    }
}
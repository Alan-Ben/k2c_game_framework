using System;
using UnityEngine;

namespace GOE
{
    public class CameraDebugMono : MonoBehaviour
    {
        public float originSize = 0.3f;
        public Vector3 cameraPosLimiterOrigin = new Vector3(1, 1, 1);
        public Vector3 cameraFocusLimiterOrigin = new Vector3(2, 2, 2);
        public bool showCameraPosLimiter = true;
        
#if NP_GAME
        
        public void Update()
        {
            if (GTDSceneMain.instance.curShowScene is not _ABasicAdditionMainTDScene tdScene)
                return;

            // 画出原点和坐标轴
            SceneInfoRefObj sceneInfoRef = tdScene.sceneRefObj;
            DebugPlus.DrawBox(sceneInfoRef.corner_pos, originSize * Vector3.one, Quaternion.identity, Color.white);
            _ALogicPlane2DPosGetter posGetter = sceneInfoRef.getLogicPosGetter();
            Vector3 right = sceneInfoRef.corner_pos;
            posGetter.setLogicXPos(ref right, 1);
            Vector3 up = sceneInfoRef.corner_pos;
            posGetter.setLogicYPos(ref up, 1);
            DebugPlus.DrawArrow(sceneInfoRef.corner_pos, right - sceneInfoRef.corner_pos, Color.red);
            DebugPlus.DrawArrow(sceneInfoRef.corner_pos, up - sceneInfoRef.corner_pos, Color.green);
            
            // 画出可视范围
            // 绘制拖动可视范围
            Vector3[] dragMoveRectPoints = new Vector3[] { sceneInfoRef.corner_pos, sceneInfoRef.corner_pos, sceneInfoRef.corner_pos, sceneInfoRef.corner_pos };
            // 左下
            posGetter.setLogicXPos(ref dragMoveRectPoints[0], sceneInfoRef.map_drag_rect.xMin);
            posGetter.setLogicYPos(ref dragMoveRectPoints[0], sceneInfoRef.map_drag_rect.yMin);
            // 左上
            posGetter.setLogicXPos(ref dragMoveRectPoints[1], sceneInfoRef.map_drag_rect.xMin);
            posGetter.setLogicYPos(ref dragMoveRectPoints[1], sceneInfoRef.map_drag_rect.yMax);
            // 右上
            posGetter.setLogicXPos(ref dragMoveRectPoints[2], sceneInfoRef.map_drag_rect.xMax);
            posGetter.setLogicYPos(ref dragMoveRectPoints[2], sceneInfoRef.map_drag_rect.yMax);
            // 右下
            posGetter.setLogicXPos(ref dragMoveRectPoints[3], sceneInfoRef.map_drag_rect.xMax);
            posGetter.setLogicYPos(ref dragMoveRectPoints[3], sceneInfoRef.map_drag_rect.yMin);
            // 绘制
            DebugPlus.DrawPath(dragMoveRectPoints, true, Color.magenta);

            if (showCameraPosLimiter)
            {
                
                CameraController.instance.cameraPosLimiter?.debugDrawUpdate(cameraPosLimiterOrigin, Color.yellow);
                CameraController.instance.cameraFocusPosLimiter?.debugDrawUpdate(cameraFocusLimiterOrigin, Color.blue);
            }
            else
            {
                ScenePosLimiter posLimiter = new ScenePosLimiter(sceneInfoRef);
                SceneFocusPosLimiter focusPosLimiter = new SceneFocusPosLimiter(sceneInfoRef);
                posLimiter.refresh();
                focusPosLimiter.refresh();
                posLimiter.debugDrawUpdate(cameraPosLimiterOrigin, Color.yellow);
                focusPosLimiter.debugDrawUpdate(cameraFocusLimiterOrigin, Color.blue);
            }
        }
#endif
    }
}
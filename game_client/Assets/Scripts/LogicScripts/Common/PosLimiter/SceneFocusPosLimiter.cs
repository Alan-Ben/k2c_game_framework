using ALPackage;
using UnityEngine;

namespace GOE
{
    public class SceneFocusPosLimiter : SoftBorder2DPosLimiter
    {
        public static Rect getFocusPosStableRect(_ALogicPlane2DPosGetter _logicPosGetter, Rect _mapDragRect)
        {
            if (_logicPosGetter == null)
                return new Rect();
            
            // 获取世界坐标系下射线和 getter 表示的逻辑平面的世界坐标交点
            if (!_logicPosGetter.intersectionWorldPointWithWorldRay(CameraController.instance.viewportPosToRay(new Vector2(1, 0)), out Vector3 rightDown))
            {
                ALLog.Error($"[** Scene **] 场景的相机配置和设置的地面没有交点");
                return new Rect();
            }
            if (!_logicPosGetter.intersectionWorldPointWithWorldRay(CameraController.instance.viewportPosToRay(new Vector2(1, 1)), out Vector3 rightUp))
            {
                ALLog.Error($"[** Scene **] 场景的相机配置和设置的地面没有交点");
                return new Rect();
            }
            if (!_logicPosGetter.intersectionWorldPointWithWorldRay(CameraController.instance.viewportPosToRay(new Vector2(0, 1)), out Vector3 leftUp))
            {
                ALLog.Error($"[** Scene **] 场景的相机配置和设置的地面没有交点");
                return new Rect();
            }
            if (!_logicPosGetter.intersectionWorldPointWithWorldRay(CameraController.instance.viewportPosToRay(new Vector2(0, 0)), out Vector3 leftDown))
            {
                ALLog.Error($"[** Scene **] 场景的相机配置和设置的地面没有交点");
                return new Rect();
            }
            
            // 把世界坐标系下的交点转换成逻辑坐标
            float logicRightDownX = _logicPosGetter.getLogicXPos(rightDown);
            float logicRightUpX = _logicPosGetter.getLogicXPos(rightUp);
            float logicLeftUpX = _logicPosGetter.getLogicXPos(leftUp);
            float logicLeftDownX = _logicPosGetter.getLogicXPos(leftDown);
            float logicRightDownY = _logicPosGetter.getLogicYPos(rightDown);
            float logicRightUpY = _logicPosGetter.getLogicYPos(rightUp);
            float logicLeftUpY = _logicPosGetter.getLogicYPos(leftUp);
            float logicLeftDownY = _logicPosGetter.getLogicYPos(leftDown);

            // 获取相机在逻辑坐标系下的显示范围
            float minX = Mathf.Min(logicRightDownX, logicRightUpX, logicLeftUpX, logicLeftDownX);
            float maxX = Mathf.Max(logicRightDownX, logicRightUpX, logicLeftUpX, logicLeftDownX);
            float minY = Mathf.Min(logicRightDownY, logicRightUpY, logicLeftUpY, logicLeftDownY);
            float maxY = Mathf.Max(logicRightDownY, logicRightUpY, logicLeftUpY, logicLeftDownY);

            // 根据相机的可视范围，计算出相机焦点的可移动范围
            Rect stableRect = new Rect(
                _mapDragRect.x + (maxX - minX) / 2f, _mapDragRect.y + (maxY - minY) / 2f,
                _mapDragRect.width - (maxX - minX), _mapDragRect.height - (maxY - minY));
            if(stableRect.width < 0 || float.IsNaN(stableRect.width))
            {
                stableRect.x = _mapDragRect.x + (_mapDragRect.width / 2);
                stableRect.width = 0;
            }
            if(stableRect.height < 0 || float.IsNaN(stableRect.height))
            {
                stableRect.y = _mapDragRect.y + (_mapDragRect.height / 2);
                stableRect.height = 0;
            }
            
            // 计算当前相机焦点的位置的逻辑坐标，和使用当前焦点和逻辑平面相交的点的逻辑坐标，它们之间的差值就是 stableRect 的偏移值
            _logicPosGetter.intersectionWorldPointWithWorldRay(new Ray(CameraController.instance.cameraPos, CameraController.instance.cameraForward), out Vector3 worldIntersectionFocusPos);
            // 把相机的焦点投影到逻辑平面上
            Vector3 cameraFocusPos = CameraController.instance.cameraFocusPos;
            Vector2 logicCameraFocusPos = _logicPosGetter.getLogicPos(cameraFocusPos);
            // 把使用这个焦点后在逻辑平面上看到的点计算出来
            Vector2 logicIntersectionFocusPos = _logicPosGetter.getLogicPos(worldIntersectionFocusPos);
            // 计算这两个逻辑平面上的点相差多少，并根据这个值偏移 stableRect ，新的 stableRect 就会作用于相机的焦点上
            stableRect.center += logicCameraFocusPos - logicIntersectionFocusPos;
            return stableRect;
        }

        private readonly _ALogicPlane2DPosGetter _m_posGetter;
        private readonly Rect _m_dragRect;
        private readonly Vector2 _m_softBorderSize;
        
        public SceneFocusPosLimiter(SceneInfoRefObj _sceneRef) 
            : base(getFocusPosStableRect(_sceneRef?.getLogicPosGetter(), _sceneRef?.map_drag_rect ?? new Rect()), _sceneRef?.map_border_scale_size ?? Vector2.zero, _sceneRef?.getLogicPosGetter())
        {
            _m_posGetter = _sceneRef?.getLogicPosGetter();
            _m_dragRect = _sceneRef?.map_drag_rect ?? Rect.zero;
            _m_softBorderSize = _sceneRef?.map_border_scale_size ?? Vector2.zero;
        }
        public SceneFocusPosLimiter(_ALogicPlane2DPosGetter _posGetter, Rect _dragRect, Vector2 _softBorderSize)
            : base(getFocusPosStableRect(_posGetter, _dragRect), _softBorderSize, _posGetter)
        {
            _m_posGetter = _posGetter;
            _m_dragRect = _dragRect;
            _m_softBorderSize = _softBorderSize;
        }

        public void refresh()
        {
            Rect stableRect = getFocusPosStableRect(_m_posGetter, _m_dragRect);
            _refresh(stableRect, _m_softBorderSize, _m_posGetter);
        }
    }
}
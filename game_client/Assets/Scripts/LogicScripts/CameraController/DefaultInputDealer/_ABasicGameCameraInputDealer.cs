using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游戏摄像头的基础控制对象
    /// </summary>
    public abstract class _ABasicGameCameraInputDealer : _AGameInputDealer, _ICameraMonitor
    {
        // 场景拖动控制器
        private readonly CameraMoveDragController _m_cameraMoveDragController;
        private readonly ScenePosLimiter _m_posLimiter;
        private readonly SceneFocusPosLimiter _m_focusPosLimiter;
        private readonly WCGFloatRange _m_cameraScaleRange;
        private readonly _ALogicPlane2DPosGetter _m_posGetter;
        private readonly bool _m_useDragTipWnd;
        private readonly Vector2 _m_dragTipShowDistance;

        protected _ABasicGameCameraInputDealer(SceneInfoRefObj _sceneInfo)
        {
            if (_sceneInfo == null)
                return;

            _m_posGetter = _sceneInfo.getLogicPosGetter();
            _m_posLimiter = new ScenePosLimiter(_sceneInfo);
            _m_focusPosLimiter = new SceneFocusPosLimiter(_sceneInfo);
            _m_cameraScaleRange = new WCGFloatRange(_sceneInfo.min_org_scale, _sceneInfo.max_org_scale);
            _m_useDragTipWnd = _sceneInfo.use_drag_tip_wnd;
            _m_dragTipShowDistance = _sceneInfo.drag_tip_show_distance;
            
            _m_cameraMoveDragController = new CameraMoveDragController(_m_posGetter, _sceneInfo.air_friction, _sceneInfo.slide_friction);
        }
        protected _ABasicGameCameraInputDealer(_ALogicPlane2DPosGetter _posGetter, Rect _dragRect, Vector2 _softBorderSize, WCGFloatRange _cameraScaleRange, float _airFriction, float _slideFriction, bool _useDragTipWnd, Vector2 _dragTipShowDistance)
        {
            _m_posGetter = _posGetter;
            _m_posLimiter = new ScenePosLimiter(_posGetter, _dragRect, _softBorderSize);
            _m_focusPosLimiter = new SceneFocusPosLimiter(_posGetter, _dragRect, _softBorderSize);
            _m_cameraScaleRange = _cameraScaleRange;
            _m_useDragTipWnd = _useDragTipWnd;
            _m_dragTipShowDistance = _dragTipShowDistance;

            _m_cameraMoveDragController = new CameraMoveDragController(_posGetter, _airFriction, _slideFriction);
        }
        
        public _ALogicPlane2DPosGetter posGetter { get { return _m_posGetter; } }

        //按下按钮的时候的处理
        public override void onPress(TouchInfo _touchInfo)
        {
            //任何按下操作则需要停止惯性
            CameraController.instance.setCameraMoveController(_m_cameraMoveDragController);
            
            base.onPress(_touchInfo);
        }

        public override void onUnPress(TouchInfo _touchInfo)
        {
            _m_cameraMoveDragController?.setDone();
            
            base.onUnPress(_touchInfo);
        }

        //开始拖拽时的操作
        public override void OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            if (_touchInfo == null)
                return;
            
            //每次点击操作都记录当前按下的位置作为拖拽的起始位置
            _m_cameraMoveDragController?.beginDrag(_touchInfo.curPos);
            
            //开始高帧率处理
            FrameRateController.instance.setHighFrameOpen();

            _onDragStartEx(_go, _touchInfo, _pressToDragTime);
        }

        public override void OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo)
        {
            if (_touchInfo == null)
                return;
            
            _m_cameraMoveDragController?.drag(_touchInfo.curPos);
            
            _onDrag(_go, _delta, _touchInfo);
        }

        public override void OnDragEnd(GameObject _go, TouchInfo _touchInfo)
        {
            _m_cameraMoveDragController?.endDrag();
            
            //开启监控处理，相机不动了返回低帧率
            FrameRateController.instance.setHighFrameOpenAndMonitGo(CameraController.instance.controlCamera.transform);
            
            _onDragEnd(_go, _touchInfo);
        }

        //缩放变更的处理
        public override void OnScaleChg(float _changeValue)
        {
            if (CameraController.instance.controlCamera.orthographic)
            {
                float targetSize = CameraController.instance.cameraOrthographicSizeAfterFitting * (1f - _changeValue);
                if (_m_cameraScaleRange != null)
                    targetSize = _m_cameraScaleRange.clampValue(targetSize);
                CameraController.instance.cameraOrthographicSize = targetSize;
            }
            else
            {
                float targetSize = CameraController.instance.controlCamera.fieldOfView * (1f - _changeValue);
                if (_m_cameraScaleRange != null)
                    targetSize = _m_cameraScaleRange.clampValue(targetSize);
                CameraController.instance.cameraFieldOfView = targetSize;
            }
        }

        protected override void _onEnter()
        {
            CameraController.instance.setCameraPosLimiter(_m_posLimiter);
            CameraController.instance.setCameraFocusPosLimiter(_m_focusPosLimiter);
            if (_m_useDragTipWnd)
            {
                GGUIWndSceneDragTip.instance.load(GGUIWndSceneDragTip.instance.showWnd);
                GGUIWndSceneDragTip.instance.setShowTipDistance(_m_dragTipShowDistance.x, _m_dragTipShowDistance.y);
            }
            CameraController.instance.registerCameraMonitor(this);
        }

        //退出时的处理的内部方法，子类实现
        protected override void _onExit()
        {
            if (_m_useDragTipWnd)
                GGUIWndSceneDragTip.instance.discard();
            
            _m_cameraMoveDragController?.setDone();
            
            CameraController.instance.resetCameraPosLimiter(_m_posLimiter);
            CameraController.instance.resetCameraFocusPosLimiter(_m_focusPosLimiter);
            CameraController.instance.unregisterCameraMonitor(this);
        }

        /// <summary>
        /// 拖拽的子类处理函数
        /// </summary>
        protected virtual void _onDragStartEx(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime){}
        protected virtual void _onDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo){}
        protected virtual void _onDragEnd(GameObject _go, TouchInfo _touchInfo){}
        

        #region _ICameraMonitor 接口实现

        void _ICameraMonitor.onEnter()
        {
        }

        void _ICameraMonitor.onExit()
        {
        }

        void _ICameraMonitor.onCameraPosChg()
        {
        }

        void _ICameraMonitor.onCameraFocusChg()
        {
        }

        void _ICameraMonitor.onCameraTransformChg()
        {
        }

        void _ICameraMonitor.onOrthographicSizeChg()
        {
        }

        void _ICameraMonitor.onFieldOfViewChg()
        {
        }

        void _ICameraMonitor.onViewScaleChg()
        {
            _m_posLimiter?.refresh();
            _m_focusPosLimiter?.refresh();
        }

        void _ICameraMonitor.onPosLimiterChg()
        {
        }

        void _ICameraMonitor.onFocusPosLimiterChg()
        {
        }

        #endregion
    }
}

using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 3d 场景的可拖拽提示窗口
    /// </summary>
    public class GGUIWndSceneDragTip : _ANPGGUIBasicWnd<GGUIMonoSceneDragTip>, _ICameraMonitor
    {
        [NotNull] public static GGUIWndSceneDragTip instance { get { return _g_instance ??= new GGUIWndSceneDragTip(); } }
        private static GGUIWndSceneDragTip _g_instance;
    
        // 这个场景的拖动方向，外部指定两个方向形成一个十字
        private Vector2 _m_dragRight;
        private Vector2 _m_dragUp;
        
        // 拖动提示距离屏幕边缘的距离
        private float _m_tipOffsetRight;
        private float _m_tipOffsetLeft;
        private float _m_tipOffsetUp;
        private float _m_tipOffsetDown;
        
        // 当相机离对应边缘多远时显示拖动提示
        private float _m_showTipDistanceHorizontal;
        private float _m_showTipDistanceVertical;

        // 当前相机计算出的拖动边缘
        private Vector3 _m_rightEdge;
        private Vector3 _m_leftEdge;
        private Vector3 _m_upEdge;
        private Vector3 _m_downEdge;

        public GGUIWndSceneDragTip() 
            : base(EALUIWndLayer.GAME_WORLD_UI)
        {
            _m_dragRight = Vector2.right;
            _m_dragUp = Vector2.up;

            _m_showTipDistanceHorizontal = 0.5f;
            _m_showTipDistanceVertical = 0.5f;
            
            _m_tipOffsetRight = 0;
            _m_tipOffsetLeft = 0;
            _m_tipOffsetUp = 0;
            _m_tipOffsetDown = 0;
        }

        protected override string _monoAssetPath { get { return GGUIMonoSceneDragTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSceneDragTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnGoToRight, _onBtnGoToRightClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnGoToUp, _onBtnGoToUpClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnGoToLeft, _onBtnGoToLeftClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnGoToDown, _onBtnGoToDownClick);
            }
            
            CameraController.instance.unregisterCameraMonitor(this);
        }

        protected override void _onWndInitDone()
        {
            if (wnd != null)
            {
                ALUGUICommon.combineBtnClick(wnd.btnGoToRight, _onBtnGoToRightClick);
                ALUGUICommon.combineBtnClick(wnd.btnGoToUp, _onBtnGoToUpClick);
                ALUGUICommon.combineBtnClick(wnd.btnGoToLeft, _onBtnGoToLeftClick);
                ALUGUICommon.combineBtnClick(wnd.btnGoToDown, _onBtnGoToDownClick);
            }

            refreshWnd();
            
            CameraController.instance.registerCameraMonitor(this);
        }

        /// <summary>
        /// 设置拖动的方向
        /// </summary>
        public void setDragDirection(Vector2 _right, Vector2 _up)
        {
            _m_dragRight = _right.normalized;
            _m_dragUp = _up.normalized;

            // 先重新计算边缘值，再刷新提示的显示
            refreshWnd();
        }
        /// <summary>
        /// 设置距离拖动方向的边缘多远显示可拖动按钮
        /// </summary>
        public void setShowTipDistance(float _horizontal, float _vertical)
        {
            _m_showTipDistanceHorizontal = _horizontal;
            _m_showTipDistanceVertical = _vertical;

            // 刷新提示的显示
            refreshWnd();
        }
        /// <summary>
        /// 设置拖动提示距离屏幕边缘的距离
        /// </summary>
        public void setTipOffset(float _right, float _left, float _up, float _down)
        {
            _m_tipOffsetRight = _right;
            _m_tipOffsetLeft = _left;
            _m_tipOffsetUp = _up;
            _m_tipOffsetDown = _down;

            // 刷新提示的显示
            refreshWnd();
        }

        /// <summary>
        /// 刷新现在提示显示的位置
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null)
                return;
            
            // 如果不存在限制器，就都隐藏
            _APosLimiter posLimiter = CameraController.instance.cameraPosLimiter;
            if (posLimiter == null || wnd.btnParent == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnGoToRight, false);
                ALUGUICommon.setGameObjEnable(wnd.btnGoToUp, false);
                ALUGUICommon.setGameObjEnable(wnd.btnGoToLeft, false);
                ALUGUICommon.setGameObjEnable(wnd.btnGoToDown, false);
                return;
            }

            // 先设置边缘值 ==================
            
            // 把设置的 right 和 up 转换到相机坐标系下，因为这里只支持平移，所以做这样的处理
            Vector3 right, up;
            if (posLimiter is SoftBorder2DPosLimiter softBorder2DPosLimiter)
            {
                Vector3 origin = Vector3.zero;
                _ALogicPlane2DPosGetter posGetter = softBorder2DPosLimiter.posGetter;
                posGetter.setLogicXPos(ref origin, 0);
                posGetter.setLogicYPos(ref origin, 0);
                Vector3 logicRight, logicUp;
                logicRight = logicUp = origin;
                posGetter.setLogicXPos(ref logicRight, 1);
                posGetter.setLogicYPos(ref logicUp, 1);
                logicRight -= origin;
                logicUp -= origin;

                right = logicRight * _m_dragRight.x + logicUp * _m_dragRight.y;
                up = logicRight * _m_dragUp.x + logicUp * _m_dragUp.y;
            }
            else
            {
                Quaternion rotation = CameraController.instance.cameraRotation;
                right = rotation * new Vector3(_m_dragRight.x, _m_dragRight.y, 0);
                up = rotation * new Vector3(_m_dragUp.x, _m_dragUp.y, 0);
            }

            // 计算四个边缘
            // todo: 换成直线和范围求交的做法
            Vector3 cameraPos = CameraController.instance.cameraPos;
            posLimiter.isPosInStableArea(cameraPos + right * 100000f, out _m_rightEdge);
            posLimiter.isPosInStableArea(cameraPos - right * 100000f, out _m_leftEdge);
            posLimiter.isPosInStableArea(cameraPos + up * 100000f, out _m_upEdge);
            posLimiter.isPosInStableArea(cameraPos - up * 100000f, out _m_downEdge);

            // 再设置提示按钮 =================
            
            // 设置左右
            Vector3 center = (_m_rightEdge + _m_leftEdge) / 2f;
            _setTipBtnShow(center, _m_rightEdge, _m_showTipDistanceHorizontal, wnd.btnGoToRight, _m_dragRight, _m_tipOffsetRight);
            _setTipBtnShow(center, _m_leftEdge, _m_showTipDistanceHorizontal, wnd.btnGoToLeft, -_m_dragRight, _m_tipOffsetLeft);
            // 设置上下
            center = (_m_upEdge + _m_downEdge) / 2f;
            _setTipBtnShow(center, _m_upEdge, _m_showTipDistanceVertical, wnd.btnGoToUp, _m_dragUp, _m_tipOffsetUp);
            _setTipBtnShow(center, _m_downEdge, _m_showTipDistanceVertical, wnd.btnGoToDown, -_m_dragUp, _m_tipOffsetDown);
        }

        private void _setTipBtnShow(Vector3 _center, Vector3 _edgePoint, float _tipDistance, GameObject _tipBtn, Vector2 _dragDirection, float _offset)
        {
            if (wnd == null || wnd.btnParent == null || _tipBtn == null) 
                return;
            
            Vector3 centerToEdge = _edgePoint - _center;
            Vector3 centerToCamera = CameraController.instance.cameraPos - _center;
            if (centerToEdge.magnitude - Vector3.Dot(centerToCamera, centerToEdge.normalized) > _tipDistance)
            {
                ALUGUICommon.setGameObjEnable(_tipBtn, true);
                _tipBtn.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(_dragDirection.x, _dragDirection.y) * Mathf.Rad2Deg);
                Rect screenRect = wnd.btnParent.rect;
                float t = Mathf.Min(
                    _dragDirection.x == 0 ? float.PositiveInfinity : screenRect.width / Mathf.Abs(_dragDirection.x), 
                    _dragDirection.y == 0 ? float.PositiveInfinity : screenRect.height / Mathf.Abs(_dragDirection.y));
                t /= 2f;
                _tipBtn.transform.localPosition = screenRect.center + _dragDirection * (t - _offset);
            }
            else
                ALUGUICommon.setGameObjEnable(_tipBtn, false);
        }

        private void _onBtnGoToRightClick(GameObject _)
        {
            if (wnd == null)
                return;

            CameraController.instance.setCameraMoveController(new CameraMoveEaseController(_m_rightEdge, wnd.moveDuration));
        }
        private void _onBtnGoToUpClick(GameObject _)
        {
            if (wnd == null)
                return;

            CameraController.instance.setCameraMoveController(new CameraMoveEaseController(_m_upEdge, wnd.moveDuration));
        }
        private void _onBtnGoToLeftClick(GameObject _)
        {
            if (wnd == null)
                return;

            CameraController.instance.setCameraMoveController(new CameraMoveEaseController(_m_leftEdge, wnd.moveDuration));
        }
        private void _onBtnGoToDownClick(GameObject _)
        {
            if (wnd == null)
                return;

            CameraController.instance.setCameraMoveController(new CameraMoveEaseController(_m_downEdge, wnd.moveDuration));
        }

        void _ICameraMonitor.onEnter()
        {
        }
        void _ICameraMonitor.onExit()
        {
        }
        void _ICameraMonitor.onCameraPosChg()
        {
            // 刷新相机的位置，再刷新提示的展示
            refreshWnd();
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
        }
        void _ICameraMonitor.onPosLimiterChg()
        {
            refreshWnd();
        }
        void _ICameraMonitor.onFocusPosLimiterChg()
        {
        }
    }
}
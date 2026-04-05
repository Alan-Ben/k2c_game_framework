using ALPackage;
using System;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 控制聚焦到入口点的的wnd基类
    /// </summary>
    public abstract class _AGGUIBasicFocusEntryWnd<T> : _ATALBasicUIWnd<T> 
        where T : _ABasicUIWndFocusEntryMono
    {
        //原始fov
        protected float _m_origCameraViewValue;
        //原始位置
        protected Vector3 _m_origCameraPos;
        //关闭输入序列
        private int _m_inputMaskSerialize;
        //显示序列
        private long _m_lShowSerialize;
        //是否跳过打开窗口移动相机过程
        private bool _m_bIsSkipShowMoveCamera;
        //是否跳过关闭窗口移动相机过程
        private bool _m_bIsSkipHideMoveCamera;
        //是否正在移动相机
        private bool _m_bIsMovingCamera;

        /// <summary>
        /// 是否正在移动相机
        /// </summary>
        public bool isMovingCamera { get { return _m_bIsMovingCamera; } }

        protected _AGGUIBasicFocusEntryWnd(EALUIWndLayer _layer) : base(_layer) { }

        protected sealed override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsMovingCamera = false;
            _onShowWndEx();
            if (_m_bIsSkipShowMoveCamera)
                _onEnterFocusDone();
            else
                _moveCameraToEntry();

            //窗口打开后就重置
            _m_bIsSkipShowMoveCamera = false;
        }

        protected sealed override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsMovingCamera = false;
            _onHideWndEx();
            if(_m_bIsSkipHideMoveCamera)
                _onQuitFocusDone();
            else
                _returnMoveCameraToEntry();

            //窗口关闭后就重置
            _m_bIsSkipHideMoveCamera = false;
        }

        /// <summary>
        /// 是否跳过打开窗口移动相机过程
        /// </summary>
        /// <param name="_isSkip"></param>
        public void setSkipShowMoveCamera(bool _isSkip)
        {
            _m_bIsSkipShowMoveCamera = _isSkip;
        }

        /// <summary>
        /// 是否跳过关闭窗口移动相机过程
        /// </summary>
        public void setSkipHideMoveCamera(bool _isSkip)
        {
            _m_bIsSkipHideMoveCamera = _isSkip;
        }

        /// <summary>
        /// 设置相机原始位置
        /// </summary>
        /// <param name="_origCameraPos"></param>
        /// <param name="_origCameraViewValue"></param>
        public void setOriCameraValue(Vector3 _origCameraPos, float _origCameraViewValue)
        {
            _m_origCameraPos = _origCameraPos;
            _m_origCameraViewValue = _origCameraViewValue;
        }

        /// <summary>
        /// 把摄像头移动到建筑
        /// </summary>
        private void _moveCameraToEntry()
        {
            if (null == wnd)
                return;

            Transform target = _getTargetTD();
            if(null == target)
                return;
            
            //屏蔽点击拖拽
            long serialize = _m_lShowSerialize;
            _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_bIsMovingCamera = true;

            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize == _m_lShowSerialize)
                {
                    _onEnterFocusDone();
                    _m_bIsMovingCamera = false;
                }

                //开启点击拖拽
                MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
            },  wnd.durationTime);
         
            //先记录一下移动前位置
            _m_origCameraPos = CameraController.instance.cameraPos;
            _m_origCameraViewValue = CameraController.instance.controlCamera.orthographic
                ? CameraController.instance.cameraOrthographicSize
                : CameraController.instance.cameraFieldOfView;

            //移动fov
            _smoothChgFieldOfView(wnd.cameraScaleValue, wnd.durationTime, 0, null);
            
            //目标建筑位置 + ui偏移量等于最终位置
            Vector3 cameraTargetPos = target.position + _getOffsetOfTDEntrying();
            _m_origCameraPos.x = cameraTargetPos.x;//记录到目前x值
            _smoothMoveFocusTo(cameraTargetPos, wnd.durationTime, 0, target, null);
        }

        /// <summary>
        /// 把摄像头移动到建筑的反操作
        /// </summary>
        private void _returnMoveCameraToEntry()
        {
            if (_m_origCameraViewValue == 0)
                return;

            if (wnd == null)
                return;

            float durationTime = wnd.durationTime;

            //屏蔽点击拖拽
            long serialize = _m_lShowSerialize;
            _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_bIsMovingCamera = true;

            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize == _m_lShowSerialize)
                {
                    _onQuitFocusDone();
                    _m_bIsMovingCamera = false;
                }

                //开启点击拖拽
                MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
            }, durationTime);

            //移动fov
            _smoothChgFieldOfView(_m_origCameraViewValue, durationTime, 0, null);
            //移动位置
            _smoothMoveFocusTo(_m_origCameraPos, durationTime, 0, null, null);
        }

        /// <summary>
        /// 计算ui偏移量偏移位置
        /// </summary>
        /// <returns></returns>
        private Vector3 _getOffsetOfTDEntrying()
        {
            if (null == wnd || wnd.transFocusPos == null)
                return new Vector2(0f, 0f);

            //定位点的屏幕坐标
            Vector2 targetScreenPoint = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(wnd.transFocusPos.position);
            Vector2 midScreenPoint = new Vector2(Screen.width / 2, Screen.height / 2);;
            
            //获取相机采样器
            _ALogicPlane2DPosGetter cameraMoveGetter = _getLogicPlanePosGetter();

            if (cameraMoveGetter == null)
                return Vector3.zero;
            
            cameraMoveGetter.intersectionWorldPointWithWorldRay(
                CameraController.instance.screenPosToRay(midScreenPoint), out Vector3 groundMid);
            cameraMoveGetter.intersectionWorldPointWithWorldRay(
                CameraController.instance.screenPosToRay(targetScreenPoint), out Vector3 groundViewport);
            
            return groundMid - groundViewport;
        }

        /// <summary>
        /// 相机进入聚焦移动完成需要做的事
        /// </summary>
        protected virtual void _onEnterFocusDone(){}

        /// <summary>
        /// 相机退出聚焦移动完成需要做的事
        /// </summary>
        protected virtual void _onQuitFocusDone(){}
        
        /// <summary>
        /// 显示窗口的事件函数
        /// </summary>
        protected abstract void _onShowWndEx();
        /// <summary>
        /// 隐藏窗口的事件函数
        /// </summary>
        protected abstract void _onHideWndEx();
        /// <summary>
        /// 获取目标td的位置
        /// </summary>
        protected abstract Transform _getTargetTD();
        /// <summary>
        /// 移动fov到指定位置
        /// </summary>
        protected abstract void _smoothChgFieldOfView(float _targetValue, float _duration, float _accTime, Action _onComplete);
        /// <summary>
        /// 移动焦点到指定位置
        /// </summary>
        protected abstract void _smoothMoveFocusTo(Vector3 _targetPos, float _duration, float _accTime, Transform _targetTans, Action _onComplete);
        /// <summary>
        /// 获得坐标转换器
        /// </summary>
        protected abstract _ALogicPlane2DPosGetter _getLogicPlanePosGetter();
    }
}

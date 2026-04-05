
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 自定义的相机移动
    /// </summary>
    /// <remarks>
    /// 脚本 enable 时会获得相机的控制权，但不是强制占有，如果其它地方使用了相机对应功能，控制权会被夺取
    /// </remarks>
    public class GTDCustomMonoCameraMovement : MonoBehaviour
#if NP_GAME
        , _ICameraCustomMovement
#endif
    {
        [ALHeader("退出时是否要还原相机的原始值")]
        public bool setOriginValueWhenExit = true;
        [ALHeader("是否要打断上一个相机移动操作")]
        public bool interruptPreviousMovement = true;
        [ALHeader("是否在启用时屏蔽点击事件")]
        public bool disableInputWhenEnable = true;
        [ALInfo("在脚本 enable 后，相机的 Transform 会保持和这个对象一致")]
        [ALHeader("相机移动的代理对象")]
        public Transform cameraTransformAgent;


#if NP_GAME
        private bool _m_setOriginValueWhenExit;
        private bool _m_disableInputWhenEnable;
        private int _m_inputMaskSerialize;
        private Vector3 _m_originCameraPos;
        private Vector3 _m_originCameraFocusPos;
        private CameraPosTransformSynchronizeController _m_posController;
        private CameraFocusPosTransformSynchronizeController _m_focusPosController;
            

        /// <summary>
        /// 是否要打断上一个相机移动操作
        /// </summary>
        bool _ICameraCustomMovement.interruptPreviousMovement { get { return interruptPreviousMovement; } }
        
        
        public void OnEnable()
        {
            CameraCustomMovementMgr.instance.doMovement(this);
        }
        public void OnDisable()
        {
            CameraCustomMovementMgr.instance.cancelMovement(this);
        }
        
        
        /// <summary>
        /// 当这个移动生效时的处理
        /// </summary>
        protected virtual void _enableMovement()
        {
            _m_setOriginValueWhenExit = setOriginValueWhenExit;
            _m_disableInputWhenEnable = disableInputWhenEnable;
            
            // 如果需要还原原始值，记录原始值
            if (_m_setOriginValueWhenExit)
            {
                _m_originCameraPos = CameraController.instance.cameraPos;
                _m_originCameraFocusPos = CameraController.instance.cameraFocusPos;
            }
            // 关闭位置限制器
            CameraController.instance.pausePosLimiter();
            // 关闭输入
            if (_m_disableInputWhenEnable)
                _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            
            // 启用相机的位置和焦点位置控制器
            _m_posController = new CameraPosTransformSynchronizeController(cameraTransformAgent);
            _m_focusPosController = new CameraFocusPosTransformSynchronizeController(cameraTransformAgent);
            CameraController.instance.setCameraPosController(_m_posController);
            CameraController.instance.setCameraFocusController(_m_focusPosController);
        }
        /// <summary>
        /// 当这个移动失效时的处理
        /// </summary>
        protected virtual void _disableMovement()
        {
            // 恢复输入
            if (_m_disableInputWhenEnable)
                MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
            // 如果需要还原原始值，还原原始值
            if (_m_setOriginValueWhenExit)
            {
                // 先整体关闭限制器，再还原位置，最后再开启限制器，否则值设入的瞬间就会受到限制器的影响而发生改变
                CameraController.instance.pausePosLimiter();
                
                // 还原位置
                if (CameraController.instance.isPosControllerEnable(_m_posController))
                    CameraController.instance.cameraPos = _m_originCameraPos;
                if (CameraController.instance.isFocusPosControllerEnable(_m_focusPosController))
                    CameraController.instance.cameraFocusPos = _m_originCameraFocusPos;
                
                // 打开限制器后再使用还原好的数值刷新限制器，打开的限制器要下一帧才会发挥效果，在这之前刷新限制器即可
                CameraController.instance.resumePosLimiter();
            }
            else
            {
                _m_posController?.setMovingDone();
                _m_focusPosController?.setMovingDone();
            }
            
            // 恢复限制器，这个接口内部有计数，所以调用次数必须和 pausePosLimiter 成对出现
            CameraController.instance.resumePosLimiter();
            
            // 统一刷新一次限制器
            _refreshPosLimiter();
            
            _m_posController = null;
            _m_focusPosController = null;
        }


        // 刷新限制器的范围，相机的视角和视野大小会影响限制器的范围
        private void _refreshPosLimiter()
        {
            if (CameraController.instance.cameraPosLimiter is ScenePosLimiter scenePosLimiter)
                scenePosLimiter.refresh();
            if (CameraController.instance.cameraFocusPosLimiter is SceneFocusPosLimiter sceneFocusPosLimiter)
                sceneFocusPosLimiter.refresh();
        }
        
        
        void _ICameraCustomMovement.enableMovement()
        {
            _enableMovement();
        }
        void _ICameraCustomMovement.disableMovement()
        {
            _disableMovement();
        }
#endif
    }
}
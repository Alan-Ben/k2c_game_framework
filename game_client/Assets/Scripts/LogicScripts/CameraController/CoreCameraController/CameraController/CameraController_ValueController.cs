using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class CameraController
    {
        // 这里都是相机内部的控制器
        // 这些类存在的理由主要是内部有一些值需要在内部处理，不暴露给外部。
        
        private class _CameraMoveController : _IALCameraPosController, _IALCameraFocusController, _IHasBeforeLimitationPos, _IHasBeforeLimitationFocusPos
        {
            [NotNull] private readonly _ACameraMoveController _m_moveController;
            private readonly Vector3 _m_focusOffset;
            
            private Vector3 _m_cameraPos;
            private Vector3 _m_currentCameraPos;
            private Vector3 _m_currentFocusPos;
            private bool _m_bIsMovingDone;
            
            public _CameraMoveController([NotNull] _ACameraMoveController _moveController, _IHasBeforeLimitationPos _beforePos, _IHasBeforeLimitationFocusPos _beforeFocusPos)
            {
                _m_moveController = _moveController;
                _m_currentCameraPos = _m_cameraPos = _beforePos?.cameraPosBeforeLimitation ?? instance.cameraPos;
                _m_currentFocusPos = _beforeFocusPos?.focusPosBeforeLimitation ?? instance.cameraFocusPos;
                _m_focusOffset = _m_currentFocusPos - _m_currentCameraPos;
                _m_moveController.start(instance.cameraPos, instance.cameraFocusPos, _m_currentCameraPos, _m_currentFocusPos, instance.cameraPosLimiter, instance.cameraFocusPosLimiter);
                _m_bIsMovingDone = false;
            }
            
            _IALCameraPosController _IALCameraPosController.checkUpdate()
            {
                _m_currentCameraPos = _m_cameraPos = _m_moveController.updateCameraPos(instance.cameraPosLimiter, instance.cameraFocusPosLimiter);
                if (_m_moveController.isMovingDone)
                {
                    _m_bIsMovingDone = true;
                    instance._m_posStaticController.update(_m_cameraPos);
                    return instance._m_posStaticController;
                }
                
                if (instance.cameraPosLimiter != null)
                    _m_currentCameraPos = instance.cameraPosLimiter.limitPos(_m_currentCameraPos, out bool isLimited);
                
                return this;
            }
            _IALCameraFocusController _IALCameraFocusController.checkUpdate()
            {
                _m_currentFocusPos = _m_cameraPos + _m_focusOffset;
                if (_m_bIsMovingDone)
                {
                    instance._m_focusPosStaticController.update(_m_currentFocusPos);
                    return instance._m_focusPosStaticController;
                }
                
                if (instance.cameraFocusPosLimiter != null)
                    _m_currentFocusPos = instance.cameraFocusPosLimiter.limitPos(_m_currentFocusPos, out bool isLimited);
                
                return this;
            }

            public Vector3 cameraPosBeforeLimitation { get { return _m_cameraPos; } }
            public Vector3 cameraPos { get { return _m_currentCameraPos; } }
            public Vector3 focusPosBeforeLimitation { get { return _m_cameraPos + _m_focusOffset; } }
            public Vector3 focusPoint { get { return _m_currentFocusPos; } }

            public Vector3 focusPointMoveSpeed { get { return Vector3.zero; } }
            public Vector3 targetFocusPoint { get { return Vector3.zero; } }            
            public Vector3 cameraMoveSpeed { get { return Vector3.zero; } }
            public Vector3 cameraTargetPos { get { return Vector3.zero; } }
            public bool isMoving { get { return true; } }
            public int priority { get { return 0; } }
            public _ACameraMoveController realController { get { return _m_moveController; } }
        }
        private class _CameraPosController : _IALCameraPosController, _IHasBeforeLimitationPos
        {
            [NotNull] private readonly _ACameraPosController _m_posController;
            private Vector3 _m_cameraPos;
            private Vector3 _m_currentCameraPos;

            public _CameraPosController([NotNull] _ACameraPosController _posController, _IHasBeforeLimitationPos _beforePos)
            {
                _m_posController = _posController;
                _m_currentCameraPos = _m_cameraPos = _beforePos?.cameraPosBeforeLimitation ?? instance.cameraPos;
                _m_posController.start(instance.cameraPos, _m_currentCameraPos, instance.cameraPosLimiter);
            }
            
            public _IALCameraPosController checkUpdate()
            {
                _m_currentCameraPos = _m_cameraPos = _m_posController.updateCameraPos(instance.cameraPosLimiter);
                if (_m_posController.isMovingDone)
                {
                    instance._m_posStaticController.update(_m_currentCameraPos);
                    return instance._m_posStaticController;
                }
                
                if (instance.cameraPosLimiter != null)
                    _m_currentCameraPos = instance.cameraPosLimiter.limitPos(_m_currentCameraPos, out bool isLimited);

                return this;
            }

            public bool isMoving { get { return true; } }
            public Vector3 cameraPosBeforeLimitation { get { return _m_cameraPos; } }
            public Vector3 cameraPos { get { return _m_currentCameraPos; } }
            public Vector3 cameraMoveSpeed { get { return Vector3.zero; } }
            public Vector3 cameraTargetPos { get { return Vector3.zero; } }
            public int priority { get { return 0; } }
            public _ACameraPosController realController { get { return _m_posController; } }
        }
        private class _CameraFocusPosController : _IALCameraFocusController, _IHasBeforeLimitationFocusPos
        {
            [NotNull] private readonly _ACameraFocusPosController _m_focusPosController;
            private Vector3 _m_focusPos;
            private Vector3 _m_currentFocusPos;
            
            public _CameraFocusPosController([NotNull] _ACameraFocusPosController _focusPosController, _IHasBeforeLimitationFocusPos _beforeFocusPos)
            {
                _m_focusPosController = _focusPosController;
                _m_currentFocusPos = _m_focusPos = _beforeFocusPos?.focusPosBeforeLimitation ?? instance.cameraFocusPos;
                _m_focusPosController.start(instance.cameraFocusPos, _m_currentFocusPos, instance.cameraFocusPosLimiter);
            }

            public _IALCameraFocusController checkUpdate()
            {
                _m_currentFocusPos = _m_focusPos = _m_focusPosController.updateFocusPos(instance.cameraFocusPosLimiter);
                if (_m_focusPosController.isMovingDone)
                {
                    instance._m_focusPosStaticController.update(_m_currentFocusPos);
                    return instance._m_focusPosStaticController;
                }
                
                if (instance.cameraFocusPosLimiter != null)
                    _m_currentFocusPos = instance.cameraFocusPosLimiter.limitPos(_m_currentFocusPos, out bool isLimited);

                return this;
            }

            public Vector3 focusPosBeforeLimitation { get { return _m_focusPos; } }
            public Vector3 focusPoint { get { return _m_currentFocusPos; } }
            public Vector3 focusPointMoveSpeed { get { return Vector3.zero; } }
            public Vector3 targetFocusPoint { get { return Vector3.zero; } }
            public bool isMoving { get { return true; } }
            public int priority { get { return 0; } }
            public _ACameraFocusPosController realController { get { return _m_focusPosController; } }
        }
        private class _CameraFieldOfViewController : _IALCameraFieldOfViewController
        {
            [NotNull] private readonly _ACameraFieldOfViewController _m_fieldOfViewController;
            private float _m_currentFieldOfView;
            
            public _CameraFieldOfViewController([NotNull] _ACameraFieldOfViewController _fieldOfViewController)
            {
                _m_fieldOfViewController = _fieldOfViewController;
                _m_currentFieldOfView = instance.cameraFieldOfView;
                _m_fieldOfViewController.start(_m_currentFieldOfView);
            }

            public _IALCameraFieldOfViewController checkUpdate()
            {
                _m_currentFieldOfView = _m_fieldOfViewController.updateFieldOfView();
                if (_m_fieldOfViewController.isMovingDone)
                {
                    instance._m_fieldOfViewStaticController.update(_m_currentFieldOfView);
                    return instance._m_fieldOfViewStaticController;
                }

                return this;
            }

            public float fieldOfView { get { return _m_currentFieldOfView; } }
            public float fieldOfViewChgSpeed { get { return 0; } }
            public float fieldOfViewTargetValue { get { return 0; } }
            public bool isMoving { get { return true; } }
            public int priority { get { return 0; } }
            public _ACameraFieldOfViewController realController { get { return _m_fieldOfViewController; } }
        }
        private class _CameraOrthographicSizeController : _AALCameraOrthographicSizeController
        {
            [NotNull] private readonly _ACameraOrthographicSizeController _m_orthographicSizeController;
            private float _m_currentOrthographicSize;
            
            public _CameraOrthographicSizeController([NotNull] _ACameraOrthographicSizeController _orthographicSizeController)
            {
                _m_orthographicSizeController = _orthographicSizeController;
                _m_currentOrthographicSize = instance.cameraOrthographicSize;
                _m_orthographicSizeController.start(_m_currentOrthographicSize);
            }
            
            public override _AALCameraOrthographicSizeController checkUpdate()
            {
                _m_currentOrthographicSize = _m_orthographicSizeController.updateOrthographicSize();
                if (_m_orthographicSizeController.isMovingDone)
                {
                    instance._m_orthographicSizeStaticController.update(_m_currentOrthographicSize);
                    return instance._m_orthographicSizeStaticController;
                }
                
                return this;
            }

            public override float orthographicSize { get { return _m_currentOrthographicSize; } }
            public override float orthographicSizeChgSpeed { get { return 0; } }
            public override float orthographicSizeTargetValue { get { return 0; } }
            public override bool isMoving { get { return true; } }
            public override int priority { get { return 0; } }
            public _ACameraOrthographicSizeController realController { get { return _m_orthographicSizeController; } }
        }
        
        
        private class CameraPosStaticController : _IALCameraPosController
        {
            private Vector3 _m_lastCameraPos;
            private Vector3 _m_cameraPos;

            private Vector3 _m_smoothDampVelocity;

            public void update(Vector3 _cameraPos)
            {
                _m_cameraPos = _cameraPos;
                // 如果存在限制器，要限制一下传入的值
                if (instance.cameraPosLimiter != null)
                    _m_cameraPos = instance.cameraPosLimiter.limitPos(_m_cameraPos, out bool isLimited);

                _m_lastCameraPos = _m_cameraPos;
                // 重置速度
                _m_smoothDampVelocity = Vector3.zero;
            }

            public _IALCameraPosController checkUpdate()
            {
                _m_lastCameraPos = _m_cameraPos;
                // 如果存在坐标限制器，尝试判断坐标是否在稳定区域内，如果不在就移动到稳定区域内
                if (instance.cameraPosLimiter != null && !instance.cameraPosLimiter.isPosInStableArea(_m_cameraPos, out Vector3 closestPos))
                    _m_cameraPos = Vector3.SmoothDamp(_m_cameraPos, closestPos, ref _m_smoothDampVelocity, 0.1f);
                
                return this;
            }
            public Vector3 cameraPos { get { return _m_lastCameraPos; } }
            public bool isMoving { get { return false; } }
            public Vector3 cameraMoveSpeed { get { return Vector3.zero; } }
            public Vector3 cameraTargetPos { get { return Vector3.zero; } }
            public int priority { get { return 0; } }
        }
        private class CameraFocusPosStaticController : _IALCameraFocusController
        {
            private Vector3 _m_lastFocusPos;
            private Vector3 _m_focusPos;
            private Vector3 _m_smoothDampVelocity;

            public void update(Vector3 _focusPos)
            {
                _m_focusPos = _focusPos;
                // 如果存在限制器，要限制一下传入的值
                if (instance.cameraFocusPosLimiter != null)
                    _m_focusPos = instance.cameraFocusPosLimiter.limitPos(_m_focusPos, out bool isLimited);

                _m_lastFocusPos = _m_focusPos;
                // 重置速度
                _m_smoothDampVelocity = Vector3.zero;
            }

            public _IALCameraFocusController checkUpdate()
            {
                _m_lastFocusPos = _m_focusPos;
                // 如果存在坐标限制器，尝试判断坐标是否在稳定区域内，如果不在就移动到稳定区域内
                if (instance.cameraFocusPosLimiter != null && !instance.cameraFocusPosLimiter.isPosInStableArea(_m_focusPos, out Vector3 closestPos))
                    _m_focusPos = Vector3.SmoothDamp(_m_focusPos, closestPos, ref _m_smoothDampVelocity, 0.1f);
                
                return this;
            }
            public Vector3 focusPoint { get { return _m_lastFocusPos; } }
            public bool isMoving { get { return false; } }
            public Vector3 focusPointMoveSpeed { get { return Vector3.zero; } }
            public Vector3 targetFocusPoint { get { return Vector3.zero; } }
            public int priority { get { return 0; } }
        }
        private class CameraFieldOfViewStaticController : _IALCameraFieldOfViewController
        {
            private float _m_fieldOfView;

            public void update(float _fieldOfView)
            {
                _m_fieldOfView = _fieldOfView;
            }
            
            public _IALCameraFieldOfViewController checkUpdate() { return this; }

            public float fieldOfView { get { return _m_fieldOfView; } }
            public bool isMoving { get { return false; } }
            public float fieldOfViewChgSpeed { get { return 0; } }
            public float fieldOfViewTargetValue { get { return 0; } }
            public int priority { get { return 0; } }
        }
        private class CameraOrthographicSizeStaticController : _AALCameraOrthographicSizeController
        {
            private float _m_orthographicSize;

            public void update(float _orthographicSize)
            {
                _m_orthographicSize = _orthographicSize;
            }
            
            public override _AALCameraOrthographicSizeController checkUpdate() { return this; }

            public override float orthographicSize { get { return _m_orthographicSize; } }
            public override bool isMoving { get { return false; } }
            public override float orthographicSizeChgSpeed { get { return 0; } }
            public override float orthographicSizeTargetValue { get { return 0; } }
            public override int priority { get { return 0; } }
        }

        private interface _IHasBeforeLimitationPos
        {
            public Vector3 cameraPosBeforeLimitation { get; }
        }
        private interface _IHasBeforeLimitationFocusPos
        {
            public Vector3 focusPosBeforeLimitation { get; }   
        }
    }
}
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <remarks>
    /// 这个类设置相机的参数方法，都要等到 LateUpdate 运行时才能生效，或是直接调用 frameCheck
    /// </remarks>
    public sealed partial class CameraController
    {
        [NotNull] private readonly CameraPosStaticController _m_posStaticController; // 相机静态位置控制器
        [NotNull] private readonly CameraFocusPosStaticController _m_focusPosStaticController; // 相机静态看向的点控制器
        [NotNull] private readonly CameraFieldOfViewStaticController _m_fieldOfViewStaticController; // 相机静态 Field Of View 控制器
        [NotNull] private readonly CameraOrthographicSizeStaticController _m_orthographicSizeStaticController; // 相机静态正交尺寸控制器
        
        private float _m_cameraNearClipPlane = 0.3f; // 相机的近裁剪面
        private float _m_cameraFarClipPlane = 1000; // 相机的远裁剪面
        private bool _m_cameraIsOrthographic; // 是否是正交相机
        private bool _m_cameraIsOpenRender; // 是否开启渲染

        private uint _m_pausePosLimiterCount; // 是否要暂停坐标限制器的引用计数
        private _APosLimiter _m_cameraPosLimiter; // 相机坐标限制器
        private _APosLimiter _m_cameraFocusPosLimiter; // 相机看向的点限制器

        /// <summary>
        /// 当前相机的坐标
        /// </summary>
        /// <remarks>
        /// <para>如果你不熟悉 CameraController 你可能看不懂这条 remarks ，但如果你想要的是 Camera Transform 组件的 position 那么用
        /// <see cref="cameraPosWithExtraOffset"/> 来代替。</para>
        /// <para>这个 cameraPos 是不含有 ExtraController 的 offset 的，如果你想要得到相机含有 ExtraController 的 offset 可以使用
        /// cameraPosWithExtraOffset， 当然在没有 ExtraController 的时候他们两的值是相等的。</para>
        /// </remarks>
        public Vector3 cameraPos
        {
            get { return _m_alCameraController.posController?.cameraPos ?? (controlCamera == null ? Vector3.zero : controlCamera.transform.position); }
            set
            {
                _m_posStaticController.update(value);
                if (_m_alCameraController.posController != _m_posStaticController)
                    _m_alCameraController.setCameraPosController(_m_posStaticController);
                
                // 如果此时的 focusController 是平移控制器，就把平移操作取消掉
                if (_m_alCameraController.focusCOntroller is _CameraMoveController controller)
                {
                    _m_focusPosStaticController.update(controller.focusPosBeforeLimitation);
                    _m_alCameraController.setCameraFocusController(_m_focusPosStaticController);
                }
            }
        }
        /// <summary>
        /// 当前相机加上了额外偏移值的坐标
        /// </summary>
        /// <remarks>
        /// <para>如果你不熟悉 CameraController 你可能不知道这是什么，你可以简单理解成这个是 Camera Transform 组件的 position。</para>
        /// <para>这个值是 CameraController 中的 cameraPos 加上 ExtraController 带来的位移差之后的 pos 就结果上来讲确实是 Camera
        /// Transform 组件的 position。</para>
        /// </remarks>
        public Vector3 cameraPosWithExtraOffset
        {
            get { return controlCamera == null ? Vector3.zero : controlCamera.transform.position; }
        }
        /// <summary>
        /// 当前相机看向的点
        /// </summary>
        /// <remarks>
        /// <para>如果你不熟悉 CameraController 你可能看不懂这条 remarks ，但如果你想要的是 Camera Transform 组件的 Z 轴所指向方向的
        /// 一个点，就使用 cameraFocusPosWithExtraOffset。</para>
        /// <para>这个 cameraFocusPos 是不含有 ExtraController 的 offset 的，如果你想要得到相机含有 ExtraController 的 focusPos 可以使用
        /// cameraFocusPosWithExtraOffset， 当然在没有 ExtraController 的时候他们两的值是相等的。</para>
        /// </remarks>
        public Vector3 cameraFocusPos
        {
            get { return _m_alCameraController.focusCOntroller?.focusPoint ?? _m_alCameraController.curFocusPos; }
            set
            {
                _m_focusPosStaticController.update(value);
                if (_m_alCameraController.focusCOntroller != _m_focusPosStaticController)
                    _m_alCameraController.setCameraFocusController(_m_focusPosStaticController);
             
                // 如果此时的 posController 是平移控制器，就把平移操作取消掉
                if (_m_alCameraController.posController is _CameraMoveController controller)
                {
                    _m_posStaticController.update(controller.cameraPosBeforeLimitation);
                    _m_alCameraController.setCameraPosController(_m_posStaticController);
                }
            }
        }
        /// <summary>
        /// 当前相机加上了额外偏移量的看向的点
        /// </summary>
        /// <remarks>
        /// <para>如果你不熟悉 CameraController 你可能不知道这是什么，你可以简单理解成这个是 Camera Transform 组件的 Z 轴所指向方向的
        /// 一个点。</para>
        /// <para>这个值是 CameraController 中的 cameraFocusPos 加上 ExtraController 带来的位移差之后的 pos。</para>
        /// </remarks>
        public Vector3 cameraFocusPosWithExtraOffset
        {
            get { return _m_alCameraController.curFocusPos; }
        }
        /// <summary>
        /// 当前相机的 Field Of View（透视下的视野宽度）
        /// </summary>
        public float cameraFieldOfView
        {
            get { return _m_alCameraController.fieldOfViewController?.fieldOfView ?? (controlCamera == null ? 60 : controlCamera.fieldOfView); }
            set
            {
                _m_fieldOfViewStaticController.update(value);
                if (_m_alCameraController.fieldOfViewController != _m_fieldOfViewStaticController)
                    _m_alCameraController.setCameraFieldOfViewController(_m_fieldOfViewStaticController);
            }
        }
        /// <summary>
        /// 当前相机的正交尺寸（正交下的视野宽度）
        /// </summary>
        public float cameraOrthographicSize
        {
            get { return _m_alCameraController.OrthogrphicSizeController?.orthographicSize ?? (controlCamera == null ? 5 : controlCamera.orthographicSize); }
            set
            {
                _m_orthographicSizeStaticController.update(value);
                if (_m_alCameraController.OrthogrphicSizeController != _m_orthographicSizeStaticController)
                    _m_alCameraController.setCameraOrthogrphicSizeController(_m_orthographicSizeStaticController);
            }
        }
        /// <summary>
        /// 经过适配后的相机正交尺寸，也是真正会赋值给相机的正交尺寸
        /// </summary>
        public float cameraOrthographicSizeAfterFitting
        {
            get { return _m_alCameraController.OrthographicSize; }
        }
        /// <summary>
        /// 默认的正交尺寸
        /// </summary>
        public float defaultOrthographicSize
        {
            get { return _m_alCameraController.defaultOrthographicSize; }
        }
        /// <summary>
        /// 当前正交值和设置的原始正交值的比例
        /// </summary>
        public float inverseProportion
        {
            get { return _m_alCameraController.inverseProportion; }
        }
        /// <summary>
        /// 当前相机的近裁剪面
        /// </summary>
        public float cameraNearClipPlane 
        { 
            get { return _m_cameraNearClipPlane; }
            set
            {
                if (_m_cameraNearClipPlane == value)
                    return;
                
                _m_cameraNearClipPlane = value; 
                if (controlCamera != null)
                    controlCamera.nearClipPlane = _m_cameraNearClipPlane;
                if (_m_childCameras != null)
                {
                    foreach (Camera childCamera in _m_childCameras)
                    {
                        childCamera.nearClipPlane = _m_cameraNearClipPlane;
                    }
                }
            } 
        }
        /// <summary>
        /// 当前相机的远裁剪面
        /// </summary>
        public float cameraFarClipPlane 
        { 
            get { return _m_cameraFarClipPlane; }
            set
            {
                if (_m_cameraFarClipPlane == value)
                    return;
                
                _m_cameraFarClipPlane = value; 
                if (controlCamera != null)
                    controlCamera.farClipPlane = _m_cameraFarClipPlane;
                if (_m_childCameras != null)
                {
                    foreach (Camera childCamera in _m_childCameras)
                    {
                        childCamera.farClipPlane = _m_cameraFarClipPlane;
                    }
                }
            }
        }
        /// <summary>
        /// 是否是正交相机
        /// </summary>
        public bool cameraIsOrthographic 
        { 
            get { return _m_cameraIsOrthographic; }
            set
            {
                _m_cameraIsOrthographic = value; 
                if (controlCamera != null)
                    controlCamera.orthographic = _m_cameraIsOrthographic;
                if (_m_childCameras != null)
                {
                    foreach (Camera childCamera in _m_childCameras)
                    {
                        childCamera.orthographic = _m_cameraIsOrthographic;
                    }
                }
            } 
        }
        /// <summary>
        /// 是否开启了相机的渲染
        /// </summary>
        public bool cameraIsOpenRender 
        { 
            get { return _m_cameraIsOpenRender; }
            set
            {
                _m_cameraIsOpenRender = value; 
                
                URPCameraManager.instance.setMainCameraEnable(_m_cameraIsOpenRender);

                // if (controlCamera != null)
                //     controlCamera.cullingMask = _m_cameraIsOpenRender ? _m_cullingMask : 0;
                
                if (_m_childCameras != null)
                {
                    for (int i = 0; i < _m_childCameras.Length; i++)
                    {
                        Camera childCamera = _m_childCameras[i];
                        if (childCamera == null)
                            continue;
                        
                        childCamera.cullingMask = _m_cameraIsOpenRender ? _m_childCameraCullingMasks[i] : 0;
                    }
                }
            }
        }
        /// <summary>
        /// 相机的前方向
        /// </summary>
        public Vector3 cameraForward { get { return cameraFocusPos - cameraPos; } }
        /// <summary>
        /// 这个相机的旋转值
        /// </summary>
        public Quaternion cameraRotation { get { return Quaternion.LookRotation(cameraForward); } }
        /// <summary>
        /// 相机的坐标限制器
        /// </summary>
        public _APosLimiter cameraPosLimiter { get { return _m_pausePosLimiterCount > 0 ? null : _m_cameraPosLimiter; } }
        /// <summary>
        /// 相机的焦点限制器
        /// </summary>
        public _APosLimiter cameraFocusPosLimiter { get { return _m_pausePosLimiterCount > 0 ? null : _m_cameraFocusPosLimiter; } }

        /// <summary>
        /// 设置相机移动的控制器，移动期间会保持相机的 rotation 不变
        /// </summary>
        public void setCameraMoveController(_ACameraMoveController _cameraMoveController)
        {
            if (_cameraMoveController == null)
                return;
            
            _CameraMoveController moveController = 
                new _CameraMoveController(_cameraMoveController, 
                    _m_alCameraController.posController as _IHasBeforeLimitationPos,
                    _m_alCameraController.focusCOntroller as _IHasBeforeLimitationFocusPos);
            _m_alCameraController.setCameraPosController(moveController);
            _m_alCameraController.setCameraFocusController(moveController);
        }
        /// <summary>
        /// 设置相机坐标的控制器，期间不会改变相机看向的点
        /// </summary>
        public void setCameraPosController(_ACameraPosController _cameraPosController)
        {
            if (_cameraPosController == null)
                return;
            
            _CameraPosController posController = 
                new _CameraPosController(_cameraPosController, _m_alCameraController.posController as _IHasBeforeLimitationPos);
            _m_alCameraController.setCameraPosController(posController);
            // 这个方法和上面的 setCameraMoveController 互斥，这里要判断是否是上面的方法，是的话把 Controller 取消掉
            if (_m_alCameraController.focusCOntroller is _CameraMoveController controller)
            {
                _m_focusPosStaticController.update(controller.focusPosBeforeLimitation);
                _m_alCameraController.setCameraFocusController(_m_focusPosStaticController);
            }
        }
        public void setCameraFocusController(_ACameraFocusPosController _cameraFocusPosController)
        {
            if (_cameraFocusPosController == null)
                return;
            
            _CameraFocusPosController focusPosController = 
                new _CameraFocusPosController(_cameraFocusPosController, _m_alCameraController.focusCOntroller as _IHasBeforeLimitationFocusPos);
            _m_alCameraController.setCameraFocusController(focusPosController);
            // 这个方法和上面的 setCameraMoveController 互斥，这里要判断是否是上面的方法，是的话把 Controller 取消掉
            if (_m_alCameraController.posController is _CameraMoveController controller)
            {
                _m_posStaticController.update(controller.cameraPosBeforeLimitation);
                _m_alCameraController.setCameraPosController(_m_posStaticController);
            }
        }
        public void setCameraFieldOfViewController(_ACameraFieldOfViewController _cameraFieldOfViewController)
        {
            if (_cameraFieldOfViewController == null)
                return;
            
            _CameraFieldOfViewController fieldOfViewController = new _CameraFieldOfViewController(_cameraFieldOfViewController);
            _m_alCameraController.setCameraFieldOfViewController(fieldOfViewController);
        }
        public void setCameraOrthographicSizeController(_ACameraOrthographicSizeController _cameraOrthographicSizeController)
        {
            if (_cameraOrthographicSizeController == null)
                return;
            
            _CameraOrthographicSizeController orthographicSizeController = new _CameraOrthographicSizeController(_cameraOrthographicSizeController);
            _m_alCameraController.setCameraOrthogrphicSizeController(orthographicSizeController);
        }
        /// <summary>
        /// 根据输入的范围，刷新相机的裁剪面
        /// </summary>
        public void refreshClipPlanesByBounds(Bounds _bounds)
        {
            refreshClipPlanesByBounds(_bounds, Quaternion.identity);
        }
        /// <summary>
        /// 划定一个范围，刷新相机的裁剪面
        /// </summary>
        public void refreshClipPlanesByBounds(Bounds _bounds, Quaternion _quaternion)
        {
            Vector3[] points = new Vector3[8];
            points[0] = _bounds.center + _quaternion * new Vector3(_bounds.extents.x, _bounds.extents.y, _bounds.extents.z);
            points[1] = _bounds.center + _quaternion * new Vector3(_bounds.extents.x, _bounds.extents.y, -_bounds.extents.z);
            points[2] = _bounds.center + _quaternion * new Vector3(_bounds.extents.x, -_bounds.extents.y, _bounds.extents.z);
            points[3] = _bounds.center + _quaternion * new Vector3(_bounds.extents.x, -_bounds.extents.y, -_bounds.extents.z);
            points[4] = _bounds.center + _quaternion * new Vector3(-_bounds.extents.x, _bounds.extents.y, _bounds.extents.z);
            points[5] = _bounds.center + _quaternion * new Vector3(-_bounds.extents.x, _bounds.extents.y, -_bounds.extents.z);
            points[6] = _bounds.center + _quaternion * new Vector3(-_bounds.extents.x, -_bounds.extents.y, _bounds.extents.z);
            points[7] = _bounds.center + _quaternion * new Vector3(-_bounds.extents.x, -_bounds.extents.y, -_bounds.extents.z);
            float maxDistance = float.NegativeInfinity;
            float minDistance = float.PositiveInfinity;
            Vector3 cameraForward = (cameraFocusPos - cameraPos).normalized;
            for (int i = 0; i < 8; i++)
            {
                Vector3 point = points[i];
                float distance = Vector3.Dot(cameraForward, point - cameraPos);
                if (distance > maxDistance)
                    maxDistance = distance;
                if (distance < minDistance)
                    minDistance = distance;
            }

            if (maxDistance == minDistance ||
                maxDistance < 0 ||
                minDistance < 0)
            {
                ALLog.Error("[CameraController] refreshClipPlanesByBounds 失败，输入的 bounds 计算出的裁剪平面为不合法的值");
                return;
            }
            cameraFarClipPlane = maxDistance;
            cameraNearClipPlane = minDistance;
        }

        public void setCameraPosLimiter(_APosLimiter _cameraPosLimiter)
        {
            _m_cameraPosLimiter = _cameraPosLimiter;
            // 如果当前使用的是静态控制器，那么就更新一下，限制第一帧的位置
            if (_m_alCameraController.posController == _m_posStaticController)
                _m_posStaticController.update(cameraPos);
            
            foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
            {
                monitor.onPosLimiterChg();
            }
        }
        public void resetCameraPosLimiter(_APosLimiter _cameraPosLimiter = null)
        {
            if (_cameraPosLimiter != null && _m_cameraPosLimiter != _cameraPosLimiter)
                return;
            
            _m_cameraPosLimiter = null;
            
            foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
            {
                monitor.onPosLimiterChg();
            }
        }

        public void setCameraFocusPosLimiter(_APosLimiter _cameraFocusPosLimiter)
        {
            _m_cameraFocusPosLimiter = _cameraFocusPosLimiter;
            // 如果当前使用的是静态控制器，那么就更新一下，限制第一帧的位置
            if (_m_alCameraController.focusCOntroller == _m_focusPosStaticController)
                _m_focusPosStaticController.update(cameraFocusPos);
            
            foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
            {
                monitor.onFocusPosLimiterChg();
            }
        }
        public void resetCameraFocusPosLimiter(_APosLimiter _cameraFocusPosLimiter = null)
        {
            if (_cameraFocusPosLimiter != null && _m_cameraFocusPosLimiter != _cameraFocusPosLimiter)
                return;
            
            _m_cameraFocusPosLimiter = null;
            
            foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
            {
                monitor.onFocusPosLimiterChg();
            }
        }

        public void pausePosLimiter()
        {
            _m_pausePosLimiterCount++;
        }
        public void resumePosLimiter()
        {
            _m_pausePosLimiterCount--;
        }

        public bool isMoveControllerEnable(_ACameraMoveController _cameraMoveController)
        {
            if (_cameraMoveController == null)
                return false;
            
            if (_m_alCameraController.posController is not _CameraMoveController controller)
                return false;
            
            return controller.realController == _cameraMoveController;
        }
        public bool isPosControllerEnable(_ACameraPosController _cameraPosController)
        {
            if (_cameraPosController == null)
                return false;
            
            if (_m_alCameraController.posController is not _CameraPosController controller)
                return false;
            
            return controller.realController == _cameraPosController;
        }
        public bool isFocusPosControllerEnable(_ACameraFocusPosController _cameraFocusPosController)
        {
            if (_cameraFocusPosController == null)
                return false;
            
            if (_m_alCameraController.focusCOntroller is not _CameraFocusPosController controller)
                return false;
            
            return controller.realController == _cameraFocusPosController;
        }
        public bool isFieldOfViewControllerEnable(_ACameraFieldOfViewController _cameraFieldOfViewController)
        {
            if (_cameraFieldOfViewController == null)
                return false;
            
            if (_m_alCameraController.fieldOfViewController is not _CameraFieldOfViewController controller)
                return false;
            
            return controller.realController == _cameraFieldOfViewController;
        }
        public bool isOrthographicSizeControllerEnable(_ACameraOrthographicSizeController _cameraOrthographicSizeController)
        {
            if (_cameraOrthographicSizeController == null)
                return false;
            
            if (_m_alCameraController.OrthogrphicSizeController is not _CameraOrthographicSizeController controller)
                return false;
            
            return controller.realController == _cameraOrthographicSizeController;
        }
    }
}
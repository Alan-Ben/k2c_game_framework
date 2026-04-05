using System;
using System.Collections.Generic;

using UnityEngine;

/******************
 * 摄像头控制对象，根据不同的状态对象对摄像头进行控制
 **/
namespace ALPackage
{
    /***************
     * 摄像头移动状态枚举
     **/
    public enum EALCameraTimingTransMoveState
    {
        DEFAULT,            //默认
        FIRST_MOVE,         //第一阶段移动
        STAY,               //停留阶段
        RESET,              //重置移动阶段
        DONE,               //完成阶段
    }

    public class ALCameraController
    {
        /// <summary>
        /// 特定相机角度计算用的中间变量
        /// </summary>
        protected static readonly float g_radianToAngle = 2f * 180f / 3.1415926f;
        /** 控制的摄像头对象 */
        private Camera _m_cControlCamera;
        /** 摄像头的子控制对象 */
        private Camera[] _m_arrChildCamera;

        /** 摄像头位置控制对象 */
        private _IALCameraPosController _m_pcCameraPosController;
        /** 摄像头焦点位置控制对象 */
        private _IALCameraFocusController _m_fcCameraFocusController;
        /** 摄像头视角控制对象 */
        private _IALCameraFieldOfViewController _m_vcCameraFieldOfViewController;

        //之前在计算前的正交视野变量
        private float _m_fPreOriginFieldOfView = -1;
        /** 静态的额外附加操作控制对象，当设置新对象的时候会将旧对象替换 */
        private _IALCameraExtraController _m_cecStaticExtraController;
        /** 附加操作的控制对象队列 */
        private List<_IALCameraExtraController> _m_lExtraControllerList;

        /** 当前的焦点位置 */
        private Vector3 _m_vCurFocusPos;

        private float _m_fInverseProportion = 1.0f;//缩放反比例
        private float _m_fProportion = 1.0f;//缩放正比例

        private float _m_fDefaultOrthographicSize = 6;//Camera.orthographicSize初始值
        //之前在计算前的正交视野变量
        private float _m_fPreOriginOrthogrphicSize = -1f;
        //之前的正交视野值
        private float _m_fPreOrthogrphicSize = 0f;
        //正交视野控制对象
        private _AALCameraOrthographicSizeController _m_osOrthogrphicSizeController;

        //存储的当前摄像头朝向的临时变量
        //private Vector3 _m_vTmpCameraForward;
        private Quaternion _m_vTmpCameraForward;
        //存储额外移动对象
        private Vector3 _m_vTmpExtraPosition;
        private Vector3 _m_vTmpExtraFocusPosition;

        /// <summary>
        /// 是否开启摄像头的宽度适配
        /// 宽度适配模式下，将根据摄像头的标准宽高比，调整摄像头的prthogrphic参数，以使得摄像头的宽度保持在一定范围内（一般只针对正交相机有效）
        /// </summary>
        private bool _m_bOpenCameraWidthFit;
        /// <summary>
        /// 是否开启摄像头的宽度适配 ,透视相机
        /// 宽度适配模式下，将根据摄像头的标准宽高比，调整摄像头的prthogrphic参数，以使得摄像头的宽度保持在一定范围内（一般只针对正交相机有效）
        /// </summary>
        private bool _m_bOpenCameraWidthFitPerspective;
        /// <summary>
        /// 标准模式下的宽高比
        /// </summary>
        private Vector2 _m_vNormalWHRate;
        /// <summary>
        /// 如果需要匹配固定宽度，则需要调整的fieldofview值比例
        /// </summary>
        private float _m_fCurNormalRateForOrthogrphicSize;

        public ALCameraController(float _defaultOrthographicSize)
        {
            _m_cControlCamera = null;
            _m_arrChildCamera = null;

            _m_pcCameraPosController = null;
            _m_fcCameraFocusController = null;
            _m_vcCameraFieldOfViewController = null;
            _m_osOrthogrphicSizeController = null;

            _m_cecStaticExtraController = null;
            _m_lExtraControllerList = new List<_IALCameraExtraController>();

            _m_fDefaultOrthographicSize = _defaultOrthographicSize;
            _m_vCurFocusPos = Vector3.zero;

            _m_bOpenCameraWidthFit = false;
            _m_vNormalWHRate = Vector2.one;
            _m_fCurNormalRateForOrthogrphicSize = 1f;
        }

        public Camera controlCamera { get { return _m_cControlCamera; } }
        public bool isCameraMoving { get { if (null == _m_pcCameraPosController) return false; else return _m_pcCameraPosController.isMoving; } }
        public bool isFocusMoving { get { if (null == _m_fcCameraFocusController) return false; else return _m_fcCameraFocusController.isMoving; } }
        public float defaultOrthographicSize { get { return _m_fDefaultOrthographicSize; } }
        public float inverseProportion { get { return _m_fInverseProportion; } }
        public float Proportion { get { return _m_fProportion; } }
        /// <summary>
        /// 最终显示的屏幕缩放参数，可能会受到分辨率自适应CurNormalRateForOrthogrphicSize的影响
        /// </summary>
        public float OrthographicSize { get { return _m_fPreOrthogrphicSize; } }

        /// <summary>
        /// 没有进行屏幕宽度自适应的处理的值，如果需要对缩放进行控制，需要使用这个值进行计算
        /// </summary>
        public float OriginOrthographicSize { get { return _m_fPreOriginOrthogrphicSize; } }
        public float curNormalRateForOrthographicSize { get { return _m_fCurNormalRateForOrthogrphicSize; } }
        public float OriginFieldOfView { get { return _m_fPreOriginFieldOfView; } }

        public _IALCameraPosController posController { get { return _m_pcCameraPosController; } }
        public _IALCameraFocusController focusCOntroller { get { return _m_fcCameraFocusController; } }
        public _IALCameraFieldOfViewController fieldOfViewController { get { return _m_vcCameraFieldOfViewController; } }
        public _AALCameraOrthographicSizeController OrthogrphicSizeController { get { return _m_osOrthogrphicSizeController; } }

        public Vector3 curFocusPos { get { return _m_vCurFocusPos; } }

        /// <summary>
        /// 开启摄像头的宽度自适应，根据默认情况是高度不变，而由此调整fieldofview参数改动摄像头区域
        /// </summary>
        /// <param name="_normalRate"></param>
        /// <param name="_openPersPectiveWidthFit">开启透视相机的宽度自适应</param>
        /// <param name="_openOrthographicWidthFit">开启正交相机的宽度自适应</param>
        public void openNormalRateWidthFit(Vector2 _normalRate, bool _openPersPectiveWidthFit, bool _openOrthographicWidthFit)
        {
            if(null == _m_cControlCamera)
                return;

            _m_bOpenCameraWidthFit = _openOrthographicWidthFit;
          
            _m_vNormalWHRate.Set(1f, _normalRate.y / _normalRate.x);
            //需要关注摄像头本身的视野区域比例
            Vector2 screenR = new Vector2(1f, (Screen.height * controlCamera.rect.height) / (Screen.width * controlCamera.rect.width));

            //比较双方的比例
            _m_fCurNormalRateForOrthogrphicSize = screenR.y / _m_vNormalWHRate.y;
            if (_m_bOpenCameraWidthFit)
            {
                //调整倍率
                if(_m_fPreOriginOrthogrphicSize > 0)
                    _setOrthographicSize(_m_fPreOriginOrthogrphicSize * _m_fCurNormalRateForOrthogrphicSize);
                else
                    _setOrthographicSize(_m_cControlCamera.orthographicSize * _m_fCurNormalRateForOrthogrphicSize);
            }
         
            _m_bOpenCameraWidthFitPerspective = _openPersPectiveWidthFit;
            //调整透视相机倍率
            if (_m_bOpenCameraWidthFitPerspective)
            {
                _setPerspectiveFieldOfView(_m_fPreOriginFieldOfView > 0 ? _m_fPreOriginFieldOfView : _m_cControlCamera.fieldOfView);
            }
        }
        public void closeNormalRateWidthFit()
        {
            _m_bOpenCameraWidthFit = false;
            _m_bOpenCameraWidthFitPerspective = false;
        }

        /// <summary>
        /// 设置标准的正交视野范围
        /// </summary>
        /// <param name="_defaultOrthographicSize"></param>
        public void setDefaultOrthographicsSize(float _defaultOrthographicSize)
        {
            _m_fDefaultOrthographicSize = _defaultOrthographicSize;
        }

        /******************
         * 设置本对象控制的摄像头
         **/
        public void setControlCamera(Camera _camera)
        {
            if (null == _camera)
                return;

            _m_cControlCamera = _camera;

            //查询是否有子摄像头对象，如有则同步
            if(null != _m_cControlCamera)
                _m_arrChildCamera = _m_cControlCamera.transform.GetComponentsInChildren<Camera>();

            //设置基本静态对象
            _m_pcCameraPosController = new ALCameraPosControllerStaticPos(_camera.transform.position);
            _m_fcCameraFocusController = null;
            _m_vcCameraFieldOfViewController = new ALCameraFieldOfViewControllerStaticValue(_camera.fieldOfView);
        }

        /*******************
         * 设置摄像头位置控制对象
         **/
        public void setCameraPosController(_IALCameraPosController _controller)
        {
            _m_pcCameraPosController = _controller;
        }
        //设置摄像头位置变化
        public void setCameraPosTransMoveTo(int _priority, Vector3 _targetPos, float _totalTime, float _accTime)
        {
            setCameraPosController(
                new ALCameraPosControllerTransMove(_priority, _m_pcCameraPosController.cameraPos, _targetPos, _m_pcCameraPosController.cameraMoveSpeed, _totalTime, _accTime));
        }
        public void setCameraPosTimingTransMoveTo(int _priority, Vector3 _stayPos, float _totalTime, float _accTime, float _stayTime, float _fallbackTotalTime, float _fallbackAccTime)
        {
            setCameraPosController(
                new ALCameraPosControllerTimingTransMove(_priority, _m_pcCameraPosController.cameraPos, _stayPos, _m_pcCameraPosController.cameraMoveSpeed, _totalTime, _accTime, _stayTime, _m_pcCameraPosController.cameraTargetPos, _fallbackTotalTime, _fallbackAccTime));
        }

        /*******************
         * 设置摄像头焦点控制对象
         **/
        public void setCameraFocusController(_IALCameraFocusController _controller)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if (_controller != null)
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]setCameraFocusController {_controller.GetType()}");
                else
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]setCameraFocusController，_controller == null");
            }

            _m_fcCameraFocusController = _controller;
        }
        //设置摄像头焦点变化
        public void setCameraFocusTransMoveTo(int _priority, Vector3 _targetFocusPoint, float _totalTime, float _accTime)
        {
            setCameraFocusController(
                new ALCameraFocusControllerTransMove(_priority, _m_fcCameraFocusController.focusPoint, _targetFocusPoint, _m_fcCameraFocusController.focusPointMoveSpeed, _totalTime, _accTime));
        }
        public void setCameraFocusTimingTransMoveTo(int _priority, Vector3 _stayPoint, float _totalTime, float _accTime, float _stayTime, float _fallbackTotalTime, float _fallbackAccTime)
        {
            setCameraFocusController(
                new ALCameraFocusControllerTimingTransMove(_priority, _m_fcCameraFocusController.focusPoint, _stayPoint, _m_fcCameraFocusController.focusPointMoveSpeed, _totalTime, _accTime, _stayTime, _m_fcCameraFocusController.focusPoint, _fallbackTotalTime, _fallbackAccTime));
        }

        /*******************
         * 设置摄像头视野范围控制对象
         **/
        public void setCameraFieldOfViewController(_IALCameraFieldOfViewController _controller)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if (_controller != null)
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]setCameraFieldOfViewController {_controller.GetType()}");
                else
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]setCameraFieldOfViewController, _controller == null");
            }

            _m_vcCameraFieldOfViewController = _controller;
        }
        //设置摄像头视野变化
        public void setCameraFieldOfViewTransMoveTo(int _priority, float _targetFieldOfView, float _totalTime, float _accTime)
        {
            setCameraFieldOfViewController(
                new ALCameraFieldOfViewControllerTransMove(_priority, _m_vcCameraFieldOfViewController.fieldOfView, _targetFieldOfView, _m_vcCameraFieldOfViewController.fieldOfViewChgSpeed, _totalTime, _accTime));
        }
        public void setCameraFieldOfViewTimingTransMoveTo(int _priority, float _stayFieldOfView, float _totalTime, float _accTime, float _stayTime, float _fallbackTotalTime, float _fallbackAccTime)
        {
            setCameraFieldOfViewController(
                new ALCameraFieldOfViewControllerTimingTransMove(_priority, _m_vcCameraFieldOfViewController.fieldOfView, _stayFieldOfView, _m_vcCameraFieldOfViewController.fieldOfViewChgSpeed, _totalTime, _accTime, _stayTime, _m_vcCameraFieldOfViewController.fieldOfView, _fallbackTotalTime, _fallbackAccTime));
        }

        /*******************
         * 设置摄像头视野范围控制对象
         **/
        public void setCameraOrthogrphicSizeController(_AALCameraOrthographicSizeController _controller)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if (_controller != null)
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]setCameraOrthogrphicSizeController {_controller.GetType()}");
                else
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]setCameraOrthogrphicSizeController, _controller == null");
            }

            _m_osOrthogrphicSizeController = _controller;
        }

        //设置摄像头视野变化
        public void chgCameraOrthogrphicSizeTransMoveTo(float _delta, float _totalTime, float _accTime)
        {
            setCameraOrthogrphicSizeTransMoveTo(1, _m_osOrthogrphicSizeController.orthographicSize + _delta, _totalTime, _accTime);
        }


        public void setCameraOrthogrphicSizeTransMoveTo(int _priority, float _targetFieldOfView, float _totalTime, float _accTime)
        {
            setCameraOrthogrphicSizeController(
                new ALCameraOrthographicSizeControllerTransMove(_priority, _m_osOrthogrphicSizeController.orthographicSize, _targetFieldOfView, _m_osOrthogrphicSizeController.orthographicSizeChgSpeed, _totalTime, _accTime));
        }
        public void setCameraOrthogrphicSizeTimingTransMoveTo(int _priority, float _stayFieldOfView, float _totalTime, float _accTime, float _stayTime, float _fallbackTotalTime, float _fallbackAccTime)
        {
            setCameraOrthogrphicSizeController(
                new ALCameraOrthographicSizeControllerTimingTransMove(_priority, _m_osOrthogrphicSizeController.orthographicSize, _stayFieldOfView, _m_osOrthogrphicSizeController.orthographicSizeChgSpeed, _totalTime, _accTime, _stayTime, _m_osOrthogrphicSizeController.orthographicSize, _fallbackTotalTime, _fallbackAccTime));
        }

        /*******************
         * 注册一个附加位移信息的控制对象
         **/
        public void regExtraController(_IALCameraExtraController _controller)
        {
            if (null == _controller)
                return;

            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[ALCameraController]regExtraController {_controller.GetType()}");
            }
            _m_lExtraControllerList.Add(_controller);
        }

        /*******************
         * 注销一个附加位移信息的控制对象
         **/
        public void unregExtraController(_IALCameraExtraController _controller)
        {
            if (null == _controller)
                return;

            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[ALCameraController]unregExtraController {_controller.GetType()}");
            }

            _m_lExtraControllerList.Remove(_controller);
        }

        /*******************
         * 设置唯一的静态位移信息控制对象
         **/
        public void setStaticExtraController(_IALCameraExtraController _controller)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if (_controller != null)
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]setStaticExtraController {_controller.GetType()}");
                else
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]setStaticExtraController, _controller == null");

            }
            _m_cecStaticExtraController = _controller;
        }

        /*******************
         * 重置唯一的静态位移信息控制对象
         **/
        public void resetStaticExtraController()
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[ALCameraController]resetStaticExtraController.");
            }
            _m_cecStaticExtraController = null;
        }
        /// <summary>
        /// 重置静态的位移控制对象，且会在重置前对对象进行审核，如果对象不是带入参数的情况则不会重置
        /// </summary>
        /// <param name="_checkController"></param>
        public void resetStaticExtraController(_IALCameraExtraController _checkController)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                if (_checkController != null)
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]resetStaticExtraController {_checkController.GetType()}");
                else
                    Debug.Log($"【{Time.frameCount}】[ALCameraController]resetStaticExtraController, _checkController == null");
            }

            if (_checkController != _m_cecStaticExtraController)
                return;

            _m_cecStaticExtraController = null;
        }

        /**************
         * 每帧处理的检测函数
         **/
        public void frameCheck()
        {
            if (null == _m_cControlCamera)
                return;

            //进行附加位移的处理
            //获取当前朝向
            //_m_vTmpCameraForward = _m_cControlCamera.transform.forward;
            _m_vTmpCameraForward = _m_cControlCamera.transform.rotation;
            //存储额外移动对象
            _m_vTmpExtraPosition = Vector3.zero;
            _m_vTmpExtraFocusPosition = Vector3.zero;

            if(null != _m_cecStaticExtraController)
            {
                if (_m_cecStaticExtraController.enable)
                {
                    _m_vTmpExtraPosition += ALUnityCommon.getTransPosByCameraForward(_m_vTmpCameraForward, _m_cecStaticExtraController.getExtraPosition());
                    _m_vTmpExtraFocusPosition += ALUnityCommon.getTransPosByCameraForward(_m_vTmpCameraForward, _m_cecStaticExtraController.getExtraFocusPosition());

                    //附加绝对偏移
                    _m_vTmpExtraPosition += _m_cecStaticExtraController.getExtraOriginPosition();
                    _m_vTmpExtraFocusPosition += _m_cecStaticExtraController.getExtraOriginFocusPosition();
                }
                else
                {
                    //重置
                    _m_cecStaticExtraController = null;
                }
            }
            //计算附加移动的信息
            for(int i = 0; i < _m_lExtraControllerList.Count; )
            {
                _IALCameraExtraController controller = _m_lExtraControllerList[i];

                if (controller.enable)
                {
                    _m_vTmpExtraPosition += ALUnityCommon.getTransPosByCameraForward(_m_vTmpCameraForward, controller.getExtraPosition());
                    _m_vTmpExtraFocusPosition += ALUnityCommon.getTransPosByCameraForward(_m_vTmpCameraForward, controller.getExtraFocusPosition());

                    //附加绝对偏移
                    _m_vTmpExtraPosition += controller.getExtraOriginPosition();
                    _m_vTmpExtraFocusPosition += controller.getExtraOriginFocusPosition();

                    i++;
                }
                else
                {
                    //删除节点
                    _m_lExtraControllerList.RemoveAt(i);
                }
            }

            //进行移动坐标的每帧检测
            if(null != _m_pcCameraPosController)
            {
                _IALCameraPosController newPosController = _m_pcCameraPosController.checkUpdate();
                if (null != newPosController)
                    _m_pcCameraPosController = newPosController;

                //获取当前相关位置
                Vector3 cameraPos = _m_pcCameraPosController.cameraPos + _m_vTmpExtraPosition;
                //设置摄像头位置信息
                _m_cControlCamera.transform.position = cameraPos;
            }

            //进行摄像头焦点处理
            if (null != _m_fcCameraFocusController)
            {
                //进行焦点的每帧检测
                _IALCameraFocusController newFocusController = _m_fcCameraFocusController.checkUpdate();
                if (null != newFocusController)
                    _m_fcCameraFocusController = newFocusController;

                //获取当前相关位置
                _m_vCurFocusPos = _m_fcCameraFocusController.focusPoint + _m_vTmpExtraFocusPosition;

                //摄像头朝向目标点
                _m_cControlCamera.transform.LookAt(_m_vCurFocusPos);
                _m_cControlCamera.transform.eulerAngles.Set(_m_cControlCamera.transform.eulerAngles.x, _m_cControlCamera.transform.eulerAngles.y, 0);
            }

            //进行摄像头视角宽度处理
            if (null != _m_vcCameraFieldOfViewController)
            {
                //进行焦点的每帧检测
                _IALCameraFieldOfViewController newFieldOfViewController = _m_vcCameraFieldOfViewController.checkUpdate();
                if (null != newFieldOfViewController)
                    _m_vcCameraFieldOfViewController = newFieldOfViewController;

                //获取当前相关视角信息
                float fieldOfView = _m_vcCameraFieldOfViewController.fieldOfView;

                _m_fPreOriginFieldOfView = fieldOfView;
                //设置摄像头视角宽度
                _setPerspectiveFieldOfView(fieldOfView);
            }

            //修改正交视野
            if(null != _m_osOrthogrphicSizeController)
            {
                //进行正交视野的每帧检测, 不为NULL则说明有新状态，需要切换
                _AALCameraOrthographicSizeController newOrthographicSizeController = _m_osOrthogrphicSizeController.checkUpdate();
                if(null != newOrthographicSizeController)
                    _m_osOrthogrphicSizeController = newOrthographicSizeController;

                //使用旧数据对象则需要判断是否更改
                _m_fPreOriginOrthogrphicSize = _m_osOrthogrphicSizeController.orthographicSize;
                float curOthSize = _m_fPreOriginOrthogrphicSize;
                //根据是否需要自适应进行调整计算
                if(_m_bOpenCameraWidthFit)
                    curOthSize = _m_fPreOriginOrthogrphicSize * _m_fCurNormalRateForOrthogrphicSize;

                //判断是否更改
                if(curOthSize == _m_fPreOrthogrphicSize && _m_osOrthogrphicSizeController.isInited)
                    return;

                //设置初始化完成
                _m_osOrthogrphicSizeController.setInited();

                //设置正交视野尺寸
                _setOrthographicSize(curOthSize);

                //设置之前值
                _m_fPreOrthogrphicSize = curOthSize;
            }
        }

        /*******************
         * 设置正交视野的宽度
         **/
        protected void _setOrthographicSize(float _orthographicSize)
        {
            if(_m_cControlCamera == null)
                return;

            _m_cControlCamera.orthographicSize = _orthographicSize;

            //查询是否有子摄像头对象，如有则同步
            if(null != _m_arrChildCamera)
            {
                for(int i = 0; i < _m_arrChildCamera.Length; i++)
                {
                    _m_arrChildCamera[i].orthographicSize = _orthographicSize;
                }
            }

            //计算血条等对象的缩放比例
            if(0 != _orthographicSize)
                _m_fInverseProportion = _m_fDefaultOrthographicSize / _orthographicSize;
            //计算视野框等对象的缩放正比例
            if(0 != _m_fDefaultOrthographicSize)
                _m_fProportion = _orthographicSize / _m_fDefaultOrthographicSize;
        }

        /// <summary>
        /// 设置透视相机视野范围，如果开启了宽度自适应，则会根据宽度调整范围
        /// </summary>
        /// <param name="fieldOfView"></param>
        private void _setPerspectiveFieldOfView(float fieldOfView)
        {
            if (_m_bOpenCameraWidthFitPerspective)
            {
                // 宽度自适应，通过换算，保持宽度的角度不变，即达到宽度自适应的效果
                fieldOfView = Mathf.Atan(_m_fCurNormalRateForOrthogrphicSize * Mathf.Tan(_m_fPreOriginFieldOfView / g_radianToAngle)) * g_radianToAngle;
            }
            _m_cControlCamera.fieldOfView = fieldOfView;
        }
    }
}

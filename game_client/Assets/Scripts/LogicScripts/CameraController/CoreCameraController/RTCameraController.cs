using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using ALPackage;
using UnityEngine.Rendering.Universal;

namespace GOE
{
    /// <summary>
    /// RenderTexture摄像头控制对象
    /// </summary>
    public class RTCameraController : ALCameraController, _IShowcaseCameraController
    {
        /// <summary>
        /// 用于渲染的摄像机，由本管理器自己生成
        /// </summary>
        private Camera _m_RTCamera;
        /// <summary>
        /// 主相机的 额外相机数据 UniversalAdditionalCameraData
        /// </summary>
        private UniversalAdditionalCameraData _m_additionalCameraData;
        /// <summary>
        /// 渲染的RenderTexture对象
        /// </summary>
        private RenderTexture _m_renderTexture;
        /// <summary>
        /// 当前使用的Descriptor
        /// </summary>
        private RenderTextureDescriptor _m_renderTextureDescriptor = new RenderTextureDescriptor(1024,1024,RenderTextureFormat.Default,24);
        //相机坐标获取对象
        private _ALogicPlane2DPosGetter _m_cgCameraMoveGetter;
        
        public RTCameraController(string _name)
            : base(6f)
        {
            GameObject camera = new GameObject(_name);
            _m_RTCamera = camera.AddComponent<Camera>();
            _m_RTCamera.forceIntoRenderTexture = true;
            _m_RTCamera.nearClipPlane = 1;
            _m_RTCamera.farClipPlane = 100;
            _m_additionalCameraData = camera.AddComponent<UniversalAdditionalCameraData>();
            _m_additionalCameraData.renderPostProcessing = true;
            setControlCamera(_m_RTCamera);
        }
        
        public RenderTexture renderTexture { get { return _m_renderTexture; } set { _m_renderTexture = value; } }
        
        /// <summary>
        /// 初始化信息
        /// </summary>
        public void initRenderTexture()
        {
            _resetRTDescriptor();
            _initRenderTexture();
        }

        public void initRenderTexture(RenderTextureDescriptor _descriptor)
        {
            if(_descriptor.width > 0 && _descriptor.height > 0)
            {
                _setRTDescriptor(_descriptor);
            }
            else
            {
#if UNITY_EDITOR
                //如果文件用了AspectRatioFilter，就会有问题，只在Editor下报错就好
                Debug.LogError($"RenderTexture 的尺寸不对， {_descriptor.width},{_descriptor.height}");
#endif
                _resetRTDescriptor();
            }
           
            _initRenderTexture();
        }

        /// <summary>
        /// 设置RenderTextureDescriptor
        /// </summary>
        /// <param name="_descriptor"></param>
        private void _setRTDescriptor(RenderTextureDescriptor _descriptor)
        {
            _m_renderTextureDescriptor = _descriptor;
        }

        /// <summary>
        /// 重制RenderTextureDescriptor
        /// </summary>
        private void _resetRTDescriptor()
        {
            _m_renderTextureDescriptor = CommonQualityMgr.instance.getRTDescriptor();
        }

        /// <summary>
        /// 初始化rendertexture，根据Descriptor 调用RenderTexture.GetTemporary申请rendertexture
        /// </summary>
        private void _initRenderTexture()
        {
            releaseRenderTexture();
            
            _m_renderTexture = RenderTexture.GetTemporary(_m_renderTextureDescriptor);
            _m_RTCamera.targetTexture = _m_renderTexture;
        }

        //释放旧的RenderTexture
        private void releaseRenderTexture()
        {
            if (_m_renderTexture != null)
            {
                RenderTexture.ReleaseTemporary(_m_renderTexture);
                _m_renderTexture = null;
            }
        }
        
        /// <summary>
        /// 显示摄像头
        /// </summary>
        public void enableShowcaseCamera()
        {
            if(null == controlCamera)
                return;

            ALPackage.ALUGUICommon.setGameObjEnable(controlCamera.gameObject, true);
            controlCamera.enabled = true;
        }

        /// <summary>
        /// 设置摄像头无效
        /// </summary>
        public void disableShowcaseCamera()
        {
            if(null == controlCamera)
                return;

            controlCamera.enabled = false;
            
            releaseRenderTexture();
        }

        /// <summary>
        /// 设置摄像头的信息
        /// </summary>
        /// <param name="_cameraController"></param>
        /// <param name="_isMainCamera"></param>
        /// <param name="_parent"></param>
        public void setDataTo(Transform _parent, CameraData _cameraData)
        {
            if(null == _cameraData || null == controlCamera)
                return;

            Transform tempTsf = controlCamera.transform;
            tempTsf.position = _parent.TransformPoint(_cameraData.localPosition);
            tempTsf.eulerAngles = _cameraData.euler;
            tempTsf.localScale = _cameraData.localScale;

            controlCamera.clearFlags = _cameraData.clearFlags;
            controlCamera.backgroundColor = _cameraData.backgroundColor;
            controlCamera.cullingMask = _cameraData.cullingMask;

            controlCamera.orthographic = _cameraData.orthographic;
            controlCamera.orthographicSize = _cameraData.orthographicSize;
            controlCamera.nearClipPlane = _cameraData.nearClipPlane;
            controlCamera.farClipPlane = _cameraData.farClipPlane;
            
            //获取相机采样器
            _m_cgCameraMoveGetter = NPGameUtility.getAxisAlignedPlanePosGetter(controlCamera.transform.forward.normalized);
            
            //设置位置
            setCameraPosController(new ALCameraPosControllerStaticPos(controlCamera.transform.position));
            //设置焦点
            setCameraFocusController(new ALCameraFocusControllerStaticPos(controlCamera.transform.TransformPoint(Vector3.forward)));

            setCameraOrthogrphicSizeController(new ALCameraOrthographicSizeControllerStaticValue(_cameraData.orthographicSize));
            setCameraFieldOfViewController(new ALCameraFieldOfViewControllerStaticValue(_cameraData.fieldOfView));

            //计算一次
            frameCheck();
        }
        
        //将屏幕坐标转成世界坐标
        public Vector3 getOnlyGroundPos(Vector2 _screenPos)
        {
            if(null == _m_cgCameraMoveGetter)
                return Vector3.zero;

            _m_cgCameraMoveGetter.intersectionWorldPointWithWorldRay(controlCamera.ScreenPointToRay(_screenPos), out Vector3 intersectionPos);
            return intersectionPos;
        }
        
        /// <summary>
        ///  设置阴影裁剪边界，提高阴影精度
        /// </summary>
        /// <param name="_openShadowClipOverride">是否要开启重载阴影裁剪边界</param>
        /// <param name="points">围住目标组成的包围盒的8个点</param>
        public void setShadowClipBounds(bool _openShadowClipOverride, Vector3[] points)
        {
            if (_m_additionalCameraData == null)
            {
                Debug.LogError("AdditionalCameraData 为空，@Ben检查");
                return;
            }
            // if(_openShadowClipOverride)
            //     _m_additionalCameraData.shadowClipPoints = points;
            // else
            //     _m_additionalCameraData.shadowClipPoints = null;
        }
    }
}
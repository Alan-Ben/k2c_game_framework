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
    public class RTMainCameraController : ALCameraController, _IShowcaseCameraController
    {
        private static readonly Rect _g_rDefaultUIScreenRect = new Rect(-540, -960, 1080, 1920); // 默认UI Rect范围

        private static RTMainCameraController _g_instance = new RTMainCameraController();
        public static RTMainCameraController instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new RTMainCameraController();
                return _g_instance;
            }
        }
        
        /// <summary>
        /// 当前设定的UI视觉中心
        /// </summary>
        private RectTransform _m_vViewRect;
        /// <summary>
        /// 视觉中心与相机的距离，透视相机需要根据这个深度计算中心的偏移量
        /// </summary>
        private float _m_fCenterDepthZ;

        //相机坐标获取对象
        private _ALogicPlane2DPosGetter _m_cgCameraMoveGetter;
        
        /// <summary>
        /// 主相机的 额外相机数据 UniversalAdditionalCameraData
        /// </summary>
        private UniversalAdditionalCameraData _m_additionalCameraData;
        
        private bool _m_cameraIsOpenRender; // 是否开启渲染

        /// <summary>
        /// 是否开启了相机的渲染
        /// </summary>
        public bool cameraIsOpenRender 
        { 
            get { return _m_cameraIsOpenRender; }
            set
            {
                _m_cameraIsOpenRender = value;
                URPCameraManager.instance.setRTMainCameraEnable(_m_cameraIsOpenRender);
            }
        }

        public RTMainCameraController()
            : base(6f)
        {
            //默认隐藏，要用再显示
            disableShowcaseCamera();
        }

        /// <summary>
        /// 设置RT主相机的UniversalAdditionalCameraData
        /// </summary>
        /// <param name="_data"></param>
        public void setUniversalAdditionalCameraData(UniversalAdditionalCameraData _data)
        {
            if(null == _data)
                return;

            _m_additionalCameraData = _data;
            _m_cameraIsOpenRender = false;// RT相机默认关闭
        }

        

        /// <summary>
        /// 显示摄像头
        /// </summary>
        public void enableShowcaseCamera()
        {
            if(null == controlCamera)
                return;
            cameraIsOpenRender = true;
            // ALPackage.ALUGUICommon.setGameObjEnable(controlCamera.gameObject, true);
            // controlCamera.enabled = true;
            // 由于RT相机可能在blur的时候未开启，所以需要在开启后手动刷新一下模糊贴图
            ScreenBlurMgr.instance.refreshBlurRT();
        }

        /// <summary>
        /// 设置摄像头无效
        /// </summary>
        public void disableShowcaseCamera()
        {
            if(null == controlCamera)
                return;
            cameraIsOpenRender = false;

            // controlCamera.enabled = false;
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
        
        public new void frameCheck()
        {
            base.frameCheck();
            
            if(null == controlCamera || _m_vViewRect == null)
                return;
           
            float _m_fOriginViewCenterYRatio;
            float _m_fCurViewCenterYRatio;
            
            // 在1920x1080下的中心位置比例
            _m_fOriginViewCenterYRatio = _calculateDefaultViewCenterRect(_m_vViewRect).center.y / 1920f;

            Rect rect = controlCamera.rect;
            Vector2 lbScreen = new Vector2(rect.x * Screen.width, rect.y * Screen.height);
            Vector2 rtScreen = new Vector2(rect.xMax * Screen.width, rect.yMax * Screen.height);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_m_vViewRect, lbScreen , Game.instance.mainCamera.uiCamera, out Vector2 leftBottom);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_m_vViewRect, rtScreen , Game.instance.mainCamera.uiCamera, out Vector2 rightTop);
            Vector2 size = rightTop - leftBottom;
            Vector2 pos = _m_vViewRect.rect.center - leftBottom;
            
            // 在当前分辨率下的中心位置比例，正为上，负为下
            _m_fCurViewCenterYRatio = (pos.y - size.y / 2) / size.y;

            if (!controlCamera.orthographic)
            {
                //根据新旧的视角范围，计算中心偏移量
                float offset = (_m_fOriginViewCenterYRatio * Mathf.Tan(OriginFieldOfView/g_radianToAngle) - 
                                _m_fCurViewCenterYRatio * Mathf.Tan(controlCamera.fieldOfView/g_radianToAngle) ) * _m_fCenterDepthZ * 2 ;
                Transform transform = controlCamera.transform;
                Vector3 extraOffset = transform.up * offset;
                transform.position += extraOffset;
            }
            else
            {
                //根据新旧的视角范围，计算中心偏移量
                float offset = (_m_fOriginViewCenterYRatio * OriginOrthographicSize - _m_fCurViewCenterYRatio * controlCamera.orthographicSize ) * 2;
                Transform transform = controlCamera.transform;
                Vector3 extraOffset = transform.up * offset;
                transform.position += extraOffset;
            }
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
        /// <summary>
        /// 设置UI视觉中心
        /// </summary>
        /// <param name="_viewRect">UI视觉中心</param>
        /// <param name="_viewCenterY">1920x1080分辨率下的UI中心位置（坐标值为：-960 - 960）</param>
        /// <param name="_centerDepthZ">视觉中心与相机的距离，透视相机需要根据这个深度计算中心的偏移量</param>
        public void setUIViewCenter(RectTransform _viewRect, float _centerDepthZ)
        {
            _m_fCenterDepthZ = _centerDepthZ;
            _m_vViewRect = _viewRect;
        }
        /// <summary>
        /// 计算在默认UI尺寸下的这个RectTransform的范围
        /// </summary>
        /// <param name="_viewCenterRect"></param>
        /// <returns></returns>
        private static Rect _calculateDefaultViewCenterRect(RectTransform _viewCenterRect)
        {
            List<RectTransform> rects = new List<RectTransform>();
            _getSpecialParentRectTransformList(_viewCenterRect, rects);

            Rect pRect = _g_rDefaultUIScreenRect;
            for (var i = rects.Count - 1; i >= 0; i--)
            {
                RectTransform rect = rects[i];
                if(rect == null)
                    continue;
                Rect nRect = pRect;
                nRect.min = pRect.min + pRect.size * (rect.anchorMin) + rect.anchoredPosition - Vector2.Scale(rect.sizeDelta, rect.pivot) ;
                nRect.max = pRect.max + pRect.size * (rect.anchorMax -Vector2.one)  + rect.anchoredPosition + Vector2.Scale(rect.sizeDelta, Vector2.one - rect.pivot);;
                pRect = nRect;
            }
            return pRect;
        }
        /// <summary>
        /// 获取指定Transform的父节点，直到找到Canvas或者父物体为空
        /// </summary>
        /// <param name="_tran"></param>
        /// <param name="_rects"></param>
        private static void _getSpecialParentRectTransformList(Transform _tran, List<RectTransform> _rects)
        {
            if (_rects == null)
                _rects = new List<RectTransform>();
            
            if(_tran == null)
                return;
            
            Canvas canvas = _tran.GetComponent<Canvas>();
            if(canvas != null)
                return;
         
            RectTransform rectTransform = _tran.GetComponent<RectTransform>();
            _rects.Add(rectTransform);
            _getSpecialParentRectTransformList(_tran.parent, _rects);
        }
    }
}
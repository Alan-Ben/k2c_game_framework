// using System.Collections.Generic;
// using JetBrains.Annotations;
// using UnityEngine;
//
// namespace GOE
// {
//     public class PlanarShadowMgr
//     {
//         private static PlanarShadowMgr _g_instance = new PlanarShadowMgr();
//         public static PlanarShadowMgr instance
//         {
//             get
//             {
//                 if (null == _g_instance)
//                     _g_instance = new PlanarShadowMgr();
//                 return _g_instance;
//             }
//         }
//
//         private RenderTexture _m_shadowRt;          // 阴影RenderTexture
//         private Vector3 _m_targetCamPosOffset;      // 跟随的主相机和阴影相机的位置偏移，用于减少主相机移动，阴影相机跟随的计算
//         private Vector2 _m_vUVOffset2CamPos;        // uv和相机的偏移值，用于减少每帧的相机位置和uv坐标的计算
//         private Camera _m_shadowCam;        // 阴影相机
//         private Rect _m_rCurViewRect;       // 当前阴影相机视野范围
//         private int _m_iRTPixelHeight;      //RT的最大高度
//         private float _m_fMargin;           // 阴影相机范围 外边界大小
//         private Vector2Int _m_vRtSize;      // RenderTexture的大小
//         private Transform _m_tLight;        //灯光
//
//         private Color _m_defaultCamColor = new Color(0,0,0,1); //相机默认颜色
//         
//         private bool _m_bIsOpen = false; // 是否开启平面阴影
//         
//         private bool _m_bIsCurPlanarShadowOpen = false; //当前平面阴影是否开启
//         
//         public bool isCurPlanarShadowOpen { get => _m_bIsCurPlanarShadowOpen; }
//
//         /// <summary>
//         /// 设置是否开启平面阴影
//         /// </summary>
//         /// <param name="_isOpen"></param>
//         public void setIsOpen(bool _isOpen)
//         {
//             _m_bIsOpen = _isOpen;
//             if (!_m_bIsOpen)
//             {
//                 _closeShadow();
//             }
//         }
//         /// <summary>
//         /// 阴影管理器初始化
//         /// </summary>
//         /// <param name="_shadowCam"></param>
//         public void init(Camera _shadowCam)
//         {
//             _m_shadowCam = _shadowCam;
//             if (_m_shadowCam != null)
//                 _m_shadowCam.backgroundColor = _m_defaultCamColor;
//             _m_iRTPixelHeight = 2048;
//             Shader.DisableKeyword("_MJ_PLANAR_SHADOW");
//
//         }
//
//         public void openShadow(GSOSceneDirLightsInfo _sLightsInfo, Camera _targetCam)
//         {
// #if NP_GAME
//             if(!_m_bIsOpen)
//                 return;
//             _openShadow(_targetCam, 0, Game.instance.mainCamera.directionLight.transform,
//                 _sLightsInfo.shadowStrength);      
// #endif
//         }
//
//         /// <summary>
//         /// 关闭阴影
//         /// </summary>
//         public void closeShadow()
//         {
//             if(!_m_bIsOpen)
//                 return;
//             _closeShadow();
//         }
//
//
//         /// <summary>
//         /// 更新材质阴影的世界坐标，来做可以高低变化的平面阴影
//         /// </summary>
//         /// <param name="_renderers"></param>
//         /// <param name="_worldPos"></param>
//         public void updateMatShadowWorldPos(List<Renderer> _renderers, Vector4 _worldPos)
//         {
//             if(!isCurPlanarShadowOpen)
//                 return;
//             if(_renderers == null)
//                 return;
//
//             foreach (var render in _renderers)
//             {
//                 if(render == null)
//                     continue;
//                 foreach (var mat in render.materials)
//                 {
//                     if(mat == null)
//                         continue;
//                     mat.SetVector(ShaderPropertyMgr.g_ShadowWorldPos, _worldPos);
//                 }
//             }
//         }
//
//         /// <summary>
//         /// 更新材质阴影的世界坐标，来做可以高低变化的平面阴影
//         /// </summary>
//         /// <param name="_renderers"></param>
//         /// <param name="_worldPos"></param>
//         public void updateMatShadowWorldPos(Renderer[] _renderers, Vector4 _worldPos)
//         {
//             if(!isCurPlanarShadowOpen)
//                 return;
//             if(_renderers == null)
//                 return;
//
//             foreach (var render in _renderers)
//             {
//                 if(render == null)
//                     continue;
//                 foreach (var mat in render.materials)
//                 {
//                     if(mat == null)
//                         continue;
//                     mat.SetVector(ShaderPropertyMgr.g_ShadowWorldPos, _worldPos);
//                 }
//             }
//         }
//         
//         /// <summary>
//         /// 更新材质阴影的世界坐标，来做可以高低变化的平面阴影
//         /// </summary>
//         /// <param name="_renderers"></param>
//         /// <param name="_worldPos"></param>
//         public void updateMatShadowWorldPos(Renderer _renderer, Vector4 _worldPos)
//         {
//             if(!isCurPlanarShadowOpen)
//                 return;
//             if(_renderer == null)
//                 return;
//             foreach (var mat in _renderer.materials)
//             {
//                 if(mat == null)
//                     continue;
//                 mat.SetVector(ShaderPropertyMgr.g_ShadowWorldPos, _worldPos);
//             }
//         }
//
//         /// <summary>
//         /// 相机位置谁阿信
//         /// </summary>
//         /// <param name="_targetCam"></param>
//         public void laterUpdateCameraPos(Camera _targetCam)
//         {
//             if (!_m_bIsCurPlanarShadowOpen)
//                 return;
//             
//             if (_m_shadowCam == null || _targetCam == null || _m_shadowCam.enabled == false) 
//                 return;
//             
//             Vector3 targetCamPos = _targetCam.transform.position;
//             _m_shadowCam.transform.position = targetCamPos - _m_targetCamPosOffset;
//             Shader.SetGlobalVector(ShaderPropertyMgr.g_PlaneShadowOffset, 
//                 new Vector4(targetCamPos.x - _m_vUVOffset2CamPos.x, targetCamPos.z - _m_vUVOffset2CamPos.y));
//             _onViewChange(_targetCam);
// #if NP_GAME
//             if(_m_tLight != null)
//                 SpriteMeshModifyMgr.instance.updateLightDir(_m_tLight.forward);
// #endif
//         }
//         
//         
//         /// <summary>
//         /// 开启平面阴影
//         /// </summary>
//         /// <param name="_targetCam"></param>
//         /// <param name="_margin"></param>
//         /// <param name="_lightTrans"></param>
//         /// <param name="_intensity"></param>
//         private void _openShadow(Camera _targetCam, float _margin, Transform _lightTrans, float _intensity = 1)
//         {
//             if (_m_shadowCam == null) 
//                 return;
//             _m_tLight = _lightTrans;
// #if NP_GAME
//             if(_m_tLight != null)
//                 SpriteMeshModifyMgr.instance.initLightDir(_m_tLight.forward);
// #endif
//             _m_bIsCurPlanarShadowOpen = true;
//             _m_shadowCam.enabled = true;
//             _m_fMargin = _margin;
//             _m_vRtSize = Vector2Int.zero;
//             _onViewChange(_targetCam);
//             Shader.SetGlobalFloat(ShaderPropertyMgr.g_PlanarShadowIntensity, 1-_intensity);
//             Shader.SetGlobalVector(ShaderPropertyMgr.g_ShadowPlane, new Vector4(0,1,0,0));
//             Shader.EnableKeyword("_MJ_PLANAR_SHADOW");
//         }
//         
//         /// <summary>
//         /// 关闭阴影
//         /// </summary>
//         private void _closeShadow()
//         {
//             SpriteMeshModifyMgr.instance.closeShadow();
//             if (_m_shadowCam != null)
//             {
//                 _m_shadowCam.enabled = false;
//                 _m_shadowCam.targetTexture = null;
//             }
//             if(_m_shadowRt != null)
//                 RenderTexture.ReleaseTemporary(_m_shadowRt);
//             _m_vRtSize = Vector2Int.zero;
//             _m_bIsCurPlanarShadowOpen = false;
//             Shader.DisableKeyword("_MJ_PLANAR_SHADOW");
//         }
//         
//         private void _onViewChange(Camera _targetCam)
//         {
//             _onViewChange(_targetCam, _m_fMargin, _targetCam.nearClipPlane, _targetCam.farClipPlane);
//         }
//
//         private void _onViewChange(Camera _targetCam, float _margin, float _near, float _far)
//         {
//             if (_m_shadowCam == null || _targetCam == null) 
//                 return;
//             Transform transform = _m_shadowCam.transform;
//             transform.forward = Vector3.down;
//             transform.up = Vector3.forward;
//             Vector3 targetCamPos = _targetCam.transform.position;
//
//             _m_rCurViewRect = _genSimpleCameraRect(_targetCam, Screen.width, Screen.height, _near, _far, _margin, out float maxY, out float minY);
//             int rtWidth = (int)(_m_iRTPixelHeight * _m_rCurViewRect.width / _m_rCurViewRect.height);
//             int rtHeight = _m_iRTPixelHeight;
//             Vector3 centerPos = new Vector3(_m_rCurViewRect.center.x, targetCamPos.y,_m_rCurViewRect.center.y);
//             if (rtWidth <= 0 || rtHeight <= 0)
//             {
//                 // Debug.LogError($"ShadowMgr:onViewChange:{rtWidth}, {rtHeight}");
//                 return;
//             }
//             if (rtWidth != _m_vRtSize.x || rtHeight != _m_vRtSize.y)
//             {
//                 _m_vRtSize.x = rtWidth;
//                 _m_vRtSize.y = rtHeight;
//                 if (_m_shadowRt != null)
//                 {
//                     _m_shadowCam.targetTexture = null;
//                     RenderTexture.ReleaseTemporary(_m_shadowRt);
//                 }
//                 _m_shadowRt = RenderTexture.GetTemporary(rtWidth,rtHeight);
//                 _m_shadowRt.wrapMode = TextureWrapMode.Repeat;
//                 Shader.SetGlobalTexture(ShaderPropertyMgr.g_PlaneShadowMap, _m_shadowRt);
//             }
//
//             Shader.SetGlobalVector(ShaderPropertyMgr.g_PlaneShadowScale, new Vector4(_m_rCurViewRect.width,_m_rCurViewRect.height));
//             _m_vUVOffset2CamPos = new Vector2(targetCamPos.x - _m_rCurViewRect.xMin, targetCamPos.z - _m_rCurViewRect.yMin);
//             Shader.SetGlobalVector(ShaderPropertyMgr.g_PlaneShadowOffset, 
//                 new Vector4(targetCamPos.x - _m_vUVOffset2CamPos.x, targetCamPos.z - _m_vUVOffset2CamPos.y));
//             
//             _m_targetCamPosOffset = targetCamPos - centerPos;
//             _m_shadowCam.targetTexture = _m_shadowRt;
//             _m_shadowCam.transform.position = targetCamPos - _m_targetCamPosOffset;
//             _m_shadowCam.orthographic = true;
//             _m_shadowCam.orthographicSize = _m_rCurViewRect.height / 2;
//             
//             _m_shadowCam.nearClipPlane = targetCamPos.y - maxY;
//             _m_shadowCam.farClipPlane = targetCamPos.y - minY;
//         }
//
//         /// <summary>
//         /// 获取在xz屏幕的最大范围
//         /// </summary>
//         /// <param name="_targetCam"> 目标相机</param>
//         /// <param name="width">屏幕宽度</param>
//         /// <param name="height">屏幕高度</param>
//         /// <param name="_near">视野范围 近</param>
//         /// <param name="_far">视野范围 远</param>
//         /// <param name="_add">外边距</param>
//         /// <param name="_maxY">获取最大的y</param>
//         /// <param name="_minY">获取最小的y</param>
//         /// <returns></returns>
//         private Rect _genSimpleCameraRect([NotNull] Camera _targetCam, float width, float height, float _near, float _far, float _add, out float _maxY, out float _minY)
//         {
//             Vector3 lb;
//             Vector3 rt;
//             Vector3 lt;
//             Vector3 rb;
//             if (_targetCam.orthographic)
//             {
//                 lb = NPResUtil.getGroundPosByOrthographicCamera(_targetCam.rect, _targetCam.transform, _targetCam.orthographicSize, new Vector2(0,0));
//                 rt = NPResUtil.getGroundPosByOrthographicCamera(_targetCam.rect, _targetCam.transform, _targetCam.orthographicSize, new Vector2(1,1));
//                 lt = NPResUtil.getGroundPosByOrthographicCamera(_targetCam.rect, _targetCam.transform, _targetCam.orthographicSize, new Vector2(0,1));
//                 rb = NPResUtil.getGroundPosByOrthographicCamera(_targetCam.rect, _targetCam.transform, _targetCam.orthographicSize, new Vector2(1,0));
//             }
//             else
//             {
//                 lb = NPResUtil.getGroundPosByPerspectiveCamera(_targetCam.rect, _targetCam.transform, _targetCam.fieldOfView, new Vector2(0,0));
//                 rt = NPResUtil.getGroundPosByPerspectiveCamera(_targetCam.rect, _targetCam.transform, _targetCam.fieldOfView, new Vector2(1,1));
//                 lt = NPResUtil.getGroundPosByPerspectiveCamera(_targetCam.rect, _targetCam.transform, _targetCam.fieldOfView, new Vector2(0,1));
//                 rb = NPResUtil.getGroundPosByPerspectiveCamera(_targetCam.rect, _targetCam.transform, _targetCam.fieldOfView, new Vector2(1,0));
//             }
//             if (lb.IsNaN() || rt.IsNaN() || lt.IsNaN() || rb.IsNaN())
//             {
//                 _maxY = 0;
//                 _minY = 0;
//                 return new Rect(0, 0, 0, 0);
//             }
//
//             Vector3 lb_n = _targetCam.ScreenToWorldPoint(new Vector3(0, 0, _near));
//             Vector3 lt_n = _targetCam.ScreenToWorldPoint(new Vector3(0, height, _near));
//             Vector3 rb_n = _targetCam.ScreenToWorldPoint(new Vector3(width, 0, _near));
//             Vector3 rt_n = _targetCam.ScreenToWorldPoint(new Vector3(width, height, _near));
//             
//             Vector3 lt_f = _targetCam.ScreenToWorldPoint(new Vector3(0, height, _far));
//             Vector3 lb_f = _targetCam.ScreenToWorldPoint(new Vector3(0, 0, _far));
//             Vector3 rt_f = _targetCam.ScreenToWorldPoint(new Vector3(width, height, _far));
//             Vector3 rb_f = _targetCam.ScreenToWorldPoint(new Vector3(width, 0, _far));
//             
//             // 支持相机绕着 Y 轴 360° 旋转，但是相机绕着自己的 X 轴旋转不能超过 90 度，并且不支持绕着自己的 Z 轴旋转
//             Vector2 min = new Vector2(
//                 Mathf.Min(lb.x, lt.x, rb.x, rt.x),
//                 Mathf.Min(lb.z, lt.z, rb.z, rt.z));
//             Vector2 max = new Vector2(
//                 Mathf.Max(lb.x, lt.x, rb.x, rt.x),
//                 Mathf.Max(lb.z, lt.z, rb.z, rt.z));
//             
//             Vector2 addV = new Vector2(_add, _add);
//
//             _maxY = Mathf.Max(lt_f.y, lt_n.y);
//             _minY = Mathf.Min(lb_n.y, lb_f.y);
//             
//             Rect rect = new Rect(min - addV, max - min + addV * 2);
//             return rect;
//         }
//         
//         private Rect _genSimpleCameraRectOld([NotNull] Camera _targetCam, float width, float height, float _near, float _far, float _add, out float _maxY, out float _minY)
//         {
//             Vector3 lb_n = _targetCam.ScreenToWorldPoint(new Vector3(0, 0, _near));
//             Vector3 lt_n = _targetCam.ScreenToWorldPoint(new Vector3(0, height, _near));
//             Vector3 rb_n = _targetCam.ScreenToWorldPoint(new Vector3(width, 0, _near));
//             Vector3 rt_n = _targetCam.ScreenToWorldPoint(new Vector3(width, height, _near));
//             
//             Vector3 lt_f = _targetCam.ScreenToWorldPoint(new Vector3(0, height, _far));
//             Vector3 lb_f = _targetCam.ScreenToWorldPoint(new Vector3(0, 0, _far));
//             Vector3 rt_f = _targetCam.ScreenToWorldPoint(new Vector3(width, height, _far));
//             Vector3 rb_f = _targetCam.ScreenToWorldPoint(new Vector3(width, 0, _far));
//
//             // 支持相机绕着 Y 轴 360° 旋转，但是相机绕着自己的 X 轴旋转不能超过 90 度，并且不支持绕着自己的 Z 轴旋转
//             Vector2 min = new Vector2(
//                 Mathf.Min(lb_n.x, lt_n.x, rb_n.x, rt_n.x, lt_f.x, lb_f.x, rt_f.x, rb_f.x),
//                 Mathf.Min(lb_n.z, lt_n.z, rb_n.z, rt_n.z, lt_f.z, lb_f.z, rt_f.z, rb_f.z));
//             Vector2 max = new Vector2(
//                 Mathf.Max(lb_n.x, lt_n.x, rb_n.x, rt_n.x, lt_f.x, lb_f.x, rt_f.x, rb_f.x),
//                 Mathf.Max(lb_n.z, lt_n.z, rb_n.z, rt_n.z, lt_f.z, lb_f.z, rt_f.z, rb_f.z));
//
//             Vector2 addV = new Vector2(_add, _add);
//
//             Rect rect = new Rect(min - addV, max - min + addV * 2);
//             _maxY = Mathf.Max(lt_f.y, lt_n.y);
//             _minY = Mathf.Min(lb_n.y, lb_f.y);
//             return rect;
//         }
//     }
// }
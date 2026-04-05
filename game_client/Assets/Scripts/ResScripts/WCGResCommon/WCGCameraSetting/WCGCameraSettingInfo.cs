using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;


/*****************
 * 游戏摄像头设置信息对象
 **/
[System.Serializable]
public class WCGCameraSettingInfo
{
    /** 摄像头位置信息 */
    public Vector3 cameraPosition;
    /** 摄像头焦点位置 */
    public Vector3 cameraFocusPosition;
    /** 摄像头视野宽度 */
    public float cameraFieldOfView;

    /** 摄像头相关设置 */
    public bool isOrthographic;//是否正交相机
    public float orthographicSize;//正交相机尺寸

    /** 标准缩放的尺寸 */
    public float originSize;

    public float clippingNear;//镜头最近视距
    public float clippingFar;//镜头最远视距

    public WCGCameraSettingInfo() 
    {
    }
    public WCGCameraSettingInfo(WCGCameraSettingInfo _cameraSetting)
    {
        if (_cameraSetting == null)
            return;
        
        cameraPosition = _cameraSetting.cameraPosition;
        cameraFocusPosition = _cameraSetting.cameraFocusPosition;
        cameraFieldOfView = _cameraSetting.cameraFieldOfView;
        isOrthographic = _cameraSetting.isOrthographic;
        orthographicSize = _cameraSetting.orthographicSize;
        originSize = _cameraSetting.originSize;
        clippingNear = _cameraSetting.clippingNear;
        clippingFar = _cameraSetting.clippingFar;
    }

#if NP_GAME
    /// <summary>
    /// 获取默认的地面尺寸
    /// </summary>
    public Vector2 getCameraDefaultSize(float _groundY = 0f)
    {
        Vector3 cameraForward = cameraFocusPosition - cameraPosition;
        if (isOrthographic)
        {
            Vector3 min = GCommon.getGroundPosByOrthographicCamera(cameraPosition, cameraForward, originSize, new Vector2(0, 0), _groundY);
            Vector3 max = GCommon.getGroundPosByOrthographicCamera(cameraPosition, cameraForward, originSize, new Vector2(1, 1), _groundY);
            Vector3 size = max - min;
            return new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.z));
        }
        else
        {
            Vector3 leftDownP = GCommon.getGroundPosByPerspectiveCamera(cameraPosition, cameraForward, cameraFieldOfView, new Vector2(0, 0), _groundY);
            Vector3 rightDownP = GCommon.getGroundPosByPerspectiveCamera(cameraPosition, cameraForward, cameraFieldOfView, new Vector2(1, 0), _groundY);
            Vector3 leftUpP = GCommon.getGroundPosByPerspectiveCamera(cameraPosition, cameraForward, cameraFieldOfView, new Vector2(0, 1), _groundY);
            Vector3 rightUpP = GCommon.getGroundPosByPerspectiveCamera(cameraPosition, cameraForward, cameraFieldOfView, new Vector2(1, 1), _groundY);

            Vector2 min = new Vector2(
                Mathf.Min(rightDownP.x, leftUpP.x, leftDownP.x, rightUpP.x),
                Mathf.Min(rightDownP.z, leftUpP.z, leftDownP.z, rightUpP.z)
            );
            Vector2 max = new Vector2(
                Mathf.Max(rightDownP.x, leftUpP.x, leftDownP.x, rightUpP.x),
                Mathf.Max(rightDownP.z, leftUpP.z, leftDownP.z, rightUpP.z)
            );
            return max - min;
        }
    }

    public Vector3 getCameraDefaultGroundPos(Vector2 _viewportPos, float _groundY = 0f)
    {
        Vector3 cameraForward = cameraFocusPosition - cameraPosition;
        if (isOrthographic)
            return GCommon.getGroundPosByOrthographicCamera(cameraPosition, cameraForward, originSize, _viewportPos, _groundY);
        else
            return GCommon.getGroundPosByPerspectiveCamera(cameraPosition, cameraForward, cameraFieldOfView, _viewportPos, _groundY);
    }

    /// <summary>
    /// 判断相机角度是否合法
    /// </summary>
    /// <remarks>
    /// NPCameraMoveControl 的相关逻辑需要相机朝着地面看去，相机的角度不能过高，代码里写死了 10 度是极限
    /// </remarks>
    public bool checkCameraForwardLegal()
    {
        // cos(10°)
        float legalCosValue = 0.98481f;

        Vector3 cameraDir = (cameraFocusPosition - cameraPosition).normalized;
        Vector3 forward = new Vector3(cameraDir.x, 0, cameraDir.z).normalized;
        return Vector3.Dot(cameraDir, forward) < legalCosValue;
    }
#endif
}

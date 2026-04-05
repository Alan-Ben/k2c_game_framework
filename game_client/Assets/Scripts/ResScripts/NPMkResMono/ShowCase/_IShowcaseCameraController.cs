using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase的摄像头控制接口对象
    /// </summary>
    public interface _IShowcaseCameraController
    {
        /// <summary>
        /// 获取摄像头对象
        /// </summary>
        Camera controlCamera { get; }

        /// <summary>
        /// 设置摄像头有效无效的状态
        /// </summary>
        void enableShowcaseCamera();
        void disableShowcaseCamera();

        /// <summary>
        /// 设置摄像头的信息
        /// </summary>
        /// <param name="_cameraController"></param>
        /// <param name="_isMainCamera"></param>
        /// <param name="_parent"></param>
        void setDataTo(Transform _parent, CameraData _cameraData);

        //将屏幕坐标转成世界坐标
        public Vector3 getOnlyGroundPos(Vector2 _screenPos);
        
        /// <summary>
        ///  开启阴影裁剪边界，提高阴影精度
        /// </summary>
        public void setShadowClipBounds(bool _openShadowClipOverride, Vector3[] points);
    }
}
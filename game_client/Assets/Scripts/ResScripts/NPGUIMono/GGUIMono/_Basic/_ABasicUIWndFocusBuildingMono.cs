using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 控制聚焦到建筑的mono基类
    /// </summary>
    public abstract class _ABasicUIWndFocusBuildingMono : _ANPBasicUIWndResBarMono
    {
        [ALHeader("将建筑聚焦到的UI位置")]
        [ALInfo("====以下3个配置是相机聚焦到建筑相关配置====")]
        public Transform transFocusPos;
        [ALHeader("摄像机视野改变值（如果是正交相机就是OrthographicSize的值，如果是透视相机就是FieldOfView）")]
        public float cameraScaleValue = 3.5f;
        [ALHeader("透视摄像机视野拉近的过渡总时间")]
        public float durationTime = 0.5f;
    }   
}
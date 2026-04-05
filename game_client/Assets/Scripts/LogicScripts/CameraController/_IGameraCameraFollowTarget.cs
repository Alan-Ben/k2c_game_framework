using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 实现这个接口，让对象可以作为相机跟随的目标
    /// </summary>
    public interface _IGameraCameraFollowTarget
    {
        /// <summary>
        /// 世界坐标
        /// </summary>
        Vector3 position { get; }
    }
}
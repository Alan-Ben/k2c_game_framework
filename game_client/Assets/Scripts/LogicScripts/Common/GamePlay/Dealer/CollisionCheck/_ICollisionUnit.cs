using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 碰撞单位接口
    /// </summary>
    public interface _ICollisionUnit
    {
        bool isEnabled { get; }
        Vector3 position { get; }
        void onCollisionStart([NotNull] _ICollisionUnit _other);
        void onCollision([NotNull] _ICollisionUnit _other);
        void onCollisionEnd([NotNull] _ICollisionUnit _other);
    }
    /// <summary>
    /// 球形碰撞体
    /// </summary>
    public interface _ISphereCollisionUnit : _ICollisionUnit
    {
        /// <summary>
        /// 碰撞半径或大小
        /// </summary>
        float radius { get; }
    }
    /// <summary>
    /// 轴对齐包围盒碰撞体
    /// </summary>
    public interface _IAABBCollisionUnit : _ICollisionUnit
    {
        /// <summary>
        /// 碰撞盒的偏移位置
        /// </summary>
        /// <remarks>
        /// position + offset = 碰撞盒的中心位置
        /// </remarks>
        Vector3 offset { get; }
        /// <summary>
        /// 碰撞盒的大小
        /// </summary>
        Vector3 size { get; }
    }
}
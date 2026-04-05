using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public enum EEntryPointAniType
    {
        [InspectorName("LOCK（未解锁）")]
        LOCK,
        [InspectorName("UNLOCKING_PROCESS（解锁过程）")]
        UNLOCKING_PROCESS,
        [InspectorName("UNLOCK（已解锁）")]
        UNLOCK,
        [InspectorName("SCREEN_CLICK_HIDE_PROCESS （屏幕未点击隐藏过程，只有UI上有效）")]
        SCREEN_CLICK_HIDE_PROCESS,
        [InspectorName("SCREEN_CLICK_SHOW_PROCESS （屏幕点击显示过程，只有UI上有效）")]
        SCREEN_CLICK_SHOW_PROCESS,
    }

    /// <summary>
    /// 功能入口点基类
    /// </summary>
    public abstract class _AGTDHomeEntryPointMono_Base : MonoBehaviour
    {
        [ALHeader("功能入口点表的id ")]
        public long entryPointId;
        [ALHeader("入口点状态动画")]
        public CommonAnimationShowTypeInfo<EEntryPointAniType> stateAniInfo;
        [ALHeader("UI跟随的节点")]
        public Transform followParent;
        [ALHeader("功能入口点的点击脚本")]
        public GTDHomeEntryPointClickMono clickMono;
    }
}
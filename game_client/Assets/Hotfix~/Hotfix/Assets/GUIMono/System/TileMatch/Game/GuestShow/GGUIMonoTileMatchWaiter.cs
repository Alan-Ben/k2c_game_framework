using GOE;
using UnityEngine;

namespace Hotfix
{
    public enum ETileMatchWaiterState
    {
        None,
        IDLE,//待机状态
        SUCCESS,//成功状态
        FAIL,//失败状态
    }
    
    public class GGUIMonoTileMatchWaiter : _AHotfixBaseMono
    {
        // [HotfixMono("不同状态的表现对象")]
        // public HotfixMultiStateShow<ETileMatchWaiterState> multiStateShow;
        
        [HotfixMono("spine动画")]
        public Spine.Unity.SkeletonGraphic spineAni;
        
        [HotfixMono("待机表现动画配置")]
        public CommonSkeletonGraphicAnimationConfig idleShowAniConfig;
        [HotfixMono("成功表现动画配置")]
        public CommonSkeletonGraphicAnimationConfig successShowAniConfig;
        [HotfixMono("失败表现动画配置")]
        public CommonSkeletonGraphicAnimationConfig failShowAniConfig;
    }
}
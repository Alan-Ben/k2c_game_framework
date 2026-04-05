using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public enum ETileMatchGuestState
    {
        None,
        IDLE,//待机状态
        WALK,//行走状态
        SUCCESS,//成功状态
        FAIL,//失败状态
    }
    
    public class GGUIMonoTileMatchGuest : _AHotfixBaseMono
    {
        [HotfixMono("spine动画")]
        public Spine.Unity.SkeletonGraphic spineAni;
        
        [HotfixMono("待机表现动画配置")]
        public CommonSkeletonGraphicAnimationConfig idleShowAniConfig;
        [HotfixMono("行走表现动画配置")]
        public CommonSkeletonGraphicAnimationConfig walkShowAniConfig;
        [HotfixMono("成功表现动画配置")]
        public CommonSkeletonGraphicAnimationConfig successShowAniConfig;
        [HotfixMono("失败表现动画配置")]
        public CommonSkeletonGraphicAnimationConfig failShowAniConfig;

        [HotfixMono("皮肤名列表")]
        public List<string> skinNameList;
    }
}
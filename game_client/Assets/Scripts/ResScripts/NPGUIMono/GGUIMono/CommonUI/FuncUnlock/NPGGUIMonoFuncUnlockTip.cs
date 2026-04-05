using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIMonoFuncUnlockTip: _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("解锁功能容器")]
        public NPGGUIMonoFuncUnlockTipContainer monoContainer;
        [ALHeader("入口解锁飞行特效开始位置")]
        public Transform flySfxStartTransform;
        [ALHeader("入口解锁飞行特效及镜头移动时间")]
        public float sfxFlyTimeSec = 1f;
        [ALHeader("延时展示界面和模糊背景时间")]
        public float delayShowTimeSec = 0.5f;

        [ALInfo("========animator动画配置========")]
        [ALHeader("animator组件")]
        public Animator animator;
        [ALHeader("进入动画名称")]
        public string enterAnimatorName;
        [ALHeader("idle动画名称")]
        public string idleAnimatorName;
        [ALHeader("idle动画最少需要展示时间秒(超过这个时间才能关闭弹窗)")]
        public float idleMiniShowTimeS;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1728); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1728); } }
    }
}
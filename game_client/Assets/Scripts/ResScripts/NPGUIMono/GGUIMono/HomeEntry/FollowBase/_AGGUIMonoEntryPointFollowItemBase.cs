using ALPackage;
using TMPro;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 功能入口点基类
    /// </summary>
    public abstract class _AGGUIMonoEntryPointFollowItemBase : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("入口名字")]
        public TextEx textName;
        public TextMeshProUGUI textName2;
        [ALHeader("入口点状态动画")]
        public CommonAnimationShowTypeInfo<EEntryPointAniType> stateAniInfo;
        [ALHeader("红点")]
        public NPGGUIMonoCommonRedTip monoRed;
        [ALHeader("点击按钮")]
        public GameObject btnCLick;
        [ALHeader("功能入口点解锁提示")]
        public GGUIMonoSubEntryFuncUnlockTip monoSubEntryFuncUnlockTip;
    }
}
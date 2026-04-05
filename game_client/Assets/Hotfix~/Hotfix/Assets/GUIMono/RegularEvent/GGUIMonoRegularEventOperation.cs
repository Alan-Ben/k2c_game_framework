using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    /// <summary>
    /// 万能活动操作界面
    /// </summary>
    public class GGUIMonoRegularEventOperation : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("关闭按钮")]
        public GameObject btnClose;
        [HotfixMonoAttribute("使用按钮")]
        public GameObject btnUse;
        [HotfixMonoAttribute("添加道具按钮")]
        public GameObject btnAddItem;
        [HotfixMonoAttribute("消耗的道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [HotfixMonoAttribute("使用10次道具开关")]
        public NPGGUIMonoCommonToggleEx toggleUseTen;
        [HotfixMonoAttribute("有放置道具时显示的GO列表")]
        public List<GameObject> goHaveItemShowList;
        [HotfixMonoAttribute("有放置道具时隐藏的GO列表")]
        public List<GameObject> goHaveItemHideList;
        [HotfixMonoAttribute("不可使用时需要置灰的列表")]
        public List<MaskableGraphic> goCanNotUseGrayList;
        [HotfixMonoAttribute("延时展示奖励提示时间秒")]
        public float delayShowRewardTimeSec;
        [HotfixMonoAttribute("排行榜附加窗口")]
        public GGUIHotfixCommonMono monoSubRank;

        [HotfixMonoAttribute("使用道具Animator动画")]
        public Animator useItemAnimator;
        [HotfixMonoAttribute("切换Animator动画参数名")]
        public string aniParamName;
        [HotfixMonoAttribute("切换到使用道具动画的id")]
        public int useItemAniValue;
        [HotfixMonoAttribute("切换到停留动画的id")]
        public int idleAniValue;

        [HotfixMonoAttribute("使用道具特效id父节点")]
        public Transform sfxParent;
        [HotfixMonoAttribute("使用道具特效id")]
        public long useSfxId;
    }
}
using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 权益卡领奖状态
    /// </summary>
    public enum EPrivilegeCardRewardState
    {
        [InspectorName("NOT_ACTIVATE（未激活）")]
        NOT_ACTIVATE,
        [InspectorName("ACTIVATE_CAN_GET_REWARD（已激活未领取奖励）")]
        ACTIVATE_CAN_GET_REWARD,
        [InspectorName("ACTIVATE_ALREADY_GET_REWARD（已激活已领取奖励）")]
        ACTIVATE_ALREADY_GET_REWARD,
    }

    /// <summary>
    /// 权益卡子窗口
    /// </summary>
    public class GGUIMonoSubPrivilegeCard : _AALBasicUIWndMono
    {
        [ALHeader("购买按钮")]
        public GGUIMonoCommonBuyButton monoBuyButton;
        [ALHeader("领奖按钮")]
        public GameObject btnGetReward;
        [ALHeader("已领奖按钮")]
        public GameObject btnAlreadyGetReward;
        [ALHeader("激活剩余时间")]
        public Text txtLeftTime;
        [ALHeader("获得奖励描述")]
        public Text txtRewardDesc;
        [ALHeader("折扣价值百分比")]
        public Text txtProfitPer;
        [ALHeader("权益描述列表")]
        public GGUIMonoPrivilegeCardDescContainer monoDescContainer;
        [ALHeader("购买奖励列表")]
        public NPGGUIMonoCommonItemContainer monoBuyRewardContainer;
        [ALHeader("每日奖励列表")]
        public NPGGUIMonoCommonItemContainer monoDailyRewardContainer;
        [ALHeader("权益卡显示状态列表")]
        public List<NPCommonEnumStatInfo<EPrivilegeCardRewardState>> privilegeCardStateList;
        [ALHeader("引导手指GO")]
        public GameObject goGuideHand;
        [ALHeader("延时隐藏引导手指GO")]
        public float delayHideGuideHand = 3f;
    }
}
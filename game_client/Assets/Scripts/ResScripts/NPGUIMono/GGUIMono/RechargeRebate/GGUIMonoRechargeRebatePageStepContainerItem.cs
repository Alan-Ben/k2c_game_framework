using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 充值返利组步骤列表item
    /// </summary>
    public class GGUIMonoRechargeRebatePageStepContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("充值按钮")]
        public GameObject btnRecharge;
        [ALHeader("领取按钮")]
        public GameObject btnGetReward;
        [ALHeader("礼包名称")]
        public Text txtGiftPackName;
        [ALHeader("奖励道具列表")]
        public GGUIMonoCommonRewardContainer monoItemContainer;
        [ALHeader("进度条")]
        public Slider sldProgress;
        [ALHeader("进度文本")]
        public Text txtProgress;
        [ALHeader("不同领奖状态显示的GO列表")]
        public List<NPCommonEnumStatInfo<ECommonRewardType>> goRewardStatList;

    }
}

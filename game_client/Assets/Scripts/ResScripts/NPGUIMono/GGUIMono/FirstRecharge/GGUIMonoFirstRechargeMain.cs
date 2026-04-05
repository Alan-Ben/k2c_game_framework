using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 首充礼包主界面
    /// </summary>
    public class GGUIMonoFirstRechargeMain : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("形象列表")]
        public List<GGUIMonoSubFirstRechargeSpecialRewardActor> monoSpecialRewardActorList;
        [ALHeader("礼包价值百分比")]
        public TextMeshProUGUIEx txtDiscount;
        [ALHeader("奖励列表标题")]
        public Text txtRewardTitle;
        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer monoItemContainer;
        [ALHeader("顾问特殊奖励item")]
        public GGUIMonoHeroCommonCardItem monoHeroCard;
        [ALHeader("情人特殊奖励item")]
        public GGUIMonoConsortCardItem monoConsortCard;
        [ALHeader("道具特殊奖励item")]
        public GGUIMonoCommonRewardContainerItem monoSpecialItem;
        [ALHeader("道具特殊奖励详情按钮(藏品需要打开藏品界面)")]
        public GameObject btnSpecItemDetail;
        [ALHeader("页签列表")]
        public List<NPGGUIMonoCommonTab> monoTabList;
        [ALHeader("特殊奖励已领取时显示的GO列表")]
        public List<GameObject> goSpecialRewardGetShowList;
        [ALHeader("没有特殊奖励时显示的GO列表")]
        public List<GameObject> goNoSpecialItemShowList;
        [ALHeader("没有特殊奖励时隐藏的GO列表")]
        public List<GameObject> goNoSpecialItemHideList;

        [ALInfo("========领取相关========")]
        [ALHeader("领奖按钮")]
        public GameObject btnGetReward;
        [ALHeader("待领取倒计时")]
        public Text txtCD;
        [ALHeader("奖励状态列表")]
        public List<NPCommonEnumStatInfo<ECommonRewardType>> rewardStateList;

        [ALInfo("========充值相关========")]
        [ALHeader("购买按钮")]
        public GGUIMonoCommonBuyButton monoBuyButton;
        [ALHeader("已购买显示的GO列表")]
        public List<GameObject> goHadRechargeShowList;
        [ALHeader("已购买隐藏的GO列表")]
        public List<GameObject> goHadRechargeHideList;



        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8000); } }
    }

}

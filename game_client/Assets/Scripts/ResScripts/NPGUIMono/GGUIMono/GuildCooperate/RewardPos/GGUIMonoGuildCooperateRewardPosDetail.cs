using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 公会协作奖励据点详情弹窗
    /// </summary>
    public class GGUIMonoGuildCooperateRewardPosDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("奖励据点图标")]
        public RawImage imgIcon;
        [ALHeader("奖励据点名称")]
        public Text txtName;
        [ALHeader("奖励据点描述")]
        public Text txtDesc;
        [ALHeader("必得奖励物品容器")]
        public NPGGUIMonoCommonItemContainer monoDefinitelyItemContainer;
        [ALHeader("随机奖励物品容器")]
        public NPGGUIMonoCommonItemContainer monoRandomItemContainer;
        [ALHeader("领取按钮")]
        public GameObject btnGetReward;
        [ALHeader("不可领取按钮")]
        public GameObject btnCanNotGetReward;
        [ALHeader("领奖状态展示")]
        public MultiStateShow<ECommonRewardType> rewardTypeShow;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4934); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4934); } }
    }
}
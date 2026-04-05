using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP奖励主界面
    /// </summary>
    public class GGUIMonoVIPMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;
        [ALHeader("充值途径按钮")]
        public GameObject btnAccess;
        [ALHeader("上一个按钮")]
        public GameObject btnPrevious;
        [ALHeader("下一个按钮")]
        public GameObject btnNext;
        [ALHeader("预览vip特权")]
        public GameObject btnVIPPreview;
        [ALHeader("页签列表")]
        public GGUIMonoVIPTabContainer monoTabContainer;
        [ALHeader("页面父节点")]
        public Transform pageParent;
        [ALHeader("奖励列表标题")]
        public Text txtRewardTitle;
        [ALHeader("奖励列表容器")]
        public GGUIMonoCommonRewardContainer monoRewardContainer;
        [ALHeader("角色特殊奖励容器")]
        public GGUIMonoVIPActorItemContainer monoActorItemContainer;
        [ALHeader("不同领奖状态显示的GO列表")]
        public List<NPCommonEnumStatInfo<ECommonRewardType>> goRewardStatList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8100); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8100); } }
    }
}

using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励任务列表item
    /// </summary>
    public class GGUIMonoActivityStepRewardGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("阶段奖励名")]
        public Text txtName;
        [ALHeader("阶段奖励步骤进度文本(可领取时展示)")]
        public Text txtProcess;
        [ALHeader("阶段奖励步骤进度文本(不可领取时展示)")]
        public Text txtCanNotGetProcess;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonMaskItemContainer rewardItemContainer;
        [ALHeader("状态列表")]
        public List<GGUIAchievePointProgressState> stateList;
    }
}

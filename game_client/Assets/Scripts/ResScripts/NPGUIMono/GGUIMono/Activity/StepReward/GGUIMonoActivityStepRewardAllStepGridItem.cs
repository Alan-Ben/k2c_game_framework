using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励所有步骤列表item
    /// </summary>
    public class GGUIMonoActivityStepRewardAllStepGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("成就名")]
        public Text txtName;
        [ALHeader("是否需要显示步骤序号")]
        public bool needShowStepOrder;
        [ALHeader("成就步骤进度文本")]
        public Text txtProcess;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonMaskItemContainer rewardItemContainer;
        [ALHeader("状态列表")]
        public List<GGUIMonoAchieveStepGridItemState> stateList;
        
        [ALHeader("领取按钮")]
        public GameObject btnGet;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
    }
}
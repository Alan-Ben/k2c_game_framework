using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励步骤列表item
    /// </summary>
    public class GGUIMonoActivityStepRewardStepGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("成就名")]
        public Text txtName;
        [ALHeader("成就步骤进度文本")]
        public Text txtProcess;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonMaskItemContainer rewardItemContainer;
        [ALHeader("状态列表")]
        public List<GGUIMonoAchieveStepGridItemState> stateList;
        
        [ALHeader("当前进度文本大小")]
        public long curProcessTextSize = 40;
        [ALHeader("当前是完成状态需要置灰的列表")]
        public List<MaskableGraphic> finishGrayList;
    }
}

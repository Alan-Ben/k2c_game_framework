using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;


/// <summary>
/// 成就阶段状态枚举
/// </summary>
public enum EAchieveStepGetStat
{
    NONE,
    [InspectorName("可以领取")]
    CAN_GET,
    [InspectorName("正在进行中不能领取")]
    DOING_CAN_NOT_GET,
    [InspectorName("不在进行中的不能领取")]
    CAN_NOT_GET,
    [InspectorName("已经领取完成")]
    HAS_GET,
}

/// <summary>
/// 每日任务状态
/// </summary>
[System.Serializable]
public class GGUIMonoAchieveStepGridItemState
{
    [ALHeader("状态枚举")]
    public EAchieveStepGetStat state;
    [ALHeader("当前状态显示的go列表")]
    public List<GameObject> goListShow;
    [ALHeader("当前状态隐藏的go列表")]
    public List<GameObject> goListHide;
    [ALHeader("需要显示的文本颜色")]
    public Color txtColor;
    [ALHeader("是否改变整个文本")]
    public bool isChgAll;
    [ALHeader("需要置灰的列表")]
    public List<MaskableGraphic> grayList;
    [ALHeader("需要取消置灰的列表")] 
    public List<MaskableGraphic> disGrayList;
}

namespace GOE
{
    public class GGUIMonoAchieveStepGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("成就名")]
        public Text txtName;
        [ALHeader("成就步骤进度文本")]
        public Text txtProcess;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;
        [ALHeader("跳转按钮")]
        public GameObject btnGoTo;
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

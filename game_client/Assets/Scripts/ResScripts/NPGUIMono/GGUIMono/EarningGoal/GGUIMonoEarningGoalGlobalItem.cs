using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 千万目标全民奖励状态枚举
/// </summary>
public enum EEarningGoalGlobalGetStat
{
    [InspectorName("未达成")]
    None = 0,
    [InspectorName("可领取")]
    CanGet = 1,
    [InspectorName("已领取")]
    HadGet = 2,
}
/// <summary>
/// item
/// </summary>
public class GGUIMonoEarningGoalGlobalItem : _TALUGUIMonoGridItem
{
    [ALHeader("标题")]
    public Text txtTitle;
    [ALHeader("领取按钮")]
    public GameObject btnGet;
    [ALHeader("首达玩家信息")]
    public NPGGUIMonoPlayerIcon firstPlayerInfo;
    [ALHeader("首达奖励列表")]
    public GGUIMonoCommonRewardContainer firstItemContainer;
    [ALHeader("奖励列表")]
    public GGUIMonoCommonRewardContainer itemContainer;
    [ALHeader("不同状态显示配置")]
    public List<NPCommonEnumStatInfo<EEarningGoalGlobalGetStat>> statInfos;

    [ALHeader("有玩家达成时显示隐藏的GO")] 
    public List<GameObject> hasAchieveShowGos;
    public List<GameObject> hasAchieveHideGos;

}

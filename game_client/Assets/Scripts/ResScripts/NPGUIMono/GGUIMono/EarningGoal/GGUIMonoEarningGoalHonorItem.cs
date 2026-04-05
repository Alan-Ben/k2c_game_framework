using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 千万目标全民奖励状态枚举
/// </summary>
public enum EEarningGoalHonorGetStat
{
    [InspectorName("未达成")]
    None = 0,
    [InspectorName("可领取")]
    CanGet = 1,
    [InspectorName("已领取")]
    HadGet = 2,
    [InspectorName("还没人达成")]
    Wait = 3,
}
[Serializable]
public class EarningGoalHonorSpecialShow
{
    [ALHeader("荣耀目标id")]
    public long earningGoalHonorId;
    [ALHeader("需要特殊显示的Go")]
    public List<GameObject> specialShowGos;

}
/// <summary>
/// item
/// </summary>
public class GGUIMonoEarningGoalHonorItem : _AALBasicUIWndMono
{
    [ALHeader("领取按钮")]
    public GameObject btnGet;
    [ALHeader("标题")]
    public Text txtTitle;
    [ALHeader("进度")]
    public Text txtProcess;
    [ALHeader("首达玩家信息")]
    public NPGGUIMonoPlayerIcon firstPlayerInfo;
    [ALHeader("奖励列表")]
    public GGUIMonoCommonRewardContainer itemContainer;

    [ALHeader("特殊显示go")] 
    public List<EarningGoalHonorSpecialShow> specialShows;
    [ALHeader("不同状态显示配置")]
    public List<NPCommonEnumStatInfo<EEarningGoalHonorGetStat>> statInfos;
}


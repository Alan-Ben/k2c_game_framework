using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EarningGoalPlayerShowMono
{
    [ALHeader("荣耀目标id")]
    public long earningGoalHonorId;
    [ALHeader("该玩家形象展示")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("玩家形象视频展示")]
    public GGUIMonoCommonShowCase monoShowcase;
    [ALHeader("赚速目标翻译key")] 
    public string earningGoalTransKey = TransKeyConst.earning_goal_main_earning_goal_desc;
    [ALHeader("赚速目标文本")]
    public Text txtEarningGoal;
    [ALHeader("达成时间")]
    public Text txtAchieveTime;
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer itemContainer;
    [ALHeader("有玩家达成后的显隐Go")]
    public List<GameObject> hasAchieveShowGos;
    public List<GameObject> hasAchieveHideGos;

}
/// <summary>
/// 
/// </summary>
public class GGUIMonoEarningGoalMain : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("活动倒计时")]
    public Text txtCD;
    [ALHeader("荣誉奖励")]
    public GameObject btnHonor;
    [ALHeader("个人奖励")]
    public GameObject btnSelf;
    [ALHeader("目标显示信息")]
    public List<EarningGoalPlayerShowMono> earningGoalPlayerShowList = new List<EarningGoalPlayerShowMono>();
}
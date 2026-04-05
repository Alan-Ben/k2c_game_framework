using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 排行榜点赞成功
/// </summary>
///
public class GGUIMonoRankFixedLikeSuc : _AALBasicUIWndMono
{
    [ALHeader("玩家或公会名称")]
    public Text txtPlayerName;
    [ALHeader("玩家或公会排名")]
    public Text txtRank;
    [ALHeader("玩家形象")]
    public GGUIMonoCommonShowCase monoShowcase;
    [ALHeader("排行榜名称")]
    public Text txtRankName;
    [ALHeader("排行榜分数")]
    public Text txtScore;
    [ALHeader("随机感谢文本")]
    public Text txtRandom;
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer rewardContainerMono;
    [ALHeader("请求数据中需要隐藏的GO列表，请求完成显示")]
    public List<GameObject> goReqDataHideList;
    [ALHeader("关闭按钮")]
    public GameObject closeBtn;


    public static string assetPath { get { return UIResPathAssistant.getAssetPath(4302); } }
    public static string objName { get { return UIResPathAssistant.getObjName(4302); } }
}

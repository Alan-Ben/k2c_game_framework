using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerChallenge : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("研究按钮")]
    public GameObject btnResearch;
    [ALHeader("展示挑战列表数量")]
    public int showCount = 4;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("关卡名称文本")]
    public TextEx txtLevelName;
    [ALHeader("每日获得金币数量文本")]
    public TextEx txtDailyCoins;
    [ALHeader("收益加成")]
    public TextEx txtEarnBonus;
    [ALHeader("跳过战斗表现开关")]
    public NPGGUIMonoCommonToggleEx toggleSkipBattle;	
    [ALHeader("可以跳过战斗表现显隐Gos")]
    public List<GameObject> goSkipShowList = new List<GameObject>();
    public List<GameObject> goSkipHideList = new List<GameObject>();
    [ALHeader("爬塔挑战列表")]
    public GGUIMonoTowerChallengeItemContainer towerChallengeItemGrid;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5302); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5302);} }
}
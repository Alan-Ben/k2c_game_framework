using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;



/// <summary>
/// 玩家升级成功弹窗
/// </summary>
///
public class GGUIMonoPlayerLvlUpSuc : _ANPBasicUIWndResBarMono
{
    [ALHeader("玩家形象显示")]
    public GGUIMonoCommonShowCase playerShowcase;
    [ALHeader("玩家半身像")]
    public RawImage imgPlayer;
    [ALHeader("升级需要播放的动作名称")]
    public string lvUpActStr;
    [ALHeader("等级显示")]
    public GGUIMonoPlayerLv lvMono;
    [ALHeader("条目列表容器")]
    public GGUIMonoPlayerLvlUpSucContainer itemMono;
    [ALHeader("特殊条目列表容器")]
    public GGUIMonoPlayerLvlUpSucContainer specialItemMono;
    [ALHeader("关闭按钮")]
    public GameObject closeBtn;
    [ALHeader("首次升级或每日奖励没有变化时显示的GO列表")]
    public List<GameObject> goFirstUpgradeShowList;
    [ALHeader("首次升级或每日奖励没有变化时隐藏的GO列表")]
    public List<GameObject> goFirstUpgradeHideList;
    [ALHeader("升级日期显示")]
    public Text txtDate;

    [ALInfo("========每日获得奖励========")]
    [ALHeader("每日获得奖励图标")]
    public RawImage imgDailyRewardIcon;
    [ALHeader("上一等级每日获得奖励数量")]
    public Text txtLastDailyRewardCount;
    [ALHeader("每日获得奖励数量")]
    public Text txtDailyRewardCount;
    [ALHeader("每日获得奖励新增数量")]
    public Text txtDailyRewardAddCount;
    [ALHeader("每日获得数量数字跳动时间(秒)")]
    public float countChgDurationSec;
    [ALHeader("数字开始跳动时需要播放的动画")]
    public CommonAnimationSingleInfo aniShowTextChg;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1710); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1710); } }
}

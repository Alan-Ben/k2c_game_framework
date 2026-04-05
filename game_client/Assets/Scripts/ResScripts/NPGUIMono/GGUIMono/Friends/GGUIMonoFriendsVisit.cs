using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 好友拜访弹窗
/// </summary>
public class GGUIMonoFriendsVisit : _AALBasicUIWndMono
{
    [ALHeader("玩家形象")]
    public GGUIMonoCommonShowCase showcaseMono;

    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfoMono;

    [ALHeader("对话配表id列表 随机出一个展示")]
    public List<long> dialogIdList;

    [ALHeader("展示对话按钮")]
    public GameObject showDialogBtn;

    [ALHeader("对话开始隐藏的GoList")]
    public List<GameObject> dialogStartHideGoList;

    [ALHeader("对话结束显示的GoList")]
    public List<GameObject> dialogEndShowGoList;

    [ALHeader("领取奖励按钮")]
    public GameObject rewardBtn;

    [ALHeader("播放的动画")]
    public Animation rewardAni;

    [ALHeader("领取奖励播放的动画名称")]
    public string rewardAniStr;

    [ALHeader("收取播放的动画名称")]
    public string particalAniStr;

    [ALHeader("领取奖励播放的人物动画名称")]
    public string playerAniName;

    [ALHeader("奖励物品列表")]
    public NPGGUIMonoCommonItemContainer itemContainerMono;

    [ALHeader("领取奖励播放的动画多久后播放收取动画")]
    public float getRewardMarginTime = 1.0f;

    [ALHeader("播放收取动画时要显示的GoList")]
    public List<GameObject> getRewardShowGoList;

    [ALHeader("播放收取动画时要隐藏的GoList")]
    public List<GameObject> getRewardHideGoList;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1361); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1361); } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;



/// <summary>
/// 其他玩家信息
/// </summary>
///
public class GGUIMonoOtherPlayerInfo : _ANPBasicUIWndResBarMono
{

    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerIconMono;

    [ALHeader("复制按钮")]
    public GameObject copyBtn;

    [ALHeader("聊天按钮")]
    public GameObject chatBtn;

    [ALHeader("删除好友按钮")]
    public GameObject deleteBtn;

    [ALHeader("添加好友按钮")]
    public GameObject addFriendBtn;

    [ALHeader("举报按钮")]
    public GameObject btnReport;

    [ALHeader("当前经验进度条")]
    public NPGGUIMonoProgress expProgress;
    [ALHeader("当前赚速进度条")]
    public NPGGUIMonoProgress earningsProgress;

    [ALHeader("没有联盟需要显示的Go List")]
    public List<GameObject> noUnionShowGoList;
    [ALHeader("没有联盟需要隐藏的Go List")]
    public List<GameObject> noUnionHideGoList;

    [ALHeader("是好友显示的Go List")]
    public List<GameObject> isFriendShowGoList;

    [ALHeader("不是好友显示的Go List")]
    public List<GameObject> noFriendShowGoList;

    // [ALHeader("称号总览")]
    // public GGUIMonoOtherPlayerTitleListItemGrid titleGridMono;

    [ALHeader("玩家形象显示")]
    public GGUIMonoCommonShowCase playerShowcase;

    [ALHeader("展开关闭属性列表toggle")]
    public NPGGUIMonoCommonToggleEx toggleExMono;

    [ALHeader("展开关闭称号总览列表toggle")]
    public NPGGUIMonoCommonToggleEx allTitleToggleExMono;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;
    
    [ALHeader("屏蔽按钮")]
    public GameObject btnShield;
    [ALHeader("可以屏蔽显示，否则隐藏")]
    public List<GameObject> canShieldShow;
    [ALHeader("取消屏蔽按钮")]
    public GameObject btnUnShield;
    [ALHeader("可以取消屏蔽显示，否则隐藏")]
    public List<GameObject> canUnShieldShow;
    [ALHeader("点赞按钮")]
    public GameObject likeBtn;
    [ALHeader("点赞显示的列表")]
    public List<GameObject> likeClickShow;
    [ALHeader("点赞数量")]
    public TextEx txtLikeCount;
    [ALHeader("点赞状态配置")]
    public List<NPCommonEnumStatInfo<EPlayerLikeStat>> likesEnumStatInfos;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1709); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1709); } }
}

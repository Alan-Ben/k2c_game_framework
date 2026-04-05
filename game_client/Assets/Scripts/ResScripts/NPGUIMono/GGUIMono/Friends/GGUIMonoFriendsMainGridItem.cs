using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 好友容器Item
/// </summary>
/// 
public class GGUIMonoFriendsMainGridItem : _TALUGUIMonoGridItem
{

    [ALHeader("头像相关")]
    public NPGGUIMonoPlayerIcon playerIconMono;

    [ALHeader("私聊按钮")]
    public GameObject chatBtn;

    [ALHeader("更多按钮")]
    public GameObject moreBtn;

    [ALHeader("在线时显示Go List")]
    public List<GameObject> onlineShowGoList;

    [ALHeader("离线时显示的Go List")]
    public List<GameObject> offlineShowGoList;

    [ALHeader("上一次上线时间")]
    public Text lastOnlineTxt;

    [ALHeader("更多tip跟随go")]
    public RectTransform moreTipFollowGo;

    [ALHeader("更多tip跟随go偏移")]
    public float moreTipInterval;
}

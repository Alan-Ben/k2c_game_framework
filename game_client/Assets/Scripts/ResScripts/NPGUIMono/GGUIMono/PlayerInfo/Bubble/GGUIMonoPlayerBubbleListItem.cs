using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using System;



/// <summary>
/// 玩家气泡列表item
/// </summary>
public class GGUIMonoPlayerBubbleListItem : _TALUGUIMonoGridItem
{
    [ALHeader("预制体加载父节点")]
    public Transform parentPos;

    [ALHeader("使用中标识")]
    public GameObject usingFlagImg;

    [ALHeader("新头像标识")]
    public GameObject newFlagImg;

    [ALHeader("加锁标识")]
    public GameObject lockImg;

    [ALHeader("选中标识")]
    public GameObject selectImg;

    [ALHeader("点击对象")]
    public GameObject clickGo;

    [ALHeader("选中动画")]
    public Animation selectAni;
}

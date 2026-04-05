using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using System;



/// <summary>
/// 玩家头像列表item
/// </summary>
public class GGUIMonoPlayerIconListItem : _TALUGUIMonoGridItem
{
    [ALHeader("玩家头像加载父节点")]
    public Transform iconPosPar;

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
}

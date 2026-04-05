using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using System;
using GOE;


/// <summary>
/// 常驻排行榜列表容器item
/// </summary>
public class NPGGUIMonoRankFixedItem : _AALBasicUIWndMono
{
    [ALHeader("常驻排行榜id")]
    public long fixedRankId;

    [ALHeader("排行榜名称")]
    public TextEx rankNameTxt;

    [ALHeader("第一名玩家/联盟名称")]
    public TextEx firstNameTxt;

    [ALHeader("第一名玩家/联盟分数文本")]
    public Text firstScoreTxt;

    [ALHeader("没有第一名时需要显示的go列表， 默认隐藏")]
    public List<GameObject> noFirstShowGo;

    [ALHeader("没有第一名时需要隐藏的go列表")]
    public List<GameObject> noFirstHideGo;

    [ALHeader("有点赞次数显示的GoList")]
    public List<GameObject> canLikeShowGoList;

    [ALHeader("点击按钮")]
    public GameObject clickBtn;

    [ALHeader("需要重置的动画")]
    public CommonAnimationSingleInfo needResetAni;
}

using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

/// <summary>
/// 通用奖励容器 
/// </summary>

public class GGUIMonoCommonRewardContainer : _TALUGUIMonoContainerWnd<GGUIMonoCommonRewardContainerItem>
{
    [ALHeader("列表为空时显示")]
    public GameObject noneItemsTips;

    [ALHeader("显示完成后ScrollRect的移动方式")]
    public List<EScrollRectMoveType> ScrollRectMoveTypeList;
}

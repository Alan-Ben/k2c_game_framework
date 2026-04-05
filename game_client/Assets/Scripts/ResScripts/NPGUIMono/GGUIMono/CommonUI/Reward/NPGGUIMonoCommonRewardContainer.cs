using System.Collections.Generic;
using ALPackage;
using UnityEngine;

/// <summary>
/// 通用奖励容器 展示reward_item_list
/// </summary>

public class NPGGUIMonoCommonRewardContainer : _TALUGUIMonoContainerWnd<NPGGUIMonoCommonItem>
{
    [ALHeader("列表为空时显示")]
    public GameObject noneItemsTips;
    
    [ALHeader("显示完成后ScrollRect的移动方式")]
    public List<EScrollRectMoveType> ScrollRectMoveTypeList;
}

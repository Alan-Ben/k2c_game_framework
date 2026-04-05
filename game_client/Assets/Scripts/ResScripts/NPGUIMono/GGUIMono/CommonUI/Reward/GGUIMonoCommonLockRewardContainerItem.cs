using ALPackage;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用奖励容器（带锁定状态） 展示reward_item_list
/// </summary>

public enum ECommonLockRewardType
{
    NONE,
    [InspectorName("CAN_GET_REWARD（可领取）")]
    CAN_GET_REWARD,//待领取
    [InspectorName("HAS_GET_REWARD（已领取）")]
    HAS_GET_REWARD,//已领取
    [InspectorName("NOT_GET_REWARD（还不可领取）")]
    NOT_GET_REWARD,//还不可以领取
    [InspectorName("LOCK（未解锁）")]
    LOCK,//未解锁
}

public class GGUIMonoCommonLockRewardContainerItem : _AALBasicUIWndMono
{
    [ALHeader("不同奖励状态显示的Go List")]
    public List<NPCommonEnumStatInfo<ECommonLockRewardType>> rewardStatList;

    [ALHeader("奖励物品item")]
    public NPGGUIMonoCommonItem itemMono;
}

using System;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum EGuildDungeonState
{
    [InspectorName("未解锁")]
    Lock = 0, // 未解锁
    [InspectorName("已解锁未开启")]
    WaitStart = 1, // 已解锁未开启
    [InspectorName("已开启")]
    Started = 2, // 开启
    [InspectorName("已击败")]
    End = 3, // 结束
}

[Serializable]
public class GuildDungeonStateShow
{
    public EGuildDungeonState state; // 状态
    public List<GameObject> showObjs; // 显示的物体列表
}
/// <summary>
/// 联盟PVE主界面
/// </summary>
public class GGUIMonoGuildDungeonMainGridItem : _TALUGUIMonoGridItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("进入按钮")]
    public GameObject btnEnter;
    [ALHeader("副本名")]
    public Text txtName;
    [ALHeader("副本名")]
    public Text txtNum;
    [ALHeader("血条")]
    public NPGGUIMonoProgress progressBlood;
    [ALHeader("奖励数量")]
    public Text txtReward;
    [ALHeader("进度")]
    public Text txtProcess;
    [ALHeader("boss形象")]
    public RawImage bossIcon;
    // </AutoGen:MonoDeclaration>
    [ALHeader("解锁条件文本")]
    public Text txtLockCondition; // 解锁条件文本
    [ALHeader("副本状态显示")]
    public List<GuildDungeonStateShow> stateShows = new List<GuildDungeonStateShow>();
    [ALHeader("有奖励可领取时显示的Go")]
    public List<GameObject> goShowOnReward; // 有奖励可领取时显示的Go
    [ALHeader("红点提示")]
    public GameObject goRedTip;
}


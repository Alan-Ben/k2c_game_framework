using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public enum GuildDungeonMonsterState
{
    [InspectorName("无状态")]
    None = 0,        
    [InspectorName("未击败")]
    NotDefeat = 1,   
    [InspectorName("未击败有宝箱")]
    NotDefeatReward = 2,     
    [InspectorName("已击败")]
    Defeat = 3, 
    [InspectorName("有奖励可领取")]
    CanReward = 4,
    [InspectorName("奖励已领取")]
    GainedReward = 5,
}

[Serializable]
public class GuildDungeonMonsterStateShow
{
    public GuildDungeonMonsterState state;
    public List<GameObject> showGos;

}
/// <summary>
/// 联盟PVE地图跟随窗口
/// </summary>
public class GGUIMonoGuildDungeonMapFollowItem : _AALBasicUIWndMono
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("血条")]
    public NPGGUIMonoProgress progressBlood;
    [ALHeader("战斗按钮")]
    public GameObject btnBattle;
    [ALHeader("怪物图标")]
    public RawImage imgIcon;
    // </AutoGen:MonoDeclaration>
    [ALHeader("标记状态显示Gos")]
    public List<GameObject> listTagShowGos;
    [ALHeader("怪物状态显示Gos")]
    public List<GuildDungeonMonsterStateShow> monsterStateShowGos;

    public Transform linePointRoot;
    [ALHeader("显示宝箱奖励预览")]
    public GameObject btnShowReward;
    [ALInfo("====宝箱预览配置====")]
    [ALHeader("每日任务宝箱奖励预览pathid")]
    public long boxRewardPreviewResPathId = 53;
    [ALHeader("每日任务宝箱奖励预览标题翻译key")]
    public string boxRewardPreviewTitleStrKey;
    [ALHeader("宝箱奖励预览位置")]
    public RectTransform boxRewardPreviewRoot;
    [ALHeader("宝箱奖励预览弹窗位置偏移")]
    public Vector2 boxRewardPreviewToolTipOffset;
    [ALHeader("可攻击Boss显示Gos")]
    public List<GameObject> listCanFightShowGos;

}
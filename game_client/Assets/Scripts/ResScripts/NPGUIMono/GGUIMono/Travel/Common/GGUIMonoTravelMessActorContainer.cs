using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 情报信息界面妃子和npc容器
/// </summary>
public class GGUIMonoTravelMessActorContainer:_AALBasicUIWndMono
{
    [ALHeader("npc、妃子加载出来的父节点")]
    public Transform itemContainer;
    [ALHeader("妃子预制体")]
    public GGUIMonoTravelConsortItem consortItem;
    [ALHeader("npc预制体")]
    public GGUIMonoTravelNpcItem npcItem;
}
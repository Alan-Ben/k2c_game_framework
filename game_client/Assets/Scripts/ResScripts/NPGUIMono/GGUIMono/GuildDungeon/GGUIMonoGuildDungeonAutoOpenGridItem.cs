using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// 联盟PVE自动开启
/// </summary>
public class GGUIMonoGuildDungeonAutoOpenGridItem : _TALUGUIMonoGridItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("boss图标")]
    public RawImage icoBoss;
    [ALHeader("boss名字")]
    public Text txtName;
    [ALHeader("开启消耗")]
    public Text txtOpenCost;
    [ALHeader("boss血量")]
    public NPGGUIMonoProgress bossBlood;
    [ALHeader("勾选自动开启")]
    public NPGGUIMonoCommonToggleEx toggleOpen;
    // </AutoGen:MonoDeclaration>
    [ALHeader("解锁条件文本")]
    public Text txtLockCondition; // 解锁条件文本
    [ALHeader("副本未解锁需要显示的Gos")]
    public List<GameObject> lockShowGos; // 显示的物体列表
    public List<GameObject> lockHideGos; // 显示的物体列表

}


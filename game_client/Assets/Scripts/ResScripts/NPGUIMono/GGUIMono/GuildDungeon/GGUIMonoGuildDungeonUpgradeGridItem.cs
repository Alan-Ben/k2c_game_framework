using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// 联盟副本升级
/// </summary>
public class GGUIMonoGuildDungeonUpgradeGridItem : _TALUGUIMonoGridItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("boss图标")]
    public RawImage icoBoss;
    [ALHeader("boss名字")]
    public Text txtName;
    [ALHeader("开启消耗")]
    public Text txtOpenCost;
    [ALHeader("boss血量")]
    public Text txtBossBlood;
    [ALHeader("副本总血量")]
    public Text txtTotalBlood;
    [ALHeader("升级按钮")]
    public GameObject btnUpgrade;
    // </AutoGen:MonoDeclaration>
    [ALHeader("解锁条件文本")]
    public Text txtLockCondition; // 解锁条件文本
    [ALHeader("副本未解锁需要显示的Gos")]
    public List<GameObject> lockShowGos; // 显示的物体列表
    public List<GameObject> lockHideGos; // 显示的物体列表
}


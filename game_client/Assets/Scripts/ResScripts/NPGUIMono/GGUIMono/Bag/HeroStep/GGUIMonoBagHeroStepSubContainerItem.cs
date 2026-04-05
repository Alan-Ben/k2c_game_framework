using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using System;

/// <summary>
/// 背包骑士阶段合成的单个道具
/// </summary>
public class GGUIMonoBagHeroStepSubContainerItem : _AALBasicUIWndMono
{
    [ALHeader("物品")]
    public NPGGUIMonoCommonItem commonItemMono;

    [ALHeader("合成按钮")]
    public GameObject combinedGo;

    [ALHeader("获取途径按钮")]
    public GameObject accessWayGo;

    [ALHeader("合成显示的GoList")]
    public List<GameObject> combinedShowGoList;

    [ALHeader("不能合成显示的GoList")]
    public List<GameObject> noCombinedShowGoList;
}

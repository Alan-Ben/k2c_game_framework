using ALPackage;
using GOE;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单层的被合成界面
/// </summary>
public class GGUIMonoBagItemBeCombined : _AALBasicUIWndMono
{
    [ALHeader("目标物品")]
    public NPGGUIMonoCommonItem monoTargetItem;

    [ALHeader("合成原料物品")]
    public NPGGUIMonoCommonItem monoOriItem;

    [ALHeader("目标物品数量")]
    public TextEx txtTargetItemCount;

    [ALHeader("目标物品数量使用的key")]
    public string txtTargetItemCountKey;

    [ALHeader("合成数量纯数字")]
    public TextEx txtCombinedNum;

    [ALHeader("合成资源物品列表")]
    public NPGGUIMonoCommonItemContainer monoResItemList;

    [ALHeader("使用数量计数器")]
    public GGUIMonoBagPopCounter useCounter;

    [ALHeader("不可合成时需要置灰的列表")]
    public List<MaskableGraphic> grayImgList;

    [ALHeader("原材料不足或合成条件不通过时显示的GoList")]
    public List<GameObject> oriNoEnoughShowGoList;

    [ALHeader("原材料足够并且合成条件通过时显示的GoList")]
    public List<GameObject> oriEnoughShowGoList;

    [ALHeader("合成按钮")]
    public GameObject btnCombine;
    [ALHeader("物品不足按钮")]
    public GameObject btnNotEnough;

    [ALHeader("跳转获取途径按钮")]
    public GameObject toAccessBtn;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1918); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1918); } }
}

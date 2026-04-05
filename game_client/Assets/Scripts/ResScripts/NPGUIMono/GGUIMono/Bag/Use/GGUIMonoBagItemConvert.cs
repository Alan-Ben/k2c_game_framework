using ALPackage;
using GOE;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包合成物品弹窗
/// </summary>
public class GGUIMonoBagItemConvert : _AALBasicUIWndMono
{
    [ALHeader("目标物品列表")]
    public NPGGUIMonoCommonItemContainer monoTargetItemContainer;

    [ALHeader("目标物品数量是否显示合成数量 - false 显示当前拥有的数量")]
    public bool isShowCombineCount = true;

    [ALHeader("合成原料物品")]
    public NPGGUIMonoCommonItem monoOriItem;

    [ALHeader("合成原料物品数量")]
    public Text txtOriItemCount;

    [ALHeader("合成资源物品列表")]
    public NPGGUIMonoCommonItemContainer monoResItemList;

    [ALHeader("使用数量计数器")]
    public GGUIMonoBagPopCounter useCounter;

    [ALHeader("合成按钮")]
    public GameObject btnCombine;

    [ALHeader("不可合成时需要置灰的列表")]
    public List<MaskableGraphic> grayImgList;
    [ALHeader("物品不足按钮")]
    public GameObject btnNotEnough;
    [ALHeader("物品不足时需要显示的GO列表")]
    public List<GameObject> goItemNotEnoughShowList;
    [ALHeader("物品不足时需要隐藏的GO列表")]
    public List<GameObject> goItemNotEnoughHideList;

    [ALHeader("跳转获取途径按钮")]
    public GameObject toAccessBtn;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1912); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1912); } }
}

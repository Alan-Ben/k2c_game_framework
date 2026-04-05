using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

// 批量购买弹窗
public class NPGGUIMonoBatchBuy : _AALBasicUIWndMono
{
    [ALHeader("购买的物品")]
    public NPGGUIMonoCommonItem gainItemMono;

    [ALHeader("购买的物品描述")]
    public Text gainItemDetail;

    [ALHeader("折扣挂载父节点")]
    public Transform discountPosParent;

    [ALHeader("推荐标识挂载节点")]
    public Transform flagPosParent;

    [ALHeader("限购文本")]
    public Text limitBuyTxt;

    [ALHeader("有限购时显示的Go List")]
    public List<GameObject> hasBuyLimitList;

    [ALHeader("商品为免费时需要显示的Go List")]
    public List<GameObject> freeShowGoList;

    [ALHeader("商品为免费时需要隐藏的Go List")]
    public List<GameObject> freeHideGoList;

    [ALHeader("数量为1时需要隐藏的列表")]
    public List<GameObject> oneLeftHideGoList;

    [ALHeader("数量为1时是否要隐藏")]
    public bool isOneLeftNeedHide;

    [ALHeader("购买数量计数器")]
    public GGUIMonoBagPopCounter useCounter;

    [ALHeader("消耗足够展示的GoList")]
    public List<GameObject> costEnoughShowGoList;

    [ALHeader("消耗不足展示的GoList")]
    public List<GameObject> costNoEnoughShowGoList;

    [ALHeader("消耗展示")]
    public NPGGUIMonoCommonItem costUseItem;

    [ALHeader("消耗按钮")]
    public GameObject costUseBtn;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3102); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3102); } }
}

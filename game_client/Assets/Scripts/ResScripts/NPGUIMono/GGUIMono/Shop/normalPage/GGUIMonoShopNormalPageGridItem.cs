using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//商品打折类型枚举
public enum EShopItemDiscountType
{
    NO_DISCOUNT,//原价
    DISCOUNT,//打折
    FREE,//免费
}

//商品显示类型枚举
public enum EShopItemShowType
{
    NORMAL,//可购买
    LOCKED,//未解锁
    SALE_OUT,//售罄
}

[System.Serializable]
public class ShopNormalPageGridItemGrayMono
{
    [ALHeader("显示枚举")]
    public EShopItemShowType type;
    [ALHeader("需要置灰的列表")]
    public List<MaskableGraphic> grayList;
    [ALHeader("不需要置灰的列表")]
    public List<MaskableGraphic> disgrayList;
}

/// <summary>
/// 普通商店 容器Item
/// </summary>
/// 
public class GGUIMonoShopNormalPageGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("通用物品显示")]
    public NPGGUIMonoCommonItem commonItemMono;
    [ALHeader("物品特殊品质图")]
    public RawImage qualityImg;
    [ALHeader("物品特殊数量文本")]
    public TextEx numTxt;
    [ALHeader("物品特殊图片")]
    public RawImage itemImg;

    [ALHeader("购买消耗物品")]
    public NPGGUIMonoCommonItem costItemMono;

    [ALHeader("购买按钮")]
    public GameObject buyBtn;

    [ALHeader("商品标识挂载节点")]
    public Transform flagPosParent;

    [ALHeader("打折商品标识挂载节点")]
    public Transform offFlagPosParent;

    [ALHeader("需要置灰的列表")]
    public List<ShopNormalPageGridItemGrayMono> needGrayList;

    [ALHeader("限购显示的Go")]
    public List<GameObject> hasBuyLimitList;

    [ALHeader("限购次数文本")]
    public Text buyLimitTxt;

    [ALInfo(" LOCKED,//为解锁" +
            "NORMAL,//可购买" +
            "SALE_OUT,//售罄")]
    [ALHeader("商品打折类型显示列表")]
    public List<NPCommonEnumStatInfo<EShopItemShowType>> showStatList;
    [ALInfo("NO_DISCOUNT,//原价" +
            "DISCOUNT,//打折" +
            "FREE,//免费")]
    [ALHeader("商品打折类型显示列表")]
    public List<NPCommonEnumStatInfo<EShopItemDiscountType>> discountStatList;

    [ALHeader("原价文本列表")]
    public List<Text> noDiscountTxtList;

    [ALHeader("数量为1时需要隐藏的列表")]
    public List<GameObject> oneLeftHideGoList;

    [ALHeader("数量为1时是否要隐藏")]
    public bool isOneLeftNeedHide;

    [ALHeader("背景架子")]
    public GameObject bkGo;

    [ALHeader("未解锁时购买条件文本")]
    public Text txtBuyCondition;
}

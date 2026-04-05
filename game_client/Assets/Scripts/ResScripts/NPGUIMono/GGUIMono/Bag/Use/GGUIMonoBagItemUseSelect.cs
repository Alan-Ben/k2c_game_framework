using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

// 使用物品自选弹窗
public class GGUIMonoBagItemUseSelect : _AALBasicUIWndMono
{
    [ALHeader("物品")]
    public NPGGUIMonoCommonItem itemWnd;

    [ALHeader("物品描述")]
    public Text itemDetail;

    [ALHeader("使用数量计数器")]
    public GGUIMonoBagPopCounter useCounter;

    [ALHeader("自选物品列表")]
    public GGUIMonoBagSelectItemContainer selectGrid;

    [ALHeader("带消耗使用按钮")]
    public GGUIMonoBagCostUseBtn costUseBtn;
    [ALHeader("带消耗物品不足按钮")]
    public GGUIMonoBagCostUseBtn notEnoughUseBtn;

    [ALHeader("没有选择足够道具时需要置灰的列表")]
    public List<MaskableGraphic> grayImgList;
    [ALHeader("物品不足时需要显示的GO列表")]
    public List<GameObject> goItemNotEnoughShowList;
    [ALHeader("物品不足时需要隐藏的GO列表")]
    public List<GameObject> goItemNotEnoughHideList;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    [ALHeader("当前选中个数的文字")]
    public Text selectNumText;

    [ALHeader("选择个数不足时显示的颜色值")]
    public Color noEnoughSelectCountColor;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1910); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1910); } }
}

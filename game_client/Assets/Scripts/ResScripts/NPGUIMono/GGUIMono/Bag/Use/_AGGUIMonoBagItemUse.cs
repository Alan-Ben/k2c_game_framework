using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 背包使用弹窗基类
public abstract class _AGGUIMonoBagItemUse : _AALBasicUIWndMono
{
    [ALHeader("物品")]
    public NPGGUIMonoCommonItem itemWnd;

    [ALHeader("物品描述")]
    public Text itemDetail;

    [ALHeader("弹窗标题")]
    public Text titleTxt;

    [ALHeader("使用数量计数器")]
    public GGUIMonoBagPopCounter useCounter;

    [ALHeader("带消耗使用按钮")]
    public GGUIMonoBagCostUseBtn costUseBtn;
    [ALHeader("带消耗物品不足按钮")]
    public GGUIMonoBagCostUseBtn notEnoughUseBtn;

    [ALHeader("不可使用时需要置灰的列表")]
    public List<MaskableGraphic> grayImgList;
    [ALHeader("物品不足时需要显示的GO列表")]
    public List<GameObject> goItemNotEnoughShowList;
    [ALHeader("物品不足时需要隐藏的GO列表")]
    public List<GameObject> goItemNotEnoughHideList;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

// 使用物品概率弹窗
public class GGUIMonoBagItemUsePercent : _AALBasicUIWndMono
{
    [ALHeader("物品")]
    public NPGGUIMonoCommonItem itemWnd;

    [ALHeader("物品描述")]
    public Text itemDetail;

    [ALHeader("使用数量计数器")]
    public GGUIMonoBagPopCounter useCounter;

    [ALHeader("概率获取的物品列表")]
    public GGUIMonoBagPercentContainer percentContainer;

    [ALHeader("打开后随机获得道具文本")]
    public Text getRandomCountText;

    [ALHeader("带消耗使用按钮")]
    public GGUIMonoBagCostUseBtn costUseBtn;
    [ALHeader("带消耗物品不足按钮")]
    public GGUIMonoBagCostUseBtn notEnoughUseBtn;
    [ALHeader("物品不足时需要显示的GO列表")]
    public List<GameObject> goItemNotEnoughShowList;
    [ALHeader("物品不足时需要隐藏的GO列表")]
    public List<GameObject> goItemNotEnoughHideList;
    [ALHeader("不可使用时需要置灰的列表")]
    public List<MaskableGraphic> grayImgList;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1909); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1909); } }
}

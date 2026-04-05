using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

public class GGUIMonoBagPopItemUseSimple : _AALBasicUIWndMono
{

    [ALHeader("名称")]
    public Text itemName;

    [ALHeader("描述")]
    public Text itemDesc;

    [ALHeader("使用条件父节点 当使用条件为空时可以隐藏")]
    public GameObject itemUseDescParent;

    [ALHeader("使用条件")]
    public Text itemUseDesc;

    [ALHeader("物品个数不足或者消耗物品个数不足需要置灰的列表")]
    public List<MaskableGraphic> grayImgList;

    [ALHeader("物品不足时需要显示的GO列表")]
    public List<GameObject> goItemNotEnoughShowList;
    [ALHeader("物品不足时需要隐藏的GO列表")]
    public List<GameObject> goItemNotEnoughHideList;
    [ALHeader("带消耗使用按钮")]
    public GGUIMonoBagCostUseBtn costUseBtn;
    [ALHeader("带消耗物品不足按钮")]
    public GGUIMonoBagCostUseBtn notEnoughUseBtn;

    [ALHeader("获取途径入口")]
    public GameObject gainWayBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1907); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1907); } }
}

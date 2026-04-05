using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public class GGUIMonoBagPopItemConvertSimple : _AALBasicUIWndMono
{
    [ALHeader("名称")]
    public Text itemName;
    [ALHeader("描述")]
    public Text itemDesc;
    [ALHeader("合成按钮")]
    public GameObject btnCombine;

    [ALHeader("合成数量不足时需要置灰的列表")]
    public List<MaskableGraphic> notEnoughGrayList;
    [ALHeader("物品不足按钮")]
    public GameObject btnNotEnough;
    [ALHeader("物品不足时需要显示的GO列表")]
    public List<GameObject> goItemNotEnoughShowList;
    [ALHeader("物品不足时需要隐藏的GO列表")]
    public List<GameObject> goItemNotEnoughHideList;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1911); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1911); } }
}

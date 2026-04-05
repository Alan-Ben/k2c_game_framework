using GOE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 背包使用弹窗- 展示实际获得的物品
public class GGUIMonoBagItemUseShowRealGainItem : _AGGUIMonoBagItemUse
{
    [ALHeader("实际获得的物品图片")]
    public RawImage realGainItemImg;

    [ALHeader("实际获得的物品数量")]
    public Text realGainItemNumTxt;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1920); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1920); } }
}

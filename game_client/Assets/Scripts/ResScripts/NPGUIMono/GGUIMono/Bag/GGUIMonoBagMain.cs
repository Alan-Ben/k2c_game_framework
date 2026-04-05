using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

/// <summary>
/// 页签类型
/// </summary>
public enum EBagMainTabMonoType
{
    BAG_ITEM,//背包物品
    BAG_CONVERT,//道具合成
    USE_ITEM,//可使用道具
}

//页签类型
[System.Serializable]
public class GGUIBagMainTabMono
{
    [ALHeader("页签类型")]
    public EBagMainTabMonoType tabType;
    [ALHeader("通用页签脚本")]
    public NPGGUIMonoCommonTab monoTab;
    [ALHeader("页签路径id")]
    public long resPathId;
}
// 背包
public class GGUIMonoBagMain : _ANPBasicUIWndResBarMono
{
    [ALHeader("页签列表")]
    public List<GGUIBagMainTabMono> monoTabList;
    
    [ALHeader("子窗口父节点")]
    public Transform pageParent;

    [ALHeader("默认选中页签")]
    public EBagMainTabMonoType defaultTab = EBagMainTabMonoType.BAG_ITEM;

    [ALHeader("返回按钮")]
    public GameObject returnBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1905); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1905); } }
}

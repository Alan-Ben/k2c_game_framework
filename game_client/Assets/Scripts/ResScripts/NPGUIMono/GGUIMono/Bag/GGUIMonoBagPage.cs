using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;


//二级页签  
[System.Serializable]
public class GGUIBagTabSecondMono
{
    [ALHeader("物品类型枚举")]
    public List<NPEnum.ENPBagItemType> secondItemTypeList; //物品类型枚举
    [ALHeader("页签脚本")]
    public NPGGUIMonoCommonTab monoTab;//页签脚本
    [ALHeader("是否显示类型bar")]
    public bool isShowTypeBar;
    [ALHeader("类型bar资源id，如果isShowTypeBar = false，则可不配")]
    public long typeBarUiPathId;//类型bar资源id，如果isShowTypeBar = false，则可不配
}

//一级页签
[System.Serializable]
public class GGUIBagTabFirstMono
{
    [ALHeader("二级页签列表")]
    public List<GGUIBagTabSecondMono> secondMonoList; //二级页签列表
    [ALHeader("页签脚本")]
    public NPGGUIMonoCommonTab monoTab;//页签脚本
    [ALHeader("没有二级标签时需要配置这个")]
    public List<NPEnum.ENPBagItemType> itemTypeList;//物品类型枚举
}

// 背包
public class GGUIMonoBagPage : _ANPBasicUIWndResBarMono
{
    [ALHeader("物品一级页签列表")]
    public List<GGUIBagTabFirstMono> monoTabList;

    [ALHeader("只有一个页签时需要隐藏的Go")]
    public GameObject oneTabHideGo;

    [ALHeader("物品容器")]
    public GGUIMonoBagItemGrid itemGrid;
}

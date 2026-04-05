using System.Collections.Generic;
using ALPackage;
using UnityEngine;

/// <summary>
/// 背包物品红点类型
/// </summary>
public enum EBagItemRedTipType
{
    [InspectorName("NEW（新物品）")]
    NEW,
    [InspectorName("COMBINE（可合成物品）")]
    COMBINE,
    [InspectorName("BE_COMBINE（可被合成物品）")]
    BE_COMBINE,
    [InspectorName("ADD（数量增加的物品）")]
    ADD,
}

[System.Serializable]
public class BagItemRedTipShowInfo
{
    [ALHeader("红点类型")]
    public EBagItemRedTipType redTipType;
    [ALHeader("红点GO")]
    public GameObject redTipGo;
}


// 背包中的物品
public class GGUIMonoBagGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("物品")]
    public NPGGUIMonoCommonItem item;
    [ALHeader("选中显示的对象")]
    public GameObject selectTag;
    [ALHeader("红点提示列表，根据配置的顺序当做优先级展示")]
    public List<BagItemRedTipShowInfo> goRedTipList;
    [ALHeader("点击对象")]
    public GameObject clickGo;
    [ALHeader("物品数量为空时显示")]
    public GameObject unableShow;
}

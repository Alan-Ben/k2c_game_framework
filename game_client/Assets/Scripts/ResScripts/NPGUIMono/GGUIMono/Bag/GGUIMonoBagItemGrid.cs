using ALPackage;
using System.Collections;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;

public enum EGGUIMonoBagItemGridTargetItemType
{
    NONE,
    ITEM_ID,//物品id
    IMPROVE_TARGET_TYPE_ITEM,//可提升的目标类型对应的道具
}

public class GGUIMonoBagItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoBagGridItem>
{
    // 无物品提示
    public GameObject noneItemsTips;
    public List<NPBagItemTypeKeyConfig> typeKeyConfig;
}

[System.Serializable]
public class NPBagItemTypeKeyConfig
{
    [ALHeader("类型")]
    public ENPBagItemType type;
    [ALHeader("翻译key")]
    public string typeKey;
}

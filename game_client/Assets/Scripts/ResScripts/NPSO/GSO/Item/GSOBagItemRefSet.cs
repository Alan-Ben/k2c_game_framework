using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using ClientEnum;

[System.Serializable]
public class BagItemRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;                               //物品ID
    public long sort_id;                          //排序id
    public NPEnum.ENPBagItemType bag_item_type;   // 背包物品类型
    public bool can_sell;                         // 能否出售
    public List<NPCommonCostItem> sell_price;     // 商品出售价格
    public bool need_redtip_when_add;             // 数量累加时，是否需要小红点提示
    public bool need_redtip_when_first_get;       // 是否需要新物品提示

    public bool is_hiding;//是否国库隐藏隐藏
    public List<NPCommonItem> defect_common_item;//缺失类型-id
    public EBagClickShowView show_type; //点击打开的下拉类型
    public bool is_expire_item;//是否是过期道具
}

/**************
 * 物品表
 **/
public class GSOBagItemRefSet : _TALSOBasicRefSet<BagItemRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/bag_refdata.unity3d"; } }
    public static string objName { get { return "bag_item"; } }
}

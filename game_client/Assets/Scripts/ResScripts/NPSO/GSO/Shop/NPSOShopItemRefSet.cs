using ALPackage;
using GOE;
using NPEnum;
using System.Collections.Generic;



/**************
 * 商店中的商品表
 **/

[System.Serializable]
public class NPShopItemRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;
    public long sort_id;//排序id
    public NPCommonCostItem item;//获得的物品
    public NPCommonCostItem cost_item;//固定消耗
    public int times_price_type_id;//递增消耗
    public int ui_res_id;//预制体路径id
    public bool is_recommend;//是否为推荐商品
}

public class NPSOShopItemRefSet : _TALSOBasicRefSet<NPShopItemRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/shop_refdata.unity3d"; } }
    public static string objName { get { return "shop_item"; } }
}

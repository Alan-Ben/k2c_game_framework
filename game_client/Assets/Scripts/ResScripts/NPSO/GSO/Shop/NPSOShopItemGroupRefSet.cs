using ALPackage;
using GOE;
using NPEnum;
using System.Collections.Generic;



/**************
 * 商店中的商品组表
 **/

[System.Serializable]
public class NPShopItemGroupRefObj : _IALBasicRefObj
{
    public long _refId { get { return group_id; } }
    public long group_id;
    public _NPPlayerConditionSerializeInfo enable_cond;//商品组生效条件
    public _NPPlayerConditionSerializeInfo show_cond;//展示条件
    public _NPPlayerConditionSerializeInfo buy_condition;//可购买条件
    public string buy_condition_desc;//可购买条件描述
    public List<string> buy_condition_desc_args;//可购买条件描述参数
}

public class NPSOShopItemGroupRefSet : _TALSOBasicRefSet<NPShopItemGroupRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/shop_refdata.unity3d"; } }
    public static string objName { get { return "shop_item_group"; } }
}

using ALPackage;
using GOE;
using NPEnum;
using System.Collections.Generic;



/**************
 * 商品打折配表
 **/

[System.Serializable]
public class NPShopItemDiscountRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;

    //分组id
    public long group_id;

    //折扣
    public long discount;

    //折扣对应的预制体路径id
    public int ui_res_id;
}

public class NPSOShopItemDiscountRefSet : _TALSOBasicRefSet<NPShopItemDiscountRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/shop_refdata.unity3d"; } }
    public static string objName { get { return "shop_item_discount"; } }
}

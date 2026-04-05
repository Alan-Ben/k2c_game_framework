using ALPackage;
using GOE;
using NPEnum;
using System.Collections.Generic;



/**************
 * 商店表
 **/

[System.Serializable]
public class NPShopRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;
    public string name;//商店名称
    public long red_tip_id;//红点表id
    public _NPPlayerConditionSerializeInfo unlock_cond;//解锁条件
    public string unlock_cond_desc;//解锁条件描述
    public List<string> unlock_cond_desc_args;//解锁条件描述参数
    public NPCommonItem free_refresh_cd; //免费刷新cd
    public long pay_refresh_times_price_id; //付费刷新递增消耗
    public int pay_refresh_limit; //付费刷新次数上限
    public List<long> relate_activity_id_list;//关联活动id列表（当服务端没有传刷新时间时 会根据此活动id列表获取第一个有效的活动 取活动结束时间来展示倒计时）
}

public class NPSOShopRefSet : _TALSOBasicRefSet<NPShopRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/shop_refdata.unity3d"; } }
    public static string objName { get { return "shop"   ; } }
}

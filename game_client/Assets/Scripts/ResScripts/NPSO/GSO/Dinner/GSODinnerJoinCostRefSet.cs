using ALPackage;
using NPEnum;
using SQLite4Unity3d;
using System.Collections.Generic;


/**************
 * 宴会消耗表
 **/

[System.Serializable]
public class GDinnerJoinCostRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public NPCommonCostItem cost_item;//消耗的物品CommonCostItem
    public long join_gain_score;//赴宴获得积分
    public long join_gain_coin;//赴宴获得商店币
    public long banquet_host_factor;//开宴者宴会币结算系数
    public long fixed_cd_id;//fixed_cd表id，0表示不显示次数
    public string name;//赴宴方式名称
}

public class GSODinnerJoinCostRefSet : _TALSOBasicRefSet<GDinnerJoinCostRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/dinner.unity3d"; } }
    public static string objName { get { return "dinner_join_cost"; } }
}

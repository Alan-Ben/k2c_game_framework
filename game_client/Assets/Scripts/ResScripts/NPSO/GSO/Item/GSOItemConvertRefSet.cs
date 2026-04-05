using ALPackage;
using System.Collections.Generic;
using GOE;

[System.Serializable]
public class ItemConvertRefObj : _IALBasicRefObj
{
    public long _refId { get { return bag_item_id; } }

    public long bag_item_id;//原材料物品ID
    public long ori_item_num;//原材料数量
    public List<NPCommonCostItem> cost_item_list;//消耗列表
    public NPCommonItem target_item;//产物 数量固定为1
    public int sort_id;//排序id 从小到大
    public _NPPlayerConditionSerializeInfo convert_condition;//合成条件
    public string convert_condition_desc;//合成条件描述
    public List<string> convert_condition_desc_args;//合成条件描述参数
}

/**************
 * 物品兑换表
 **/
public class GSOItemConvertRefSet : _TALSOBasicRefSet<ItemConvertRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/bag_refdata.unity3d"; } }
    public static string objName { get { return "item_convert"; } }
}

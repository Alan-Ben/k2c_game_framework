using System.Collections.Generic;
using ALPackage;

/// <summary>
/// 操作消耗表
/// </summary>
[System.Serializable]
public class OpCostGroupRefObj : _IALBasicRefObj
{
    public long _refId { get { return cost_group_id; } }

    public long cost_group_id;//消耗组id

    public List<OpCostRefObj> opCostRefObjList;//操作消耗列表(在导出代码OpCostExportMenu中按照操作次数从小到大排序了)
    
    /// <summary>
    /// 获取当前操作次数的消耗物品
    /// </summary>
    /// <param name="_opCount"></param>
    /// <returns></returns>
    public OpCostRefObj getOpCostRefObj(int _opCount)
    {
        if (opCostRefObjList == null)
            return null;

        // 传入的_opCount参数是当前已经操作过的次数, 在这里需要找到下一个操作次数的消耗物品, 所以加1
        _opCount++;
        
        OpCostRefObj result = null;
        foreach (OpCostRefObj item in opCostRefObjList)
        {
            if(item == null)
                continue;
         
            // 当遍历到配表数据的操作次数大于_opCount时, 直接退出循环, 因为opCostRefObjList是按照操作次数从小到大排序的
            if(item.op_count > _opCount)
                break;
            
            result = item;
        }

        return result;
    }
}

[System.Serializable]
public class OpCostRefObj
{
    public long id;//唯一id
    
    public int op_count;//操作次数
    public NPCommonCostItem cost_item;//消耗物品
}

[System.Serializable]
public class TmpOpCostRefObj
{
    public long id;//唯一id
    public long cost_group_id;//消耗组id
    public int op_count;//操作次数
    public NPCommonCostItem cost_item;//消耗物品
}

/// <summary>
/// 操作消耗表
/// </summary>
public class GSOOpCostGroupRefSet : _TALSOBasicRefSet<OpCostGroupRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "op_cost"; } }
}
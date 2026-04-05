using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using GOE;

[System.Serializable]
public class ItemDefRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;                               //物品ID

    public NPCommonItem item;   // item
    
    public _NPPlayerVariableSerializeInfo count_formula;//通过高级公式的方式取物品数量
    
#if NP_GAME
    /// <summary> 获取数据</summary>
    public long getCount()
    {
        if(count_formula == null)
            return 0;

        return count_formula.CalculateVariableResult(null);
    }
#endif
}

/**************
 * 物品表
 **/
public class GSOItemDefRefSet : _TALSOBasicRefSet<ItemDefRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "item_def"; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using GOE;

[System.Serializable]
public class ItemExchangeRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;                               //物品ID

    public NPCommonItem ori_item;   // 原物品
    public _NPPlayerConditionSerializeInfo cond;                         // 转换条件
    public NPCommonCostItem target_item;     // 新物品
    public string desc;             // 转换的描述
    public List<string> desc_args;       // 转换的描述参数
}

/**************
 * 物品表
 **/
public class GSOItemExchangeRefSet : _TALSOBasicRefSet<ItemExchangeRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "item_exchange"; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

[System.Serializable]
public class ItemAlterRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一识别标记

    public ENPInsteadItemType type;//替换功能枚举

    public NPEnum.ENPItemType src_item_type;//源物品类型
    public long src_item_id;//源物品ID

    public NPEnum.ENPItemType alter_item_type;//代替物品类型
    public long alter_item_id;//代替物品ID 
}

/**************
 * 物品替换表
 **/
public class GSOItemAlterRefSet : _TALSOBasicRefSet<ItemAlterRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "item_alter"; } }
}

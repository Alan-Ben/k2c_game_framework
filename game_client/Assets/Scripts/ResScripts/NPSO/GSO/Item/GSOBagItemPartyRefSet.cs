using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

//道具- 聚会保护罩道具子表
[System.Serializable]
public class BagItemPartyRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;                               //物品ID

    public int protect_sesc; //保护时长（秒）

}

/**************
 * 聚会保护罩道具子表
 **/
public class GSOBagItemPartyRefSet : _TALSOBasicRefSet<BagItemPartyRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/bag_refdata.unity3d"; } }
    public static string objName { get { return "bag_item_party"; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using GOE;

[System.Serializable]
public class NPSOEffectGotoRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;                               //物品ID

    public _NPPlayerConditionSerializeInfo deal_cond;//执行条件
    public _NPPlayerEffectSerializeInfo effect;//执行效果
}

/**************
 * 物品表
 **/
public class NPSOEffectGotoRefSet : _TALSOBasicRefSet<NPSOEffectGotoRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "effect_goto"; } }
}

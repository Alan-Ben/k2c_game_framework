using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

//版本更新奖励表
[System.Serializable]
public class NPGVersionUpRewardRefObj : _IALBasicRefObj
{
    public long _refId { get { return div_num; } }

    public long div_num; //对比版本号的除数
    public List<NPCommonCostItem> reward_list;                  //奖励ID
}

public class NPSOVersionUpRewardRefSet : _TALSOBasicRefSet<NPGVersionUpRewardRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "version_up_reward"; } }
}
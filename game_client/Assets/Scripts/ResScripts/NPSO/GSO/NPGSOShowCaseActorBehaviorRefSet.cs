using ALPackage;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;

/// <summary>
/// showcase展示模型额外表现表，对showcase加载对应模型的初始角度和位置做微调
/// </summary>
[System.Serializable]
public class NPShowCaseActorBehaviorRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long)index.GetHashCode(); } }
    public NPGModelIndex index;//资源索引
    public Vector3 local_position;//位置微调
    public Vector3 local_rotation;//角度微调
}


public class NPGSOShowCaseActorBehaviorRefSet : _TALSOBasicRefSet<NPShowCaseActorBehaviorRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "show_case_actor_behavior"; } }
}

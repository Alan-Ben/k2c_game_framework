using System;
using ALPackage;
using GOE;

/// <summary>
/// 活动合并展示推送表
/// </summary>
[Serializable]
public class PushNoticeActivityMergeRefObj : _IALBasicRefObj
{
    public long _refId { get { return activity_id; } }
    
    public long activity_id; //活动ID
    public NPGTextureIndex banner;//banner图
    public _NPPlayerEffectSerializeInfo go_to_effect;//前往效果
    public int sorting_order;//排序
}

/// <summary>
/// 
/// </summary>
public class GSOPushNoticeActivityMergeRefSet : _TALSOBasicRefSet<PushNoticeActivityMergeRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/main_city_push_notice.unity3d"; } }
    public static string objName { get { return "push_notice_activity_merge"; } }
}
    
    
using ALPackage;
using System;
using NPEnum;

/// <summary>
/// 本地推送表
/// </summary>
[Serializable]
public class LocalPushRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long)type; } }

    public ELocalPushType type;//类型（ELocalPushType）
    public string switch_show_name;//开关展示名称
    public ENPFunctionType func_type;//对应系统类型（ENPFunctionType），系统解锁才会推送
    public long simple_unlock_id;//可推送条件的simple_unlock_id
    public string title;//标题
    public string content;//内容
    public long advance_time;//提前通知时间（秒）
}

public class GSOLocalPushRefSet : _TALSOBasicRefSet<LocalPushRefObj> {

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "local_push"; } }
}

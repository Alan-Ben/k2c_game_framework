using ALPackage;
using System;
using GOE;

/// <summary>
/// 运营公告游戏内跳转表
/// </summary>
[Serializable]
public class AnnouncementJumpRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public _NPPlayerEffectSerializeInfo jump_effect_str;//跳转的效果字符串
    public _NPPlayerConditionSerializeInfo jump_condition;//跳转条件集合
}

public class GSOAnnouncementJumpRefSet : _TALSOBasicRefSet<AnnouncementJumpRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/announcement_refdata.unity3d"; } }
    public static string objName { get { return "announcement_jump"; } }
}

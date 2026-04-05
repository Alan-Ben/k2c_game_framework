using ALPackage;
using NPEnum;
using System;

/// <summary>
/// FixedCD表
/// </summary>
[Serializable]
public class NPFixedCDRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public NPTimeRefreshInfo refresh_rule;//刷新规则
    public int count_max; //计数最大值
    public ENPPlayerPropertyType property_add_count_max;//玩家数据增加的计数最大值
    public bool is_can_excceed_limit;//是否能超出上限
    public int add_count_per_time; //每次固定恢复点数
    public ENPPlayerPropertyType property_add_count_per_time;//玩家属性中每次恢复的点数加值.
}

public class NPGSOFixedCDRefSet : _TALSOBasicRefSet<NPFixedCDRefObj>
{

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "player_fixed_cd"; } }
}

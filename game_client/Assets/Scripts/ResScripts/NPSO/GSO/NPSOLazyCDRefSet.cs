using ALPackage;
using System;
using System.Collections.Generic;
using System.Text;
using NPEnum;
using UnityEngine;

/// <summary>
/// lazyCD表
/// </summary>
[Serializable]
public class NPLazyCDRefObj : _IALBasicRefObj
{
    public long _refId { get { return cd_id; } }

    public long cd_id;//唯一识别标记
    public int durantion;//CD时长
    public int count_max; //计数最大值
    public ENPPlayerPropertyType property_add_count_max;//玩家数据增加的计数最大值
    public bool is_can_excceed_limit = false;//是否能超出上限
    public int add_count_per_time = 1; //每次固定恢复点数
    public ENPPlayerPropertyType property_add_count_per_time = ENPPlayerPropertyType.NONE;//玩家属性中每次恢复的点数加值.
    public ENPPlayerPropertyType property_add_durantion = ENPPlayerPropertyType.NONE;//玩家属性中对CD时长的加值.
    public int consume_count;//单次消耗次数

    public string refresh_count_key;//刷新次数显示key
    public string refresh_time_key;//刷新倒计时显示key

    public long relative_activity_id;// 关联的活动ID
}

public class NPSOLazyCDRefSet : _TALSOBasicRefSet<NPLazyCDRefObj> {

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "player_lazy_cd"; } }
}

using ALPackage;
using System;
using GOE;

/// <summary>
/// 跑马灯表
/// </summary>
[Serializable]
public class MarqueeRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//唯一识别标记
    public int show_pos_id;//窗口展示位置
    public int priority_id; //优先级（越大越优先）
    public int duration_sec;//循环播放时长秒（优先于次数）
    public int duration_count;//循环播放次数
    public bool can_del;//是否可删除
    public long life_sec;//生存时间秒
    public string content;//内容
    public long ui_res_id;//预制体ID
    public _NPPlayerConditionSerializeInfo show_condition;//显示条件
}

public class GSOMarqueeRefSet : _TALSOBasicRefSet<MarqueeRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/marquee_refdata.unity3d"; } }
    public static string objName { get { return "marquee"; } }
}

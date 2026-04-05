using ALPackage;
using GOE;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;


/**************
 * 游历地点表
 **/

[System.Serializable]
public class TravelPosRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//id
    public NPGGoIndex pos_res_go_index;//地点场景资源
    public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
    public string unlock_condition_desc;//解锁条件描述
    public List<string> unlock_condition_desc_args;//解锁条件描述参数
    public long unlock_sys_info_id;//解锁的sys_info_id(用于触发地点解锁tip)
    public string name;//地点名称
    public string desc;//地点描述
    public NPGTextureIndex icon;//地点icon
    // public int sort_id;//排序id，小的在前
    public NPGGoIndex dialogue_bg_index;//对话背景
    public NPGTextureIndex banner_img;//banner图片
    public string travel_process_ani_name;//游历过程动画名
    
    public GVideoClipIndex start_travel_video_index;//开始游历视频VideoIndex
    public GVideoClipIndex landing_video_index;//降落视频VideoIndex
    
    public List<long> travel_event_list;//出现的事件列表

    public bool isUnlock(NPVarInfo _varVariableInfo)
    {
        return unlock_condition?.isNoConditionOrEnable(_varVariableInfo) ?? true;
    }
}

public class GSOTravelPosRefSet : _TALSOBasicRefSet<TravelPosRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
    public static string objName { get { return "travel_pos"; } }
}

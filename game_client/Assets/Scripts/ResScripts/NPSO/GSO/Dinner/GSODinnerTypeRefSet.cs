using ALPackage;
using System.Collections.Generic;
using ClientEnum;
using Common.DinnerEnum;
using GOE;
using UnityEngine;


/**************
 * 宴会类型表
 **/

[System.Serializable]
public class GDinnerTypeRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long)dinner_id; } }
    public long dinner_id;//宴会id
    public List<NPCommonCostItem> open_cost;//开宴会消耗道具
    public bool is_permit_open;//是否凭证开启
    public int default_seat_num;//默认席位数量
    public long duration_sec;//持续时长，秒
    public string name;//宴会名
	public string desc;//宴会描述
    public NPGTextureIndex banner;//banner图
    // public EDinnerType dinner_type;//宴会类型(EDinnerType)
    public EDinnerShowType dinner_show_type;//宴会展示类型(EDinnerShowType)
    public long scene_id;//场景id
    public List<DinnerVideoInfo> video_ids;//随机视频id列表
    public List<string> common_dialogue_keys;//常规对话ID列表
    public List<string> welcome_dialogue_keys;//欢迎对话ID列表
    
    public long ui_path_id; //创建item资源路径id
    public string name_spe;//宴会名特殊样式
    public Color txt_color;// 标题文本颜色
    public Color bg_color;// 背景颜色

}

public class GSODinnerTypeRefSet : _TALSOBasicRefSet<GDinnerTypeRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/dinner.unity3d"; } }
    public static string objName { get { return "dinner_type"; } }
}

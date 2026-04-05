using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;

[System.Serializable]
public class NPSOCommonBoxRefObj : _IALBasicRefObj, _IShareItemTypeRefObj
{
    public ENPShareItemType shareItemType { get { return ENPShareItemType.BOX; } }

    public long _refId { get { return id; } }

    public long id;//唯一id
    public string name;//宝箱名称
    public NPGTextureIndex icon;//宝箱图标
    public string sender_name; // 发送者名字
    public long chat_my_sub_ui_res_id;//自己聊天分享item附加资源id
    public long chat_other_sub_ui_res_id;//他人聊天分享item附加资源id
    public long sfx_id;//特效id
    public bool content_has_sender_name;// 描述是否带上玩家名字
    public string mini_chat_desc;//小聊天框描述
    public List<string> mini_chat_desc_args;//小聊天框描述参数
    public long expire_secs;//过期秒数
    public long box_limit;//宝箱可被领取上限
    public bool is_sender_gain;//sender是否可以领取
    public float ui_height; // UI高度
    public List<NPCommonCostItem> item_list;//宝箱奖励
    public List<NPCommonCostItem> cost_item_list;//消耗物品列表
}

/**************
 * 通用宝箱表
 **/
public class NPSOCommonBoxRefSet : _TALSOBasicRefSet<NPSOCommonBoxRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "box_comm"; } }
}

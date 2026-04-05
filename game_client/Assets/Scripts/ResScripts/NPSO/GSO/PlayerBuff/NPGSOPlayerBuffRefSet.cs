using ALPackage;
using System.Collections.Generic;
using GOE;

[System.Serializable]
public class NPPlayerBuffRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id; //唯一 id
    public string name; //名称
    public string desc; //描述
    public List<string> desc_args; //描述参数
    public NPGTextureIndex icon; //图标
    public int max_layer; //最大层数
    public bool layer_empty_remove; //层数为0是否移除
    public bool can_add_layer; //是否可以增加层数
    public _UnionBonusSerializeInfo bonus_add; //buff的bonus加成
    public NPPlayerPropertyModifier player_pro_add; //buff的玩家属性
    public MarsPropertyModifier mars_pro_add;//火星属性加成
    public long trigger_tip_ui_res_id; //触发提示ui资源id
}

public class NPGSOPlayerBuffRefSet : _TALSOBasicRefSet<NPPlayerBuffRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "player_buff"; } }
}

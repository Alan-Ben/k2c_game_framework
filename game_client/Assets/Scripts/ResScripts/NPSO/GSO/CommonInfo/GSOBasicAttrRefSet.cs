using ALPackage;
using System;
using CommonEnum;

/// <summary>
/// 通用属性表
/// </summary>
[Serializable]
public class BasicAttrRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long)type; } }

    public ESpecAttrType type;//属性类型
    public NPGTextureIndex icon;//图标
    public NPGSpriteIndex spt_icon;//图标
    public NPGTextureIndex equip_skill_bg;//藏品技能底图
    public string name;//名字
    public string desc;//描述
    public NPCommonItem graduate_item;//毕业奖励
    public string guild_cooperate_attr_pos_name;//公会协作属性据点名称
}

public class GSOBasicAttrRefSet : _TALSOBasicRefSet<BasicAttrRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/refdata.unity3d"; } }
    public static string objName { get { return "basic_attr"; } }
}

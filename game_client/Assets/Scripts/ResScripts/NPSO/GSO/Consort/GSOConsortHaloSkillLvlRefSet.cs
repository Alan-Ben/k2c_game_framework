using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子星辉技能等级
/// </summary>
[System.Serializable]
public class ConsortHaloSkillLvlRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public long halo_skill_id;//星辉技能id
    public int halo_skill_lvl;//星辉技能等级
    public string add_value_desc;//加成值描述
    public PlayerAttrPropertyModifier add_basic_attr;//加护伙伴加成（EBasicAttrType）
    public _UnionBonusSerializeInfo add_bonus_attr;//全局属性加成（EBonusPropertyType）
}

public class GSOConsortHaloSkillLvlRefSet : _TALSOBasicRefSet<ConsortHaloSkillLvlRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_halo_skill_lvl"; } }
}
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子羁绊技能等级
/// </summary>
[System.Serializable]
public class ConsortFettersSkillLvlRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    
    public long id;//唯一id
    public long fetters_skill_id;//羁绊技能id
    public int lvl;//技能等级
    public List<string> desc_args_list;//描述参数列表
    public NPPlayerPropertyModifier add_player;//玩家属性加成
    public _UnionBonusSerializeInfo add_bonus;//加成
}

public class GSOConsortFettersSkillLvlRefSet : _TALSOBasicRefSet<ConsortFettersSkillLvlRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_fetters_skill_lvl"; } }
}
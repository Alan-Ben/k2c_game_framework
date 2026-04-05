using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 加护技能等级
/// </summary>
[System.Serializable]
public class ConsortBlessSkillLvlRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public long bless_skill_id;//加护技能id
    public int lvl;//等级
    public int cost_skill_point;//到下一级消耗加护点
    public PlayerAttrPropertyModifier add;//加成的属性（枚举:属性）
    public List<string> desc_args_list;//描述参数列表
}

public class GSOConsortBlessSkillLvlRefSet : _TALSOBasicRefSet<ConsortBlessSkillLvlRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_bless_skill_lvl"; } }
}
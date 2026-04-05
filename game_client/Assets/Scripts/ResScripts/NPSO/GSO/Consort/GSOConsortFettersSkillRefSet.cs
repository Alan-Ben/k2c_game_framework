using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子羁绊技能
/// </summary>
[System.Serializable]
public class ConsortFettersSkillRefObj : _IALBasicRefObj
{
    public long _refId { get { return fetters_skill_id; } }

    public long fetters_skill_id;//唯一id
    public string name;//技能名
    public string desc;//描述
    public NPGTextureIndex icon;//技能图标
    public _NPPlayerConditionSerializeInfo enable_condition;//有效条件

    #region 羁绊技能等级列表

    [NonSerialized]
    private List<ConsortFettersSkillLvlRefObj> _m_lSkillLvlRefList;

    public List<ConsortFettersSkillLvlRefObj> skillLvlRefList
    {
        get
        {
            if (_m_lSkillLvlRefList == null)
            {
                _m_lSkillLvlRefList = new List<ConsortFettersSkillLvlRefObj>();
#if NP_GAME
                GRefdataCoreMgr.instance.consortFettersSkillLvlRefCore.dealAllRef((_refObj) =>
                {
                    if(_refObj != null && _refObj.fetters_skill_id == fetters_skill_id)
                        _m_lSkillLvlRefList.Add(_refObj);
                });
                
                _m_lSkillLvlRefList.Sort((_a, _b) =>
                {
                    if (_b == null)
                        return -1;
                    if (_a == null)
                        return 1;

                    return _a.lvl.CompareTo(_b.lvl);
                });
#endif
            }
            
            return _m_lSkillLvlRefList;
        }
    }
    
    /// <summary>
    /// 获取对应等级的技能配置
    /// </summary>
    /// <param name="_lvl"></param>
    /// <returns></returns>
    public ConsortFettersSkillLvlRefObj getSkillLvlRefByLvl(int _lvl)
    {
        if (skillLvlRefList == null)
            return null;
        
        foreach (var _refObj in skillLvlRefList)
        {
            if (_refObj != null && _refObj.lvl == _lvl)
                return _refObj;
        }

        return null;
    }

    #endregion
}

/// <summary>
/// 妃子羁绊技能
/// </summary>
public class GSOConsortFettersSkillRefSet : _TALSOBasicRefSet<ConsortFettersSkillRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_fetters_skill"; } }
}
using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子加护技能
/// </summary>
[System.Serializable]
public class ConsortBlessSkillRefObj : _IALBasicRefObj
{
    public long _refId { get { return bless_skill_id; } }
    
    public long bless_skill_id;//加护技能id
    public string name;//技能名
    public string desc;//描述

    #region 加护技能等级列表

    [NonSerialized]
    private List<ConsortBlessSkillLvlRefObj> _m_lSkillLvlRefList;

    public List<ConsortBlessSkillLvlRefObj> skillLvlRefList
    {
        get
        {
            if(_m_lSkillLvlRefList == null)
            {
                _m_lSkillLvlRefList = new List<ConsortBlessSkillLvlRefObj>();
#if NP_GAME
                GRefdataCoreMgr.instance.consortBlessSkillLvlRefCore.dealAllRef((_refObj) =>
                {
                    if(_refObj != null && _refObj.bless_skill_id == bless_skill_id)
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
    /// 获取加护技能等级配置
    /// </summary>
    /// <param name="_lvl"></param>
    /// <returns></returns>
    public ConsortBlessSkillLvlRefObj getSkillLvlRef(int _lvl)
    {
        if (skillLvlRefList == null)
            return null;
        
        return skillLvlRefList.Find((_refObj) => _refObj.lvl == _lvl);
    }

    #endregion
}

public class GSOConsortBlessSkillRefSet : _TALSOBasicRefSet<ConsortBlessSkillRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_bless_skill"; } }
}
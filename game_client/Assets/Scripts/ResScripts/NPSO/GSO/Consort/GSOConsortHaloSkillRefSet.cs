using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子星辉技能表
/// </summary>
[System.Serializable]
public class ConsortHaloSkillRefObj : _IALBasicRefObj
{
    public long _refId { get { return halo_skill_id; } }

    public long halo_skill_id;//唯一id
    public string name;//星辉id
    public string add_type_desc;//加成类型描述
    public NPGTextureIndex icon;//技能图标
    
    [NonSerialized]
    private List<ConsortHaloSkillLvlRefObj> _m_lSkillLvlRefList;//技能等级列表
    public List<ConsortHaloSkillLvlRefObj> skillLvlRefList
    {
        get
        {
            if (_m_lSkillLvlRefList == null)
            {
                _m_lSkillLvlRefList = new List<ConsortHaloSkillLvlRefObj>();
#if NP_GAME
                GRefdataCoreMgr.instance.consortHaloSkillLvlRefCore.dealAllRef((_refObj) =>
                {
                    if(_refObj != null && _refObj.halo_skill_id == halo_skill_id)
                        _m_lSkillLvlRefList.Add(_refObj);
                });
                
                _m_lSkillLvlRefList.Sort((_a, _b) =>
                {
                    if (_b == null)
                        return -1;
                    if (_a == null)
                        return 1;

                    return _a.halo_skill_lvl.CompareTo(_b.halo_skill_lvl);
                });
#endif
            }

            return _m_lSkillLvlRefList;
        }
    }
    
    /// <summary>
    /// 获取星辉技能等级配表数据
    /// </summary>
    /// <param name="_lvl"></param>
    /// <returns></returns>
    public ConsortHaloSkillLvlRefObj getHaloSkillLvlRefObj(int _lvl)
    {
        if (skillLvlRefList == null)
            return null;

        return skillLvlRefList.Find((_refObj) =>
        {
            return _refObj != null && _refObj.halo_skill_lvl == _lvl;
        });
    }
}

public class GSOConsortHaloSkillRefSet : _TALSOBasicRefSet<ConsortHaloSkillRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_halo_skill"; } }
}
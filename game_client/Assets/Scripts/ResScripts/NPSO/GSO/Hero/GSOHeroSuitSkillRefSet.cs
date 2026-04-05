using ALPackage;
using System;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 伙伴套系技能表
/// </summary>
[Serializable]
public class HeroSuitSkillRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//技能id
    public NPGTextureIndex icon;//图标
    public string name;//技能名称
    public string simple_desc;//星辉效果技能描述
    public string desc;//技能描述

    //由于等级相关计算是分阶段的，这里记录等级计算每个阶段的基础配置列表
    [NonSerialized]
    private List<HeroSuitSkillLevelRefObj> _m_lBaseSkillLevelRef;
    public void addBaseSkillLevelRef(HeroSuitSkillLevelRefObj _skillLevelRef)
    {
        if (_m_lBaseSkillLevelRef == null)
            _m_lBaseSkillLevelRef = new List<HeroSuitSkillLevelRefObj>();

        _m_lBaseSkillLevelRef.Add(_skillLevelRef);
    }

#if NP_GAME
    /// <summary>
    /// 根据等级获取技能分段基础等级数据
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public HeroSuitSkillLevelRefObj getBaseSuitSkillLevelRefByLevel(long _level)
    {
        if (_m_lBaseSkillLevelRef == null)
            return null;

        HeroSuitSkillLevelRefObj baseSkillLevelRef = null;
        for (int i = 0; i < _m_lBaseSkillLevelRef.Count; i++)
        {
            HeroSuitSkillLevelRefObj tempRef = _m_lBaseSkillLevelRef[i];
            if (tempRef == null)
                continue;

            //如果等级大于当前等级了，则直接返回上一个数据
            if (tempRef.level <= _level)
                baseSkillLevelRef = tempRef;
            else 
                return baseSkillLevelRef;
        }

        return baseSkillLevelRef;
    }
#endif
}

public class GSOHeroSuitSkillRefSet : _TALSOBasicRefSet<HeroSuitSkillRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_halo_suit_skill"; } }
}

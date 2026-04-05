using ALPackage;
using System;
using System.Collections.Generic;
using GOE;
using NPEnum;

/// <summary>
/// 伙伴资质技能表
/// </summary>
[Serializable]
public class HeroTalentSkillRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//阶段
    public long level_limit;//等级上限
    public NPGTextureIndex icon;//图标
    public string name;//资质技能名字
    public string unlock_desc;//解锁描述
    public List<string> unlock_desc_args;//解锁描述参数

    //由于等级相关计算是分阶段的，这里记录等级计算每个阶段的基础配置列表
    [NonSerialized]
    private List<HeroTalentSkillLevelRefObj> _m_lBaseSkillLevelRef;
    public void addBaseSkillLevelRef(HeroTalentSkillLevelRefObj _skillLevelRef)
    {
        if (_m_lBaseSkillLevelRef == null)
            _m_lBaseSkillLevelRef = new List<HeroTalentSkillLevelRefObj>();

        _m_lBaseSkillLevelRef.Add(_skillLevelRef);
    }

#if NP_GAME

    /// <summary>
    /// 根据等级获取技能分段基础等级数据
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public HeroTalentSkillLevelRefObj getBaseTalentSkillLevelRefByLevel(long _level)
    {
        if (_m_lBaseSkillLevelRef == null)
            return null;

        HeroTalentSkillLevelRefObj baseSkillLevelRef = null;
        for (int i = 0; i < _m_lBaseSkillLevelRef.Count; i++)
        {
            HeroTalentSkillLevelRefObj tempRef = _m_lBaseSkillLevelRef[i];
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

    /// <summary>
    /// 获取升级所需道具
    /// </summary>
    /// <param name="_level"></param>
    /// <param name="_isTen"></param>
    /// <returns></returns>
    public NPCommonCostItem getUpgradeCostItem(long _level, bool _isTen)
    {
        if (_m_lBaseSkillLevelRef == null)
            return null;

        //当前等级的分段等级配置数据
        HeroTalentSkillLevelRefObj baseLevelRef = getBaseTalentSkillLevelRefByLevel(_level);
        //目标道具列表
        List<NPCommonCostItem> costItemList = new List<NPCommonCostItem>();

        if (baseLevelRef == null)
            return null;

        if (_isTen)
        {
            //十连升级
            //目标等级
            long targetLevel = _level + 10;
            if (targetLevel > level_limit)
                targetLevel = level_limit;

            //上个分段等级配置
            HeroTalentSkillLevelRefObj lastSkillLevelRef = baseLevelRef;
            long lastLevel = _level;
            for (int i = 0; i < _m_lBaseSkillLevelRef.Count; i++)
            {
                HeroTalentSkillLevelRefObj tempRef = _m_lBaseSkillLevelRef[i];
                if (tempRef == null)
                    continue;

                //有分段，并且十连升级跨越了分段
                if (lastSkillLevelRef.level < tempRef.level && targetLevel >= tempRef.level)
                {
                    long diffValue = tempRef.level - lastLevel;
                    //添加基础
                    NPCommonCostItem baseCostItem = new NPCommonCostItem(lastSkillLevelRef.cost); 
                    baseCostItem.setCount(baseCostItem.count * diffValue);
                    costItemList.Add(baseCostItem);
                    //添加每级加成
                    NPCommonCostItem levelAddCostItem = new NPCommonCostItem(lastSkillLevelRef.cost_per_level);
                    if (levelAddCostItem.getItemType() != ENPItemType.NONE)
                    {
                        long addCostCount = ((diffValue - 1) * diffValue) / 2;//1到diffValue-1连续相加
                        levelAddCostItem.setCount(levelAddCostItem.count * addCostCount);
                        costItemList.Add(levelAddCostItem);
                    }

                    //记录当前分段
                    lastSkillLevelRef = tempRef;
                    lastLevel = lastSkillLevelRef.level;
                }
            }

            //用最后分段计算需要消耗的道具
            if (lastSkillLevelRef != null)
            {
                long diffValue = targetLevel - lastLevel;

                //添加基础
                NPCommonCostItem baseCostItem = new NPCommonCostItem(lastSkillLevelRef.cost);
                baseCostItem.setCount(baseCostItem.count * diffValue);
                costItemList.Add(baseCostItem);
                //添加每级加成
                NPCommonCostItem levelAddCostItem = new NPCommonCostItem(lastSkillLevelRef.cost_per_level);
                if (levelAddCostItem.getItemType() != ENPItemType.NONE)
                {
                    long addCostCount = ((diffValue - 1) * diffValue) / 2;//1到diffValue-1连续相加
                    levelAddCostItem.setCount(levelAddCostItem.count * addCostCount);
                    costItemList.Add(levelAddCostItem);
                }
            }
        }
        else
        { 
            //单次升级
            //添加基础消耗
            costItemList.Add(baseLevelRef.cost);
            //添加按照等级递增的消耗
            NPCommonCostItem levelAddCostItem = new NPCommonCostItem(baseLevelRef.cost_per_level);
            if (levelAddCostItem.getItemType() != ENPItemType.NONE)
            {
                levelAddCostItem.setCount(levelAddCostItem.count * (_level - baseLevelRef.level));
                if (levelAddCostItem.count > 0)
                    costItemList.Add(levelAddCostItem);
            }
        }

        //合并同类道具
        if(costItemList.Count > 1)
            costItemList = GCommon.getCombineItemList(costItemList);

        if (costItemList.Count > 0)
            return costItemList[0];
        else
            return null;
    }

#endif

}

public class GSOHeroTalentSkillRefSet : _TALSOBasicRefSet<HeroTalentSkillRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_talent_skill"; } }
}

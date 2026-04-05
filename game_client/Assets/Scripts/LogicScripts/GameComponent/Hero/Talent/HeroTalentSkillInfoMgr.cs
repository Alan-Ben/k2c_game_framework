using System.Collections.Generic;
using Common.HeroObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 伙伴资质技能数据管理器
    /// </summary>
    public class HeroTalentSkillInfoMgr
    {
        //伙伴已获得资质技能列表
        [NotNull] private List<HeroTalentSkillInfo> _m_lTalentSkillInfoList = new List<HeroTalentSkillInfo>();

        /// <summary>
        /// 伙伴资质技能列表
        /// </summary>
        public List<HeroTalentSkillInfo> talentSkillInfoList { get { return _m_lTalentSkillInfoList; } }

        public HeroTalentSkillInfoMgr()
        {
        }

        /// <summary>
        /// 初始化资质技能列表
        /// </summary>
        /// <param name="_talentInfoList"></param>
        public void initTalentSKill(HeroInfo _heroInfo, List<Hero_TalentSkillInfo> _talentInfoList)
        {
            if (_talentInfoList == null || _heroInfo == null)
                return;

            _m_lTalentSkillInfoList.Clear();

            for (int i = 0; i < _talentInfoList.Count; i++)
            {
                HeroTalentSkillInfo skillInfo = new HeroTalentSkillInfo(_heroInfo.id, _talentInfoList[i]);
                _m_lTalentSkillInfoList.Add(skillInfo);

                //累加伙伴自身加成
                _heroInfo.addSelfAttrModifier(skillInfo.baseTalentSkillLevelRefObj?.self_attr_prop_modifier);
                //如果有等级加成则添加
                long levelBonusStack = skillInfo.levelBonusStack;
                if (levelBonusStack > 0)
                    _heroInfo.addSelfAttrModifier(skillInfo.baseTalentSkillLevelRefObj?.self_attr_prop_modifier_per_level, levelBonusStack);

                //如果是皮肤带来的资质技能，需要添加到皮肤的属性管理器
                if (checkIsSkinTalentSkill(_talentInfoList[i].getTealentSkillId(), _heroInfo.heroRefObj))
                {
                    //累加玩家总加成
                    NPPlayer.instance.heroComponent.heroSkinUnionBonusMgr.addBonus(skillInfo.baseTalentSkillLevelRefObj?.union_bonus?.unionBonus);
                    if (levelBonusStack > 0)
                        NPPlayer.instance.heroComponent.heroSkinUnionBonusMgr.addBonus(skillInfo.baseTalentSkillLevelRefObj?.union_bonus_per_level?.unionBonus, levelBonusStack);
                }
                else
                {
                    //累加玩家总加成
                    NPPlayer.instance.heroComponent.commonUnionBonusMgr.addBonus(skillInfo.baseTalentSkillLevelRefObj?.union_bonus?.unionBonus);
                    if (levelBonusStack > 0)
                        NPPlayer.instance.heroComponent.commonUnionBonusMgr.addBonus(skillInfo.baseTalentSkillLevelRefObj?.union_bonus_per_level?.unionBonus, levelBonusStack);
                }
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_talentInfo"></param>
        public void updateTalentSkillInfo(HeroInfo _heroInfo, Hero_TalentSkillInfo _talentInfo)
        {
            if (null == _talentInfo || _heroInfo == null)
                return;

            HeroTalentSkillInfo talentSkillInfo = getTalentSkillInfo(_talentInfo.getTealentSkillId());
            if (talentSkillInfo != null)
            {
                //先记录原本的数据
                HeroTalentSkillLevelRefObj oldLevelRef = talentSkillInfo.baseTalentSkillLevelRefObj;
                long oldLevelBonusStack = talentSkillInfo.levelBonusStack;

                //更新数据
                talentSkillInfo.updateInfo(_heroInfo.id, _talentInfo);

                //获取新数据
                HeroTalentSkillLevelRefObj newLevelRef = talentSkillInfo.baseTalentSkillLevelRefObj;
                long newLevelBonusStack = talentSkillInfo.levelBonusStack;

                //更新属性加成
                //更新伙伴自身加成
                _heroInfo.replaceSelfAttrModifier(oldLevelRef?.self_attr_prop_modifier, newLevelRef?.self_attr_prop_modifier);
                _heroInfo.replaceSelfAttrModifier(oldLevelRef?.self_attr_prop_modifier_per_level, oldLevelBonusStack, newLevelRef?.self_attr_prop_modifier_per_level, newLevelBonusStack);

                //如果是皮肤带来的资质技能，需要添加到皮肤的属性管理器
                if (checkIsSkinTalentSkill(_talentInfo.getTealentSkillId(), _heroInfo.heroRefObj))
                {  
                    //累加玩家总加成
                    NPPlayer.instance.heroComponent.heroSkinUnionBonusMgr.replaceBonus(oldLevelRef?.union_bonus?.unionBonus, newLevelRef?.union_bonus?.unionBonus);
                    NPPlayer.instance.heroComponent.heroSkinUnionBonusMgr.replaceBonus(oldLevelRef?.union_bonus_per_level?.unionBonus, oldLevelBonusStack, newLevelRef?.union_bonus_per_level?.unionBonus, newLevelBonusStack);
                }
                else
                {
                    //累加玩家总加成
                    NPPlayer.instance.heroComponent.commonUnionBonusMgr.replaceBonus(oldLevelRef?.union_bonus?.unionBonus, newLevelRef?.union_bonus?.unionBonus);
                    NPPlayer.instance.heroComponent.commonUnionBonusMgr.replaceBonus(oldLevelRef?.union_bonus_per_level?.unionBonus, oldLevelBonusStack, newLevelRef?.union_bonus_per_level?.unionBonus, newLevelBonusStack);
                }
            }
            else
            {
                HeroTalentSkillInfo skillInfo = new HeroTalentSkillInfo(_heroInfo.id, _talentInfo);
                _m_lTalentSkillInfoList.Add(skillInfo);

                //累加伙伴自身加成
                _heroInfo.addSelfAttrModifier(skillInfo.baseTalentSkillLevelRefObj?.self_attr_prop_modifier);
                //如果有等级加成则添加
                long levelBonusStack = skillInfo.levelBonusStack;
                if (levelBonusStack > 0)
                    _heroInfo.addSelfAttrModifier(skillInfo.baseTalentSkillLevelRefObj?.self_attr_prop_modifier_per_level, levelBonusStack);

                //如果是皮肤带来的资质技能，需要添加到皮肤的属性管理器
                if (checkIsSkinTalentSkill(_talentInfo.getTealentSkillId(), _heroInfo.heroRefObj))
                {
                    //累加玩家总加成
                    NPPlayer.instance.heroComponent.heroSkinUnionBonusMgr.addBonus(skillInfo.baseTalentSkillLevelRefObj?.union_bonus?.unionBonus);
                    if (levelBonusStack > 0)
                        NPPlayer.instance.heroComponent.heroSkinUnionBonusMgr.addBonus(skillInfo.baseTalentSkillLevelRefObj?.union_bonus_per_level?.unionBonus, levelBonusStack);
                }
                else
                {
                    //累加玩家总加成
                    NPPlayer.instance.heroComponent.commonUnionBonusMgr.addBonus(skillInfo.baseTalentSkillLevelRefObj?.union_bonus?.unionBonus);
                    if (levelBonusStack > 0)
                        NPPlayer.instance.heroComponent.commonUnionBonusMgr.addBonus(skillInfo.baseTalentSkillLevelRefObj?.union_bonus_per_level?.unionBonus, levelBonusStack);
                }
            }
        }

        /// <summary>
        /// 获取伙伴资质技能数据
        /// </summary>
        /// <param name="_talentId"></param>
        /// <returns></returns>
        public HeroTalentSkillInfo getTalentSkillInfo(long _talentId)
        {
            for (int i = 0; i < _m_lTalentSkillInfoList.Count; i++)
            {
                if (_m_lTalentSkillInfoList[i] != null && _m_lTalentSkillInfoList[i].talentSkillId == _talentId)
                {
                    return _m_lTalentSkillInfoList[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 获取资质技能是否解锁
        /// </summary>
        /// <param name="_talentSkillId"></param>
        /// <returns></returns>
        public bool isTalentSkillUnlock(long _talentSkillId)
        {
            for (int i = 0; i < _m_lTalentSkillInfoList.Count; i++)
            {
                if (_m_lTalentSkillInfoList[i].talentSkillId == _talentSkillId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取资质升级消耗
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_isTen"></param>
        /// <returns></returns>
        public NPCommonCostItem getTalentSkillLevelUpCostItem(long _id, bool _isTen)
        {
            HeroTalentSkillInfo talentSkillInfo = getTalentSkillInfo(_id);
            if (talentSkillInfo == null || talentSkillInfo.talentSkillRefObj == null)
                return null;

            return talentSkillInfo.talentSkillRefObj.getUpgradeCostItem(talentSkillInfo.level, _isTen);
        }

        /// <summary>
        /// 检查是否是皮肤带来的资质技能
        /// </summary>
        /// <returns></returns>
        public bool checkIsSkinTalentSkill(long _talentSkillId, HeroRefObj _heroRef)
        {
            if (_talentSkillId <= 0 || _heroRef == null || _heroRef.heroSkinRefList == null)
                return false;

            for (int i = 0; i < _heroRef.heroSkinRefList.Count; i++)
            {
                HeroSkinRefObj heroSkinRef = _heroRef.heroSkinRefList[i];
                if(heroSkinRef == null || heroSkinRef.unlock_gain_talent_skill_id_list == null || heroSkinRef.unlock_gain_talent_skill_id_list.Count == 0)
                    continue;

                if (heroSkinRef.unlock_gain_talent_skill_id_list.Contains(_talentSkillId))
                    return true;
            }

            return false;
        }
    }
}
using System.Collections.Generic;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    //伙伴partial类
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 初始化伙伴相关配置
        /// </summary>
        private void _initHero()
        {
            heroRefCore.dealAllRef(_heroRef =>
            {
                //初始化皮肤列表
                heroSkinRefCore.dealAllRef(_skinRef =>
                {
                    if (_skinRef != null && _skinRef.hero_id == _heroRef.id)
                        _heroRef.addHeroSKinRef(_skinRef);
                });

                //初始化关联家人列表
                consortRefCore.dealAllRef(_consortRef =>
                {
                    if(_consortRef != null && _consortRef.relation_hero_id_list != null && _consortRef.relation_hero_id_list.Contains(_heroRef.id))
                        _heroRef.addRelationConsortId(_consortRef.id);
                });

                //初始化套系伙伴列表
                if (_heroRef.suit_id > 0)
                {
                    HeroSuitRefObj suitRef = heroHaloSuitRefCore.getRef(_heroRef.suit_id);
                    suitRef?.addHeroRef(_heroRef);
                }
            });

            //初始化觉醒技能最大等级
            heroStarSkillRefCore.dealAllRef(_starSkillRef =>
            {
                heroStarSkillLevelRefCore.dealAllRef(_starSkillLevelRef =>
                {
                    if (_starSkillRef.skill_id == _starSkillLevelRef.skill_id &&
                        _starSkillRef.maxLevel < _starSkillLevelRef.skill_level)
                        _starSkillRef.maxLevel = _starSkillLevelRef.skill_level;
                });
            });

            //初始化套系技能分段等级配置数据
            heroHaloSuitSkillRefCore.dealAllRef(_suitSkillRef =>
            {
                heroHaloSuitSkillLevelRefCore.dealAllRef(_suitSkillLevelRef =>
                {
                    if(_suitSkillLevelRef.halo_suit_skill_id == _suitSkillRef.id)
                        _suitSkillRef.addBaseSkillLevelRef(_suitSkillLevelRef);
                });
            });

            //初始化资质技能分段等级配置数据
            heroTalentSkillRefCore.dealAllRef(_talentSkillRef =>
            {
                heroTalentSkillLevelRefCore.dealAllRef(_talentSkillLevelRef =>
                {
                    if(_talentSkillLevelRef.talent_skill_id == _talentSkillRef.id)
                        _talentSkillRef.addBaseSkillLevelRef(_talentSkillLevelRef);
                });
            });


            HeroRefObj heroRefObj = null;
            // 初始化配音列表
            heroVoiceGroupRefCore.dealAllRef((refObj) =>
            {
                if(refObj == null)
                    return;
                heroRefObj = heroRefCore.getRef(refObj.hero_id);
                if (null == heroRefObj)
                    return;
                heroRefObj.addHeroVoiceGroupRefObj(refObj);
            });
        }

        /// <summary>
        /// 获取伙伴皮肤等级表数据
        /// </summary>
        /// <param name="_skinId"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        public HeroSkinLevelRefObj getHeroSkinLevelRef(long _skinId, long _level)
        {
            for (int i = 0; i < heroSkinLevelRefCore.refList.Count; i++)
            {
                if (heroSkinLevelRefCore.refList[i] != null && heroSkinLevelRefCore.refList[i].skin_id == _skinId &&
                    heroSkinLevelRefCore.refList[i].skin_level == _level)
                    return heroSkinLevelRefCore.refList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取伙伴光环等级表数据
        /// </summary>
        /// <param name="_haloId"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        public HeroHaloLevelRefObj getHeroHaloLevelRef(long _haloId, long _level)
        {
            for (int i = 0; i < heroHaloLevelRefCore.refList.Count; i++)
            {
                if (heroHaloLevelRefCore.refList[i] != null && heroHaloLevelRefCore.refList[i].halo_id == _haloId &&
                    heroHaloLevelRefCore.refList[i].level == _level)
                    return heroHaloLevelRefCore.refList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取伙伴经营技能升级消耗组
        /// </summary>
        /// <param name="_groupId"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        public HeroBusinessSkillUpgradeRefObj getHeroBusinessSkillUpgradeRef(long _groupId, long _level)
        {
            for (int i = 0; i < heroBusinessSkillUpgradeRefCore.refList.Count; i++)
            {
                if (heroBusinessSkillUpgradeRefCore.refList[i] != null &&
                    heroBusinessSkillUpgradeRefCore.refList[i].group_id== _groupId &&
                    heroBusinessSkillUpgradeRefCore.refList[i].business_skill_lvl == _level)
                    return heroBusinessSkillUpgradeRefCore.refList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取伙伴觉醒技能等级数据
        /// </summary>
        /// <param name="_starSkillId"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        public HeroStarSkillLevelRefObj getHeroStarSkillLevelRef(long _starSkillId, long _level)
        {
            for (int i = 0; i < heroStarSkillLevelRefCore.refList.Count; i++)
            {
                if (heroStarSkillLevelRefCore.refList[i] != null &&
                    heroStarSkillLevelRefCore.refList[i].skill_id == _starSkillId &&
                    heroStarSkillLevelRefCore.refList[i].skill_level == _level)
                    return heroStarSkillLevelRefCore.refList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取伙伴套系技能等级属性加成数据
        /// </summary>
        /// <param name="_haloSuitSkillId"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        [NotNull]
        public PlayerAttrPropertyContainer getHeroSuitSkillLevelAttrContainer(long _haloSuitSkillId, long _level)
        {
            PlayerAttrPropertyContainer targetContainer = new PlayerAttrPropertyContainer();
            HeroSuitSkillRefObj suitSkillRef = heroHaloSuitSkillRefCore.getRef(_haloSuitSkillId);
            HeroSuitSkillLevelRefObj curBaseLevelRef = suitSkillRef.getBaseSuitSkillLevelRefByLevel(_level);

            if (curBaseLevelRef != null)
            {
                targetContainer.addModifier(curBaseLevelRef.attr_prop_modifier);
                long stackNum = _level - curBaseLevelRef.level;
                if (stackNum > 0)
                    targetContainer.addModifier(curBaseLevelRef.attr_prop_modifier_per_lvl, stackNum);
            }
            return targetContainer;
        }

        /// <summary>
        /// 获取星辉等级配置列表
        /// </summary>
        /// <param name="_haloId"></param>
        /// <returns></returns>
        public List<HeroHaloLevelRefObj> getHeroHaloLevelRefList(long _haloId)
        {
            List<HeroHaloLevelRefObj> refList = new List<HeroHaloLevelRefObj>();
            heroHaloLevelRefCore.dealAllRef(_haloLevelRef =>
            {
                if (_haloLevelRef != null && _haloLevelRef.halo_id == _haloId)
                {
                    refList.Add(_haloLevelRef);
                }
            });
            refList.Sort((_a,_b)=>_a.level.CompareTo(_b.level));
            return refList;
        }

        /// <summary>
        /// 根据光环id及等级获取可拥有的套系技能列表
        /// </summary>
        /// <param name="_haloId"></param>
        /// <param name="_level"></param>
        /// <returns></returns>
        public List<WCGPairInt> getOwnSuitSkillPairListByLevel(long _haloId, long _level)
        {
            List<WCGPairInt> targetPairList = new List<WCGPairInt>();
            Dictionary<int, int> recordDic = new Dictionary<int, int>(); 
            for (int i = 0; i < heroHaloLevelRefCore.refList.Count; i++)
            {
                HeroHaloLevelRefObj refObj = heroHaloLevelRefCore.refList[i];
                if (refObj != null && refObj.halo_id == _haloId && refObj.level <= _level)
                {
                    List<WCGPairInt> tempList = refObj.halo_suit_skill_level_list;
                    if (tempList == null || tempList.Count == 0)
                        continue;

                    for (int j = 0; j < tempList.Count; j++)
                    {
                        if (recordDic.TryGetValue(tempList[j].first(), out int recordLevel))
                        {
                            if (tempList[j].second() > recordLevel)
                                recordDic[tempList[j].first()] = tempList[j].second();
                        }
                        else
                            recordDic[tempList[j].first()] = tempList[j].second();
                    }
                }
            }

            foreach (KeyValuePair<int, int> keyValuePair in recordDic)
            {
                //跳过0级
                if(keyValuePair.Value != 0)
                    targetPairList.Add(new WCGPairInt(keyValuePair.Key, keyValuePair.Value));
            }

            return targetPairList;
        }

        /// <summary>
        /// 获取套系技能在伙伴身上的最高等级
        /// </summary>
        /// <param name="_haloId"></param>
        /// <param name="_suitSkillId"></param>
        /// <returns></returns>
        public long getSuitSkillMaxLevelInHero(long _haloId, long _suitSkillId)
        {
            long maxLevel = 0;
            heroHaloLevelRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.halo_id == _haloId && _ref.halo_suit_skill_level_list != null)
                {
                    for (int i = 0; i < _ref.halo_suit_skill_level_list.Count; i++)
                    {
                        if (_ref.halo_suit_skill_level_list[i].first() == _suitSkillId &&
                            maxLevel < _ref.halo_suit_skill_level_list[i].second())
                        {
                            maxLevel = _ref.halo_suit_skill_level_list[i].second();
                            break;
                        }
                    }
                }
            });

            return maxLevel;
        }

        /// <summary>
        /// 获取套系技能默认等级
        /// </summary>
        /// <param name="_haloId"></param>
        /// <param name="_suitSkillId"></param>
        /// <returns></returns>
        public long getSuitSkillDefaultLevel(long _haloId, long _suitSkillId)
        {
            long defaultLevel = 0;
            heroHaloRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.id == _haloId && _ref.halo_suit_skill_extra_level != null)
                {
                    for (int i = 0; i < _ref.halo_suit_skill_extra_level.Count; i++)
                    {
                        WCGPairLong tempPair = _ref.halo_suit_skill_extra_level[i];
                        if (tempPair != null && tempPair.first() == _suitSkillId)
                        {
                            defaultLevel = tempPair.second();
                            break;
                        }
                    }
                }
            });

            return defaultLevel;
        }


        /// <summary>
        /// 获取所有可以委派到某种类型的建筑中的伙伴列表
        /// </summary>
        [ItemNotNull, NotNull, Pure]
        public List<HeroRefObj> getAllBuildingAvailableHeroRef(ESpecAttrType _attrType)
        {
            List<HeroRefObj> result = new List<HeroRefObj>();
            List<HeroRefObj> allRefList = heroRefCore.refList;
            foreach (HeroRefObj heroRef in allRefList)
            {
                if (heroRef == null)
                    continue;
                
                if (heroRef.getCanPlaceInBuilding(_attrType))
                    result.Add(heroRef);
            }

            return result;
        }

        /// <summary>
        /// 获取伙伴星级配置数据
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_star"></param>
        /// <returns></returns>
        public HeroStarRefObj getHeroStarRef(long _heroId, long _star)
        {
            HeroRefObj heroRef = heroRefCore.getRef(_heroId);
            if (heroRef == null)
                return null;

            long starGroupId = heroRef.hero_star_group_id;
            for (int i = 0; i < heroStarRefCore.refList.Count; i++)
            {
                if (heroStarRefCore.refList[i] != null &&
                    heroStarRefCore.refList[i].hero_star_group_id == starGroupId &&
                    heroStarRefCore.refList[i].star == _star)
                {
                    return heroStarRefCore.refList[i];
                }
            }
            return null;
        }
    }
}
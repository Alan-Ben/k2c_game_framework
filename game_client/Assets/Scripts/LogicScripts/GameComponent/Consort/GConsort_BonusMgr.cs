using System;
using System.Collections.Generic;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    public partial class GConsortComponent
    {
        private class BonusMgr
        {
            private GConsortComponent _m_component;

            [NotNull] private CommonUnionBonusMgr _m_commonUnionBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.CONSORT);//妃子对全局加成BonusMgr
            [NotNull] private Dictionary<long, PlayerAttrPropertyModifier> _m_dHeroAttrPropertyModifierDic = new Dictionary<long, PlayerAttrPropertyModifier>();//大臣属性加成
            [NotNull] private NPPlayerPropertyContainer _m_PlayerPropertyContainer = new NPPlayerPropertyContainer();//玩家属性加成
            
            public BonusMgr(GConsortComponent _component)
            {
                _m_component = _component;
            }
            
            public void init()
            {
                if (_m_component != null && _m_component.consortList != null)
                {
                    foreach (var consortInfo in _m_component.consortList)
                    {
                        initConsort(consortInfo);
                    }
                }
                
                _m_commonUnionBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
                _m_commonUnionBonusMgr.onPropertyChg += _onBonusChg;
            }

            /// <summary>
            /// 初始化妃子属性加成
            /// </summary>
            /// <param name="consortInfo"></param>
            public void initConsort(GGottenConsortInfo consortInfo)
            {
                if(consortInfo == null)
                    return;
                
                //添加羁绊功能加成
                addFetterBonus(consortInfo.fetterInfo);
                
                //添加星辉功能加成
                addHaloBonus(consortInfo.consortRefObj, consortInfo.haloInfo);
                
                //添加经营技能加成
                addBusinessSkillBonus(consortInfo.businessSkillInfoList);
                
                //添加加护技能加成
                addBlessSkillBonus(consortInfo.consortRefObj, consortInfo.blessSkillInfoList);
            }

            public void discard()
            {
                _m_commonUnionBonusMgr.clear();
                _m_commonUnionBonusMgr.onPropertyChg -= _onBonusChg;

                _discardHeroAttrPropertyModifier();
                
                _m_PlayerPropertyContainer.clear();
            }

            #region 羁绊功能对属性加成

            /// <summary>
            /// 添加羁绊功能加成
            /// </summary>
            /// <param name="_fetterInfo"></param>
            public void addFetterBonus(ConsortFetterInfo _fetterInfo)
            {
                if (_fetterInfo == null)
                    return;

                if (_fetterInfo.consortFetterSkillInfo != null &&
                    _fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj != null)
                {
                    if(_fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj.add_player != null)
                        _addPlayerPropertyModifier(_fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj.add_player);
                    
                    if(_fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj.add_bonus != null)
                        addBonus(_fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj.add_bonus.unionBonus);
                }
            }
            
            /// <summary>
            /// 移除羁绊功能加成
            /// </summary>
            /// <param name="_fetterInfo"></param>
            public void removeFetterBonus(ConsortFetterInfo _fetterInfo)
            {
                if (_fetterInfo == null)
                    return;

                if (_fetterInfo.consortFetterSkillInfo != null &&
                    _fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj != null)
                {
                    if(_fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj.add_player != null)
                        _removePlayerPropertyModifier(_fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj.add_player);
                    
                    if(_fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj.add_bonus != null)
                        removeBonus(_fetterInfo.consortFetterSkillInfo.consortFettersSkillLvlRefObj.add_bonus.unionBonus);
                }
            }

            #endregion

            #region 星辉功能对属性加成

            /// <summary>
            /// 添加星辉功能加成
            /// </summary>
            public void addHaloBonus(GConsortRefObj _consortRef, ConsortHaloInfo _consortHaloInfo)
            {
                if (_consortRef == null || _consortHaloInfo == null)
                {
                    Debug.LogError("[GConsortComponent addHaloBonus] 参数错误, _consortRef == null || _consortHaloInfo == null");
                    return;
                }
                
                if (_consortHaloInfo.nowHaloLvlRefObj != null && _consortHaloInfo.nowHaloLvlRefObj.halo_skill_list != null)
                {
                    foreach (var haloSkillInfo in _consortHaloInfo.nowHaloLvlRefObj.halo_skill_list)
                    {
                        if(haloSkillInfo == null || haloSkillInfo.haloSkillLvlRefObj == null)
                            continue;

                        // 星辉技能对全局属性提升
                        if(haloSkillInfo.haloSkillLvlRefObj.add_bonus_attr != null)
                            addBonus(haloSkillInfo.haloSkillLvlRefObj.add_bonus_attr.unionBonus);

                        // 星辉技能对关联大臣属性提升
                        if (haloSkillInfo.haloSkillLvlRefObj.add_basic_attr != null)
                        {
                            addHeroAttrPropertyModifier(_consortRef.relation_hero_id_list, haloSkillInfo.haloSkillLvlRefObj.add_basic_attr);
                        }
                    }
                }
            }

            /// <summary>
            /// 移除星辉功能加成
            /// </summary>
            public void removeHaloBonus(GConsortRefObj _consortRef, ConsortHaloInfo _consortHaloInfo)
            {
                if (_consortRef == null || _consortHaloInfo == null)
                {
                    Debug.LogError("[GConsortComponent removeHaloBonus] 参数错误, _consortRef == null || _consortHaloInfo == null");
                    return;
                }
                
                if (_consortHaloInfo.nowHaloLvlRefObj != null && _consortHaloInfo.nowHaloLvlRefObj.halo_skill_list != null)
                {
                    foreach (var haloSkillInfo in _consortHaloInfo.nowHaloLvlRefObj.halo_skill_list)
                    {
                        if(haloSkillInfo == null || haloSkillInfo.haloSkillLvlRefObj == null)
                            continue;

                        // 星辉技能对全局属性提升
                        if(haloSkillInfo.haloSkillLvlRefObj.add_bonus_attr != null)
                            removeBonus(haloSkillInfo.haloSkillLvlRefObj.add_bonus_attr.unionBonus);

                        // 星辉技能对关联大臣属性提升
                        if (haloSkillInfo.haloSkillLvlRefObj.add_basic_attr != null)
                        {
                            removeHeroAttrPropertyModifier(_consortRef.relation_hero_id_list, haloSkillInfo.haloSkillLvlRefObj.add_basic_attr);
                        }
                    }
                }
            }
            
            #endregion

            #region 经营技能对属性加成

            /// <summary>
            /// 添加经营技能加成
            /// </summary>
            /// <param name="_businessSkillInfoList"></param>
            public void addBusinessSkillBonus(ConsortBusinessSkillInfoList _businessSkillInfoList)
            {
                if (_businessSkillInfoList == null || _businessSkillInfoList.businessSkillPropertySumList == null)
                    return;
                
                foreach (var businessSkillProperty in _businessSkillInfoList.businessSkillPropertySumList)
                {
                    addBusinessSkillBonus(businessSkillProperty);
                }
            }
            
            /// <summary>
            /// 添加经营技能加成
            /// </summary>
            /// <param name="_propertySum"></param>
            public void addBusinessSkillBonus(Common.ConsortObj.Consort_BusinessSkillPropertySum _propertySum)
            {
                if (_propertySum == null)
                    return;

                if (_propertySum.getAttr() == ESpecAttrType.NONE)//若属性为NONE, 则为全局属性加成
                {
                    // 暂时不这样, 因为用Enum.GetValues(typeof(ESpecAttrType)会有额外消耗
                    // foreach (int attrTypeValue in Enum.GetValues(typeof(ESpecAttrType)))
                    // {
                    //     if(attrTypeValue == (int)ESpecAttrType.NONE)
                    //         continue;
                    //     
                    //     addBonusValue(EBonusFilterType.BUILDING_ATTR, attrTypeValue, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    // }
                    addBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_A, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    addBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_B, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    addBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_C, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    addBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_D, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    addBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_E, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                }
                else
                {
                    // 经营技能是对特长属性类型的建筑加成, 即EBonusFilterType 为 BUILDING_ATTR
                    addBonusValue(EBonusFilterType.BUILDING_ATTR, (long)_propertySum.getAttr(), EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                }
            }

            public void removeBusinessSkillBonus(ConsortBusinessSkillInfoList _businessSkillInfoList)
            {
                if (_businessSkillInfoList == null || _businessSkillInfoList.businessSkillPropertySumList == null)
                    return;
                
                foreach (var businessSkillProperty in _businessSkillInfoList.businessSkillPropertySumList)
                {
                    removeBusinessSkillBonus(businessSkillProperty);
                }
            }
            
            public void removeBusinessSkillBonus(Common.ConsortObj.Consort_BusinessSkillPropertySum _propertySum)
            {
                if (_propertySum == null)
                    return;

                if (_propertySum.getAttr() == ESpecAttrType.NONE) //若属性为NONE, 则为全局属性加成
                {
                    removeBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_A, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    removeBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_B, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    removeBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_C, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    removeBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_D, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                    removeBonusValue(EBonusFilterType.BUILDING_ATTR, (long)ESpecAttrType.TYPE_E, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                }
                else
                {
                    // 经营技能是对特长属性类型的建筑加成, 即EBonusFilterType 为 BUILDING_ATTR
                    removeBonusValue(EBonusFilterType.BUILDING_ATTR, (long)_propertySum.getAttr(), EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _propertySum.getProAddSum());
                }
            }
            
            #endregion

            #region 加护技能对属性加成

            /// <summary>
            /// 添加加护技能加成
            /// </summary>
            /// <param name="_blessSkillInfo"></param>
            public void addBlessSkillBonus(GConsortRefObj _consortRef,  ConsortBlessSkillInfo _blessSkillInfo)
            {
                if (_consortRef == null || _blessSkillInfo == null)
                {
                    Debug.LogError("[GConsortComponent addBlessSkillBonus] 参数错误, _consortRef == null || _blessSkillInfo == null");
                    return;
                }

                if (_blessSkillInfo.consortBlessSkillLvlRefObj != null && _blessSkillInfo.consortBlessSkillLvlRefObj.add != null)
                {
                    addHeroAttrPropertyModifier(_consortRef.relation_hero_id_list, _blessSkillInfo.consortBlessSkillLvlRefObj.add);
                }
            }
            
            /// <summary>
            /// 添加加护技能加成
            /// </summary>
            /// <param name="_blessSkillInfoList"></param>
            public void addBlessSkillBonus(GConsortRefObj _consortRef,  List<ConsortBlessSkillInfo> _blessSkillInfoList)
            {
                if (_consortRef == null || _blessSkillInfoList == null)
                {
                    Debug.LogError("[GConsortComponent addBlessSkillBonus] 参数错误, _consortRef == null || _blessSkillInfoList == null");
                    return;
                }

                foreach (var blessSkillInfo in _blessSkillInfoList)
                {
                    addBlessSkillBonus(_consortRef, blessSkillInfo);
                }
            }
            
            /// <summary>
            /// 移除加护技能加成
            /// </summary>
            /// <param name="_blessSkillInfo"></param>
            public void removeBlessSkillBonus(GConsortRefObj _consortRef,  ConsortBlessSkillInfo _blessSkillInfo)
            {
                if (_consortRef == null || _blessSkillInfo == null)
                {
                    Debug.LogError("[GConsortComponent removeBlessSkillBonus] 参数错误, _consortRef == null || _blessSkillInfo == null");
                    return;
                }

                if (_blessSkillInfo.consortBlessSkillLvlRefObj != null && _blessSkillInfo.consortBlessSkillLvlRefObj.add != null)
                {
                    removeHeroAttrPropertyModifier(_consortRef.relation_hero_id_list, _blessSkillInfo.consortBlessSkillLvlRefObj.add);
                }
            }

            /// <summary>
            /// 移除加护技能加成
            /// </summary>
            /// <param name="_blessSkillInfoList"></param>
            public void removeBlessSkillBonus(GConsortRefObj _consortRef,  List<ConsortBlessSkillInfo> _blessSkillInfoList)
            {
                if (_consortRef == null || _blessSkillInfoList == null)
                {
                    Debug.LogError("[GConsortComponent removeBlessSkillBonus] 参数错误, _consortRef == null || _blessSkillInfoList == null");
                    return;
                }

                foreach (var blessSkillInfo in _blessSkillInfoList)
                {
                    removeBlessSkillBonus(_consortRef, blessSkillInfo);
                }
            }
            
            #endregion
            
            
            #region 全局属性加成

            /// <summary>
            /// 添加全局属性加成
            /// </summary>
            public void addBonus(UnionBonus _bonus)
            {
                _m_commonUnionBonusMgr.addBonus(_bonus);
            }
            
            /// <summary>
            /// 添加全局属性加成
            /// </summary>
            public void addBonusValue(EBonusFilterType _filterType, long _id, EBonusPropertyType _propertyType, long _value)
            {
                _m_commonUnionBonusMgr.addValue(_filterType, _id, _propertyType, _value);
            }
            
            /// <summary>
            /// 移除全局属性加成
            /// </summary>
            /// <param name="_bonus"></param>
            public void removeBonus(UnionBonus _bonus)
            {
                _m_commonUnionBonusMgr.removeBonus(_bonus);
            }
            
            /// <summary>
            /// 移除全局属性加成
            /// </summary>
            /// <param name="_filterType"></param>
            /// <param name="_id"></param>
            /// <param name="_propertyType"></param>
            /// <param name="_value"></param>
            public void removeBonusValue(EBonusFilterType _filterType, long _id, EBonusPropertyType _propertyType, long _value)
            {
                _m_commonUnionBonusMgr.removeValue(_filterType, _id, _propertyType, _value);
            }
            
            /// <summary>
            /// 玩家全局属性加成变更
            /// </summary>
            /// <param name="_type"></param>
            private void _onBonusChg(EBonusPropertyType _type)
            {
            }
            
            #endregion

            #region 对大臣属性加成
            
            private void _discardHeroAttrPropertyModifier()
            {
                foreach (PlayerAttrPropertyModifier modifier in _m_dHeroAttrPropertyModifierDic.Values)
                {
                    modifier?.clear();
                }
                _m_dHeroAttrPropertyModifierDic.Clear();
            }
            
            /// <summary>
            /// 添加对大臣属性加成
            /// </summary>
            /// <param name="_heroId"></param>
            /// <param name="_modifier"></param>
            public void addHeroAttrPropertyModifier(long _heroId, PlayerAttrPropertyModifier _modifier)
            {
                if(_modifier == null)
                    return;
                
                PlayerAttrPropertyModifier modifier = _ensuredHeroAttrPropertyModifier(_heroId);
                modifier.addModifier(_modifier);
                
                WinMsg.SendMsg(WinMsgType.ON_CONSORT_RELATION_HERO_ATTR_PROPERTY_CHG, _heroId);
            }
            
            /// <summary>
            /// 添加对大臣属性加成
            /// </summary>
            /// <param name="_heroIdList"></param>
            /// <param name="_modifier"></param>
            public void addHeroAttrPropertyModifier(List<long> _heroIdList, PlayerAttrPropertyModifier _modifier)
            {
                if(_heroIdList == null || _modifier == null)
                    return;

                foreach (long heroId in _heroIdList)
                {
                    addHeroAttrPropertyModifier(heroId, _modifier);
                }
            }
            
            /// <summary>
            /// 移除对大臣属性加成
            /// </summary>
            /// <param name="_heroId"></param>
            /// <param name="_modifier"></param>
            public void removeHeroAttrPropertyModifier(long _heroId, PlayerAttrPropertyModifier _modifier)
            {
                if(_modifier == null)
                    return;
                
                PlayerAttrPropertyModifier modifier = _ensuredHeroAttrPropertyModifier(_heroId);
                modifier.removeModifier(_modifier);
                
                WinMsg.SendMsg(WinMsgType.ON_CONSORT_RELATION_HERO_ATTR_PROPERTY_CHG, _heroId);
            }
            
            /// <summary>
            /// 移除对大臣属性加成
            /// </summary>
            /// <param name="_heroIdList"></param>
            /// <param name="_modifier"></param>
            public void removeHeroAttrPropertyModifier(List<long> _heroIdList, PlayerAttrPropertyModifier _modifier)
            {
                if(_heroIdList == null || _modifier == null)
                    return;

                foreach (long heroId in _heroIdList)
                {
                    removeHeroAttrPropertyModifier(heroId, _modifier);
                }
            }
            
            /// <summary>
            /// 获取对应的大臣属性加成Modifier
            /// </summary>
            /// <param name="_heroId"></param>
            /// <returns></returns>
            [NotNull] private PlayerAttrPropertyModifier _ensuredHeroAttrPropertyModifier(long _heroId)
            {
                PlayerAttrPropertyModifier modifier = null;
                if (!_m_dHeroAttrPropertyModifierDic.TryGetValue(_heroId, out modifier) || modifier == null)
                {
                    modifier = new PlayerAttrPropertyModifier();
                    _m_dHeroAttrPropertyModifierDic.Add(_heroId, modifier);
                }
                return modifier;
            }
            
            public PlayerAttrPropertyModifier getHeroAttrPropertyModifier(long _heroId)
            {
                PlayerAttrPropertyModifier modifier = null;
                _m_dHeroAttrPropertyModifierDic.TryGetValue(_heroId, out modifier);
                return modifier;
            }

            #endregion

            #region 对玩家属性加成

            public NPPlayerPropertyContainer playerPropertyContainer => _m_PlayerPropertyContainer;
            
            /// <summary>
            /// 添加对玩家属性加成
            /// </summary>
            /// <param name="_modifier"></param>
            private void _addPlayerPropertyModifier(NPPlayerPropertyModifier _modifier)
            {
                if (_modifier == null)
                    return;
                
                _m_PlayerPropertyContainer.addModifier(_modifier);
            }
            
            /// <summary>
            /// 移除
            /// </summary>
            /// <param name="_modifier"></param>
            private void _removePlayerPropertyModifier(NPPlayerPropertyModifier _modifier)
            {
                if (_modifier == null)
                    return;
                
                _m_PlayerPropertyContainer.removeModifier(_modifier);
            }

            #endregion
        }
    }
}
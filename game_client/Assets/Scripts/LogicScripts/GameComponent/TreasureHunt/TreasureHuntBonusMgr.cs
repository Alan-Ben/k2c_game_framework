using JetBrains.Annotations;

namespace GOE
{
    public partial class TreasureHuntComponent
    {
        private class TreasureHuntBonusMgr
        {
            [NotNull] private TreasureHuntComponent _m_component;
        
            [NotNull] private CommonUnionBonusMgr _m_commonUnionBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.TREASURE_HUNT);//太空寻宝对全局加成BonusMgr
            [NotNull] private NPPlayerPropertyContainer _m_PlayerPropertyContainer = new NPPlayerPropertyContainer();//玩家属性加成

            public TreasureHuntBonusMgr([NotNull] TreasureHuntComponent _component)
            {
                _m_component = _component;
            }

            public NPPlayerPropertyContainer playerPropertyContainer { get { return _m_PlayerPropertyContainer; } }
            
            public void init()
            {
                discard();
                foreach (var oreInfo in _m_component.gotOreInfoList)
                {
                    addOre(oreInfo);
                }
                foreach (var treasureInfo in _m_component.gotTreasureInfoList)
                {
                    addTreasure(treasureInfo);
                }
                foreach (var compositeCatalogInfo in _m_component.compositeCatalogInfoList)
                {
                    addCompositeCatalog(compositeCatalogInfo);
                }
                
                _m_commonUnionBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            }
            
            public void discard()
            {
                _m_commonUnionBonusMgr.clear();
                
                _m_PlayerPropertyContainer.clear();
            }

            /// <summary>
            /// 增加矿石加成
            /// </summary>
            /// <param name="_oreInfo"></param>
            public void addOre(TreasureHuntGotOreInfo _oreInfo)
            {
                if(_oreInfo == null)
                    return;
                
                if(_oreInfo.normalSkillInfo != null)
                    addSkill(_oreInfo.normalSkillInfo);
                
                if(_oreInfo.advanceSkillInfo != null)
                    addSkill(_oreInfo.advanceSkillInfo);
            }

            public void removeOre(TreasureHuntGotOreInfo _oreInfo)
            {
                if(_oreInfo == null)
                    return;
                
                if(_oreInfo.normalSkillInfo != null)
                    removeSkill(_oreInfo.normalSkillInfo);
                
                if(_oreInfo.advanceSkillInfo != null)
                    removeSkill(_oreInfo.advanceSkillInfo);
            }

            /// <summary>
            /// 增加矿石加成
            /// </summary>
            public void addTreasure(TreasureHuntGotTreasureInfo _treasureInfo)
            {
                if(_treasureInfo == null)
                    return;
                
                if(_treasureInfo.skillInfo != null)
                    addSkill(_treasureInfo.skillInfo);
            }
            
            public void removeTreasure(TreasureHuntGotTreasureInfo _treasureInfo)
            {
                if(_treasureInfo == null)
                    return;
                
                if(_treasureInfo.skillInfo != null)
                    removeSkill(_treasureInfo.skillInfo);
            }
            
            /// <summary>
            /// 增加组合图鉴加成
            /// </summary>
            /// <param name="_compositeCatalogInfo"></param>
            public void addCompositeCatalog(TreasureHuntCompositeCatalogInfo _compositeCatalogInfo)
            {
                if(_compositeCatalogInfo == null)
                    return;
                
                if(_compositeCatalogInfo.normalSkillInfo != null)
                    addSkill(_compositeCatalogInfo.normalSkillInfo);
                
                if(_compositeCatalogInfo.advancedSkillInfo != null)
                    addSkill(_compositeCatalogInfo.advancedSkillInfo);
            }
            
            public void removeCompositeCatalog(TreasureHuntCompositeCatalogInfo _compositeCatalogInfo)
            {
                if(_compositeCatalogInfo == null)
                    return;
                
                if(_compositeCatalogInfo.normalSkillInfo != null)
                    removeSkill(_compositeCatalogInfo.normalSkillInfo);
                
                if(_compositeCatalogInfo.advancedSkillInfo != null)
                    removeSkill(_compositeCatalogInfo.advancedSkillInfo);
            }

            /// <summary>
            /// 移除技能加成
            /// </summary>
            public void removeSkill(_ITreasureHuntSkillInfo _skillInfo)
            {
                if(_skillInfo == null || _skillInfo.skillLevelRefObj == null)
                    return;
                
                if(_skillInfo.skillLevelRefObj.union_bonus != null)
                    _m_commonUnionBonusMgr.removeBonus(_skillInfo.skillLevelRefObj.union_bonus.unionBonus);
                
                if(_skillInfo.skillLevelRefObj.add_player != null)
                    _m_PlayerPropertyContainer.removeModifier(_skillInfo.skillLevelRefObj.add_player);
            }
            
            /// <summary>
            /// 添加技能加成
            /// </summary>
            public void addSkill(_ITreasureHuntSkillInfo _skillInfo)
            {
                if(_skillInfo == null || _skillInfo.skillLevelRefObj == null)
                    return;
                
                if(_skillInfo.skillLevelRefObj.union_bonus != null)
                    _m_commonUnionBonusMgr.addBonus(_skillInfo.skillLevelRefObj.union_bonus.unionBonus);
                
                if(_skillInfo.skillLevelRefObj.add_player != null)
                    _m_PlayerPropertyContainer.addModifier(_skillInfo.skillLevelRefObj.add_player);
            }
        }
    }
}
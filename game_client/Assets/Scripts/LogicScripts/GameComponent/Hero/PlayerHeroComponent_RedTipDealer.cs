
using Common.HeroObj;
using JetBrains.Annotations;
using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴管理器
    /// </summary>
    public partial class PlayerHeroComponent
    {
        private class RedTipDealer
        {
            //伙伴组件
            private PlayerHeroComponent _m_heroComponent;
            //红点字典
            [NotNull] private Dictionary<string, CommonForceRedTipNode> _m_dMyNode = new Dictionary<string, CommonForceRedTipNode>(); // 为了根据自定义的索引取到对应节点的字典

            public RedTipDealer(PlayerHeroComponent _comp)
            {
                _m_heroComponent = _comp;
            }

            #region 初始化红点

            /// <summary>
            /// 初始化顾问红点管理器
            /// </summary>
            public void init()
            {
                if (_m_heroComponent == null)
                    return;

                clear();

                foreach (HeroInfo heroInfo in _m_heroComponent._m_lHeroInfoList)
                {
                    _addRedTipNode(heroInfo);
                }

                //刷新一下红点
                onBagItemChg();
                onCurrencyChg();
            }

            //初始化添加红点
            private void _addRedTipNode(HeroInfo _heroInfo)
            {
                if (_heroInfo == null)
                    return;

                _addNode(RedTipConst.RED_HERO_FIRST_GET, _heroInfo);//顾问首次获得
                _addNode(RedTipConst.RED_HERO_CAN_WEAR_EQUIP, _heroInfo);//顾问可穿戴藏品红点
                _addNode(RedTipConst.RED_HERO_LEVEL_UP, _heroInfo);//顾问可升级
                _addNode(RedTipConst.RED_HERO_STEP_UP, _heroInfo);//顾问可晋升
                _addNode(RedTipConst.RED_HERO_BUSINESS_UPGRADE, _heroInfo);//顾问经营技能可升级
                _addNode(RedTipConst.RED_HERO_TALENT_UPGRADE, _heroInfo);//顾问资质可升级
                _addNode(RedTipConst.RED_HERO_STAR_UPGRADE, _heroInfo);//顾问深造可升级
                _addNode(RedTipConst.RED_HERO_HALO_UPGRADE, _heroInfo);//顾问威望可升级
                // _addNode(RedTipConst.RED_HERO_SKIN_UNLOCK, _heroInfo);//顾问皮肤可解锁
                // _addNode(RedTipConst.RED_HERO_NEW_SKIN, _heroInfo);//顾问新获得皮肤
                // _addNewSkinNode(_heroInfo);//新皮肤红点
            }

            //添加红点
            private void _addNode(long _parentId, HeroInfo _heroInfo)
            {
                if (_heroInfo == null)
                    return;

                string nodeKey = _createRedKey(_parentId, _heroInfo.id);
                if (_m_dMyNode.ContainsKey(nodeKey))
                    return;

                CommonForceRedTipNode node = new CommonForceRedTipNode(nodeKey);
                _m_dMyNode[nodeKey] = node;
                RedTipMgr.instance.addRedTipNodeWithParent(node, _parentId);
            }

            //设置新皮肤红点
            // private void _addNewSkinNode(HeroInfo _heroInfo)
            // {
            //     if (_heroInfo == null || _heroInfo.heroRefObj == null)
            //         return;
            //
            //     string newSkinParentNodeKey = _createRedKey(RedTipConst.RED_HERO_NEW_SKIN, _heroInfo.id);
            //     _m_dMyNode.TryGetValue(newSkinParentNodeKey, out CommonForceRedTipNode _newSkinParentNode);
            //     if (_newSkinParentNode == null)
            //         return;
            //
            //     List<HeroSkinRefObj> skinRefList = _heroInfo.heroRefObj.heroSkinRefList;
            //     for (int i = 0; i < skinRefList.Count; i++)
            //     {
            //         HeroSkinRefObj skinRef = skinRefList[i];
            //         if (skinRef == null)
            //             continue;
            //
            //         string nodeKey = _createNewSkinRedKey(newSkinParentNodeKey, skinRef.id);
            //         CommonForceRedTipNode node = new CommonForceRedTipNode(nodeKey);
            //         _m_dMyNode[nodeKey] = node;
            //         RedTipMgr.instance.addRedTipNodeWithParent(node, _newSkinParentNode);
            //     }
            // }

            #endregion




            #region 事件变更刷新红点

            /// <summary>
            /// 刷新首次获得红点
            /// </summary>
            /// <param name="_heroInfo"></param>
            public void onGainHero(HeroInfo _heroInfo)
            {
                if (_heroInfo == null)
                    return;

                _addRedTipNode(_heroInfo);

                string nodeKey = _createRedKey(RedTipConst.RED_HERO_FIRST_GET, _heroInfo.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(1);
            }

            /// <summary>
            /// 背包物品变化
            /// </summary>
            public void onBagItemChg()
            {
                foreach (HeroInfo heroInfo in _m_heroComponent._m_lHeroInfoList)
                {
                    if (heroInfo == null)
                        continue;
            
                    //刷新升阶红点
                    refreshStepUpRedTip(heroInfo);
                    //刷新经营技能红点
                    refreshBusinessSkillUpgradeRedTip(heroInfo);
                    //刷新资质升级红点
                    refreshTalentUpgradeRedTip(heroInfo);
                    //刷新深造可升级红点
                    refreshStarUpgradeRedTip(heroInfo);
                    //刷新威望可升级红点
                    refreshHaloUpgradeRedTip(heroInfo);
                    // //刷新可解锁皮肤红点
                    // refreshSkinUnlockRedTip(heroInfo);
                }
            }

            /// <summary>
            /// 资源变更
            /// </summary>
            public void onCurrencyChg()
            {
                foreach (HeroInfo heroInfo in _m_heroComponent._m_lHeroInfoList)
                {
                    if (heroInfo == null)
                        continue;

                    refreshLevelUpRedTip(heroInfo);
                }
            }

            /// <summary>
            /// 藏品新增
            /// </summary>
            public void onEquipChg()
            {
                foreach (HeroInfo heroInfo in _m_heroComponent._m_lHeroInfoList)
                {
                    if (heroInfo == null)
                        continue;

                    refreshCanWearEquipRedTip(heroInfo);
                }
            }

            /// <summary>
            /// 刷新可升级红点
            /// </summary>
            public void refreshLevelUpRedTip(HeroInfo _heroInfo)
            {
                if (_heroInfo == null)
                    return;

                //是否可升级
                bool canLvlUp = _heroInfo.getCanLvlUp(false);
                string nodeKey = _createRedKey(RedTipConst.RED_HERO_LEVEL_UP, _heroInfo.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(canLvlUp ? 1 : 0);
            }

            /// <summary>
            /// 刷新升阶红点
            /// </summary>
            /// <param name="_heroInfo"></param>
            public void refreshStepUpRedTip(HeroInfo _heroInfo)
            {
                if (_heroInfo == null)
                    return;
            
                //是否可升阶
                bool canStepUp = _heroInfo.isStepLvlLimit() && _heroInfo.nextHeroStepRef != null && _heroInfo.curHeroStepRef != null && GCommon.isItemEnough(_heroInfo.curHeroStepRef.cost_item_list, false);
                string nodeKey = _createRedKey(RedTipConst.RED_HERO_STEP_UP, _heroInfo.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(canStepUp ? 1 : 0);
            }

            /// <summary>
            /// 刷新可穿戴藏品红点
            /// </summary>
            /// <param name="_heroInfo"></param>
            public void refreshCanWearEquipRedTip(HeroInfo _heroInfo)
            {
                if (_heroInfo == null)
                    return;

                EquipInfo equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_heroInfo.id);
                bool haveNoWearEquip = NPPlayer.instance.equipComp.haveNoWearEquip();
                //是否可穿戴藏品
                bool canWearEquip = equipInfo == null && haveNoWearEquip;
                //是否有更高品质藏品可替换
                bool haveBetterEquip = equipInfo != null && NPPlayer.instance.equipComp.haveHigherQualityNoWear(GCommon.getItemQuality(ENPItemType.EQUIP, equipInfo.equipId));

                string nodeKey = _createRedKey(RedTipConst.RED_HERO_CAN_WEAR_EQUIP, _heroInfo.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(canWearEquip || haveBetterEquip ? 1 : 0);
            }

            /// <summary>
            /// 刷新经营技能红点
            /// </summary>
            public void refreshBusinessSkillUpgradeRedTip(HeroInfo _heroInfo)
            {
                if (_heroInfo == null || _heroInfo.heroRefObj == null)
                    return;

                long skillId = _heroInfo.heroRefObj.business_skill_id;
                HeroBusinessSkillInfo businessSkillInfo = _heroInfo.heroBusinessSkillInfoMgr.getBusinessSkillInfo(skillId);
                if (businessSkillInfo == null || businessSkillInfo.businessSkillRefObj == null || !businessSkillInfo.businessSkillRefObj.can_upgrade)
                    return;

                HeroBusinessSkillUpgradeRefObj skillUpgradeRef = GRefdataCoreMgr.instance.getHeroBusinessSkillUpgradeRef(businessSkillInfo.businessSkillRefObj.upgrade_cost_group, businessSkillInfo.level);
                bool canUpgrade = skillUpgradeRef != null && GCommon.isItemEnough(skillUpgradeRef.upgrade_cost_item, false);
                string nodeKey = _createRedKey(RedTipConst.RED_HERO_BUSINESS_UPGRADE, _heroInfo.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(canUpgrade ? 1 : 0);
            }

            /// <summary>
            /// 刷新资质升级红点
            /// </summary>
            /// <param name="_heroInfo"></param>
            public void refreshTalentUpgradeRedTip(HeroInfo _heroInfo)
            {
                if (_heroInfo == null)
                    return;

                long redTipCount = 0;
                for (int i = 0; i < _heroInfo.heroTalentSkillInfoMgr.talentSkillInfoList.Count; i++)
                {
                    HeroTalentSkillInfo talentSkillInfo = _heroInfo.heroTalentSkillInfoMgr.talentSkillInfoList[i];
                    if(talentSkillInfo == null)
                        continue;

                    if(talentSkillInfo.canUpgrade())
                        redTipCount++;
                }

                string nodeKey = _createRedKey(RedTipConst.RED_HERO_TALENT_UPGRADE, _heroInfo.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(redTipCount);
            }

            /// <summary>
            /// 刷新深造可升级红点
            /// </summary>
            public void refreshStarUpgradeRedTip(HeroInfo _heroInfo)
            {
                if (_heroInfo == null || _heroInfo.heroRefObj == null)
                    return;

                HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_heroInfo.id, _heroInfo.star);
                if (starRef == null)
                    return;

                bool canUpgrade = (starRef.upgrade_player_condition == null || starRef.upgrade_player_condition.IsEnable(null)) &&
                                  (starRef.upgrade_condition == null || starRef.upgrade_condition.IsEnable(_heroInfo.heroRefObj, null)) &&
                                  GCommon.isItemEnough(starRef.upgrade_cost, false);

                string nodeKey = _createRedKey(RedTipConst.RED_HERO_STAR_UPGRADE, _heroInfo.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(canUpgrade ? 1 : 0);
            }

            /// <summary>
            /// 刷新威望可升级红点
            /// </summary>
            /// <param name="_heroInfo"></param>
            public void refreshHaloUpgradeRedTip(HeroInfo _heroInfo)
            {
                if (_heroInfo == null || _heroInfo.heroRefObj == null)
                    return;

                bool canUpgrade = false;
                if (_heroInfo.haveHalo)
                {
                    if (!_heroInfo.heroHaloInfo.isUnlock)
                    {
                        //可激活
                        canUpgrade = true;
                    }
                    else
                    {
                        //可升级
                        if(_heroInfo.heroHaloInfo.nextHaloLevelRef != null && GCommon.isItemEnough(_heroInfo.heroHaloInfo.curHaloLevelRef.upgrade_cost, false))
                            canUpgrade = true;
                    }
                }

                string nodeKey = _createRedKey(RedTipConst.RED_HERO_HALO_UPGRADE, _heroInfo.id);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    _node?.setCount(canUpgrade ? 1 : 0);
            }

            // /// <summary>
            // /// 刷新可解锁皮肤红点
            // /// </summary>
            // /// <param name="_heroInfo"></param>
            // public void refreshSkinUnlockRedTip(HeroInfo _heroInfo)
            // {
            //     if (_heroInfo == null || _heroInfo.heroRefObj == null)
            //         return;
            //
            //     string nodeKey = _createRedKey(RedTipConst.RED_HERO_SKIN_UNLOCK, _heroInfo.id);
            //     _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
            //     List<HeroSkinRefObj> skinRefList = _heroInfo.heroRefObj.heroSkinRefList;
            //     for (int i = 0; i < skinRefList.Count; i++)
            //     {
            //         HeroSkinRefObj skinRef = skinRefList[i];
            //         if (skinRef == null)
            //             continue;
            //
            //         if (_heroInfo.heroSkinInfoMgr.getSkinInfo(skinRef.id) == null && GCommon.isItemEnough(skinRef.unlock_item, false))
            //         {
            //             _node?.setCount(1);
            //             return;
            //         }
            //     }
            //
            //     _node?.setCount(0);
            // }

            // /// <summary>
            // /// 获得新皮肤
            // /// </summary>
            // /// <param name="_heroInfo"></param>
            // /// <param name="_skinId"></param>
            // public void onGetNewSkin(HeroInfo _heroInfo, long _skinId)
            // {
            //     if (_heroInfo == null)
            //         return;
            //
            //     string newSkinParentNodeKey = _createRedKey(RedTipConst.RED_HERO_NEW_SKIN, _heroInfo.id);
            //     _m_dMyNode.TryGetValue(newSkinParentNodeKey, out CommonForceRedTipNode _newSkinParentNode);
            //     if (_newSkinParentNode == null)
            //         return;
            //
            //     //设置新皮肤红点
            //     string nodeKey = _createNewSkinRedKey(newSkinParentNodeKey, _skinId);
            //     if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode node))
            //         node?.setCount(1);
            // }

            #endregion

            /// <summary>
            /// 是否需要展示红点
            /// </summary>
            /// <param name="_redTipId"></param>
            /// <param name="_heroId"></param>
            /// <returns></returns>
            public bool needShowRedTip(long _redTipId, long _heroId)
            {
                //判断是否是已解锁红点
                RedMonitorRefObj redMonitorRef = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(_redTipId);
                if (redMonitorRef != null && !GCommon.isSimpleUnlock(redMonitorRef.simple_unlock_id))
                    return false;
                NPRedTipRefObj redTipRef = GRefdataCoreMgr.instance.redTipRefCore.getRef(_redTipId);
                if(redTipRef != null && redTipRef.function_type != ENPFunctionType.NONE && !GCommon.isFuncUnlock(redTipRef.function_type))
                    return false;

                string nodeKey = _createRedKey(_redTipId, _heroId);
                _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
                return _node != null && _node.needShow();
            }

            // /// <summary>
            // /// 是否需要展示新皮肤红点
            // /// </summary>
            // /// <param name="_heroId"></param>
            // /// <param name="_skinId"></param>
            // /// <returns></returns>
            // public bool needShowNewSkinRedTip(long _heroId, long _skinId)
            // {
            //     string newSkinParentNodeKey = _createRedKey(RedTipConst.RED_HERO_NEW_SKIN, _heroId);
            //     string nodeKey = _createNewSkinRedKey(newSkinParentNodeKey, _skinId);
            //     _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
            //     return _node != null && _node.needShow();
            // }

            /// <summary>
            /// 设置红点已读
            /// </summary>
            /// <param name="_redTipId"></param>
            /// <param name="_heroId"></param>
            public void setReadRedTip(long _redTipId, long _heroId)
            {
                string nodeKey = _createRedKey(_redTipId, _heroId);
                _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
                _node?.setCount(0);
            }

            // /// <summary>
            // /// 设置新皮肤红点已读
            // /// </summary>
            // /// <param name="_heroId"></param>
            // /// <param name="_skinId"></param>
            // public void setReadNewSkinTip(long _heroId, long _skinId)
            // {
            //     string newSkinParentNodeKey = _createRedKey(RedTipConst.RED_HERO_NEW_SKIN, _heroId);
            //     string nodeKey = _createNewSkinRedKey(newSkinParentNodeKey, _skinId);
            //     _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
            //     _node?.setCount(0);
            // }

            /// <summary>
            /// 清除数据
            /// </summary>
            public void clear()
            {
                _m_dMyNode.Clear();
            }

            //创建唯一key
            [NotNull]
            private string _createRedKey(long _key, long _heroId)
            {
                return $"{_key}_{_heroId}";
            }

            //创建新皮肤红点key
            // [NotNull]
            // private string _createNewSkinRedKey(string _parentStr, long _skinId)
            // {
            //     return $"new_skin_{_parentStr}_{_skinId}";
            // }
        }
    }
}

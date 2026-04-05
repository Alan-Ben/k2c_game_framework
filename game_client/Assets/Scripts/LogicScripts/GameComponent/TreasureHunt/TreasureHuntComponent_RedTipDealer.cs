using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public partial class TreasureHuntComponent
    {
        public class RedTipDealer
        {
            [NotNull] private readonly TreasureHuntComponent _m_comp;
            
            private TreasureHuntRedTipSaver _m_saver;
            
            private _ARedTipNode _m_pendingOreNode;// 待处理矿石红点
            private _ARedTipNode _m_lazyCdFullNode;// 体力已满红点
            private _ARedTipNode _m_treasureNotActivateNode;// 奇物未激活红点
            private _ARedTipNode _m_treasureSkillCanUpgradeNode;// 奇物技能可升级红点
            private _ARedTipNode _m_outputCanDrawNode;// 奇物产出可领取红点
            private _ARedTipNode _m_hasEnergyNode;// 有能源道具可探索红点
            private _ARedTipNode _m_oreCatalogNode;// 矿石图鉴红点
            private _ARedTipNode _m_compositeCatalogNode;// 组合图鉴红点
            
            public RedTipDealer([NotNull] TreasureHuntComponent _comp) 
            { 
                _m_comp = _comp;
            }
            
            public void init()
            {
                // 初始化存储器
                _m_saver = new TreasureHuntRedTipSaver();
                _m_saver.init();
                
                // Get nodes from RedTipConst
                _m_pendingOreNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_TREASURE_HUNT_PENDING_ORE);
                _m_lazyCdFullNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_TREASURE_HUNT_LAZY_CD_FULL);
                _m_treasureNotActivateNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_TREASURE_HUNT_TREASURE_NOT_ACTIVATE);
                _m_treasureSkillCanUpgradeNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_TREASURE_HUNT_TREASURE_SKILL_CAN_UPGRADE);
                _m_outputCanDrawNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_TREASURE_HUNT_OUTPUT_CAN_DRAW);
                _m_hasEnergyNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_TREASURE_HUNT_HAS_ENERGY);
                _m_oreCatalogNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_TREASURE_HUNT_ORE_CATALOG);
                _m_compositeCatalogNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_TREASURE_HUNT_COMPOSITE_CATALOG);
                
                // Initial refresh
                refreshPendingOreRedTip();
                refreshLazyCdFullRedTip();
                refreshTreasureNotActivateRedTip();
                refreshTreasureSkillCanUpgradeRedTip();
                refreshOutputCanDrawRedTip();
                refreshHasEnergyRedTip();
                refreshAllOreCatalogRedTip();
                refreshCompositeCatalogRedTip();
                refreshUnlockNewAreaRedTip();
                
                // Register message listener
                WinMsg.RegisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCdChg);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_TREASURE, _onTreasureUnlock);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_LEVEL_CHG, _onTreasureLevelChg);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_OUTPUT_CHG, _onTreasureOutputChg);
                WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_NORMAL_ORE, _onOreUnlock);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_ADVANCED_ORE, _onOreUnlock);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_SKILL_CHG, _onOreSkillChg);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_NORMAL_COMPOSITE_CATALOG, _onCompositeCatalogUnlock);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_ADVANCED_COMPOSITE_CATALOG, _onCompositeCatalogUnlock);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_COMPOSITE_CATALOG_CHG, _onCompositeCatalogChg);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_MAX_RECORD_CHG, _onOreMaxRecordChg);
                WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_HAD_DRAW_RECORD_REWARD_CHG, _onOreHadDrawRecordRewardChg);
            }
            
            public void clear()
            {
                // Unregister message listener
                WinMsg.UnregisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCdChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_TREASURE, _onTreasureUnlock);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_LEVEL_CHG, _onTreasureLevelChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_OUTPUT_CHG, _onTreasureOutputChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_NORMAL_ORE, _onOreUnlock);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_ADVANCED_ORE, _onOreUnlock);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_SKILL_CHG, _onOreSkillChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_NORMAL_COMPOSITE_CATALOG, _onCompositeCatalogUnlock);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_UNLOCK_ADVANCED_COMPOSITE_CATALOG, _onCompositeCatalogUnlock);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_COMPOSITE_CATALOG_CHG, _onCompositeCatalogChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_MAX_RECORD_CHG, _onOreMaxRecordChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_ORE_HAD_DRAW_RECORD_REWARD_CHG, _onOreHadDrawRecordRewardChg);
                
                _m_pendingOreNode = null;
                _m_lazyCdFullNode = null;
                _m_treasureNotActivateNode = null;
                _m_treasureSkillCanUpgradeNode = null;
                _m_outputCanDrawNode = null;
                _m_hasEnergyNode = null;
                _m_oreCatalogNode = null;
                _m_compositeCatalogNode = null;

                _m_saver = null;
            }

            #region 待处理矿石红点

            public void refreshPendingOreRedTip()
            {
                if (_m_pendingOreNode == null) 
                    return;
                
                // 检查是否有待处理矿石
                int pendingOreCount = _m_comp.getPendingOreCount();
                _m_pendingOreNode.setCount(pendingOreCount);
            }

            #endregion

            #region 体力已满红点

            /// <summary>
            /// 刷新体力已满红点
            /// </summary>
            public void refreshLazyCdFullRedTip()
            {
                if (_m_lazyCdFullNode == null)
                    return;
                
                // 获取寻宝体力CD信息
                PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_lazy_cd_id);

                bool cdIsFull = lazyCdInfo != null && lazyCdInfo.getCount() >= lazyCdInfo.MaxCount;//cd是否已满
                _m_lazyCdFullNode.setCount(cdIsFull ? 1 : 0);
            }

            #endregion

            #region 奇物未激活红点

            /// <summary>
            /// 刷新奇物未激活红点
            /// </summary>
            public void refreshTreasureNotActivateRedTip()
            {
                if (_m_treasureNotActivateNode == null)
                    return;
                
                bool hasNotActivateTreasure = false;//是否有未激活奇物
                
                // 遍历所有已获取的奇物
                foreach (var treasureInfo in _m_comp.gotTreasureInfoList)
                {
                    if (treasureInfo == null)
                        continue;
                    
                    // 检查奇物状态是否为未激活
                    if (treasureInfo.treasureState == ETreasureHuntTreasureState.GOT_NOT_ACTIVATE)
                    {
                        hasNotActivateTreasure = true;
                        break;
                    }
                }
                
                _m_treasureNotActivateNode.setCount(hasNotActivateTreasure ? 1 : 0);
            }

            #endregion

            #region 奇物技能可升级红点

            /// <summary>
            /// 刷新奇物技能可升级红点 - 当有奇物技能可升级时显示红点
            /// </summary>
            public void refreshTreasureSkillCanUpgradeRedTip()
            {
                if (_m_treasureSkillCanUpgradeNode == null)
                    return;
                
                bool needShowRed = false;
                
                // 遍历所有已获取的奇物
                foreach (var treasureInfo in _m_comp.gotTreasureInfoList)
                {
                    if (treasureInfo == null)
                        continue;
                    
                    // 检查奇物技能是否可升级
                    if (TreasureHuntUtil.checkCanUpgradeSkill(treasureInfo.skillInfo))
                    {
                        needShowRed = true;
                        break;
                    }
                }
                
                _m_treasureSkillCanUpgradeNode.setCount(needShowRed ? 1 : 0);
            }

            #endregion
            
            #region 奇物产出可领取红点

            /// <summary>
            /// 刷新奇物产出可领取红点
            /// </summary>
            public void refreshOutputCanDrawRedTip()
            {
                if (_m_outputCanDrawNode == null)
                    return;
                
                bool hasTreasureOutputCanDraw = false;//是否有奇物产出可领取
                
                // 遍历所有奇物产出信息
                foreach (var outputInfo in _m_comp.treasureOutputInfoList)
                {
                    if (outputInfo == null)
                        continue;
                    
                    // 检查是否可以领取
                    if (outputInfo.canDraw)
                    {
                        hasTreasureOutputCanDraw = true;
                        break;
                    }
                }
                
                _m_outputCanDrawNode.setCount(hasTreasureOutputCanDraw ? 1 : 0);
            }

            #endregion

            #region 有能源道具可探索红点

            /// <summary>
            /// 刷新有能源道具可探索红点
            /// </summary>
            public void refreshHasEnergyRedTip()
            {
                if (_m_hasEnergyNode == null)
                    return;
                
                // 普通能源道具数量
                long premiumEnergyCount = GCommon.getItemCount(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_premium_energy_common_item);
                long advancedEnergyCount = GCommon.getItemCount(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_advanced_energy_common_item);
                
                _m_hasEnergyNode.setCount(premiumEnergyCount + advancedEnergyCount);
            }

            #endregion

            #region 矿石图鉴红点

            /// <summary>
            /// 刷新矿石图鉴红点 - 当有矿石要显示红点时显示红点
            /// </summary>
            public void refreshAllOreCatalogRedTip()
            {
                // 遍历所有已获取的矿石
                foreach (var oreInfo in _m_comp.gotOreInfoList)
                {
                    if(refreshOreCatalogRedTip(oreInfo))
                        break;
                }
            }

            public bool refreshOreCatalogRedTip(long _oreId)
            {
                _ARedTipNode redTipNode = _getOreCatalogRedTipNode(_oreId);
                if(redTipNode == null)
                    return false;

                TreasureHuntGotOreInfo gotOreInfo = _m_comp.getGotOreInfo(_oreId);
                bool needShowRed = TreasureHuntUtil.oreCatalogNeedShowRed(gotOreInfo);
                redTipNode.setCount(needShowRed ? 1 : 0);
                return needShowRed;
            }
            
            public bool refreshOreCatalogRedTip(TreasureHuntGotOreInfo _gotOreInfo)
            {
                if(_gotOreInfo == null)
                    return false;
                
                _ARedTipNode redTipNode = _getOreCatalogRedTipNode(_gotOreInfo.oreId);
                if(redTipNode == null)
                    return false;
             
                bool needShowRed = TreasureHuntUtil.oreCatalogNeedShowRed(_gotOreInfo);
                redTipNode.setCount(needShowRed ? 1 : 0);
                return needShowRed;
            }
            
            /// <summary>
            /// 获取每个矿石图鉴
            /// </summary>
            /// <param name="_oreId"></param>
            /// <returns></returns>
            private _ARedTipNode _getOreCatalogRedTipNode(long _oreId)
            {
                if (_m_oreCatalogNode == null)
                    return null;
                
                string saveKey = _getOreCatalogRedTipNodeSaveKey(_oreId);
                _ARedTipNode redTipNode = _m_oreCatalogNode.getNodeBySaveKeyRecursive(saveKey);
                if (redTipNode == null)
                {
                    redTipNode = new CommonForceRedTipNode(saveKey);
                    RedTipMgr.instance.addRedTipNodeWithParent(redTipNode, _m_oreCatalogNode);
                }

                return redTipNode;
            }
            
            [NotNull] private string _getOreCatalogRedTipNodeSaveKey(long _oreId)
            {
                return $"TreasureOreCatalogRedTip_{_oreId}";
            }
            
            #endregion

            #region 组合图鉴红点

            /// <summary>
            /// 刷新组合图鉴红点 - 当有组合图鉴技能可激活或可升级时显示红点
            /// </summary>
            public void refreshCompositeCatalogRedTip()
            {
                if (_m_compositeCatalogNode == null)
                    return;
                
                bool needShowRed = false;
                
                // 遍历所有组合图鉴
                foreach (var catalogInfo in _m_comp.compositeCatalogInfoList)
                {
                    if (TreasureHuntUtil.compositeCatalogNeedShowRed(catalogInfo))
                    {
                        needShowRed = true;
                        break;
                    }
                }
                
                _m_compositeCatalogNode.setCount(needShowRed ? 1 : 0);
            }

            #endregion

            #region 新区域解锁红点

            /// <summary>
            /// 刷新新区域解锁红点
            /// </summary>
            public void refreshUnlockNewAreaRedTip()
            {
                foreach (TreasureHuntAreaRefObj areaRefObj in GRefdataCoreMgr.instance.treasureHuntAreaRefCore.refList)
                {
                    if(areaRefObj == null)
                        continue;

                    // 区域已解锁 且未读过解锁状态
                    if (areaRefObj.isUnlock() && !(_m_saver?.isAreaUnlockedRead(areaRefObj.area_id) ?? true))
                    {
                        RedTipMgr.instance.setCountByRefRedTipId(areaRefObj.unlock_red_tip_id, 1);
                    }
                    else
                    {
                        RedTipMgr.instance.setCountByRefRedTipId(areaRefObj.unlock_red_tip_id, 0);
                    }
                }
            }

            /// <summary>
            /// 设置已读新区域解锁红点
            /// </summary>
            public void setReadUnlockNewAreaRedTip(long _areaId)
            {
                TreasureHuntAreaRefObj areaRefObj = GRefdataCoreMgr.instance.treasureHuntAreaRefCore.getRef(_areaId);
                if (areaRefObj != null && areaRefObj.isUnlock())
                {
                    _m_saver?.setReadUnlockedAreaId(_areaId);
                    RedTipMgr.instance.setCountByRefRedTipId(areaRefObj.unlock_red_tip_id, 0);
                }
            }

            /// <summary>
            /// 设置已读所有新区域解锁红点
            /// </summary>
            public void setReadAllUnlockNewAreaRedTip()
            {
                foreach (TreasureHuntAreaRefObj areaRefObj in GRefdataCoreMgr.instance.treasureHuntAreaRefCore.refList)
                {
                    if (areaRefObj != null && areaRefObj.isUnlock())
                    {
                        _m_saver?.setReadUnlockedAreaId(areaRefObj.area_id);
                        RedTipMgr.instance.setCountByRefRedTipId(areaRefObj.unlock_red_tip_id, 0);
                    }
                }
            }
            
            #endregion
            
            #region 消息监听

            /// <summary>
            /// 监听LazyCD变化的回调
            /// </summary>
            private void _onLazyCdChg(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _cdId) || 
                   _cdId != GRefdataCoreMgr.instance.npGeneral.treasure_hunt_lazy_cd_id)
                    return;
                
                refreshLazyCdFullRedTip();
            }

            /// <summary>
            /// 监听奇物解锁的回调
            /// </summary>
            private void _onTreasureUnlock(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntGotTreasureInfo
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntGotTreasureInfo))
                    return;
                
                refreshTreasureNotActivateRedTip();
                refreshTreasureSkillCanUpgradeRedTip();
            }

            /// <summary>
            /// 监听奇物等级变化的回调
            /// </summary>
            private void _onTreasureLevelChg(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntGotTreasureInfo
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntGotTreasureInfo))
                    return;
                
                refreshTreasureNotActivateRedTip();
                refreshTreasureSkillCanUpgradeRedTip();
            }

            /// <summary>
            /// 监听奇物产出变化的回调
            /// </summary>
            private void _onTreasureOutputChg(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntTreasureOutputInfo
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntTreasureOutputInfo))
                    return;
                
                refreshOutputCanDrawRedTip();
            }

            /// <summary>
            /// 监听通用道具数量变化的回调
            /// </summary>
            private void _onCommonItemCountChg(params object[] _objs)
            {
                // 参数：_objs[0] 为 ENPItemType, _objs[1] 为 long subId, _objs[2] 为 long count
                if (_objs == null || _objs.Length < 2 || 
                    !(_objs[0] is ENPItemType _itemType) || 
                    !(_objs[1] is long _subId))
                    return;
                
                // 检查是否是能源道具变化
                NPCommonItem premiumEnergyItem = GRefdataCoreMgr.instance.npGeneral.treasure_hunt_premium_energy_common_item;
                NPCommonItem advancedEnergyItem = GRefdataCoreMgr.instance.npGeneral.treasure_hunt_advanced_energy_common_item;
                
                bool isEnergyItem = false;
                if (premiumEnergyItem != null && premiumEnergyItem.itemType == _itemType && premiumEnergyItem.itemId == _subId)
                {
                    isEnergyItem = true;
                }
                else if (advancedEnergyItem != null && advancedEnergyItem.itemType == _itemType && advancedEnergyItem.itemId == _subId)
                {
                    isEnergyItem = true;
                }
                
                if (isEnergyItem)
                {
                    refreshHasEnergyRedTip();
                    return;
                }
                
                // 检查是否是技能点道具变化
                NPCommonItem normalSkillPointItem = GRefdataCoreMgr.instance.npGeneral.treasure_hunt_ore_normal_skill_point_item;
                NPCommonItem advancedSkillPointItem = GRefdataCoreMgr.instance.npGeneral.treasure_hunt_ore_advanced_skill_point_item;
                
                bool isSkillPointItem = false;
                if (normalSkillPointItem != null && normalSkillPointItem.itemType == _itemType && normalSkillPointItem.itemId == _subId)
                {
                    isSkillPointItem = true;
                }
                else if (advancedSkillPointItem != null && advancedSkillPointItem.itemType == _itemType && advancedSkillPointItem.itemId == _subId)
                {
                    isSkillPointItem = true;
                }
                
                if (isSkillPointItem)
                {
                    refreshAllOreCatalogRedTip();
                    return;
                }
                
                // 检查是否是奇物技能点道具变化
                bool isTreasureSkillPointItem = false;
                foreach (var treasureInfo in _m_comp.gotTreasureInfoList)
                {
                    if (treasureInfo == null || treasureInfo.skillInfo == null || treasureInfo.skillInfo.skillPointItem == null)
                        continue;
                    
                    if (treasureInfo.skillInfo.skillPointItem.itemType == _itemType && treasureInfo.skillInfo.skillPointItem.itemId == _subId)
                    {
                        isTreasureSkillPointItem = true;
                        break;
                    }
                }
                
                if (isTreasureSkillPointItem)
                {
                    refreshTreasureSkillCanUpgradeRedTip();
                }
            }

            /// <summary>
            /// 监听矿石解锁的回调
            /// </summary>
            private void _onOreUnlock(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntGotOreInfo
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntGotOreInfo _gotOreInfo))
                    return;
                
                refreshOreCatalogRedTip(_gotOreInfo);
            }

            /// <summary>
            /// 监听矿石技能变化的回调
            /// </summary>
            private void _onOreSkillChg(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntGotOreInfo, _objs[1] 为 bool isNormal
                if (_objs == null || _objs.Length < 2 || 
                    !(_objs[0] is TreasureHuntGotOreInfo _gotOreInfo) ||
                    !(_objs[1] is bool))
                    return;
                
                refreshOreCatalogRedTip(_gotOreInfo);
            }

            /// <summary>
            /// 组合图鉴解锁的回调
            /// </summary>
            private void _onCompositeCatalogUnlock(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntCompositeCatalogInfo
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntCompositeCatalogInfo))
                    return;
                
                refreshCompositeCatalogRedTip();
            }
            
            /// <summary>
            /// 监听组合图鉴变化的回调
            /// </summary>
            private void _onCompositeCatalogChg(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntCompositeCatalogInfo
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntCompositeCatalogInfo))
                    return;
                
                refreshCompositeCatalogRedTip();
            }

            /// <summary>
            /// 监听矿石质量最高记录变化的回调
            /// </summary>
            private void _onOreMaxRecordChg(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntGotOreInfo
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntGotOreInfo _gotOreInfo))
                    return;
                
                // 矿石最高记录变化可能影响矿石图鉴红点（技能解锁条件等）
                refreshOreCatalogRedTip(_gotOreInfo);
            }

            /// <summary>
            /// 监听矿石质量记录奖励领取变化的回调
            /// </summary>
            private void _onOreHadDrawRecordRewardChg(params object[] _objs)
            {
                // 参数：_objs[0] 为 TreasureHuntGotOreInfo
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is TreasureHuntGotOreInfo _gotOreInfo))
                    return;
                
                // 矿石记录奖励领取变化可能影响矿石图鉴红点
                refreshOreCatalogRedTip(_gotOreInfo);
            }

            #endregion

            /// <summary>
            /// 内置存储器
            /// </summary>
            private class TreasureHuntRedTipSaver : _AALBasicSettingInfo
            {
                private const char FieldSplitChar = '#';
                private const char CollectionItemSplitChar = ',';
                
                private List<long> _m_lHasReadUnlockedAreaIdList; // 已读解锁区域id列表
                
                public TreasureHuntRedTipSaver() : base($"{NPPlayer.instance.playerInfo.CID}_treasure_hunt_red_tip_saver")
                {
                    _m_lHasReadUnlockedAreaIdList = new List<long>();
                }

                protected override string _makeSettingStr()
                {
                    StringBuilder sb = new StringBuilder();
                    
                    // 序列化 _m_listReadUnlockedAreaIds
                    if(_m_lHasReadUnlockedAreaIdList != null && _m_lHasReadUnlockedAreaIdList.Count > 0)
                    {
                        sb.Append(string.Join(CollectionItemSplitChar.ToString(), _m_lHasReadUnlockedAreaIdList));
                    }
                    sb.Append(FieldSplitChar);

                    return sb.ToString();
                }

                protected override void _initSettingStr(string _infoStr)
                {
                    if(string.IsNullOrEmpty(_infoStr))
                        return;
                    
                    string[] fieldsArray = _infoStr.Split(FieldSplitChar);
                    if(fieldsArray == null)
                        return;

                    try
                    {
                        // 解析 _m_lHasReadUnlockedAreaIdList 字段
                        if(_m_lHasReadUnlockedAreaIdList == null)
                            _m_lHasReadUnlockedAreaIdList = new List<long>();
                        _m_lHasReadUnlockedAreaIdList.Clear();
                        if(fieldsArray.Length > 0 && !string.IsNullOrEmpty(fieldsArray[0]))
                        {
                            string[] areaIdsArray = fieldsArray[0].Split(CollectionItemSplitChar, StringSplitOptions.RemoveEmptyEntries);
                            if(areaIdsArray != null && areaIdsArray.Length > 0)
                            {
                                for(int i = 0; i < areaIdsArray.Length; i++)
                                {
                                    long areaId = ALCommon.ParseLong(areaIdsArray[i]);
                                    if(areaId > 0)
                                    {
                                        _m_lHasReadUnlockedAreaIdList.Add(areaId);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[TreasureHuntSaver _initSettingStr] fail infoStr: {_infoStr}, error: {e.Message}");
                    }
                }

                #region _m_lHasReadUnlockedAreaIdList

                /// <summary>
                /// 设置已读解锁区域id
                /// </summary>
                /// <param name="_areaId"></param>
                public void setReadUnlockedAreaId(long _areaId)
                {
                    if(_m_lHasReadUnlockedAreaIdList == null)
                        _m_lHasReadUnlockedAreaIdList = new List<long>();

                    if(_m_lHasReadUnlockedAreaIdList.Contains(_areaId))
                        return;

                    _m_lHasReadUnlockedAreaIdList.Add(_areaId);
                    saveSetting();
                }

                /// <summary>
                /// 检查区域解锁是否已读
                /// </summary>
                /// <param name="_areaId"></param>
                /// <returns></returns>
                public bool isAreaUnlockedRead(long _areaId)
                {
                    if(_m_lHasReadUnlockedAreaIdList == null)
                        return false;

                    return _m_lHasReadUnlockedAreaIdList.Contains(_areaId);
                }

                #endregion
            }
        }
    }
}
using System.Collections.Generic;
using System.Reflection;
using ALPackage;
using Common.MarsEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine.Pool;

namespace GOE
{
    /// <summary>
    /// 火星组件红点处理器
    /// </summary>
    public partial class MarsComponent
    {
        public class RedTipDealer
        {
            [NotNull] private readonly MarsComponent _m_comp;
            
            private _ARedTipNode _m_canBuildNode; // 可建造建筑红点
            private _ARedTipNode _m_canHomeCollectNode; // 可建造建筑红点
            private _ARedTipNode _m_canUpgradeBuildingNode; // 可升级建筑红点
            private _ARedTipNode _m_buildingUpgradedOrConstructedNode; // 建筑升级或建造完成红点
            private _ARedTipNode _m_canCollectEnergyNode; // 资源可领取红点
            private bool _m_bIsRefreshCollectEnergyRedTip;//是否正在刷新资源可领取红点(因为资源的数据更新是在客户端使用值时才去计算的, 红点刷新需要去请求值, 所以可能会进入 能源变化->刷新红点->获取能源数据->能源变化->刷新红点 的循环, 这里使用该标记来避免循环)
            private _ARedTipNode _m_peopleReplenishNode;//居民补充红点
            private _ARedTipNode _m_peopleHelpPendingNode; // 居民求助待处理红点
            private _ARedTipNode _m_canDispatchPeopleNode; // 建筑可派遣居民红点
            private _ARedTipNode _m_technologyCanUpgradeNode; // 科技可升级红点
            private _ARedTipNode _m_intelligentControlCanUseNode; // AI智能控制可使用红点
            private _ARedTipNode _m_ExploreLazyCdNode;//探索体力红点
            private _ARedTipNode _m_ExploreEventRewardNode;//探索事件奖励可领取红点
            private _ARedTipNode _m_explorePvPLogNode;//探索PvP日志红点
            private _ARedTipNode _m_exploreSharedMineNode;//探索共享矿红点
            
            private bool _m_bHasCommonItemChgRefreshRedTipTask; // 是否有刷新红点任务
            [NotNull] private ObjectPool<NPCommonItem> _m_commonItemPool = new ObjectPool<NPCommonItem>(() => new NPCommonItem(), null, null, null, false, 1); // 通用道具对象池
            [NotNull] private List<NPCommonItem> _m_lChgItemList = new List<NPCommonItem>(); // 变化的道具列表
            
            
            public RedTipDealer([NotNull] MarsComponent _comp)
            {
                _m_comp = _comp;
            }
            
            public void init()
            {
                _m_canBuildNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_BUILDING_CAN_BUILD);
                _m_canHomeCollectNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_HOME_COLLECT);
                _m_canUpgradeBuildingNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_BUILDING_CAN_UPGRADE);
                _m_buildingUpgradedOrConstructedNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_BUILDING_UPGRADED_OR_CONSTRUCTED);
                _m_canCollectEnergyNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_BUILDING_CAN_COLLECT_ENERGY);
                _m_bIsRefreshCollectEnergyRedTip = false;
                _m_peopleReplenishNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_PEOPLE_REPLENISH);
                _m_peopleHelpPendingNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_PEOPLE_HELP_PENDING);
                _m_canDispatchPeopleNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_BUILDING_CAN_DISPATCH_PEOPLE);
                _m_technologyCanUpgradeNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_TECHNOLOGY_CAN_UPGRADE);
                _m_intelligentControlCanUseNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_INTELLIGENT_CONTROL_CAN_USE);
                _m_ExploreLazyCdNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_EXPLORE_LAZY_CD);
                _m_ExploreEventRewardNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_EXPLORE_EVENT_REWARD);
                _m_explorePvPLogNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_EXPLORE_PVP_LOG);
                _m_exploreSharedMineNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_EXPLORE_SHARED_MINE);
                
                // 初始化时刷新所有红点
                refreshAllRedTip();
                
                // 注册建筑状态变化事件
                _m_comp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;
                _m_comp.buildingSubComponent.onEquipmentUpgraded += _onEquipmentUpgraded;
                _m_comp.buildingSubComponent.onBuildingLevelChg += _onBuildingLevelChg;
                _m_comp.onSettleSlotPeopleLimitChanged += _onBuildingSettleSlotPeopleLimitChg;
                
                // 注册玩家物品变化消息（资源变化时刷新红点）
                WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_STORED_ENERGY_CHG, _onMarsStoredEnergyChg);
                WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
                
                // 注册居民求助变化消息
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_COLLECT, refreshHomeCollectRedTip);
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_HELP_ADD, _onMarsHelpChg);
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_HELP_UPDATE, _onMarsHelpChg);
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_HELP_DEL, _onMarsHelpChg);
                
                // 注册居民数量变化消息
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, _onMarsPeopleNumChg);
                
                // 注册科技变化消息
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _onMarsTechnologyChg);
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_UPGRADING_TECHNOLOGY_CHG, _onMarsUpgradingTechnologyChg);
                
                // 注册智能控制变化消息
                WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_INTELLIGENT_CHG, _onMarsIntelligentControlChg);
                // 注册LazyCD变化消息
                WinMsg.RegisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCdChg);
                // 注册公会情况变化的事件
                WinMsg.RegisterMsgAct(WinMsgType.ON_JOIN_GUILD, refreshExploreSharedMineRedTip);
                WinMsg.RegisterMsgAct(WinMsgType.ON_LEAVE_GUILD, refreshExploreSharedMineRedTip);
                
                // 注册探索事件变化消息
                _m_comp.exploreSubComponent.onEventAdd += _onMarsExploreEventAdd;
                _m_comp.exploreSubComponent.onEventRemove += _onMarsExploreEventRemove;
                _m_comp.exploreSubComponent.onEventChg += _onMarsExploreEventChg;
                
                NPPlayer.instance.playerPropertyMgr.propertyChgDelegate += _onPropertyChgDelegate;
                NPPlayer.instance.playerBuffComp.onChgPlayerBuff += _onBuffChgDelegate;
                if (AccountSettingMgr.instance.unreadMarsBuildingChangeSaver != null)
                    AccountSettingMgr.instance.unreadMarsBuildingChangeSaver.onUnreadMarsBuildingChanged += refreshBuildingUpgradedOrConstructedRedTip;
            }


            public void clear()
            {
                if (AccountSettingMgr.instance.unreadMarsBuildingChangeSaver != null)
                    AccountSettingMgr.instance.unreadMarsBuildingChangeSaver.onUnreadMarsBuildingChanged -= refreshBuildingUpgradedOrConstructedRedTip;
                NPPlayer.instance.playerBuffComp.onChgPlayerBuff -= _onBuffChgDelegate;
                NPPlayer.instance.playerPropertyMgr.propertyChgDelegate += _onPropertyChgDelegate;
                
                // 解除注册玩家物品变化消息
                WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_STORED_ENERGY_CHG, _onMarsStoredEnergyChg);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
                
                // 解除注册居民求助变化消息
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_HELP_ADD, _onMarsHelpChg);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_HELP_UPDATE, _onMarsHelpChg);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_HELP_DEL, _onMarsHelpChg);
                
                // 解除注册居民数量变化消息
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_NUM_CHG, _onMarsPeopleNumChg);
                
                // 解除注册科技变化消息
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _onMarsTechnologyChg);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_UPGRADING_TECHNOLOGY_CHG, _onMarsUpgradingTechnologyChg);
                
                // 解除注册智能控制变化消息
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_INTELLIGENT_CHG, _onMarsIntelligentControlChg);
                
                // 解除注册LazyCD变化消息
                WinMsg.UnregisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCdChg);
                // 解除注册公会情况变化的事件
                WinMsg.UnregisterMsgAct(WinMsgType.ON_JOIN_GUILD, refreshExploreSharedMineRedTip);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_LEAVE_GUILD, refreshExploreSharedMineRedTip);
                
                // 解除注册探索事件变化消息
                _m_comp.exploreSubComponent.onEventAdd -= _onMarsExploreEventAdd;
                _m_comp.exploreSubComponent.onEventRemove -= _onMarsExploreEventRemove;
                _m_comp.exploreSubComponent.onEventChg -= _onMarsExploreEventChg;
                
                // 解除注册建筑状态变化事件
                _m_comp.buildingSubComponent.onBuildingStateChg -= _onBuildingStateChg;
                _m_comp.buildingSubComponent.onEquipmentUpgraded -= _onEquipmentUpgraded;
                _m_comp.buildingSubComponent.onBuildingLevelChg -= _onBuildingLevelChg;
                _m_comp.onSettleSlotPeopleLimitChanged -= _onBuildingSettleSlotPeopleLimitChg;
                
                // 清空所有节点引用
                _m_canBuildNode = null;
                _m_canHomeCollectNode = null;
                _m_canUpgradeBuildingNode = null;
                _m_buildingUpgradedOrConstructedNode = null;
                _m_canCollectEnergyNode = null;
                _m_bIsRefreshCollectEnergyRedTip = false;
                _m_peopleReplenishNode = null;
                _m_peopleHelpPendingNode = null;
                _m_canDispatchPeopleNode = null;
                _m_technologyCanUpgradeNode = null;
                _m_intelligentControlCanUseNode = null;
                _m_ExploreLazyCdNode = null;
                _m_ExploreEventRewardNode = null;
                    
                _m_bHasCommonItemChgRefreshRedTipTask = false;
                _pushBackAllCommonItem();
                _m_commonItemPool.Clear();
            }

            /// <summary>
            /// 刷新可建造建筑红点
            /// </summary>
            public void refreshCanBuildRedTip()
            {
                if (_m_canBuildNode == null)
                    return;
                
                bool hasCanBuild = false;
                // 有空闲建造队列的情况下再判断是否可建造
                if(_m_comp.buildingSubComponent.hasLeisureQueue)
                {
	                foreach (MarsBuildingInfo buildingInfo in _m_comp.buildingSubComponent._getBuildingInfos())
	                {
	                    // 未建造状态且满足建造条件（包括资源检查）
	                    if (buildingInfo.state == MarsBuildingInfo.StateType.Unbuilt && 
	                        buildingInfo.checkCanBuild(true))
	                    {
	                        hasCanBuild = true;
	                        break;
	                    }
	                }
                }
                
                _m_canBuildNode.setCount(hasCanBuild ? 1 : 0);
            }

            /// <summary>
            /// 刷新可升级建筑红点
            /// </summary>
            public void refreshCanUpgradeBuildingRedTip()
            {
                if (_m_canUpgradeBuildingNode == null)
                    return;
                
                bool hasCanUpgradeBuilding = false;
                // 有空闲建造队列的情况下再判断是否可升级
                if(_m_comp.buildingSubComponent.hasLeisureQueue)
                {
	                foreach (MarsBuildingInfo buildingInfo in _m_comp.buildingSubComponent._getBuildingInfos())
	                {
	                    // 只有正常状态的建筑才能升级
	                    if (buildingInfo.state == MarsBuildingInfo.StateType.Normal && buildingInfo.equipmentData.equipmentLevelProgressIsComplete && buildingInfo.checkCanUpgrade(true))
	                    {
	                        hasCanUpgradeBuilding = true;
	                        break;
	                    }
	                }
                }
                
                _m_canUpgradeBuildingNode.setCount(hasCanUpgradeBuilding ? 1 : 0);
            }

            /// <summary>
            /// 刷新火星基地采集红点
            /// </summary>
            public void refreshHomeCollectRedTip()
            {
                if(null == _m_canHomeCollectNode)
                    return;
                _m_canHomeCollectNode.setCount(NPPlayer.instance.marsComp.buildingSubComponent.isCollectRed ? 1 : 0);
            }

            /// <summary>
            /// 刷新资源可领取红点
            /// </summary>
            public void refreshCanCollectEnergyRedTip()
            {
                if (_m_canCollectEnergyNode == null || _m_bIsRefreshCollectEnergyRedTip)
                    return;

                _m_bIsRefreshCollectEnergyRedTip = true;
                bool hasCanCollectEnergy = false;
                foreach (MarsBuildingInfo buildingInfo in _m_comp.buildingSubComponent._getBuildingInfos())
                {
                    // 检查能源建筑是否有可领取的能源（储存的能源占比 >= GRefdataCoreMgr.instance.npGeneral.mars_can_get_energy_progress_threshold）
                    float energyProgress = buildingInfo.energyProperty.maxStorage > 0 ? (float)buildingInfo.energyProperty.storedEnergy / buildingInfo.energyProperty.maxStorage : 0f;
                    if(buildingInfo.refObj.building_type == EMarsBuildingType.ENERGY && 
                       energyProgress >= GRefdataCoreMgr.instance.npGeneral.mars_can_get_energy_progress_threshold)
                    {
                        hasCanCollectEnergy = true;
                        break;
                    }
                }
                
                _m_canCollectEnergyNode.setCount(hasCanCollectEnergy ? 1 : 0);
                _m_bIsRefreshCollectEnergyRedTip = false;
            }
            
            /// <summary>

            /// 刷新建筑可派遣居民红点
            /// </summary>
            public void refreshCanDispatchPeopleRedTip()
            {
                if (_m_canDispatchPeopleNode == null)
                    return;

                // 检查是否有空闲居民
                long idlePeopleNum = _m_comp.peopleSubComponent.idlePeopleNum;
                if (idlePeopleNum <= 0)
                {
                    _m_canDispatchPeopleNode.setCount(0);
                    return;
                }

                // 遍历所有建筑，检查是否有可派遣居民的建筑
                bool hasCanDispatch = false;
                foreach (MarsBuildingInfo buildingInfo in _m_comp.buildingSubComponent._getBuildingInfos())
                {
                    if (!buildingInfo.settleSlotData.isValid())
                        continue;
                    
                    // 派遣人数未达到上限
                    if (!buildingInfo.settleSlotData.isMax)
                    {
                        hasCanDispatch = true;
                        break;
                    }
                }

                _m_canDispatchPeopleNode.setCount(hasCanDispatch ? 1 : 0);
            }
            
            /// <summary>
            /// 刷新建筑红点(将多种类型红点刷新合并为一次遍历)
            /// </summary>
            /// <param name="_needRefreshCanBuildRed">是否刷新可建造红点</param>
            /// <param name="_needRefreshCanUpgradeBuildingRed">是否刷新可升级建筑红点</param>
            /// <param name="_needRefreshCanCollectEnergyRed">是否刷新资源可领取红点</param>
            /// <param name="_needRefreshCanDispatchPeopleRed">是否刷新建筑可派遣居民红点</param>
            public void refreshBuildingRedTip(bool _needRefreshCanBuildRed, bool _needRefreshCanUpgradeBuildingRed, bool _needRefreshCanCollectEnergyRed, bool _needRefreshCanDispatchPeopleRed)
            {
                _needRefreshCanBuildRed = _needRefreshCanBuildRed && _m_canBuildNode != null;
                _needRefreshCanUpgradeBuildingRed = _needRefreshCanUpgradeBuildingRed && _m_canUpgradeBuildingNode != null;
                _needRefreshCanCollectEnergyRed = _needRefreshCanCollectEnergyRed && _m_canCollectEnergyNode != null && !_m_bIsRefreshCollectEnergyRedTip;
                if (_needRefreshCanCollectEnergyRed)
                    _m_bIsRefreshCollectEnergyRedTip = true;
                _needRefreshCanDispatchPeopleRed = _needRefreshCanDispatchPeopleRed && _m_canDispatchPeopleNode != null;

                bool buildingHasLeisureQueue = _m_comp.buildingSubComponent.hasLeisureQueue;//是否有空闲建造队列

                bool hasCanBuildBuilding = false;//是否有可建造建筑
                bool hasCanUpgradeBuilding = false;//是否有可升级建筑
                bool hasCanCollectEnergy = false;//是否有可领取能源
                bool hasCanDispatchPeople = false;//是否有可派遣居民的建筑
                foreach (MarsBuildingInfo buildingInfo in _m_comp.buildingSubComponent._getBuildingInfos())
                {
                    // 若需要刷新可建造红点，且还没有找到可建造建筑
                    if (_needRefreshCanBuildRed && !hasCanBuildBuilding)
                    {
                        // 未建造状态且满足建造条件（包括资源检查）
                        if (buildingHasLeisureQueue && buildingInfo.state == MarsBuildingInfo.StateType.Unbuilt && buildingInfo.checkCanBuild(true))
                        {
                            hasCanBuildBuilding = true;
                        }
                    }

                    // 若需要刷新可升级建筑红点，且还没有找到可升级建筑
                    if (_needRefreshCanUpgradeBuildingRed && !hasCanUpgradeBuilding)
                    {
                        // 只有正常状态的建筑才能升级
                        if (buildingHasLeisureQueue && buildingInfo.state == MarsBuildingInfo.StateType.Normal && buildingInfo.equipmentData.equipmentLevelProgressIsComplete && buildingInfo.checkCanUpgrade(true))
                        {
                            hasCanUpgradeBuilding = true;
                        }
                    }
                    
                    // 若需要刷新资源可领取红点，且还没有找到可领取能源
                    if (_needRefreshCanCollectEnergyRed && !hasCanCollectEnergy)
                    {
                        // 检查能源建筑是否有可领取的能源（储存的能源 >= GRefdataCoreMgr.instance.npGeneral.mars_can_get_energy_progress_threshold）
                        if(buildingInfo.refObj.building_type == EMarsBuildingType.ENERGY && 
                           buildingInfo.energyProperty.storedEnergy >= GRefdataCoreMgr.instance.npGeneral.mars_can_get_energy_progress_threshold)
                        {
                            hasCanCollectEnergy = true;
                        }
                    }
                    
                    // 若需要刷新可派遣居民红点，且还没有找到可派遣建筑
                    if (_needRefreshCanDispatchPeopleRed && !hasCanDispatchPeople)
                    {
                        if (buildingInfo.settleSlotData.isValid() && !buildingInfo.settleSlotData.isMax && _m_comp.peopleSubComponent.idlePeopleNum > 0)
                        {
                            hasCanDispatchPeople = true;
                        }
                    }
                }

                // 若需要刷新对应红点，则设置红点状态
                if (_needRefreshCanBuildRed)
                    _m_canBuildNode.setCount(hasCanBuildBuilding ? 1 : 0);
                if (_needRefreshCanUpgradeBuildingRed)
                    _m_canUpgradeBuildingNode.setCount(hasCanUpgradeBuilding ? 1 : 0);
                if (_needRefreshCanCollectEnergyRed)
                {
                    _m_canCollectEnergyNode.setCount(hasCanCollectEnergy ? 1 : 0);
                    _m_bIsRefreshCollectEnergyRedTip = false;
                }
                if (_needRefreshCanDispatchPeopleRed)
                    _m_canDispatchPeopleNode.setCount(hasCanDispatchPeople ? 1 : 0);
            }

            /// <summary>
            /// 刷新居民补充红点
            /// </summary>
            public void refreshPeopleReplenishRedTip()
            {
                if (_m_peopleReplenishNode == null)
                    return;

                EMarsResidentReplenishState state = MarsUtil.getMarsResidentReplenishState();
                // 在空闲或补充完成状态下显示红点
                _m_peopleReplenishNode.setCount((state is EMarsResidentReplenishState.ReplenishComplete) ? 1 : 0);
            }

            /// <summary>
            /// 刷新居民求助待处理红点
            /// </summary>
            public void refreshPeopleHelpPendingRedTip()
            {
                if (_m_peopleHelpPendingNode == null)
                    return;

                var helpList = _m_comp.peopleSubComponent.helpList;
                if (helpList == null)
                {
                    _m_peopleHelpPendingNode.setCount(0);
                    return;
                }

                // 检查是否有待处理的求助
                bool hasPendingHelp = false;
                foreach (var help in helpList)
                {
                    if (help != null && help.state == EMarsPopularWillHelpState.WAIT_HANDLE)
                    {
                        hasPendingHelp = true;
                        break;
                    }
                }

                _m_peopleHelpPendingNode.setCount(hasPendingHelp ? 1 : 0);
            }

            /// <summary>
            /// 刷新科技可升级红点
            /// </summary>
            public void refreshTechnologyCanUpgradeRedTip()
            {
                if (_m_technologyCanUpgradeNode == null || _m_comp.technologySubComponent.technologyInfoList == null)
                    return;

                // 遍历所有科技，检查是否有可升级的科技
                bool hasCanUpgradeTech = false;
                foreach (MarsTechnologyInfo techInfo in _m_comp.technologySubComponent.technologyInfoList)
                {
                    if (techInfo == null)
                        continue;

                    if (techInfo.checkCanUpgrade(false))
                    {
                        hasCanUpgradeTech = true;
                        break;
                    }
                }

                _m_technologyCanUpgradeNode.setCount(hasCanUpgradeTech ? 1 : 0);
            }

            /// <summary>
            /// 刷新AI智能控制可使用红点
            /// </summary>
            public void refreshIntelligentControlCanUseRedTip()
            {
                if (_m_intelligentControlCanUseNode == null)
                    return;

                // 遍历所有智能控制，检查是否有可使用的
                bool hasCanUseControl = false;
                foreach (var intelligentControlRefObj in GRefdataCoreMgr.instance.marsIntelligentControlRefCore.refList)
                {
                    if(intelligentControlRefObj == null)
                        continue;

                    if (intelligentControlRefObj.getState(false) == EMarsIntelligentControlState.CAN_USE)
                    {
                        hasCanUseControl = true;
                        break;
                    }
                }

                _m_intelligentControlCanUseNode.setCount(hasCanUseControl ? 1 : 0);
            }
            
            /// <summary>
            /// 刷新探索体力红点
            /// </summary>
            public void refreshExploreLazyCdRedTip()
            {
                if (_m_ExploreLazyCdNode == null)
                    return;

                PlayerLazyCDInfo cdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(GRefdataCoreMgr.instance.npGeneral.mars_explore_cd);
                bool needShowRedTip = cdInfo != null && cdInfo.getCount() >= cdInfo.MaxCount / 2;//超过体力上限50%时显示红点
                _m_ExploreLazyCdNode.setCount(needShowRedTip ? 1 : 0);
            }

            /// <summary>
            /// 刷新探索事件奖励可领取红点
            /// </summary>
            public void refreshExploreEventRewardRedTip()
            {
                if (_m_ExploreEventRewardNode == null)
                    return;

                // 检查是否有已完成且可领取奖励的事件
                int hasCanCollectRewardCount = 0;
                _m_comp.exploreSubComponent.doEachEvent(
                    (_eventInfo) =>
                    {
                        if (_eventInfo != null && _eventInfo.isDone)
                        {
                            hasCanCollectRewardCount++;
                        }
                    });

                _m_ExploreEventRewardNode.setCount(hasCanCollectRewardCount);
            }
            
            /// <summary>
            /// 刷新建筑升级或建造完成红点
            /// </summary>
            public void refreshBuildingUpgradedOrConstructedRedTip()
            {
                if (_m_buildingUpgradedOrConstructedNode == null)
                    return;

                // 检查是否有未读的建筑ID
                HashSet<long> unreadBuildingIds = AccountSettingMgr.instance.unreadMarsBuildingChangeSaver.getUnreadBuildingIds();
                int unreadCount = unreadBuildingIds != null ? unreadBuildingIds.Count : 0;

                _m_buildingUpgradedOrConstructedNode.setCount(unreadCount);
            }
            
            /// <summary>
            /// 刷新探索PvP日志红点
            /// </summary>
            public void refreshExplorePvPLogRedTip()
            {
                if (_m_explorePvPLogNode == null)
                    return;

                long readLogTime = AccountSettingMgr.instance.accountSetting.readMarsExplorePvPLogTime;
                long newestLogTime = _m_comp.exploreSubComponent.newestLogTime;
                _m_explorePvPLogNode.setCount(newestLogTime > readLogTime ? 1 : 0);
            }

            /// <summary>
            /// 刷新探索共享矿红点
            /// </summary>
            public void refreshExploreSharedMineRedTip()
            {
                if (_m_exploreSharedMineNode == null)
                    return;
                
                NewestSharedMineData readSharedMineId = AccountSettingMgr.instance.accountSetting.readMarsExploreSharedMineData;
                NewestSharedMineData newestSharedMineData = _m_comp.exploreSubComponent.newestSharedMineData;
                long myGuildId = NPPlayer.instance?.guildComp?.guildInfo?.guildId ?? 0;
                
                bool needShowRedTip = false;
                // 最新的矿的公会 ID 与自己公会 ID 相同才有效
                if (myGuildId > 0 && myGuildId == newestSharedMineData.guildId &&
                    // 已读的矿的工会 id 如果和最新矿的工会 id 不同认为未读，或者已读的矿的 ID 小于最新矿的 ID 认为未读
                    (readSharedMineId.guildId != myGuildId || newestSharedMineData.newestId > readSharedMineId.newestId))
                    needShowRedTip = true;
                
                _m_exploreSharedMineNode.setCount(needShowRedTip ? 1 : 0);
            }

            /// <summary>
            /// 刷新所有红点
            /// </summary>
            public void refreshAllRedTip()
            {
                refreshBuildingRedTip(true, true, true, true);
                refreshPeopleReplenishRedTip();
                refreshPeopleHelpPendingRedTip();
                refreshTechnologyCanUpgradeRedTip();
                refreshIntelligentControlCanUseRedTip();
                refreshExploreLazyCdRedTip();
                refreshExploreEventRewardRedTip();
                refreshBuildingUpgradedOrConstructedRedTip();
                refreshExplorePvPLogRedTip();
                refreshExploreSharedMineRedTip();
            }

            #region 消息处理

            /// <summary>
            /// 建筑状态变化事件处理
            /// </summary>
            private void _onBuildingStateChg(long _buildingId, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
            {
                refreshBuildingRedTip(true, true, false, false);
            }


            /// <summary>
            /// 部件升级事件处理(部件升级可能会影响建筑各个红点, 不刷新派遣红点是因为派遣人数上限变化有独立推送)
            /// </summary>
            private void _onEquipmentUpgraded(long _buildingId, long _equipmentId)
            {
                refreshBuildingRedTip(true, true, true, false);
            }

            /// <summary>
            /// 建筑等级变化事件处理
            /// </summary>
            private void _onBuildingLevelChg(long _buildingId, long _oldValue)
            {
                refreshBuildingRedTip(true, true, false, false);
            }


            /// <summary>
            /// 通用道具数量变化
            /// </summary>
            /// <param name="_objs">参数: _objs[0] 为 ENPItemType, _objs[1] 为 long(itemId)</param>
            private void _onCommonItemChg(params object[] _objs)
            {
                if (_objs == null || _objs.Length < 2 || !(_objs[0] is ENPItemType itemType) || !(_objs[1] is long subId))
                    return;

                NPCommonItem chgCommonItem = _m_commonItemPool.Get();
                if (chgCommonItem == null)
                    return;
                
                chgCommonItem.itemType = itemType;
                chgCommonItem.itemId = subId;
                _m_lChgItemList.Add(chgCommonItem);
                
                _createCommonItemChgRefreshRedTipTask();
            }


            /// <summary>
            /// 创建CommonItem变化刷新红点任务
            /// </summary>
            private void _createCommonItemChgRefreshRedTipTask()
            {
                if (_m_bHasCommonItemChgRefreshRedTipTask)
                    return;

                _m_bHasCommonItemChgRefreshRedTipTask = true;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (!_m_bHasCommonItemChgRefreshRedTipTask)
                        return;
                    
                    _m_bHasCommonItemChgRefreshRedTipTask = false;
                    
                    _refreshCommonItemChgRedTipTask(); // 刷新物品变化相关红点
                    
                }, 1f); // 延迟一会刷新红点，防止频繁刷新
            }


            /// <summary>
            /// 刷新物品变化相关红点任务
            /// </summary>
            private void _refreshCommonItemChgRedTipTask()
            {
                if (_m_lChgItemList.Count <= 0)
                    return;
                
                // 根据_m_lChgItemList刷新红点
                bool needRefreshCanBuildRed = false;
                bool needRefreshCanUpgradeBuildingRed = false;
                bool needRefreshTechnologyCanUpgradeRed = false;
                bool needRefreshIntelligentControlCanUseRed = false;
                
                foreach (var chgItem in _m_lChgItemList)
                {
                    if(chgItem == null)
                        continue;
                    
                    // 若循环中判断到所有红点都需要刷新，则提前退出循环
                    if(needRefreshCanBuildRed && needRefreshCanUpgradeBuildingRed && needRefreshTechnologyCanUpgradeRed && needRefreshIntelligentControlCanUseRed)
                        break;
                    
                    foreach (MarsBuildingInfo buildingInfo in _m_comp.buildingSubComponent._getBuildingInfos())
                    {
                        if (!needRefreshCanBuildRed)
                        {
                            if (buildingInfo.refObj.build_cost_list.getItem(chgItem.itemType, chgItem.itemId) != null)
                            {
                                needRefreshCanBuildRed = true;
                            }
                        }

                        if (!needRefreshCanUpgradeBuildingRed)
                        {
                            if (buildingInfo.levelData.refObj != null &&
                                buildingInfo.levelData.refObj.upgrade_cost_list.getItem(chgItem.itemType, chgItem.itemId) != null)
                            {
                                needRefreshCanUpgradeBuildingRed = true;
                            }
                        }
                    }
                    
                    // 检查科技升级消耗
                    if (!needRefreshTechnologyCanUpgradeRed && _m_comp.technologySubComponent.technologyInfoList != null)
                    {
                        foreach (MarsTechnologyInfo techInfo in _m_comp.technologySubComponent.technologyInfoList)
                        {
                            if (techInfo == null || techInfo.upgradeConsumeList == null)
                                continue;
                            
                            if (techInfo.upgradeConsumeList.getItem(chgItem.itemType, chgItem.itemId) != null)
                            {
                                needRefreshTechnologyCanUpgradeRed = true;
                                break;
                            }
                        }
                    }
                    
                    // 检查智能控制消耗（满意值）
                    if (!needRefreshIntelligentControlCanUseRed)
                    {
                        NPCommonItem satisfactionValueItem = GRefdataCoreMgr.instance.npGeneral.mars_satisfaction_value_common_item;
                        if (satisfactionValueItem != null && chgItem.itemType == satisfactionValueItem.itemType && chgItem.itemId == satisfactionValueItem.itemId)
                        {
                            needRefreshIntelligentControlCanUseRed = true;
                        }
                    }
                }
                _pushBackAllCommonItem();
                    
                refreshBuildingRedTip(needRefreshCanBuildRed, needRefreshCanUpgradeBuildingRed, false, false);
                
                // 刷新科技红点
                if (needRefreshTechnologyCanUpgradeRed)
                    refreshTechnologyCanUpgradeRedTip();
                
                // 刷新智能控制红点
                if (needRefreshIntelligentControlCanUseRed)
                    refreshIntelligentControlCanUseRedTip();
            }


            /// <summary>
            /// 放回所有通用道具对象
            /// </summary>
            private void _pushBackAllCommonItem()
            {
                foreach (var item in _m_lChgItemList)
                {
                    _m_commonItemPool.Release(item);
                }
                _m_lChgItemList.Clear();
            }

            /// <summary>
            /// 当火星储存能源变化
            /// </summary>
            private void _onMarsStoredEnergyChg()
            {
                refreshCanCollectEnergyRedTip();
            }

            /// <summary>
            /// 当节点变化时
            /// </summary>
            private void _onNodeChg()
            {
                _AALQueueBaseNode lastNode = QueueMgr.instance._lastNode;
                // 当最后一个节点是 _AGNodeMainSub(主界面几个主要功能节点的基类), 或 火星节点存在时，刷新相关红点
                if (lastNode != null && lastNode is _AGNodeMainSub || QueueMgr.instance.findLastNode(UINodeTagConst.C_NODE_MARS) != null)
                {
                    //刷新主基地采集红点
                    refreshHomeCollectRedTip();

                    // 刷新下资源红点
                    refreshCanCollectEnergyRedTip();

                    // 刷新居民补充红点
                    refreshPeopleReplenishRedTip();
                    
                    // 刷新智能控制红点
                    refreshIntelligentControlCanUseRedTip();
                    
                    // 刷新探索体力红点
                    refreshExploreLazyCdRedTip();
                }
            }

            /// <summary>
            /// 当居民求助变化时（新增/更新/删除）
            /// </summary>
            private void _onMarsHelpChg()
            {
                refreshPeopleHelpPendingRedTip();
            }

            /// <summary>
            /// 当建筑可派遣居民上限变化时
            /// </summary>
            private void _onBuildingSettleSlotPeopleLimitChg(long _buildingId)
            {
                refreshCanDispatchPeopleRedTip();
            }

            /// <summary>
            /// 当火星居民数量变化时
            /// </summary>
            private void _onMarsPeopleNumChg()
            {
                refreshCanDispatchPeopleRedTip();
            }

            /// <summary>
            /// 当火星科技变化时
            /// </summary>
            private void _onMarsTechnologyChg()
            {
                refreshTechnologyCanUpgradeRedTip();
            }

            /// <summary>
            /// 当火星升级中科技变化时
            /// </summary>
            private void _onMarsUpgradingTechnologyChg()
            {
                // refreshTechnologyCanUpgradeRedTip();
            }

            /// <summary>
            /// 当火星智能控制变化时
            /// </summary>
            private void _onMarsIntelligentControlChg()
            {
                refreshIntelligentControlCanUseRedTip();
            }
            
            /// <summary>
            /// 当玩家属性变化时
            /// </summary>
            /// <param name="_type"></param>
            /// <param name="_oldValue"></param>
            /// <param name="_newValue"></param>
            private void _onPropertyChgDelegate(ENPPlayerPropertyType _type, long _oldValue, long _newValue)
            {
                // 若变化的是火星建筑队列数量属性，则刷新建筑红点
                if (_type == ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM)
                    refreshBuildingRedTip(true, true, false, false);
            }
            private void _onBuffChgDelegate(NPPlayerBuffInfo _buffInfo, int _layer, long _timeMs)
            {
                if (_buffInfo == null)
                    return;
            
                // 若变化的是火星建筑队列临时解锁Buff，则刷新建筑红点
                if (_buffInfo.buffId == GRefdataCoreMgr.instance.npGeneral.mars_temp_building_queue_gain_buff_item.subId)
                    refreshBuildingRedTip(true, true, false, false);
            }
            
            /// <summary>
            /// 当LazyCD变化时
            /// </summary>
            /// <param name="_objs">参数: _objs[0] 为 long(cdId)</param>
            private void _onLazyCdChg(params object[] _objs)
            {
                if (_objs == null || _objs.Length < 1 || !(_objs[0] is long _cdId))
                    return;
             
                // 若变化的是火星探索体力CD，则刷新探索体力红点
                if(_cdId == GRefdataCoreMgr.instance.npGeneral.mars_explore_cd)
                {
                    refreshExploreLazyCdRedTip();
                }
            }

            /// <summary>
            /// 当火星探索事件添加时
            /// </summary>
            private void _onMarsExploreEventAdd(_AMarsExploreEventInfo _eventInfo)
            {
                refreshExploreEventRewardRedTip();
            }

            /// <summary>
            /// 当火星探索事件移除时
            /// </summary>
            private void _onMarsExploreEventRemove(_AMarsExploreEventInfo _eventInfo)
            {
                refreshExploreEventRewardRedTip();
            }

            /// <summary>
            /// 当火星探索事件变化时
            /// </summary>
            private void _onMarsExploreEventChg(_AMarsExploreEventInfo _eventInfo, bool _newEventStacked)
            {
                refreshExploreEventRewardRedTip();
            }

            #endregion
        }
    }
}

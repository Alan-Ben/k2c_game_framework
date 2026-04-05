using System;
using System.Collections.Generic;
using Common.MarsEnum;
using Common.MarsObj;
using CommonEnum;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class MarsUtil
    {
        
        
        /// <summary>
        /// 对求助排序 - 未处理的排后面, id从小到大
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        public static int sortMarsHelp_UnDealPostpone_IdL2S(_IMarsPeopleWillHelp _a, _IMarsPeopleWillHelp _b)
        {
            if(_b == null) return -1;
            if(_a == null) return 1;
            if (ReferenceEquals(_a, _b)) return 0;

            if (_a.state != _b.state)
                return _a.state.CompareTo(_b.state);//因为枚举值是按顺序定义的，未处理的值最大，所以直接比较枚举值即可
            
            return _a.instanceId.CompareTo(_b.instanceId);//再按实例id排序
        }

        /// <summary>
        /// 对求助排序 - 未处理的排前面, id从大到小
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        public static int sortMarsHelp_DealPostpone_IdS2L(_IMarsPeopleWillHelp _a, _IMarsPeopleWillHelp _b)
        {
            if(_b == null) return -1;
            if(_a == null) return 1;
            if (ReferenceEquals(_a, _b)) return 0;

            if (_a.state != _b.state)
                return -_a.state.CompareTo(_b.state);//因为枚举值是按顺序定义的，未处理的值最大，所以直接比较枚举值即可
            
            return -_a.instanceId.CompareTo(_b.instanceId);//再按实例id排序
        }
        /// <summary>
        /// 计算加速所需水晶数
        /// </summary>
        /// <param name="_remainMs"></param>
        /// <returns></returns>
        public static int calculateGemCostForSpeedUpMs(long _remainMs)
        {
            if (_remainMs <= 0)
                return 0;
            
            return (int) Math.Ceiling(1.0f * _remainMs / (GRefdataCoreMgr.instance.npGeneral.mars_building_sec_to_diamond_ratio * 1000f));
        }
        
        public static bool checkConditionIdListEnable(List<long> _conditionIdList)
        {
            if (_conditionIdList == null)
                return true;
            
            foreach (long conditionId in _conditionIdList)
            {
                MarsBuildingConditionRefObj conditionRef = GRefdataCoreMgr.instance.marsBuildingConditionRefCore.getRef(conditionId);
                if (conditionRef != null && !conditionRef.condition.IsEnable(null))
                    return false;
            }

            return true;
        }
        [NotNull, ItemNotNull]
        public static List<_IConditionDescShow> generateConditionShowList(List<long> _conditionIdList, List<NPCommonCostItem> _buildCostList)
        {
            List<_IConditionDescShow> showList = new List<_IConditionDescShow>();
            if (_conditionIdList != null)
            {
                foreach (long conditionId in _conditionIdList)
                {
                    MarsBuildingConditionRefObj conditionRef = GRefdataCoreMgr.instance.marsBuildingConditionRefCore.getRef(conditionId);
                    if (conditionRef != null)
                        showList.Add(conditionRef);
                }
            }

            if (_buildCostList != null)
            {
                foreach (NPCommonCostItem costItem in _buildCostList)
                {
                    if (costItem == null)
                        continue;
                    
                    showList.Add(costItem);
                }
            }

            return showList;
        }
        public static void showEnergyCollectTip(Vector3 _position, long _count, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID, long _particleId = 0)
        {
            Vector3 screenPos = CameraController.instance.controlCamera.WorldToScreenPoint(_position);
            NPGUIAddSceneCenterTip.instance.showIconTextTip(GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long) ECurrency.MARS_ENERGY), 
                TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _count), _tipId, _wnd => _wnd?.setTipPos(screenPos));
            GCommon.getParticleUIPosByScreenPos(screenPos, _uiPos =>
                GCommon.showItemParticle(ENPItemType.CURRENCY, (long)ECurrency.MARS_ENERGY, _count, _uiPos, _particleId));
        }
        
        /// <summary>
        /// 获取居民补充状态
        /// </summary>
        /// <returns></returns>
        public static EMarsResidentReplenishState getMarsResidentReplenishState()
        {
            Mars_PeopleImmigrant immigrantInfo = NPPlayer.instance.marsComp.peopleSubComponent.getImmigrantInfo();
            if (immigrantInfo == null)
            {
                // 没有移民数据时, 判断今日移民次数是否达到上限
                if (NPPlayer.instance.marsComp.peopleSubComponent.todayImmigrantCount >=
                    GRefdataCoreMgr.instance.npGeneral.mars_immigration_daily_max_num)
                {
                    // 达到上限
                    return EMarsResidentReplenishState.ReplenishCountLimit;
                }
                else
                {
                    // 未达到上限
                    return EMarsResidentReplenishState.Idle;
                }
            }
            else
            {
                // 有移民数据时, 判断当前时间是否已经超过结束时间
                if (FpsAndPingMgr.instance.serverTimeTag >= immigrantInfo.getEndMs())
                {
                    // 当前时间已经超过结束时间
                    return EMarsResidentReplenishState.ReplenishComplete;
                }
                else
                {
                    // 当前时间未超过结束时间
                    return EMarsResidentReplenishState.Replenishing;
                }
            }
        }

        /// <summary>
        /// 获取可以建造或升级的居住舱
        /// </summary>
        /// <returns></returns>
        public static MarsBuildingInfo getCanBuildOrUpdateLivingBuilding()
        {
            List<MarsBuildingInfo> buildingList = NPPlayer.instance.marsComp.buildingSubComponent._getBuildingInfos();
            MarsBuildingInfo targetLivingBuilding = null;//聚焦的居住舱
            foreach (var buildingInfo in buildingList)
            {
                // 非居住仓, 跳过
                if(buildingInfo.refObj.building_type != EMarsBuildingType.LIVING)
                    continue;

                // 若果是未建造状态, 直接前往该建筑建造
                if (buildingInfo.state == MarsBuildingInfo.StateType.Unbuilt && buildingInfo.checkCanShowBuildBtn())
                {
                    targetLivingBuilding = buildingInfo;
                    break;
                }
                
                // 记录等级最低的居住舱
                if(targetLivingBuilding == null || buildingInfo.level < targetLivingBuilding.level)
                    targetLivingBuilding = buildingInfo;
            }
            
            return targetLivingBuilding;
        }
        
        public static List<NPCommonCostItem> getMarsExploreEventRewards(int _exploreLevel, EQuality _quality)
        {
            MarsExploreLvlRefObj exploreLvlRefObj = GRefdataCoreMgr.instance.marsExploreLvlRefCore.getRef(_exploreLevel);
            if (exploreLvlRefObj == null)
                return new List<NPCommonCostItem>(0);

            int index = exploreLvlRefObj.refresh_event_quality_list.FindIndex(_data => _data.quality == _quality);
            if (index < 0 || index >= exploreLvlRefObj.battle_event_quality_reward_list.Count)
                return new List<NPCommonCostItem>(0);
            
            return exploreLvlRefObj.battle_event_quality_reward_list[index].costItemList;
        }
        public static long getMarsExploreEventSoliderPower(int _exploreLevel, EQuality _quality)
        {
            MarsExploreLvlRefObj exploreLvlRefObj = GRefdataCoreMgr.instance.marsExploreLvlRefCore.getRef(_exploreLevel);
            if (exploreLvlRefObj == null)
                return 0;

            int index = exploreLvlRefObj.refresh_event_quality_list.FindIndex(_data => _data.quality == _quality);
            if (index < 0 || index >= exploreLvlRefObj.battle_event_quality_solider_power_list.Count)
                return 0;
            
            return exploreLvlRefObj.battle_event_quality_solider_power_list[index];
        }
        public static long getMarsExploreEventSoliderNum(int _exploreLevel, EQuality _quality)
        {
            MarsExploreLvlRefObj exploreLvlRefObj = GRefdataCoreMgr.instance.marsExploreLvlRefCore.getRef(_exploreLevel);
            if (exploreLvlRefObj == null)
                return 0;

            int index = exploreLvlRefObj.refresh_event_quality_list.FindIndex(_data => _data.quality == _quality);
            if (index < 0 || index >= exploreLvlRefObj.battle_event_quality_solider_num_list.Count)
                return 0;

            return exploreLvlRefObj.battle_event_quality_solider_num_list[index];
        }

        #region 探索队伍

        /// <summary>
        /// 计算火星探索队伍最大士兵数
        /// </summary>
        /// <returns>最大士兵数</returns>
        public static long calculateMarsExploreTeamSoldierMax(List<HeroInfo> _heroList)
        {
            long heroPower = 0;
            if (_heroList != null)
            {
                foreach (HeroInfo heroInfo in _heroList)
                {
                    double sqrtPower = Math.Sqrt(heroInfo.power);
                    heroPower += (long)Math.Ceiling(sqrtPower);
                }
            }

            heroPower *= 10000;
            heroPower /= GRefdataCoreMgr.instance.npGeneral.mars_explore_lead_soldier_cut_coefficient;
            long troopNum = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_TEAM_TROOP_NUM);
            long troopPer = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_TEAM_TROOP_NUM_PER);
            long troopExtNum = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_TEAM_TROOP_EXT_NUM);

            return ((troopNum + heroPower) * (10000 + troopPer) / 10000) + troopExtNum;
        }

        /// <summary>
        /// 计算火星探索队伍的战力加成百分比
        /// </summary>
        /// <param name="_heroList">英雄列表</param>
        /// <returns>战力加成百分比 (万分比, 例如 1000 代表 10%)</returns>
        public static long calculateMarsExploreTeamPowerAddPer([CanBeNull, ItemNotNull] List<HeroInfo> _heroList)
        {
            if (_heroList == null)
                return 0;

            long powerAddPer = 0;
            foreach (HeroInfo heroInfo in _heroList)
            {
                if (heroInfo == null)
                    continue;

                HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(heroInfo.id, heroInfo.star);
                if (starRef?.mars_team_player_property == null)
                    continue;

                powerAddPer += starRef.mars_team_player_property.getPropertyValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
            }

            return powerAddPer;
        }

        /// <summary>
        /// 计算火星探索队伍战力
        /// </summary>
        /// <param name="_soldierNum">士兵数量</param>
        /// <param name="_powerAddPer">战力加成百分比 (万分比)</param>
        /// <returns>队伍总战力</returns>
        public static long calculateMarsExploreTeamPower(long _soldierNum, long _teamPowerAddPer)
        {
            long soldierPower = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER);
            long playerSoliderPowerPer = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
            long soldierPowerPer = soldierPower * (10000 + playerSoliderPowerPer + _teamPowerAddPer) / 10000;
            return _soldierNum * soldierPowerPer;
        }

        #endregion

        #region 科技

        public static EMarsTechnologyState getTechnologyState(MarsTechnologyRefObj _technologyRefObj)
        {
            if (_technologyRefObj == null)
                return EMarsTechnologyState.NONE;

            MarsTechnologyInfo technologyInfo = NPPlayer.instance.marsComp.technologySubComponent.getTechnologyInfoById(_technologyRefObj.id);
            if(technologyInfo != null)
                return getTechnologyState(technologyInfo);
            else
                return _technologyRefObj.technologyState;
        }
        
        public static EMarsTechnologyState getTechnologyState(MarsTechnologyInfo _technologyInfo)
        {
            // 自身或配表数据不存在, 处于EMarsTechnologyState.NONE状态
            if (_technologyInfo == null || _technologyInfo.technologyRefObj == null)
            {
                return EMarsTechnologyState.NONE;
            }

            // 获取下一等级配表数据
            MarsTechnologyLevelRefObj nextLevelRefObj = GRefdataCoreMgr.instance.getMarsTechnologyLevelRefObj(_technologyInfo.technologyId, _technologyInfo.lvl + 1);
            if (nextLevelRefObj == null) //没有下一等级配表数据, 说明已达最高级
            {
                return EMarsTechnologyState.MAX_LEVEL;
            }

            if (_technologyInfo.isUpgraded)
            {
                return EMarsTechnologyState.UPGRADED;
            }

            if (_technologyInfo.isUpgrading)
            {
                return EMarsTechnologyState.UPGRADEING;
            }

            if (_technologyInfo.technologyLvlRefObj == null) //当前等级配表数据不存在, 处于EMarsTechnologyState.NONE状态
            {
                return EMarsTechnologyState.NONE;
            }
            
            // 若升级条件未达成
            if (!_technologyInfo.technologyLvlRefObj.upgradeConditionEnable() // 前置条件
                || !checkTechnologyReachLevel(_technologyInfo.technologyRefObj.parent_list, 1)) // 前置科技等级
            {
                if (_technologyInfo.lvl <= 0) // 自身0级 且 升级前置条件未达成, 为未解锁状态
                {
                    return EMarsTechnologyState.LOCK;
                }
                else // 自身非0级 且 升级前置条件未达成
                {
                    return EMarsTechnologyState.LEVEL_NOT0_CONDITION_NOTREACH;
                }
            }
            else //升级前置条件已经达成
            {
                if (_technologyInfo.lvl <= 0)
                {
                    return EMarsTechnologyState.LEVEL_0_CONDITION_REACH;
                }
                else
                {
                    return EMarsTechnologyState.NORMAL;
                }
            }
        }
        
        public static bool checkTechnologyReachLevel(List<long> _technologyIdList, int _level)
        {
            if (_technologyIdList == null)
                return true;
            
            foreach (long technologyId in _technologyIdList)
            {
                if (getTechnologyLvl(technologyId) < _level)
                    return false;
            }

            return true;
        }
        
        /// <summary>
        /// 获取科技等级
        /// </summary>
        /// <returns></returns>
        public static int getTechnologyLvl(long _technologyId)
        {
            MarsTechnologyInfo technologyInfo = NPPlayer.instance.marsComp.technologySubComponent.getTechnologyInfoById(_technologyId);
            return technologyInfo?.lvl ?? 0;
        }
        
        #endregion

        public static void jumpToBuildingUpgrade(MarsBuildingInfo _marsBuildingInfo)
        {
            GNodeMars marsNode = QueueMgr.instance.findLastNode(typeof(GNodeMars)) as GNodeMars;
            if (marsNode == null)
            {
                QueueMgr.instance.AddNode(new GNodeMars((_node) =>
                {
                    _IMarsBuildingView buildingView = _node?.viewMgr.getBuildingView(_marsBuildingInfo) as _IMarsBuildingView;
                    jumpToBuildingUpgrade(buildingView);
                }));
            }
            else
            {
                QueueMgr.instance.QuitUntilCanStop(_node => _node == marsNode);
                _IMarsBuildingView buildingView = marsNode.viewMgr.getBuildingView(_marsBuildingInfo) as _IMarsBuildingView;
                jumpToBuildingUpgrade(buildingView);
            }
        }
        public static void jumpToBuildingUpgrade(_IMarsBuildingView _buildingView)
        {
            MarsBuildingInfo buildingInfo = _buildingView?.buildingInfo;
            if (buildingInfo == null)
                return;
            
            bool isEquipmentBuilding = buildingInfo.equipmentData.isValid();
            switch (buildingInfo.state)
            {
                case MarsBuildingInfo.StateType.Unbuilt:
                    if (!buildingInfo.checkCanShowBuildBtn())
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_tryConstructOtherBuildingTip_none);
                        return;
                    }
                    
                    GGUIWndMarsBuildingBuild.instance.refreshWnd(_buildingView);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingBuild.instance, GGUIWndMarsBuildingBuild.instance.showWnd,
                        EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_BUILD, false, false);
                    break;
                case MarsBuildingInfo.StateType.Constructing:
                    GGUIWndMarsBuildingConstructing.instance.refreshWnd(_buildingView);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingConstructing.instance, GGUIWndMarsBuildingConstructing.instance.showWnd,
                        EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_CONSTRUCTING, false, false);
                    break;
                case MarsBuildingInfo.StateType.Normal:
                    if (!buildingInfo.levelData.isValid() || buildingInfo.levelData.isLevelMax)
                    {
                        if (buildingInfo.buildRefId == GRefdataCoreMgr.instance.npGeneral.mars_building_home_id)
                        {
                            GGUIWndMarsBuildingHomeInfo.instance.refreshWnd(_buildingView);
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingHomeInfo.instance, GGUIWndMarsBuildingHomeInfo.instance.showWnd,
                                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_HOME_INFO, false, false);
                        }
                        else if (isEquipmentBuilding)
                        {
                            GGUIWndMarsBuildingInfo.instance.refreshWnd(_buildingView);
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingInfo.instance, GGUIWndMarsBuildingInfo.instance.showWnd,
                                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_INFO, false, false);
                        }
                        else
                        {
                            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingDetail(_buildingView));
                        }
                    }
                    else
                    {
                        if (buildingInfo.buildRefId == GRefdataCoreMgr.instance.npGeneral.mars_building_home_id)
                        {
                            GGUIWndMarsBuildingHomeUpgrade.instance.refreshWnd(_buildingView);
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingHomeUpgrade.instance, GGUIWndMarsBuildingHomeUpgrade.instance.showWnd,
                                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_HOME_UPGRADE, false, false);
                        }
                        else if (isEquipmentBuilding)
                        {
                            if (buildingInfo.equipmentData.totalMainLevel >= buildingInfo.equipmentData.totalMainLevelEnd)
                            {
                                GGUIWndMarsBuildingUpgrade.instance.refreshWnd(_buildingView);
                                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingUpgrade.instance, GGUIWndMarsBuildingUpgrade.instance.showWnd,
                                    EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_UPGRADE, false, false);
                            }
                            else
                            {
                                GGUIWndMarsBuildingInfo.instance.refreshWnd(_buildingView);
                                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingInfo.instance, GGUIWndMarsBuildingInfo.instance.showWnd,
                                    EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_INFO, false, false);
                            }
                        }
                        else
                        {
                            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingUpgrade(_buildingView));
                        }
                    }

                    break;
                case MarsBuildingInfo.StateType.Upgrading:
                    if (buildingInfo.buildRefId == GRefdataCoreMgr.instance.npGeneral.mars_building_home_id)
                    {
                        GGUIWndMarsBuildingHomeUpgrading.instance.refreshWnd(_buildingView);
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingHomeUpgrading.instance, GGUIWndMarsBuildingHomeUpgrading.instance.showWnd,
                            EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_HOME_UPGRADING, false, false);
                    }
                    else
                    {
                        GGUIWndMarsBuildingUpgrading.instance.refreshWnd(_buildingView);
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingUpgrading.instance, GGUIWndMarsBuildingUpgrading.instance.showWnd,
                            EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_UPGRADING, false, false);
                    }

                    break;
            }
        }
        public static void jumpToBuildingInfo(MarsBuildingInfo _marsBuildingInfo, GGUIMonoMarsBuildingInfoPageTabType _pageType)
        {
            GNodeMars marsNode = QueueMgr.instance.findLastNode(typeof(GNodeMars)) as GNodeMars;
            if (marsNode == null)
            {
                QueueMgr.instance.AddNode(new GNodeMars((_node) =>
                {
                    _IMarsBuildingView buildingView = _node?.viewMgr.getBuildingView(_marsBuildingInfo) as _IMarsBuildingView;
                    jumpToBuildingInfo(buildingView, _pageType);
                }));
            }
            else
            {
                QueueMgr.instance.QuitUntilCanStop(_node => _node == marsNode);
                _IMarsBuildingView buildingView = marsNode.viewMgr.getBuildingView(_marsBuildingInfo) as _IMarsBuildingView;
                jumpToBuildingInfo(buildingView, _pageType);
            }
        }
        public static void jumpToBuildingInfo(_IMarsBuildingView _buildingView, GGUIMonoMarsBuildingInfoPageTabType _pageType)
        {
            MarsBuildingInfo buildingInfo = _buildingView?.buildingInfo;
            if (buildingInfo == null)
                return;

            bool isEquipmentBuilding = buildingInfo.equipmentData.isValid();
            if (buildingInfo.buildRefId == GRefdataCoreMgr.instance.npGeneral.mars_building_home_id)
            {
                GGUIWndMarsBuildingHomeInfo.instance.refreshWnd(_buildingView);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingHomeInfo.instance, GGUIWndMarsBuildingHomeInfo.instance.showWnd,
                    EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_HOME_INFO, false, false);
            }
            else if (isEquipmentBuilding)
            {
                GGUIWndMarsBuildingInfo.instance.refreshWnd(_buildingView, _pageType);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingInfo.instance, GGUIWndMarsBuildingInfo.instance.showWnd,
                    EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_INFO, false, false);
            }
            else
            {
                QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingDetail(_buildingView));
            }
        }
        public static void jumpToBuildingSettle(MarsBuildingInfo _marsBuildingInfo)
        {
            jumpToBuildingInfo(_marsBuildingInfo, GGUIMonoMarsBuildingInfoPageTabType.Settle);
        }
        public static void showNoLeisureQueue()
        {
            int totalQueueCount = (int)NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM);
            int maxShowQueueCount = GRefdataCoreMgr.instance.npGeneral.mars_building_queue_max_count;
            
            // 如果当前的队列总数已经达到显示的上限，就弹出队列详情界面
            if (totalQueueCount >= maxShowQueueCount)
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildQueueDetail.instance, GGUIWndMarsBuildQueueDetail.instance.showWnd, UINodeTagConst.C_MARS_BUILD_QUEUE_DETAIL);
                return;
            }
            
            // 否则弹出购买队列界面
            QueueMgr.instance.AddNode(new GNodeMarsBuildQueueBuy());
        }
    }
}
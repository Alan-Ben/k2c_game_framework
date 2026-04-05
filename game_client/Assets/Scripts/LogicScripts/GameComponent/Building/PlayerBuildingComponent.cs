
using System;
using System.Collections.Generic;
using ALPackage;
using Common.BuildingEnum;
using Common.BuildingObj;
using CommonEnum;
using GC2GS.p010_BuildingOp;
using GS2GC.p002_InitOp;
using GS2GC.p010_BuildingOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class PlayerBuildingComponent : _ANPBasicPlayerComponent
    {
        [ItemNotNull, NotNull] private readonly List<BuildingInfo> _m_buildingList;
        [ItemNotNull, NotNull] private readonly List<BusinessBuildingInfo> _m_businessBuildingList;
        [ItemNotNull, NotNull] private readonly List<FarmingBuildingInfo> _m_farmingBuildingList;
        [NotNull] private readonly CommonUnionBonusMgr _m_farmingBonusMgr;
        [NotNull] private readonly CommonUnionBonusMgr _m_developBonusMgr;
        [NotNull] private readonly CommonUnionBonusMgr _m_productBonusMgr;
        [NotNull] private readonly RedTipDealer _m_redTipDealer;

        private BuildingInfo _m_nextOrderedBuilding; // 下一个要建造的建筑（参与建筑顺序的建筑）
        private FarmingMultipleInfo _m_farmingMultipleInfo;//农场建筑暴击倍数信息
        private List<long> _m_lUnderConstructionBuildingSfxIdList;//正在建造中的建筑特效id列表


        public PlayerBuildingComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_buildingList = new List<BuildingInfo>();
            _m_businessBuildingList = new List<BusinessBuildingInfo>();
            _m_farmingBuildingList = new List<FarmingBuildingInfo>();
            _m_farmingBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.FARMING);
            _m_developBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.DEVELOP);
            _m_productBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.PRODUCT);
            _m_redTipDealer = new RedTipDealer(this);
        }
        

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.BUILDING; } }
        public override ENPPlayerCompType[] dependCompList { get { return new [] { ENPPlayerCompType.HERO }; } }
        public override bool canPreInit { get { return true; } }
        
        public event Action<BuildingInfo> onBuildingBuilt;
        public event Action<FarmingBuildingInfo> onFarmingBuildingChg;
        public event Action<BusinessBuildingInfo> onBusinessBuildingChg;
        public event Action<long, long> onNextOrderedBuildingChg;
        
        /// <summary>
        /// 下一个要建造的建筑（参与建造顺序的建筑）
        /// </summary>
        public BuildingInfo nextOrderedBuilding { get { return _m_nextOrderedBuilding; } }
        /// <summary>
        /// 农场点击次数
        /// </summary>
        public long farmingClickNum { get { return _m_farmingMultipleInfo != null ? _m_farmingMultipleInfo.clickNum : 0; } }
        /// <summary>
        /// 农场建筑暴击倍数信息
        /// </summary>
        public FarmingMultipleInfo farmingMultipleInfo { get { return _m_farmingMultipleInfo; } }
        /// <summary>
        /// 所有建筑列表
        /// </summary>
        [ItemNotNull, NotNull] public IReadOnlyList<BuildingInfo> buildingList { get { return _m_buildingList; } }
        /// <summary>
        /// 是否有建筑正在播放建造特效
        /// </summary>
        public bool haveBuildingUnderConstruction { get { return _m_lUnderConstructionBuildingSfxIdList != null && _m_lUnderConstructionBuildingSfxIdList.Count > 0; } }


        public override void presendInitProtocol()
        {
            NPGSClientListener.sendRequestByLog(GSWriter_002_InitOp.make_010_ReqBuildingInit(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_010_RetBuildingInit>(_msg =>
                { dealPreInitFunc(() => _initData(_msg)); }));
        }


        protected override void _dealInit()
        {
            // NPGSClientListener.sendRequestByLog(GSWriter_002_InitOp.make_010_ReqBuildingInit(),
            //     new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_010_RetBuildingInit>(_initData));
        }
        protected override void _onInitDone()
        {
            _m_farmingBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _m_developBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _m_productBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _m_redTipDealer.init();
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_PLACE_BUILDING_CHG, _onHeroPlaceBuildingChg);
        }
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerBuildingComponent init fail");
        }
        protected override void _discard()
        {
            _m_redTipDealer.clear();
            _m_farmingBonusMgr.clear();
            _m_developBonusMgr.clear();
            _m_productBonusMgr.clear();
            _m_farmingMultipleInfo = null;
            _m_lUnderConstructionBuildingSfxIdList?.Clear();
            _m_lUnderConstructionBuildingSfxIdList = null;
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_PLACE_BUILDING_CHG, _onHeroPlaceBuildingChg);
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            // 所有组件初始化后重新计算所有商业建筑收益
            recalAllBusinessBuilding(false);
        }

        /// <summary>
        /// 添加正在建造中的建筑特效id
        /// </summary>
        /// <param name="_sfxId"></param>
        public void addUnderConstructionBuildingSfxId(long _sfxId)
        {
            if (_m_lUnderConstructionBuildingSfxIdList == null)
                _m_lUnderConstructionBuildingSfxIdList = new List<long>();
            _m_lUnderConstructionBuildingSfxIdList.Add(_sfxId);
        }

        /// <summary>
        /// 移除正在建造中的建筑特效id
        /// </summary>
        /// <param name="_sfxId"></param>
        public void removeUnderConstructionBuildingSfxId(long _sfxId)
        {
            if (_m_lUnderConstructionBuildingSfxIdList == null)
                return;

            if (_m_lUnderConstructionBuildingSfxIdList.Contains(_sfxId))
                _m_lUnderConstructionBuildingSfxIdList.Remove(_sfxId);
        }

        [Pure]
        public BuildingInfo getBuildingInfo(long _buildingId)
        {
            foreach (BuildingInfo buildingInfo in _m_buildingList)
            {
                if (buildingInfo.id == _buildingId)
                    return buildingInfo;
            }
            
            return null;
        }
        [Pure]
        public BusinessBuildingInfo getBusinessBuildingInfo(long _buildingId)
        {
            foreach (BusinessBuildingInfo businessBuildingInfo in _m_businessBuildingList)
            {
                if (businessBuildingInfo.id == _buildingId)
                    return businessBuildingInfo;
            }
            
            return null;
        }
        [Pure]
        public BusinessBuildingInfo getBusinessBuildingInfo(EBusinessBuildingType _type)
        {
            BusinessBuildingInfo result = null;
            switch (_type)
            {
                case EBusinessBuildingType.MINIMUM_EMPLOYEE:
                    BusinessBuildingInfo minEmployeeBuilding = null;
                    foreach (BusinessBuildingInfo buildingInfo in _m_businessBuildingList)
                    {
                        if (minEmployeeBuilding == null || buildingInfo.employeeNum < minEmployeeBuilding.employeeNum)
                            minEmployeeBuilding = buildingInfo;
                    }

                    result = minEmployeeBuilding;
                    break;
                case EBusinessBuildingType.UPGRADABLE:
                    foreach (BusinessBuildingInfo buildingInfo in _m_businessBuildingList)
                    {
                        if (GCommon.isItemEnough(buildingInfo.levelRef.upgrade_cost_item, false))
                        {
                            if (result == null || buildingInfo.level < result.level)
                                result = buildingInfo;
                        }
                    }
                    break;
                case EBusinessBuildingType.HERO_SETTABLE:
                    foreach (BusinessBuildingInfo buildingInfo in _m_businessBuildingList)
                    {
                        if (!buildingInfo.canSettleHero()) 
                            continue;
                        
                        result = buildingInfo;
                        break;
                    }
                    break;
                case EBusinessBuildingType.EMPLOYEE_LOWEST_COST:
                    BusinessBuildingInfo employeeLowestCostBuilding = null;
                    foreach (BusinessBuildingInfo buildingInfo in _m_businessBuildingList)
                    {
                        if (employeeLowestCostBuilding == null || buildingInfo.getHirCost(1)?.getCount() < employeeLowestCostBuilding.getHirCost(1)?.getCount())
                            employeeLowestCostBuilding = buildingInfo;
                    }
                    result = employeeLowestCostBuilding;
                    break;
            }
            
            return result ?? _m_businessBuildingList.GetFirst();
        }
        [Pure]
        public BusinessBuildingInfo getNextBusinessBuilding(BusinessBuildingInfo _buildingInfo, int _offset = 1)
        {
            if (_buildingInfo == null)
                return null;

            int index = _m_businessBuildingList.IndexOf(_buildingInfo);
            if (index < 0)
                return null;

            if (_m_businessBuildingList.Count == 1)
                return null;

            index = NPGameUtility.intRepeat(index + _offset, _m_businessBuildingList.Count);
            return _m_businessBuildingList[index];
        }
        [Pure]
        public FarmingBuildingInfo getFarmingBuildingInfo(long _buildingId)
        {
            foreach (FarmingBuildingInfo farmingBuildingInfo in _m_farmingBuildingList)
            {
                if (farmingBuildingInfo.id == _buildingId)
                    return farmingBuildingInfo;
            }
            
            return null;
        }
        [Pure]
        public long getTotalBusinessBuildingEarningsPerS()
        {
            long result = 0;
            foreach (BusinessBuildingInfo buildingInfo in _m_businessBuildingList)
            {
                result += buildingInfo.earningsPerS;
            }

            return result;
        }
        [Pure]
        public long getTotalFarmingBuildingEarningsPerS()
        {
            long result = 0;
            foreach (FarmingBuildingInfo buildingInfo in _m_farmingBuildingList)
            {
                result += buildingInfo.earningsPerS;
            }

            return result;
        }
        /// <summary>
        /// 根据类型获取建筑等级，如果_buildingId为0，则获取所有建筑的等级
        /// </summary>
        /// <param name="_buildingFuncEnum"></param>
        /// <param name="_buildingId"></param>
        /// <returns></returns>
        public long getBuildingLevel(EBuildingFuncEnum _buildingFuncEnum, long _buildingId)
        {
            //如果_buildingId为0，则获取所有建筑的等级
            if (_buildingId != 0)
            {
                switch (_buildingFuncEnum)
                {
                    case EBuildingFuncEnum.FARM:
                        FarmingBuildingInfo farmBuildingInfo = getFarmingBuildingInfo(_buildingId);
                        return farmBuildingInfo != null ? farmBuildingInfo.level : 0;
                    case EBuildingFuncEnum.BUSINESS:
                        BusinessBuildingInfo businessBuildingInfo = getBusinessBuildingInfo(_buildingId);
                        return businessBuildingInfo != null ? businessBuildingInfo.level : 0;
                }
            }
            else
            {
                long level = 0;
                switch (_buildingFuncEnum)
                {
                    case EBuildingFuncEnum.FARM:
                        foreach (FarmingBuildingInfo farmingBuildingInfo in _m_farmingBuildingList)
                        {
                            level += farmingBuildingInfo.level;
                        }
                        return level;
                    case EBuildingFuncEnum.BUSINESS:
                        foreach (BusinessBuildingInfo businessBuildingInfo in _m_businessBuildingList)
                        {
                            level += businessBuildingInfo.level;
                        }
                        return level;
                }
            }

            return 0;
        }
        /// <summary>
        /// 是否拥有建筑
        /// </summary>
        /// <param name="_buildingId"></param>
        /// <returns></returns>
        public bool hasBuilding(long _buildingId)
        {
            return getBuildingInfo(_buildingId) is { isBuilt: true } || getBusinessBuildingInfo(_buildingId) != null || getFarmingBuildingInfo(_buildingId) != null;
        }
        /// <summary>
        /// 获取指定建筑员工数量
        /// </summary>
        /// <param name="_buildingId"></param>
        /// <returns></returns>
        public long getEmployeeNum(long _buildingId)
        {
            long count = 0;
            //如果_buildingId为0，则获取所有建筑的员工数量
            if (_buildingId == 0)
            {
                foreach (BusinessBuildingInfo businessBuildingInfo in _m_businessBuildingList)
                {
                    count += businessBuildingInfo.employeeNum;
                }
            }
            else
            {
                BusinessBuildingInfo businessBuildingInfo = getBusinessBuildingInfo(_buildingId);
                count = businessBuildingInfo != null ? businessBuildingInfo.employeeNum : 0;
            }

            return count;
        }
        public void setCheckedCanUnlockProductEmployeeNum(BusinessBuildingInfo _buildingInfo)
        {
            if (_buildingInfo == null)
                return;
            
            _buildingInfo.setCheckedCanUnlockProductEmployeeNum();
            onBusinessBuildingChg?.Invoke(_buildingInfo);
        }

        /// <summary>
        /// 获取已建造建筑数量
        /// </summary>
        /// <returns></returns>
        public long getOwnBuildingNum()
        {
            long count = 0;
            for (int i = 0; i < _m_buildingList.Count; i++)
            {
                if (_m_buildingList[i].isBuilt)
                    count++;
            }

            return count;
        }

        public void recalAllBusinessBuilding(bool _isDealNextFrame = true)
        {
            foreach (BusinessBuildingInfo buildingInfo in _m_businessBuildingList)
            {
                buildingInfo.recalEarnings(_isDealNextFrame);
            }
        }

        public void dealAllBusinessBuildingList(Action<BusinessBuildingInfo> _action)
        {
            if (_action == null)
                return;

            _m_businessBuildingList.ForEach((_action));
        }
        
        public void reqBuildingBuild(long _buildingId, Action<bool> _complete = null)
        {
            BuildingInfo buildingInfo = getBuildingInfo(_buildingId);
            if (buildingInfo == null)
            {
                ALLog.Error("id 为 " + _buildingId + " 的建筑不存在");
                _complete?.Invoke(false);
                return;
            }
            if (buildingInfo.isBuilt)
            {
                ALLog.Error("id 为 " + _buildingId + " 的建筑已经建造过了");
                _complete?.Invoke(false);
                return;
            }
            if (!buildingInfo.baseRef.build_condition.IsEnable(null))
            {
                ALLog.Error("id 为 " + _buildingId + " 的建筑建造条件不满足");
                _complete?.Invoke(false);
                return;
            }
            if (!GCommon.isItemEnough(buildingInfo.baseRef.build_cost, true))
            {
                _complete?.Invoke(false);
                return;
            }

            GC2GS_010_001_ReqBuildingBuild protocol = GSWriter_010_BuildingOp.make_001_ReqBuildingBuild(_buildingId);
            if (_complete != null)
                NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_010_001_RetBuildingBuild>((_isSuc, _msg) => _complete(_isSuc)));
            else
                NPGSClientListener.sendMsgByLog(protocol);
        }
        public void reqFarmUpgradeLvl(long _buildingId, Action<bool> _complete = null)
        {
            FarmingBuildingInfo buildingInfo = getFarmingBuildingInfo(_buildingId);
            if (buildingInfo == null)
            {
                ALLog.Error("id 为 " + _buildingId + " 的农田建筑不存在");
                _complete?.Invoke(false);
                return;
            }
            if (buildingInfo.levelMax)
            {
                ALLog.Error("id 为 " + _buildingId + " 的农田建筑已经升级到最高等级");
                _complete?.Invoke(false);
                return;
            }
            if (!GCommon.isItemEnough(buildingInfo.levelData.upgrade_cost_item, true))
            {
                _complete?.Invoke(false);
                return;
            }

            GC2GS_010_002_ReqFarmUpgradeLvl protocol = GSWriter_010_BuildingOp.make_002_ReqFarmUpgradeLvl(_buildingId);
            if (_complete != null)
                NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_010_002_RetFarmUpgradeLvl>((_isSuc, _msg) => _complete(_isSuc)));
            else
                NPGSClientListener.sendMsgByLog(protocol);
        }
        public void reqFarmClickOutput(long _buildingId, Action<bool> _complete = null)
        {
            FarmingBuildingInfo buildingInfo = getFarmingBuildingInfo(_buildingId);
            if (buildingInfo == null)
            {
                ALLog.Error("id 为 " + _buildingId + " 的农田建筑不存在");
                _complete?.Invoke(false);
                return;
            }
            if (FpsAndPingMgr.instance.serverTimeTag - buildingInfo.lastClientClickMS < GRefdataCoreMgr.instance.npGeneral.farming_building_click_spaceMS)
            {
                _complete?.Invoke(false);
                return;
            }

            buildingInfo.lastClientClickMS = FpsAndPingMgr.instance.serverTimeTag;
            ClientRequestPrediction_010_003_ReqFarmClickOutput prediction = new ClientRequestPrediction_010_003_ReqFarmClickOutput(_buildingId, NPPlayer.instance.buildingComp.farmingClickNum, FpsAndPingMgr.instance.serverTimeTag);
            prediction.send();
            _complete?.Invoke(true);
            
            // GC2GS_010_003_ReqFarmClickOutput protocol = GSWriter_010_BuildingOp.make_003_ReqFarmClickOutput(_buildingId);
            // if (_complete != null)
            //     NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_010_003_RetFarmClickOutput>((_isSuc, _msg) => _complete(_isSuc)));
            // else
            //     NPGSClientListener.sendMsgByLog(protocol);
        }
        public void reqBusinessUpgradeLvl(long _buildingId, Action<bool> _complete = null)
        {
            BusinessBuildingInfo buildingInfo = getBusinessBuildingInfo(_buildingId);
            if (buildingInfo == null)
            {
                ALLog.Error("id 为 " + _buildingId + " 的商业建筑不存在");
                _complete?.Invoke(false);
                return;
            }
            if (buildingInfo.levelMax)
            {
                ALLog.Error("id 为 " + _buildingId + " 的商业建筑已经升级到最高等级");
                _complete?.Invoke(false);
                return;
            }
            if (!GCommon.isItemEnough(buildingInfo.levelRef.upgrade_cost_item, true))
            {
                _complete?.Invoke(false);
                return;
            }
            
            GC2GS_010_004_ReqBusinessUpgradeLvl protocol = GSWriter_010_BuildingOp.make_004_ReqBusinessUpgradeLvl(_buildingId);
            if (_complete != null)
                NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_010_004_RetBusinessUpgradeLvl>((_isSuc, _msg) => _complete(_isSuc)));
            else
                NPGSClientListener.sendMsgByLog(protocol);
        }
        public void reqBusinessHireEmployee(long _buildingId, Action<bool> _complete = null)
        {
            BusinessBuildingInfo buildingInfo = getBusinessBuildingInfo(_buildingId);
            if (buildingInfo == null)
            {
                ALLog.Error("id 为 " + _buildingId + " 的商业建筑不存在");
                _complete?.Invoke(false);
                return;
            }
            if (buildingInfo.employeeNum >= buildingInfo.maxEmployeeNum)
            {
                ALLog.Error("id 为 " + _buildingId + " 的商业建筑雇员数量已经达到最大值");
                _complete?.Invoke(false);
                return;
            }
            NPCommonCostItem hireCost = buildingInfo.getHirCost(1);
            if (!GCommon.isItemEnough(hireCost, true))
            {
                _complete?.Invoke(false);
                return;
            }

            GC2GS_010_005_ReqBusinessHireEmployee protocol = GSWriter_010_BuildingOp.make_005_ReqBusinessHireEmployee(_buildingId);
            if (_complete != null)
                NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_010_005_RetBusinessHireEmployee>((_isSuc, _msg) => _complete(_isSuc)));
            else
                NPGSClientListener.sendMsgByLog(protocol);
        }
        public void reqBusinessHireTenEmployees(long _buildingId, Action<bool> _complete = null)
        {
            BusinessBuildingInfo buildingInfo = getBusinessBuildingInfo(_buildingId);
            if (buildingInfo == null)
            {
                ALLog.Error("id 为 " + _buildingId + " 的商业建筑不存在");
                _complete?.Invoke(false);
                return;
            }
            if (buildingInfo.employeeNum >= buildingInfo.maxEmployeeNum)
            {
                ALLog.Error("id 为 " + _buildingId + " 的商业建筑雇员数量已经达到最大值");
                _complete?.Invoke(false);
                return;
            }
            NPCommonCostItem hireCost = buildingInfo.getHirCost(10);
            if (!GCommon.isItemEnough(hireCost, true))
            {
                _complete?.Invoke(false);
                return;
            }
            
            GC2GS_010_006_ReqBusinessHireTenEmployees protocol = GSWriter_010_BuildingOp.make_006_ReqBusinessHireTenEmployees(_buildingId);
            if (_complete != null)
                NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_010_006_RetBusinessHireTenEmployees>((_isSuc, _msg) => _complete(_isSuc)));
            else
                NPGSClientListener.sendMsgByLog(protocol);
        }


        private void _initData(GS2GC_002_010_RetBuildingInit _msg)
        {
            _m_buildingList.Clear();
            // 初始化所有建筑的数据
            GRefdataCoreMgr.instance.buildingRefCore.dealAllRef(_buildingRef =>
            {
                BuildingInfo buildingInfo = new BuildingInfo(_buildingRef);
                _m_buildingList.Add(buildingInfo);
            });
            
            if (_msg != null)
            {
                List<long> serverBuiltBuildingList = _msg.getBuildingIdList();
                List<Building_Business> serverBusinessBuildingList = _msg.getBusinessList();
                List<Building_Farm> serverFarmingBuildingList = _msg.getFarmList();
                _m_farmingMultipleInfo = new FarmingMultipleInfo(_msg.getFarmMultipleInfo());

                foreach (long id in serverBuiltBuildingList)
                {
                    BuildingInfo buildingInfo = getBuildingInfo(id);
                    if (buildingInfo != null)
                    {
                        buildingInfo.setBuildingBuilt();
                        onBuildingBuilt?.Invoke(buildingInfo);
                    }
                }
                
                Dictionary<long, List<HeroInfo>> heroSlotDataDict = new Dictionary<long, List<HeroInfo>>();
                NPPlayer.instance.heroComponent.dealAllHero(_heroInfo =>
                {
                    if (_heroInfo == null || !_heroInfo.placeData.isPlaced)
                        return;

                    List<HeroInfo> heroList = heroSlotDataDict.getValueDefinitely(_heroInfo.placeData.buildingId);
                    heroList.Add(_heroInfo);
                });
                
                _m_businessBuildingList.Clear();
                foreach (Building_Business business in serverBusinessBuildingList)
                {
                    heroSlotDataDict.TryGetValue(business.getBuildingId(), out List<HeroInfo> heroList);
                    heroList?.Sort((_a, _b) => _a.placeData.placeSerialize.CompareTo(_b.placeData.placeSerialize));
                    BusinessBuildingInfo businessBuildingInfo = new BusinessBuildingInfo(business, heroList, false);
                    // baseRef 在这里是有可能为 null 的，通过这里排除掉所有 ref 为 null 的建筑，后续可以默认 baseRef 不为 null
                    if (businessBuildingInfo.baseRef == null)
                        continue;
                    
                    _m_businessBuildingList.Add(businessBuildingInfo);
                    // 添加产品的 bonus
                    foreach (BusinessBuildingProductRefObj product in businessBuildingInfo.productList)
                        _m_productBonusMgr.addBonus(product.add_bonus.unionBonus);
                    // 添加业务的 bonus
                    List<BusinessBuildingDevelopRefObj> developRefList = GRefdataCoreMgr.instance.getBusinessBuildingDevelopRefList(businessBuildingInfo.id);
                    if (developRefList != null)
                    {
                        foreach (BusinessBuildingDevelopRefObj developRef in developRefList)
                        {
                            if (developRef.level_required > businessBuildingInfo.level)
                                continue;
                            
                            _m_developBonusMgr.addBonus(developRef.add_bonus.unionBonus);
                        }
                    }
                }

                _m_farmingBuildingList.Clear();
                foreach (Building_Farm farm in serverFarmingBuildingList)
                {
                    FarmingBuildingInfo farmingBuildingInfo = new FarmingBuildingInfo(farm, false);
                    // baseRef 在这里是有可能为 null 的，通过这里排除掉所有 ref 为 null 的建筑，后续可以默认 baseRef 不为 null
                    if (farmingBuildingInfo.baseRef == null)
                        continue;
                    
                    _m_farmingBuildingList.Add(farmingBuildingInfo);
                    _m_farmingBonusMgr.addTotalModifier(farmingBuildingInfo.levelData?.bonus_modify);
                }
            }

            _refreshNextOrderedBuilding();
            
            setInitDone();
        }
        private void _refreshNextOrderedBuilding()
        {
            BuildingInfo result = null;
            int minOrder = int.MaxValue;
            foreach (BuildingInfo buildingInfo in _m_buildingList)
            {
                // 如果 order 小于 0 ，认为不参与建造排序
                if (buildingInfo.baseRef.build_order < 0 || buildingInfo.isBuilt)
                    continue;

                if (buildingInfo.baseRef.build_order < minOrder)
                {
                    minOrder = buildingInfo.baseRef.build_order;
                    result = buildingInfo;
                }
            }

            if (result == _m_nextOrderedBuilding)
                return;

            long lastId = _m_nextOrderedBuilding?.id ?? -1;
            long newId = result?.id ?? -1;
            _m_nextOrderedBuilding = result;
            onNextOrderedBuildingChg?.Invoke(lastId, newId);
            _m_redTipDealer.refreshBuildableRedTip();
        }
        private void _onHeroPlaceBuildingChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 3 || _objs[0] is not long heroId || _objs[1] is not long lastBuildingId || _objs[2] is not long newBuildingId)
                return;
            
            BusinessBuildingInfo lastBuildingInfo = getBusinessBuildingInfo(lastBuildingId);
            lastBuildingInfo?.removeHero(heroId);
            BusinessBuildingInfo newBuildingInfo = getBusinessBuildingInfo(newBuildingId);
            newBuildingInfo?.addHero(heroId);
            
            if (lastBuildingInfo != null)
                onBusinessBuildingChg?.Invoke(lastBuildingInfo);
            if (newBuildingInfo != null)
                onBusinessBuildingChg?.Invoke(newBuildingInfo);
        }


        internal void _onBuildingBuilt(GS2GC_010_050_OnBuidingBuilt _msg)
        {
            if (_msg == null)
                return;

            BuildingInfo buildingInfo = getBuildingInfo(_msg.getBuildingId());
            if (buildingInfo == null)
                return;

            buildingInfo.setBuildingBuilt();
            onBuildingBuilt?.Invoke(buildingInfo);
            _refreshNextOrderedBuilding();

            WinMsg.SendMsg(WinMsgType.ON_BUILDING_BUILT, _msg.getBuildingId());
        }
        internal void _onFarmChg(GS2GC_010_051_OnFarmChg _msg)
        {
            if (_msg == null)
                return;

            Building_Farm serverFarmingBuildingInfo = _msg.getFarm();
            FarmingBuildingInfo farmingBuildingInfo = getFarmingBuildingInfo(serverFarmingBuildingInfo.getBuildingId());
            if (farmingBuildingInfo == null)
                return;

            //建筑原等级
            long oriLevel = farmingBuildingInfo.level;

            PlayerBonusPropertyModifier lastModifier = farmingBuildingInfo.levelData?.bonus_modify;
            farmingBuildingInfo.update(serverFarmingBuildingInfo);
            PlayerBonusPropertyModifier newModifier = farmingBuildingInfo.levelData?.bonus_modify;
            if (lastModifier != newModifier)
                _m_farmingBonusMgr.replaceTotal(lastModifier, newModifier);
            
            onFarmingBuildingChg?.Invoke(farmingBuildingInfo);

            //发送建筑等级变化消息
            if (farmingBuildingInfo.level != oriLevel)
                WinMsg.SendMsg(WinMsgType.ON_BUILDING_LEVEL_CHG, EBuildingFuncEnum.FARM, farmingBuildingInfo.id);
        }
        internal void _onBusinessChg(GS2GC_010_052_OnBusinessChg _msg)
        {
            if (_msg == null)
                return;
            
            Building_Business serverBusinessBuildingInfo = _msg.getBusiness();
            BusinessBuildingInfo businessBuildingInfo = getBusinessBuildingInfo(serverBusinessBuildingInfo.getBuildingId());
            if (businessBuildingInfo == null)
                return;

            //建筑原等级
            long oriLevel = businessBuildingInfo.level;
            //建筑原本员工数量
            long oriEmployeeNum = businessBuildingInfo.employeeNum;

            businessBuildingInfo.update(serverBusinessBuildingInfo);

            //发送建筑等级变化消息
            if (businessBuildingInfo.level != oriLevel)
            {
                WinMsg.SendMsg(WinMsgType.ON_BUILDING_LEVEL_CHG, EBuildingFuncEnum.BUSINESS, businessBuildingInfo.id);
                // 添加业务的 bonus
                List<BusinessBuildingDevelopRefObj> developRefList = GRefdataCoreMgr.instance.getBusinessBuildingDevelopRefList(businessBuildingInfo.id);
                if (developRefList != null)
                {
                    foreach (BusinessBuildingDevelopRefObj developRef in developRefList)
                    {
                        if (oriLevel >= developRef.level_required)
                        {
                            if (businessBuildingInfo.level < developRef.level_required)
                                _m_developBonusMgr.removeBonus(developRef.add_bonus.unionBonus);
                        }
                        else if (businessBuildingInfo.level >= developRef.level_required)
                        {
                            _m_developBonusMgr.addBonus(developRef.add_bonus.unionBonus);
                        }
                    }
                }
            }
            //发送建筑员工数量变化消息
            if(businessBuildingInfo.employeeNum != oriEmployeeNum)
                WinMsg.SendMsg(WinMsgType.ON_BUILDING_EMPLOYEE_NUM_CHG, businessBuildingInfo.id);
            
            onBusinessBuildingChg?.Invoke(businessBuildingInfo);
            
            // 添加产品的 bonus
            // 采用单独协议 010_057 来推送了，变化在那边更新
        }
        internal void _onBusinessBuildingBuilt(GS2GC_010_054_OnBusinessBuilt _msg)
        {
            if (_msg == null)
                return;
            
            BusinessBuildingInfo businessBuildingInfo = new BusinessBuildingInfo(_msg.getBusiness(), null);
            if (businessBuildingInfo.baseRef == null)
                return;
            
            _m_businessBuildingList.Add(businessBuildingInfo);

            WinMsg.SendMsg(WinMsgType.ON_BUILDING_ADD, EBuildingFuncEnum.BUSINESS, businessBuildingInfo.id);
        }
        internal void _onFarmingBuildingBuilt(GS2GC_010_055_OnFarmBuilt _msg)
        {
            if (_msg == null)
                return;
            
            FarmingBuildingInfo farmingBuildingInfo = new FarmingBuildingInfo(_msg.getFarm());
            if (farmingBuildingInfo.baseRef == null)
                return;
            
            _m_farmingBuildingList.Add(farmingBuildingInfo);

            WinMsg.SendMsg(WinMsgType.ON_BUILDING_ADD, EBuildingFuncEnum.FARM, farmingBuildingInfo.id);
        }
        internal void _onBusinessUnlockProduct(GS2GC_010_057_OnBusinessUnlockProduct _msg)
        {
            if (_msg == null)
                return;

            long productId = _msg.getRefId();
            BusinessBuildingProductRefObj productRef = GRefdataCoreMgr.instance.businessBuildingProductRefCore.getRef(productId);
            if (productRef == null)
                return;
            
            BusinessBuildingInfo businessBuildingInfo = getBusinessBuildingInfo(productRef.building_id);
            if (businessBuildingInfo == null)
                return;
            
            businessBuildingInfo.productList.Add(productRef);
            _m_productBonusMgr.addBonus(productRef.add_bonus.unionBonus);
            onBusinessBuildingChg?.Invoke(businessBuildingInfo);
            WinMsg.SendMsg(WinMsgType.ON_BUILDING_UNLOCK_PRODUCT, productRef.building_id, productId);
        }
        internal void _onFarmMultipleInfoReset(GS2GC_010_058_OnFarmMultipleInfoReset _msg)
        {
            if (_msg == null)
                return;

            if (_m_farmingMultipleInfo == null)
                _m_farmingMultipleInfo = new FarmingMultipleInfo(_msg.getFarmMultipleInfo());
            else
                _m_farmingMultipleInfo.updateInfo(_msg.getFarmMultipleInfo());
        }
        internal void _invokeOnBusinessBuildingChg(BusinessBuildingInfo _buildingInfo)
        {
            onBusinessBuildingChg?.Invoke(_buildingInfo);
        }
    }
}
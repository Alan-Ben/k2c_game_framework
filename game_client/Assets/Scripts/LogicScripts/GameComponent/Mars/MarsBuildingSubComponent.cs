using System;
using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using Common.MarsObj;
using GC2GS.p002_InitOp;
using GC2GS.p039_MarsBuildingOp;
using GS2GC.p002_InitOp;
using GS2GC.p039_MarsBuildingOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// Mars建筑子组件 - 负责管理火星建筑系统
    /// </summary>
    public class MarsBuildingSubComponent : _AMarsSubComponent
    {
        [ItemNotNull, NotNull] private readonly List<MarsBuildingInfo> _m_buildingInfos;
        [NotNull] private readonly MarsPropertyContainer _m_marsPropertyContainer;
        [NotNull] private readonly List<MarsBuildingInfo> _m_buildingQueueList;

        //主基地资源收集时间
        private int _m_iHomeCollectTimeS;

        private long _m_extMoodIndex;
        
        
        public MarsBuildingSubComponent([NotNull] MarsComponent _parentComp)
            : base(_parentComp)
        {
            _m_buildingInfos = new List<MarsBuildingInfo>();
            _m_marsPropertyContainer = new MarsPropertyContainer();
            _m_buildingQueueList = new List<MarsBuildingInfo>();

            //0表示无效值
            _m_iHomeCollectTimeS = 0;
        }


        public event Action<long, MarsBuildingInfo.StateType, MarsBuildingInfo.StateType> onBuildingStateChg; // buildingId
        public event Action<long> onBuildingTimeChg; // buildingId
        public event Action<long, long> onEquipmentUpgraded; // buildingId, equipmentId
        public event Action<long> onBuildingSettleChg; // buildingId
        public event Action<long, long> onBuildingLevelChg; // buildingId
        public event Action onQueueListChg;
        private MarsBuildingEnergyFullLocalPushDealer _m_energyFullLocalPushDealer;//火星能源建筑能量满本地推送
        private MarsBuildingConstructUpgradeCompleteLocalPushDealer _m_constructUpgradeCompleteLocalPushDealer;//火星建筑建造升级完成本地推送
        [NotNull] public MarsPropertyContainer marsPropertyContainer { get { return _m_marsPropertyContainer; } }
        [ItemNotNull, NotNull] internal List<MarsBuildingInfo> _getBuildingInfos() { return _m_buildingInfos; }
        public long extMoodIndex { get { return _m_extMoodIndex; } }
        public int homeCollectTimeS { get { return _m_iHomeCollectTimeS; } }

        /// <summary>
        /// 是否有空闲的建筑队列
        /// </summary>
        public bool hasLeisureQueue
        {
            get
            {
                int totalQueueCount = (int)NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM);
                int currentUsingCount = getCurrentQueueCount();
                return currentUsingCount < totalQueueCount;
            }
        }

        /// <summary>
        /// 是否可以收集主基地资源
        /// </summary>
        public bool canCollecHome
        {
            get
            {
                if (_m_iHomeCollectTimeS <= 0)
                    return false;

                //计算服务器时间差
                return (FpsAndPingMgr.instance.serverTimeTagS - _m_iHomeCollectTimeS) >= GRefdataCoreMgr.instance.npGeneral.mars_home_output_pop_min_time_s;
            }
        }
        /// <summary>
        /// 是否收集是红点展示
        /// </summary>
        public bool isCollectRed
        {
            get
            {
                if (_m_iHomeCollectTimeS <= 0)
                    return false;

                //计算服务器时间差
                return (FpsAndPingMgr.instance.serverTimeTagS - _m_iHomeCollectTimeS) >= GRefdataCoreMgr.instance.npGeneral.mars_home_output_red_time_s;
            }
        }

        public override void init(Action<bool> _complete)
        {
            List<MarsBuildingRefObj> buildingRefList = GRefdataCoreMgr.instance.marsBuildingRefCore.refList;
            foreach (MarsBuildingRefObj refObj in buildingRefList)
            {
                if (refObj == null)
                    continue;
                
                MarsBuildingInfo buildingInfo = new MarsBuildingInfo(refObj);
                _m_buildingInfos.Add(buildingInfo);
                buildingInfo.init(this);
            }
            
            NPGSClientListener.sendRequestByLog(new GC2GS_002_073_ReqMarsBuildingInit(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_073_RetMarsBuildingInit>((_isSuc, _msg) =>
                {
                    _m_parentComponent.dealPreInitFunc(() =>
                    {
                        if (!_isSuc || _msg == null)
                        {
                            _complete?.Invoke(false);
                            return;
                        }

                        List<Mars_Building> serverBuildingList = _msg.getBuildingList();
                        if (serverBuildingList != null)
                        {
                            foreach (Mars_Building serverBuilding in serverBuildingList)
                                _onBuildingChg(serverBuilding);
                        }

                        List<Mars_BuildingEquipment> serverEquipmentList = _msg.getEquipmentList();
                        if (serverEquipmentList != null)
                        {
                            foreach (Mars_BuildingEquipment serverEquipment in serverEquipmentList)
                                _onEquipmentChg(serverEquipment);
                        }

                        List<Mars_PeopleBuilding> serverPeopleBuildingList = _msg.getPeopleBuildingList();
                        if (serverPeopleBuildingList != null)
                        {
                            foreach (Mars_PeopleBuilding serverPeopleBuilding in serverPeopleBuildingList)
                                _onPeopleBuildingChg(serverPeopleBuilding);
                        }

                        Mars_HomeBuilding serverHomeBuilding = _msg.getHomeBuilding();
                        if (serverHomeBuilding != null)
                        {
                            MarsBuildingInfo buildingInfo = getBuildingInfoById(serverHomeBuilding.getBuildingId());
                            buildingInfo?._updateHomeSwitch(serverHomeBuilding.getIsNormalOn(), serverHomeBuilding.getIsOverdriveOn());

                            //更新主基地最后一次收集资源的时间
                            _m_iHomeCollectTimeS = serverHomeBuilding.getLastCollectTimeS();
                        }

                        List<Mars_BuildingEnergyOutput> serverEnergyOutputList = _msg.getEnergyOutputList();
                        if (serverEnergyOutputList != null)
                        {
                            foreach (Mars_BuildingEnergyOutput serverEnergyOutput in serverEnergyOutputList)
                                _onEnergyOutputChg(serverEnergyOutput);
                        }

                        List<Mars_Mars_BuildingEquipment_Food> serverFoodList = _msg.getFoodEquipmentList();
                        if (serverFoodList != null)
                        {
                            foreach (Mars_Mars_BuildingEquipment_Food serverFood in serverFoodList)
                            {
                                if (serverFood == null)
                                    continue;

                                MarsBuildingInfo buildingInfo = getBuildingInfoById(serverFood.getBuildingId());
                                buildingInfo?._updateFoodSwitch(serverFood.getIsPowerOn());
                            }
                        }

                        List<Mars_BuildingUpQueue> serverQueueList = _msg.getBuildingUpQueueList();
                        if (serverQueueList != null)
                        {
                            foreach (Mars_BuildingUpQueue serverQueue in serverQueueList)
                                _onQueueAdd(serverQueue);
                        }

                        _onExtMoodIndexChg(_msg.getExtMoodIndex());
                        _m_parentComponent.needUpdateDispatchedPeopleNum();
                        
                        _onMarsEnergyChg();
                        NPPlayer.instance.specialItemComp.marsEnergyData.onValueChg += _onMarsEnergyChg;
                        NPPlayer.instance.playerPropertyMgr.propertyChgDelegate += _onPlayerPropertyChg;
                        _regLocalPush();
                        _complete?.Invoke(true);
                    });
                }));
        }
        public override void discard()
        {
            NPPlayer.instance.playerPropertyMgr.propertyChgDelegate -= _onPlayerPropertyChg;
            NPPlayer.instance.specialItemComp.marsEnergyData.onValueChg -= _onMarsEnergyChg;
            
            foreach (MarsBuildingInfo info in _m_buildingInfos)
            {
                info.discard();
            }
            _m_buildingInfos.Clear();
            _unRegLocalPush();
        }

        /// <summary>
        /// 注册本地推送
        /// </summary>
        private void _regLocalPush()
        {
            _m_energyFullLocalPushDealer = new MarsBuildingEnergyFullLocalPushDealer();
            LocalPushMgr.instance.regDealer(_m_energyFullLocalPushDealer);
            _m_constructUpgradeCompleteLocalPushDealer = new MarsBuildingConstructUpgradeCompleteLocalPushDealer();
            LocalPushMgr.instance.regDealer(_m_constructUpgradeCompleteLocalPushDealer);
        }

        /// <summary>
        /// 注销本地推送
        /// </summary>
        private void _unRegLocalPush()
        {
            LocalPushMgr.instance.unRegDealer(_m_energyFullLocalPushDealer);
            _m_energyFullLocalPushDealer = null;
            LocalPushMgr.instance.unRegDealer(_m_constructUpgradeCompleteLocalPushDealer);
            _m_constructUpgradeCompleteLocalPushDealer = null;
        }

        /// <summary>
        /// 主动刷新客户端的数据
        /// </summary>
        /// <remarks>
        /// 服务端整个数据层遵从懒更新，所以数据变化时不一定会产生推送，使用这个方法可以把客户端的数据更新到最新，并触发相应事件
        /// </remarks>
        public void updateClientData()
        {
            foreach (MarsBuildingInfo info in _m_buildingInfos)
            {
                MarsBuildingInfo.StateType oldState = info.state;
                info._updateTime();
                if (oldState != info.state)
                    onBuildingStateChg?.Invoke(info.refObj.id, oldState, info.state);
            }
        }

        [ItemNotNull, NotNull]
        public List<MarsBuildingInfo> getBuildingInfoList()
        {
            return new List<MarsBuildingInfo>(_m_buildingInfos);
        }
        public void getBuildingInfoListNonAlloc(List<MarsBuildingInfo> _list, Predicate<MarsBuildingInfo> _filter = null)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            if (_filter == null)
            {
                _list.AddRange(_m_buildingInfos);
                return;
            }
            
            foreach (MarsBuildingInfo info in _m_buildingInfos)
            {
                if (_filter(info))
                    _list.Add(info);
            }
        }
        public MarsBuildingInfo getBuildingInfoByBuildingOrder(int _buildingOrder)
        {
            return _m_buildingInfos.Find(_info => _info.refObj.building_order_in_number == _buildingOrder);
        }
        public MarsBuildingInfo getBuildingInfoById(long _buildingId)
        {
            return _m_buildingInfos.Find(_info => _info.refObj.id == _buildingId);
        }
        public MarsBuildingInfo getBuildingInfoByType(EMarsBuildingType _type)
        {
            return _m_buildingInfos.Find(_info => _info.refObj.building_type == _type);
        }
        public MarsBuildingInfo getBuildingInfoByQueueId(long _queueId)
        {
            return _m_buildingQueueList.Find(_info => _info.queueId == _queueId);
        }
        public MarsBuildingInfo getBuildingInfoByQueueIndex(int _index)
        {
            return _m_buildingQueueList.SafeGet(_index);
        }
        public MarsBuildingInfo getBuildingInfoByType(EMarsBuildingType _buildingType, EMarsBuildingJumpType _marsBuildingJumpType)
        {
            MarsBuildingInfo targetInfo = null;
            switch (_marsBuildingJumpType)
            {
                //最低等级建筑
                case EMarsBuildingJumpType.MINIMUM_LEVEL:
                    for (int i = 0; i < _m_buildingInfos.Count; i++)
                    {
                        if(_m_buildingInfos[i].type == _buildingType && (targetInfo == null || _m_buildingInfos[i].level < targetInfo.level))
                        {
                            targetInfo = _m_buildingInfos[i];
                        }
                    }
                    break;
            }
            return targetInfo;
        }
        [CanBeNull]
        public MarsBuildingInfo getPeopleNumMinBuilding()
        {
            MarsBuildingInfo minBuilding = null;
            long minPeopleCount = long.MaxValue;
            foreach (MarsBuildingInfo info in _m_buildingInfos)
            {
                if (!info.settleSlotData.isValid() || info.settleSlotData.isMax)
                    continue;

                if (info.settleSlotData.peopleCount < minPeopleCount)
                {
                    minPeopleCount = info.settleSlotData.peopleCount;
                    minBuilding = info;
                }
            }

            return minBuilding;
        }
        public MarsBuildingInfo getUpgradableBuildingInfo()
        {
            MarsBuildingInfo levelLowestBuildingInfo = null;
            int minEquipmentRequire = int.MaxValue;
            foreach (MarsBuildingInfo info in _m_buildingInfos)
            {
                if (info.state is MarsBuildingInfo.StateType.Constructing or MarsBuildingInfo.StateType.Upgrading)
                    continue;

                if ((info.state is MarsBuildingInfo.StateType.Unbuilt && info.checkCanBuild()) ||
                    (info.state is MarsBuildingInfo.StateType.Normal && info.checkCanUpgrade()))
                {
                    // 计算这个建筑要升级还所需要的部件等级
                    int equipmentRequire = 0;
                    if (info.state == MarsBuildingInfo.StateType.Normal && info.equipmentData.isValid())
                        equipmentRequire = info.equipmentData.totalMainLevelEnd - info.equipmentData.totalMainLevel;

                    if (levelLowestBuildingInfo == null || equipmentRequire < minEquipmentRequire || info.level < levelLowestBuildingInfo.level)
                    {
                        levelLowestBuildingInfo = info;
                        minEquipmentRequire = equipmentRequire;
                    }
                }
            }
            
            return levelLowestBuildingInfo;
        }
        public MarsBuildingInfo getFirstEquipmentUpgradableBuilding()
        {
            MarsBuildingInfo firstBuildingInfo = null;
            foreach (MarsBuildingInfo info in _m_buildingInfos)
            {
                if (info.state != MarsBuildingInfo.StateType.Normal ||
                    !info.equipmentData.isValid())
                    continue;

                firstBuildingInfo ??= info;
                if (info.equipmentData.totalMainLevel < info.equipmentData.totalMainLevelEnd)
                    return info;
            }

            return firstBuildingInfo;
        }
        public int getCurrentQueueCount()
        {
            return _m_buildingQueueList.Count;
        }
        public long getBuildingEquipLevelSum()
        {
            int totalLevel = 0;
            foreach (MarsBuildingInfo info in _m_buildingInfos)
            {
                if (info.state is MarsBuildingInfo.StateType.Unbuilt or MarsBuildingInfo.StateType.Constructing)
                    continue;

                if (!info.equipmentData.isValid())
                    continue;

                totalLevel += info.equipmentData.getTotalLevel();
            }

            return totalLevel;
        }
        public long getBuildingLvlSum()
        {
            int totalLevel = 0;
            foreach (MarsBuildingInfo info in _m_buildingInfos)
            {
                if (info.state is MarsBuildingInfo.StateType.Unbuilt or MarsBuildingInfo.StateType.Constructing)
                    continue;

                totalLevel += info.level;
            }

            return totalLevel;
        }
        
        public void startBuild(long _id, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_001_ReqCreateBuilding(_id), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_001_RetCreateBuilding>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        public void completeNowBuildOrUpgrade(long _id, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_010_ReqSetBuildingDone(_id), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_010_RetSetBuildingDone>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        public void cancelBuild(long _id, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_011_ReqCancelBuildingUpgrade(_id),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_011_RetCancelBuildingUpgrade>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        public void confirmCreateBuilding(long _id, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_007_ReqConfirmCreateBuilding(_id),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_007_RetConfirmCreateBuilding>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        public void confirmUpgradeBuilding(long _id, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_008_ReqConfirmUpgradeBuildingLvl(_id),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_008_RetConfirmUpgradeBuildingLvl>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        /// <summary>
        /// 派遣居民 (GC2GS_039_004_ReqDispatchPeople)
        /// </summary>
        public void ReqDispatchPeople(long buildingId, int peopleNum, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_004_ReqDispatchPeople(buildingId, peopleNum),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_004_RetDispatchPeople>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        public void startUpgrade(long _buildingId, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_002_ReqUpgradeBuildingLvl(_buildingId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_002_RetUpgradeBuildingLvl>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        public void requestUpgradeEquipment(long _buildingId, long _equipmentId, Action _complete = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_005_ReqUpgradeEquipment(_buildingId, _equipmentId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_005_RetUpgradeEquipment>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        public void cancelUpgrade(long _buildingId, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_011_ReqCancelBuildingUpgrade(_buildingId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_011_RetCancelBuildingUpgrade>((_isSuc, _msg) =>
                {
                    _complete?.Invoke();
                }));
        }
        public void setSwitches(long _buildingId, bool _isOn, bool _isOverdrive)
        {
            MarsBuildingInfo buildingInfo = getBuildingInfoById(_buildingId);
            buildingInfo?._updateHomeSwitch(_isOn, _isOverdrive);
            NPGSClientListener.sendMsgByLog(new GC2GS_039_003_ReqSetHonePowerOn(_isOn, _isOverdrive));
        } 
        public void collectEnergy(Action<GS2GC_039_006_RetGetBuildingEnergy> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_006_ReqGetBuildingEnergy(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_006_RetGetBuildingEnergy>((_isSuc, _msg) => _complete?.Invoke(_msg)));
        }

        /// <summary>
        /// 更新主基地资源收集时间
        /// </summary>
        /// <param name="_timeS"></param>
        public void updateHomeCollectTime(int _timeS)
        {
            _m_iHomeCollectTimeS = _timeS;
        }

        /// <summary>
        /// 收集主基地资源
        /// </summary>
        public void collectHome()
        {
            //判断是否超出基本收集时间
            if (!canCollecHome)
                return;

            //直接设置收集时间，等待请求返回
            _m_iHomeCollectTimeS = (int)FpsAndPingMgr.instance.serverTimeTagS;
            //发送协议请求
            NPGSClientListener.sendMsgByLog(new GC2GS_039_009_ReqCollectHomeOutput());

            //发送采集行为消息
            WinMsg.SendMsg(WinMsgType.ON_MARS_COLLECT);
        }


        /***************** -下面是事件响应函数 *******************/

        internal void _onBuildingChg(GS2GC_039_050_OnBuildingChg _msg)
        {
            Mars_Building serverBuildingInfo = _msg?.getInfo();
            _onBuildingChg(serverBuildingInfo);
        }
        internal void _onBuildingChg(Mars_Building _msg)
        {
            if (_msg == null)
                return;
            
            MarsBuildingInfo buildingInfo = getBuildingInfoById(_msg.getBuildingId());
            if (buildingInfo == null)
            {
                ALLog.Error($"[MarsBuildingSubComponent] _onBuildingChg: 未找到对应的建筑信息, buildingId={_msg.getBuildingId()}");
                return;
            }
            
            MarsBuildingInfo.StateType oldState = buildingInfo.state;
            int oldLevel = buildingInfo.level;
            buildingInfo._updateData(_msg);
            if (oldState != buildingInfo.state)
                onBuildingStateChg?.Invoke(buildingInfo.refObj.id, oldState, buildingInfo.state);
            if (oldLevel != buildingInfo.level)
                onBuildingLevelChg?.Invoke(buildingInfo.refObj.id, oldLevel);

            WinMsg.SendMsg(WinMsgType.ON_MARS_BUILDING_CHG, _msg.getBuildingId());
        }
        internal void _onEquipmentChg(GS2GC_039_053_OnEquipmentChg _msg)
        {
            Mars_BuildingEquipment serverEquipmentInfo = _msg?.getInfo();
            _onEquipmentChg(serverEquipmentInfo);
        }
        internal void _onEquipmentChg(Mars_BuildingEquipment _msg)
        {
            if (_msg == null)
                return;
            
            MarsBuildingInfo buildingInfo = getBuildingInfoById(_msg.getBuildingId());
            if (buildingInfo == null)
            {
                ALLog.Error($"[MarsBuildingSubComponent] _onEquipmentChg: 未找到对应的建筑信息, buildingId={_msg.getBuildingId()}");
                return;
            }
            
            buildingInfo._updateEquipmentData(_msg);
            onEquipmentUpgraded?.Invoke(buildingInfo.refObj.id, _msg.getEquipmentId());

            WinMsg.SendMsg(WinMsgType.ON_MARS_BUILDING_EQUIPMENT_CHG, _msg.getBuildingId(), _msg.getEquipmentId());
        }
        internal void _onPeopleBuildingChg(GS2GC_039_052_OnPeopleChg _msg)
        {
            Mars_PeopleBuilding serverPeopleBuildingInfo = _msg?.getInfo();
            _onPeopleBuildingChg(serverPeopleBuildingInfo);
        }
        internal void _onPeopleBuildingChg(Mars_PeopleBuilding _msg)
        {
            if (_msg == null)
                return;
            
            MarsBuildingInfo buildingInfo = getBuildingInfoById(_msg.getBuildingId());
            if (buildingInfo == null)
            {
                ALLog.Error($"[MarsBuildingSubComponent] _onPeopleBuildingChg: 未找到对应的建筑信息, buildingId={_msg.getBuildingId()}");
                return;
            }

            buildingInfo._updatePeopleCount(_msg);
            
            // 人员派遣发生变化时，触发已派遣人数总数重算
            _m_parentComponent.needUpdateDispatchedPeopleNum();
            
            onBuildingSettleChg?.Invoke(buildingInfo.refObj.id);

            WinMsg.SendMsg(WinMsgType.ON_MARS_BUILDING_DISPATCH_PEOPLE_CHG, _msg.getBuildingId());
        }
        internal void _onEnergyOutputChg(GS2GC_039_054_OnEnergyOutputChg _msg)
        {
            Mars_BuildingEnergyOutput serverEnergyOutputInfo = _msg?.getInfo();
            _onEnergyOutputChg(serverEnergyOutputInfo);
        }
        internal void _onEnergyOutputChg(Mars_BuildingEnergyOutput _msg)
        {
            if (_msg == null)
                return;
            
            MarsBuildingInfo buildingInfo = getBuildingInfoById(_msg.getBuildingId());
            if (buildingInfo == null)
            {
                ALLog.Error($"[MarsBuildingSubComponent] _onEnergyOutputChg: 未找到对应的建筑信息, buildingId={_msg.getBuildingId()}");
                return;
            }
            
            buildingInfo._updateEnergyOutput(_msg);
        }
        internal void _onQueueAdd(GS2GC_039_056_OnBuildingUpQueueAdd _msg)
        {
            Mars_BuildingUpQueue serverQueueInfo = _msg?.getInfo();
            _onQueueAdd(serverQueueInfo);
        }
        internal void _onQueueAdd(Mars_BuildingUpQueue _msg)
        {
            if (_msg == null)
                return;
            
            MarsBuildingInfo buildingInfo = getBuildingInfoById(_msg.getBuildingId());
            if (buildingInfo == null)
            {
                ALLog.Error($"[MarsBuildingSubComponent] _onQueueAdd: 未找到对应的建筑信息, buildingId={_msg.getBuildingId()}");
                return;
            }
            
            _m_buildingQueueList.Add(buildingInfo);
            
            MarsBuildingInfo.StateType oldState = buildingInfo.state;
            buildingInfo._updateConstructingOrUpgrading(_msg);
            if (oldState != buildingInfo.state)
                onBuildingStateChg?.Invoke(buildingInfo.refObj.id, oldState, buildingInfo.state);
            
            WinMsg.SendMsg(WinMsgType.ON_MARS_BUILDING_CHG, _msg.getBuildingId());
            onQueueListChg?.Invoke();
        }
        internal void onItemHelpSecsChg(long _objId, int _secs)
        {
            MarsBuildingInfo buildingInfo = getBuildingInfoById(_objId);
            if (buildingInfo == null)
            {
                ALLog.Error($"[MarsBuildingSubComponent] onItemHelpSecsChg: 未找到对应的建筑信息, _objId={_objId}");
                return;
            }
            
            MarsBuildingInfo.StateType oldState = buildingInfo.state;
            buildingInfo.onItemHelpSecsChg(_secs);
            onBuildingTimeChg?.Invoke(buildingInfo.refObj.id);
            if (oldState != buildingInfo.state)
                onBuildingStateChg?.Invoke(buildingInfo.refObj.id, oldState, buildingInfo.state);
            
            WinMsg.SendMsg(WinMsgType.ON_MARS_BUILDING_CHG, buildingInfo.buildRefId);
            onQueueListChg?.Invoke();
        }
        internal void _onQueueRemove(GS2GC_039_057_OnBuildingUpQueueDel _msg)
        {
            if (_msg == null)
                return;

            MarsBuildingInfo buildingInfo = getBuildingInfoByQueueId(_msg.getId());
            if (buildingInfo == null)
            {
                ALLog.Error($"[MarsBuildingSubComponent] _onQueueRemove: 未找到对应的建筑信息, queueId={_msg.getId()}");
                return;
            }
            
            _m_buildingQueueList.Remove(buildingInfo);
            MarsBuildingInfo.StateType oldState = buildingInfo.state;
            buildingInfo._updateConstructingOrUpgrading(null);
            if (oldState != buildingInfo.state)
                onBuildingStateChg?.Invoke(buildingInfo.refObj.id, oldState, buildingInfo.state);
            
            WinMsg.SendMsg(WinMsgType.ON_MARS_BUILDING_CHG, buildingInfo.buildRefId);
            onQueueListChg?.Invoke();
        }
        public void _onExtMoodIndexChg(GS2GC_039_061_OnExtMoodIndexChg _msg)
        {
            if (_msg == null)
                return;

            _onExtMoodIndexChg(_msg.getValue());
        }
        public void _onExtMoodIndexChg(long _newIndex)
        {
            _m_extMoodIndex = _newIndex;
            _m_parentComponent.needUpdateMoodValue();
        }
        
         
        private void _onMarsEnergyChg()
        {
            long energyValue = NPPlayer.instance.specialItemComp.marsEnergyData.getValue();
            foreach (MarsBuildingInfo info in _m_buildingInfos)
                info._onMarsEnergyChg(energyValue);
        }
        private void _onPlayerPropertyChg(ENPPlayerPropertyType _type, long _, long __)
        {
            switch (_type)
            {
                case ENPPlayerPropertyType.MARS_ENERGY_OUTPUT_PER:
                    foreach (MarsBuildingInfo info in _m_buildingInfos)
                        info.energyProperty._setDirty();
                    break;
            }
        }
    }
}
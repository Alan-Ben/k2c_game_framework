using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using Common.MarsEnum;
using Common.MarsObj;
using CommonEnum;
using GC2GS.p039_MarsBuildingOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class MarsBuildingInfo : _IMarsTimeSpeedUpObject, _IMarsCompleteNowObject, _IGuildMarsHelp
    {
        public enum StateType
        {
            [InspectorName("未建造")]
            Unbuilt,
            [InspectorName("建造中")]
            Constructing,
            [InspectorName("升级中")]
            Upgrading,
            [InspectorName("正常状态")]
            Normal,
        }


        [NotNull] private readonly MarsBuildingRefObj _m_refObj;
        [ItemNotNull, NotNull] private readonly List<_AMarsBuildingInfoProperty> _m_propertyList;
        [NotNull] private readonly MarsBuildingInfoPowerProperty _m_powerProperty;
        [NotNull] private readonly MarsBuildingInfoEnergyProperty _m_energyProperty;
        [NotNull] private readonly MarsBuildingInfoSatietyYieldProperty _m_satietyYieldProperty;
        [NotNull] private readonly MarsBuildingInfoCureRateProperty _m_cureRateProperty;
        [NotNull] private readonly MarsBuildingInfoComfortYieldProperty  _m_comfortYieldProperty;
        [NotNull] private readonly MarsBuildingInfoMoodYieldProperty _m_moodYieldProperty;
        [NotNull] private readonly MarsBuildingInfoSleepYieldProperty _m_sleepYieldProperty;
        [NotNull] private readonly MarsBuildingInfoPeopleNumLimitProperty _m_peopleNumLimitProperty;
        [NotNull] private readonly MarsBuildingInfoOxygenYieldProperty _m_oxygenYieldProperty;
        [NotNull] private readonly MarsBuildingInfoSlotNumLimitProperty _m_slotNumLimitProperty;
        
        [NotNull] private readonly MarsBuildingInfoLevelData _m_levelData;
        [NotNull] private readonly MarsBuildingInfoEquipmentData _m_equipmentData;
        [NotNull] private readonly MarsBuildingInfoSettleSlotData _m_settleSlotData;
        [NotNull] private readonly MarsBuildingInfoHomeData _m_homeData;

        private MarsBuildingSubComponent _m_component;
        
        private StateType _m_state;
        private long _m_buildOrUpgradeStartTime;
        private long _m_oriBuildOrUpgradeEndTime;//未减秒数的结束时间
        private int _m_level;
        private int _m_serverLevel;
        private long _m_queueId;
        private long _m_guildHelpId;
        private long _m_guildHelpSecs;
        private long _m_lItemHelpSecs;//道具帮助减少的秒数
        
        private bool _m_propertyAdded;
        
        
        public MarsBuildingInfo([NotNull] MarsBuildingRefObj _refObj)
        {
            _m_refObj = _refObj;
            
            _m_propertyList = new List<_AMarsBuildingInfoProperty>();
            _m_propertyList.Add(_m_powerProperty = new MarsBuildingInfoPowerProperty(this));
            _m_propertyList.Add(_m_energyProperty = new MarsBuildingInfoEnergyProperty(this));
            _m_propertyList.Add(_m_satietyYieldProperty = new MarsBuildingInfoSatietyYieldProperty(this));
            _m_propertyList.Add(_m_cureRateProperty = new MarsBuildingInfoCureRateProperty(this));
            _m_propertyList.Add(_m_comfortYieldProperty = new MarsBuildingInfoComfortYieldProperty(this));
            _m_propertyList.Add(_m_moodYieldProperty = new MarsBuildingInfoMoodYieldProperty(this));
            _m_propertyList.Add(_m_sleepYieldProperty = new MarsBuildingInfoSleepYieldProperty(this));
            _m_propertyList.Add(_m_peopleNumLimitProperty = new MarsBuildingInfoPeopleNumLimitProperty(this));
            _m_propertyList.Add(_m_oxygenYieldProperty = new MarsBuildingInfoOxygenYieldProperty(this));
            _m_propertyList.Add(_m_slotNumLimitProperty = new MarsBuildingInfoSlotNumLimitProperty(this));
            
            _m_levelData = new MarsBuildingInfoLevelData(this);
            _m_equipmentData = new MarsBuildingInfoEquipmentData(this);
            _m_settleSlotData = new MarsBuildingInfoSettleSlotData(this);
            _m_homeData = new MarsBuildingInfoHomeData(this);
            
            _m_propertyAdded = false;
            _m_serverLevel = 0;
            _m_queueId = 0;
            _m_guildHelpId = 0;
            _updateLevel(1);
            _setState(StateType.Unbuilt);
        }
        
        
        [NotNull] public MarsBuildingRefObj refObj { get { return _m_refObj; } }
        public long buildRefId { get { return _m_refObj.id; } }
        [NotNull] public MarsBuildingInfoPowerProperty powerProperty { get { return _m_powerProperty; } }
        [NotNull] public MarsBuildingInfoEnergyProperty energyProperty { get { return _m_energyProperty; } }
        [NotNull] public MarsBuildingInfoSatietyYieldProperty satietyYieldProperty { get { return _m_satietyYieldProperty; } }
        [NotNull] public MarsBuildingInfoCureRateProperty cureRateProperty { get { return _m_cureRateProperty; } }
        [NotNull] public MarsBuildingInfoComfortYieldProperty comfortYieldProperty { get { return _m_comfortYieldProperty; } }
        [NotNull] public MarsBuildingInfoMoodYieldProperty moodYieldProperty { get { return _m_moodYieldProperty; } }
        [NotNull] public MarsBuildingInfoSleepYieldProperty sleepYieldProperty { get { return _m_sleepYieldProperty; } }
        [NotNull] public MarsBuildingInfoPeopleNumLimitProperty peopleNumLimitProperty { get { return _m_peopleNumLimitProperty; } }
        [NotNull] public MarsBuildingInfoOxygenYieldProperty oxygenYieldProperty { get { return _m_oxygenYieldProperty; } }
        [NotNull] public MarsBuildingInfoSlotNumLimitProperty slotNumLimitProperty { get { return _m_slotNumLimitProperty; } }
        
        [NotNull] public MarsBuildingInfoLevelData levelData { get { return _m_levelData; } }
        [NotNull] public MarsBuildingInfoEquipmentData equipmentData { get { return _m_equipmentData; } }
        [NotNull] public MarsBuildingInfoSettleSlotData settleSlotData { get { return _m_settleSlotData; } }
        [NotNull] public MarsBuildingInfoHomeData homeData { get { return _m_homeData; } }
        
        public MarsBuildingSubComponent component { get { return _m_component; } }
        public EMarsBuildingType type { get { return _m_refObj.building_type; } }
        public string nameTranslated { get { return TextTranslate.instance.getLanguage(_m_refObj.name, _m_refObj.name_args); } }
        public string descTranslated { get { return TextTranslate.instance.getLanguage(_m_refObj.desc); } }
        public int level { get { return _m_level; } }
        public StateType state { get { return _m_state; } }
        public long buildOrUpgradeStartTime { get { return _m_buildOrUpgradeStartTime; } }
        public long buildOrUpgradeEndTime { get { return _m_oriBuildOrUpgradeEndTime - _m_guildHelpSecs * 1000 - _m_lItemHelpSecs * 1000; } }
        public long remainingBuildOrUpgradeTime
        {
            get
            {
                if (_m_state is not (StateType.Constructing or StateType.Upgrading))
                    return 0;
                
                long nowTimeMS = FpsAndPingMgr.instance.serverTimeTag;
                long remainingTime = buildOrUpgradeEndTime - nowTimeMS;
                return remainingTime > 0 ? remainingTime : 0;
            }
        }
        public bool isEnable { get { return _m_state is StateType.Normal or StateType.Upgrading; } }
        public long queueId { get { return _m_queueId; } }
        
        internal bool _propertyAdded { get { return _m_propertyAdded; } }
        
        
        public EGuildMarsHelpObjType helpObjType => EGuildMarsHelpObjType.BUILDING_QUEUE;

        public long guildHelpObjId => _m_queueId;

        public long guildHelpId => _m_guildHelpId;

        public void init(MarsBuildingSubComponent _component)
        {
            if (_m_component != null)
            {
                ALLog.Error("Init MarsBuildingInfo repeated!");
                return;
            }
            
            _m_component = _component;
            _updatePropertyState();
        }
        public void discard()
        {
            if (_m_component == null)
            {
                ALLog.Error("Discard MarsBuildingInfo without init!");
                return;
            }
            
            if (_m_propertyAdded)
            {
                _removeProperty();
                _m_propertyAdded = false;
            }
            _m_component = null;
        }
        /// <summary>
        /// 判断建筑是否可以建造
        /// </summary>
        public bool checkCanBuild(bool _checkResEnough = true)
        {
            if (!MarsUtil.checkConditionIdListEnable(_m_refObj.build_condition_id_list))
                return false;

            if (_checkResEnough)
            {
                if (!GCommon.isItemEnough(_m_refObj.build_cost_list, false))
                    return false;
            }
            
            return true;
        }
        public bool checkCanUpgrade(bool _checkResEnough = true)
        {
            if (!_m_levelData.isValid() || _m_levelData.isLevelMax)
                return false;

            if (!MarsUtil.checkConditionIdListEnable(_m_levelData.refObj.upgrade_condition_id_list))
                return false;

            if (_checkResEnough)
            {
                List<NPCommonCostItem> upgradeCostList = _m_levelData.getUpgradeCostList();
                if (!GCommon.isItemEnough(upgradeCostList, false))
                    return false;
            }

            return true;
        }
        public bool checkCanShowBuildBtn()
        {
            if (_m_component == null)
                return false;
            
            MarsBuildingInfo buildingInfo = _m_component.getBuildingInfoByBuildingOrder(_m_refObj.building_order_in_number - 1);
            if (buildingInfo is { _m_state: StateType.Unbuilt } or { _m_state: StateType.Constructing })
                return false;

            return _m_refObj.build_btn_show_condition == null || _m_refObj.build_btn_show_condition.IsEnable(null);
        }
        public NPGGoIndex getCurrentResIndex()
        {
            switch (_m_state)
            {
                case StateType.Unbuilt:
                    return _m_refObj.unbuilt_res_index; // 未建造时的资源
                case StateType.Constructing:
                    return _m_refObj.building_res_index; // 建造中时的资源
                case StateType.Upgrading:
                {
                    if (_m_levelData.isValid()) // 如果有等级数据，返回等级配置中的升级中资源
                        return _m_levelData.getUpgradingResIndex();
                    return _m_refObj.building_res_index; // 否则返回建造中资源
                }
                case StateType.Normal:
                {
                    if (_m_levelData.isValid()) // 如果有等级数据，返回等级配置中的当前资源
                        return _m_levelData.getCurrentResIndex();
                    return _m_refObj.built_res_index; // 否则返回建造完成资源
                }
                default:
                    return _m_refObj.built_res_index; // 其他状态返回建造完成资源
            }
        }
        public void actionWithAllProperty(Action<_AMarsBuildingInfoProperty> _action)
        {
            if (_action == null)
                return;
            
            foreach (_AMarsBuildingInfoProperty property in _m_propertyList)
            {
                _action.Invoke(property);
            }
        }
        public long realBuildTimeMS()
        {
            long value = refObj.build_time_cost_sec * 1000;
            long property = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_BUILDING_UP_TIME_PER);
            value = value * 10000 / (10000 + property);
            return value;
        }
        
        
        internal void _setState(StateType _newState)
        {
            if (_m_state == _newState)
                return;

            _m_state = _newState;
            
            switch (_m_state)
            {
                case StateType.Unbuilt:
                    break;
                case StateType.Constructing:
                    NPPlayer.instance.guildMarsHelpComp.regMyMarsHelp(this);
                    break;
                case StateType.Upgrading:
                    NPPlayer.instance.guildMarsHelpComp.regMyMarsHelp(this);
                    break;
                case StateType.Normal:
                    NPPlayer.instance.guildMarsHelpComp.unregMyMarsHelp(this);
                    break;
            }
            _updatePropertyState();
        }
        internal void _updateTime()
        {
            long nowTimeMS = FpsAndPingMgr.instance.serverTimeTag;
            // 如果建造或升级中，刷新建造和升级情况
            if (_m_state is StateType.Constructing or StateType.Upgrading)
            {
                if (nowTimeMS >= buildOrUpgradeEndTime)
                {
                    if (_m_state == StateType.Constructing)
                        NPGSClientListener.sendMsgByLog(new GC2GS_039_007_ReqConfirmCreateBuilding(refObj.id));
                    else
                        NPGSClientListener.sendMsgByLog(new GC2GS_039_008_ReqConfirmUpgradeBuildingLvl(refObj.id));
                    
                    _setState(StateType.Normal);
                    // 添加未读建筑变化记录
                    AccountSettingMgr.instance.unreadMarsBuildingChangeSaver?.addUnreadBuildingId(refObj.id);
                }
            }
        }
        internal void _updateData(Mars_Building _serverBuildingInfo)
        {
            if (_serverBuildingInfo == null)
                return;

            _m_serverLevel = _serverBuildingInfo.getLvl();
            _updateBuildingState();
        }
        internal void _updateConstructingOrUpgrading(Mars_BuildingUpQueue _buildingUpQueue)
        {
            _m_queueId = _buildingUpQueue?.getId() ?? 0;
            _m_buildOrUpgradeStartTime = _buildingUpQueue?.getStartUpgradeLvlMs() ?? 0;
            _m_oriBuildOrUpgradeEndTime = _buildingUpQueue?.getEndUpgradeLvlMs() ?? 0;
            _m_guildHelpId = _buildingUpQueue?.getGuildHelpId() ?? 0;
            _m_guildHelpSecs = _buildingUpQueue?.getGuildHelpSecs() ?? 0;
            _m_lItemHelpSecs = _buildingUpQueue?.getItemHelpSecs() ?? 0;
            _updateBuildingState();
        }
        internal void _updateEquipmentData(Mars_BuildingEquipment _serverEquipmentInfo)
        {
            _m_equipmentData._updateData(_serverEquipmentInfo);
        }
        internal void _updatePeopleCount(Mars_PeopleBuilding _serverPeopleInfo)
        {
            if (_serverPeopleInfo == null)
                return;
            
            _m_settleSlotData._updatePeopleCount(_serverPeopleInfo.getPeopleNum());
        }
        internal void _updateEnergyOutput(Mars_BuildingEnergyOutput _serverEnergyInfo)
        {
            _m_energyProperty._updateValue(_serverEnergyInfo);
        }
        internal void _updateHomeSwitch(bool _isOn, bool _isOverdrive)
        {
            _m_oxygenYieldProperty._setSwitches(_isOn, _isOverdrive);
        }
        internal void _updateFoodSwitch(bool _isOn)
        {
            _m_satietyYieldProperty._setSwitchOn(_isOn);
        }
        internal void _onMarsEnergyChg(long _energyValue)
        {
            if (_m_oxygenYieldProperty.energyConsumePerMin > 0 && _energyValue <= 0)
            {
                if (_m_oxygenYieldProperty.isOn)
                {
                    _updateHomeSwitch(false, _m_oxygenYieldProperty.isOverdrive);
                    NPGSClientListener.sendMsgByLog(new GC2GS_039_012_ReqHomeBuildingPowerOff());
                }
            }
            if (_m_satietyYieldProperty.consumePerMin > 0)
            {
                if (_m_satietyYieldProperty.switchOn && _energyValue <= 0)
                {
                    _updateFoodSwitch(false);
                    NPGSClientListener.sendMsgByLog(new GC2GS_039_013_ReqFoodBuildingPowerChg(_m_refObj.id, false));
                }
                else if (!_m_satietyYieldProperty.switchOn && _energyValue > 0)
                {
                    _updateFoodSwitch(true);
                    NPGSClientListener.sendMsgByLog(new GC2GS_039_013_ReqFoodBuildingPowerChg(_m_refObj.id, true));
                }
            }
        }


        private void _updateBuildingState()
        {
            long nowTimeMS = FpsAndPingMgr.instance.serverTimeTag;
            if (_m_serverLevel <= 0)
            {
                if (_m_queueId <= 0)
                    _setState(StateType.Unbuilt);
                else
                {
                    if (buildOrUpgradeEndTime > nowTimeMS)
                        _setState(StateType.Constructing);
                    else
                    {
                        NPGSClientListener.sendMsgByLog(new GC2GS_039_007_ReqConfirmCreateBuilding(refObj.id));
                        _setState(StateType.Normal);
                    }
                }
            }
            else
            {
                _updateLevel(_m_serverLevel);
                if (_m_queueId > 0)
                {
                    if (buildOrUpgradeEndTime > nowTimeMS)
                        _setState(StateType.Upgrading);
                    else
                    {
                        NPGSClientListener.sendMsgByLog(new GC2GS_039_008_ReqConfirmUpgradeBuildingLvl(refObj.id));
                        _setState(StateType.Normal);
                    }
                }
                else
                    _setState(StateType.Normal);
            }
        }
        private void _updatePropertyState()
        {
            if (_m_component == null)
                return;

            bool shouldHaveProperty = isEnable;
            switch (shouldHaveProperty)
            {
                case true when !_m_propertyAdded:
                    _addProperty();
                    _m_propertyAdded = true;
                    break;
                case false when _m_propertyAdded:
                    _removeProperty();
                    _m_propertyAdded = false;
                    break;
            }
        }
        private void _updateLevel(int _level)
        {
            _m_level = _level;
            _m_levelData._updateLevel(_m_level);
            _m_equipmentData._updateLevel(_m_level);
            _m_settleSlotData._updateLevel(_m_level);
            _m_homeData._updateLevel(_m_level);
        }
        private void _addProperty()
        {
            _m_levelData._addProperty();
            _m_equipmentData._addProperty();
            _m_settleSlotData._addProperty();
            _m_homeData._addProperty();
        }
        private void _removeProperty()
        {
            _m_levelData._removeProperty();
            _m_equipmentData._removeProperty();
            _m_settleSlotData._removeProperty();
            _m_homeData._removeProperty();
        }


        public void onGuildHelpChg(long _guildHelpId, long _guildHelpSecs)
        {
            _m_guildHelpId = _guildHelpId;
            _m_guildHelpSecs = _guildHelpSecs;
            _updateBuildingState();
        }

        public void onGuildHelpDel()
        {
            
        }
        
        public void onItemHelpSecsChg(long _itemHelpSecs)
        {
            _m_lItemHelpSecs = _itemHelpSecs;
            _updateBuildingState();
        }
        
        #region _IMarsTimeSpeedUpObject字段

        public EMarsBagItemUseTimeType timeType { get { return EMarsBagItemUseTimeType.MARS_BUILDING; } }
        public long timeObjId { get { return _m_refObj.id; } }
        long _IMarsTimeSpeedUpObject.remainTimeMs { get { return remainingBuildOrUpgradeTime; } }
        public long beforeReductionTotalTimeMs { get { return _m_oriBuildOrUpgradeEndTime - _m_buildOrUpgradeStartTime; } }
        public long afterReductionTotalTimeMs { get { return afterReductionTotalTimeMs - _m_buildOrUpgradeStartTime; } }

        #endregion

        #region _IMarsCompleteNowObject字段

        public bool checkCanCompleteNow(bool _checkCompleteNowCostEnough, bool _showUnableTip)
        {
            // 未建造状态下, 想要立刻完成建造, 需要先检查是否可以建造
            if(state is StateType.Unbuilt && !checkCanBuild())
            {
                if(_showUnableTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_buildingCantBuildTip_none);
                return false;
            }
            
            // 普通状态下, 想要立刻完成, 需要先检查是否可以升级
            if (state is StateType.Normal && !checkCanUpgrade())
            {
                if(_showUnableTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_buildingCantUpgradeTip_none);
                
                return false;
            }

            // 若需要检查立即完成消耗是否足够, 且 立即完成消耗不足
            if (_checkCompleteNowCostEnough && !GCommon.isItemEnough(completeNowCostItem, _showUnableTip))
            {
                return false;
            }

            return true;
        }
        
        public bool canCompleteNow { get { return _m_state is StateType.Constructing or StateType.Upgrading or StateType.Normal or StateType.Unbuilt; } }

        public bool hasRemainTime { get { return _m_state is StateType.Constructing or StateType.Upgrading; } }//在建筑中或升级中才有剩余时间

        long _IMarsCompleteNowObject.remainTimeMs { get { return remainingBuildOrUpgradeTime; } }

        private NPCommonCostItem _m_completeNowCostItem;

        public NPCommonCostItem completeNowCostItem
        {
            get
            {
                if (_m_completeNowCostItem == null)
                    _m_completeNowCostItem = new NPCommonCostItem(ENPItemType.CURRENCY, (int) ECurrency.GEM, 0);
                long gemCost = 0;

                if (hasRemainTime)
                {
                    gemCost = MarsUtil.calculateGemCostForSpeedUpMs(remainingBuildOrUpgradeTime);
                }
                else if (state == StateType.Unbuilt)
                {
                    gemCost = MarsUtil.calculateGemCostForSpeedUpMs(realBuildTimeMS());
                }
                else
                {
                    gemCost = MarsUtil.calculateGemCostForSpeedUpMs(_m_levelData.realUpgradeTimeMS());
                }
                _m_completeNowCostItem.setCount(gemCost);

                return _m_completeNowCostItem;
            }
        }

        public Action<Action<bool>> dealCompleteNowAction { get { return reqCompleteNow; } }

        public void reqCompleteNow(Action<bool> _onComplete)
        {
            if(!checkCanCompleteNow(true, true))
            {
                _onComplete?.Invoke(false);
                return;
            }
            
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.marsComp.buildingSubComponent.completeNowBuildOrUpgrade(refObj.id, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                
                _onComplete?.Invoke(true);
            });
        }

        #endregion
    }
}
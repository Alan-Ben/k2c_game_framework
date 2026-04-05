using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        // Mars Building Level 管理
        [NotNull] private readonly Dictionary<long, List<MarsBuildingLevelRefObj>> _m_marsBuildingLevelDictionary = new Dictionary<long, List<MarsBuildingLevelRefObj>>();
        // Mars Building Home Level 管理
        [NotNull] private readonly List<MarsBuildingHomeLevelRefObj> _m_marsBuildingHomeLevelList = new List<MarsBuildingHomeLevelRefObj>();
        // Mars Building Settle Level 管理  
        [NotNull] private readonly Dictionary<long, List<MarsBuildingSettleLevelRefObj>> _m_marsBuildingSettleLevelDictionary = new Dictionary<long, List<MarsBuildingSettleLevelRefObj>>();
        // Mars Equipment Level 管理
        [NotNull] private readonly Dictionary<long, List<MarsEquipmentLevelRefObj>> _m_marsEquipmentLevelDictionary = new Dictionary<long, List<MarsEquipmentLevelRefObj>>();
        // Mars Equipment Energy Level 管理
        [NotNull] private readonly Dictionary<long, List<MarsEquipmentEnergyLevelRefObj>> _m_marsEquipmentEnergyLevelDictionary = new Dictionary<long, List<MarsEquipmentEnergyLevelRefObj>>();
        // Mars Equipment Food Level 管理
        [NotNull] private readonly Dictionary<long, List<MarsEquipmentFoodLevelRefObj>> _m_marsEquipmentFoodLevelDictionary = new Dictionary<long, List<MarsEquipmentFoodLevelRefObj>>();
        // Mars Equipment Living Level 管理
        [NotNull] private readonly Dictionary<long, List<MarsEquipmentLivingLevelRefObj>> _m_marsEquipmentLivingLevelDictionary = new Dictionary<long, List<MarsEquipmentLivingLevelRefObj>>();
        // Mars Equipment Hospital Level 管理
        [NotNull] private readonly Dictionary<long, List<MarsEquipmentHospitalLevelRefObj>> _m_marsEquipmentHospitalLevelDictionary = new Dictionary<long, List<MarsEquipmentHospitalLevelRefObj>>();
        // Mars Building Equipment Belong 管理
        [NotNull] private readonly Dictionary<long, MarsBuildingEquipmentBelongRefObj> _m_marsBuildingEquipmentBelongDictionary = new Dictionary<long, MarsBuildingEquipmentBelongRefObj>();

        
        /// <summary>
        /// 获取火星建筑等级配置
        /// </summary>
        public MarsBuildingLevelRefObj getMarsBuildingLevelRef(long _groupId, int _level)
        {
            List<MarsBuildingLevelRefObj> levelList = _m_marsBuildingLevelDictionary.GetValueOrDefault(_groupId);
            if (levelList == null || _level <= 0 || _level > levelList.Count)
                return null;
            
            return levelList[_level - 1];
        }
        /// <summary>
        /// 获取火星建筑等级配置列表
        /// </summary>
        [NotNull]
        public List<MarsBuildingLevelRefObj> getMarsBuildingLevelRefList(long _groupId)
        {
            return _m_marsBuildingLevelDictionary.GetValueOrDefault(_groupId) ?? new List<MarsBuildingLevelRefObj>();
        }
        /// <summary>
        /// 获取火星主基地等级配置
        /// </summary>
        public MarsBuildingHomeLevelRefObj getMarsBuildingHomeLevelRef(int _level)
        {
            if (_level <= 0 || _level > _m_marsBuildingHomeLevelList.Count)
                return null;
            
            return _m_marsBuildingHomeLevelList[_level - 1];
        }
        /// <summary>
        /// 获取火星建筑派遣等级配置
        /// </summary>
        public MarsBuildingSettleLevelRefObj getMarsBuildingSettleLevelRef(long _groupId, int _level)
        {
            List<MarsBuildingSettleLevelRefObj> levelList = _m_marsBuildingSettleLevelDictionary.GetValueOrDefault(_groupId);
            if (levelList == null || _level <= 0 || _level > levelList.Count)
                return null;
            
            return levelList[_level - 1];
        }
        /// <summary>
        /// 获取火星建筑派遣等级配置列表
        /// </summary>
        [NotNull]
        public List<MarsBuildingSettleLevelRefObj> getMarsBuildingSettleLevelRefList(long _groupId)
        {
            return _m_marsBuildingSettleLevelDictionary.GetValueOrDefault(_groupId) ?? new List<MarsBuildingSettleLevelRefObj>();
        }
        /// <summary>
        /// 获取火星部件等级配置
        /// </summary>
        public MarsEquipmentLevelRefObj getMarsEquipmentLevelRef(long _groupId, int _level)
        {
            List<MarsEquipmentLevelRefObj> levelList = _m_marsEquipmentLevelDictionary.GetValueOrDefault(_groupId);
            if (levelList == null || _level <= 0 || _level > levelList.Count)
                return null;
            
            return levelList[_level - 1];
        }
        /// <summary>
        /// 获取火星部件能源等级配置
        /// </summary>
        public MarsEquipmentEnergyLevelRefObj getMarsEquipmentEnergyLevelRef(long _groupId, int _level)
        {
            List<MarsEquipmentEnergyLevelRefObj> levelList = _m_marsEquipmentEnergyLevelDictionary.GetValueOrDefault(_groupId);
            if (levelList == null || _level <= 0 || _level > levelList.Count)
                return null;
            
            return levelList[_level - 1];
        }
        /// <summary>
        /// 获取火星部件食物等级配置
        /// </summary>
        public MarsEquipmentFoodLevelRefObj getMarsEquipmentFoodLevelRef(long _groupId, int _level)
        {
            List<MarsEquipmentFoodLevelRefObj> levelList = _m_marsEquipmentFoodLevelDictionary.GetValueOrDefault(_groupId);
            if (levelList == null || _level <= 0 || _level > levelList.Count)
                return null;
            
            return levelList[_level - 1];
        }
        /// <summary>
        /// 获取火星部件生活等级配置
        /// </summary>
        public MarsEquipmentLivingLevelRefObj getMarsEquipmentLivingLevelRef(long _groupId, int _level)
        {
            List<MarsEquipmentLivingLevelRefObj> levelList = _m_marsEquipmentLivingLevelDictionary.GetValueOrDefault(_groupId);
            if (levelList == null || _level <= 0 || _level > levelList.Count)
                return null;
            
            return levelList[_level - 1];
        }
        /// <summary>
        /// 获取火星部件医院等级配置
        /// </summary>
        public MarsEquipmentHospitalLevelRefObj getMarsEquipmentHospitalLevelRef(long _groupId, int _level)
        {
            List<MarsEquipmentHospitalLevelRefObj> levelList = _m_marsEquipmentHospitalLevelDictionary.GetValueOrDefault(_groupId);
            if (levelList == null || _level <= 0 || _level > levelList.Count)
                return null;
            
            return levelList[_level - 1];
        }
        /// <summary>
        /// 获取火星建筑部件归属配置
        /// </summary>
        public MarsBuildingEquipmentBelongRefObj getMarsBuildingEquipmentBelongRef(long _groupId)
        {
            return _m_marsBuildingEquipmentBelongDictionary.GetValueOrDefault(_groupId);
        }
        

        private void _initMarsBuilding()
        {
            // 初始化 Mars Building Level
            marsBuildingLevelRefCore?.dealAllRef(_levelRef =>
            {
                if (_levelRef == null)
                    return;

                List<MarsBuildingLevelRefObj> levelList = _m_marsBuildingLevelDictionary.getValueDefinitely(_levelRef.group_id);
                levelList.Add(_levelRef);
            });
            // 初始化 Mars Building Home Level
            marsBuildingHomeLevelRefCore?.dealAllRef(_homeLevelRef =>
            {
                if (_homeLevelRef == null)
                    return;

                _m_marsBuildingHomeLevelList.Add(_homeLevelRef);
            });
            // 初始化 Mars Building Settle Level
            marsBuildingSettleLevelRefCore?.dealAllRef(_settleLevelRef =>
            {
                if (_settleLevelRef == null)
                    return;

                List<MarsBuildingSettleLevelRefObj> levelList = _m_marsBuildingSettleLevelDictionary.getValueDefinitely(_settleLevelRef.group_id);
                levelList.Add(_settleLevelRef);
            });
            // 初始化 Mars Equipment Level
            marsEquipmentLevelRefCore?.dealAllRef(_equipmentLevelRef =>
            {
                if (_equipmentLevelRef == null)
                    return;

                List<MarsEquipmentLevelRefObj> levelList = _m_marsEquipmentLevelDictionary.getValueDefinitely(_equipmentLevelRef.group_id);
                levelList.Add(_equipmentLevelRef);
            });
            // 初始化 Mars Equipment Energy Level
            marsEquipmentEnergyLevelRefCore?.dealAllRef(_energyLevelRef =>
            {
                if (_energyLevelRef == null)
                    return;

                List<MarsEquipmentEnergyLevelRefObj> levelList = _m_marsEquipmentEnergyLevelDictionary.getValueDefinitely(_energyLevelRef.group_id);
                levelList.Add(_energyLevelRef);
            });
            // 初始化 Mars Equipment Food Level
            marsEquipmentFoodLevelRefCore?.dealAllRef(_foodLevelRef =>
            {
                if (_foodLevelRef == null)
                    return;

                List<MarsEquipmentFoodLevelRefObj> levelList = _m_marsEquipmentFoodLevelDictionary.getValueDefinitely(_foodLevelRef.group_id);
                levelList.Add(_foodLevelRef);
            });
            // 初始化 Mars Equipment Living Level
            marsEquipmentLivingLevelRefCore?.dealAllRef(_livingLevelRef =>
            {
                if (_livingLevelRef == null)
                    return;

                List<MarsEquipmentLivingLevelRefObj> levelList = _m_marsEquipmentLivingLevelDictionary.getValueDefinitely(_livingLevelRef.group_id);
                levelList.Add(_livingLevelRef);
            });
            // 初始化 Mars Equipment Hospital Level
            marsEquipmentHospitalLevelRefCore?.dealAllRef(_hospitalLevelRef =>
            {
                if (_hospitalLevelRef == null)
                    return;

                List<MarsEquipmentHospitalLevelRefObj> levelList = _m_marsEquipmentHospitalLevelDictionary.getValueDefinitely(_hospitalLevelRef.group_id);
                levelList.Add(_hospitalLevelRef);
            });
            // 初始化 Mars Building Equipment Belong
            marsBuildingEquipmentBelongRefCore?.dealAllRef(_belongRef =>
            {
                if (_belongRef == null)
                    return;

                _m_marsBuildingEquipmentBelongDictionary[_belongRef.group_id] = _belongRef;
            });
            
            // 计算并设置建筑槽位信息
            _calculateBuildingSlotInfo();
            _calculateBuildingOrder();
            
            // 排序所有列表 - 按level排序并验证连续性
            foreach ((long groupId, List<MarsBuildingLevelRefObj> levelRefList) in _m_marsBuildingLevelDictionary)
            {
                levelRefList.Sort((a, b) => a.level.CompareTo(b.level));
                _validateLevelSequence(groupId, levelRefList, nameof(MarsBuildingLevelRefObj));
            }
            _m_marsBuildingHomeLevelList.Sort((a, b) => a.level.CompareTo(b.level));
            _validateLevelSequence(0, _m_marsBuildingHomeLevelList, nameof(MarsBuildingHomeLevelRefObj));
            foreach ((long groupId, List<MarsBuildingSettleLevelRefObj> settleLevelRefList) in _m_marsBuildingSettleLevelDictionary)
            {
                settleLevelRefList.Sort((a, b) => a.level.CompareTo(b.level));
                _validateLevelSequence(groupId, settleLevelRefList, nameof(MarsBuildingSettleLevelRefObj));
            }
            foreach ((long groupId, List<MarsEquipmentLevelRefObj> equipmentLevelRefList) in _m_marsEquipmentLevelDictionary)
            {
                equipmentLevelRefList.Sort((a, b) => a.level.CompareTo(b.level));
                _validateLevelSequence(groupId, equipmentLevelRefList, nameof(MarsEquipmentLevelRefObj));
            }
            foreach ((long groupId, List<MarsEquipmentEnergyLevelRefObj> energyLevelRefList) in _m_marsEquipmentEnergyLevelDictionary)
            {
                energyLevelRefList.Sort((a, b) => a.level.CompareTo(b.level));
                _validateLevelSequence(groupId, energyLevelRefList, nameof(MarsEquipmentEnergyLevelRefObj));
            }
            foreach ((long groupId, List<MarsEquipmentFoodLevelRefObj> foodLevelRefList) in _m_marsEquipmentFoodLevelDictionary)
            {
                foodLevelRefList.Sort((a, b) => a.level.CompareTo(b.level));
                _validateLevelSequence(groupId, foodLevelRefList, nameof(MarsEquipmentFoodLevelRefObj));
            }
            foreach ((long groupId, List<MarsEquipmentLivingLevelRefObj> livingLevelRefList) in _m_marsEquipmentLivingLevelDictionary)
            {
                livingLevelRefList.Sort((a, b) => a.level.CompareTo(b.level));
                _validateLevelSequence(groupId, livingLevelRefList, nameof(MarsEquipmentLivingLevelRefObj));
            }
            foreach ((long groupId, List<MarsEquipmentHospitalLevelRefObj> hospitalLevelRefList) in _m_marsEquipmentHospitalLevelDictionary)
            {
                hospitalLevelRefList.Sort((a, b) => a.level.CompareTo(b.level));
                _validateLevelSequence(groupId, hospitalLevelRefList, nameof(MarsEquipmentHospitalLevelRefObj));
            }
        }
        private void _calculateBuildingSlotInfo()
        {
            // 为每个建筑refObj计算槽位信息
            marsBuildingRefCore?.dealAllRef(_buildingRef =>
            {
                if (_buildingRef == null || _buildingRef.settle_group_id <= 0)
                    return;

                List<MarsBuildingSettleLevelRefObj> settleLevelList = getMarsBuildingSettleLevelRefList(_buildingRef.settle_group_id);
                if (settleLevelList.Count == 0)
                    return;

                // 初始化槽位解锁等级列表
                _buildingRef.slot_unlock_levels = new List<int>();
                
                // 计算最大槽位数
                int maxSlots = 0;
                int previousSlots = 0;
                
                foreach (MarsBuildingSettleLevelRefObj settleLevel in settleLevelList)
                {
                    if (settleLevel.slot_num > maxSlots)
                        maxSlots = settleLevel.slot_num;
                    
                    // 当槽位数增加时，记录解锁等级
                    for (int slot = previousSlots + 1; slot <= settleLevel.slot_num; slot++)
                    {
                        _buildingRef.slot_unlock_levels.Add(settleLevel.level);
                    }
                    
                    previousSlots = settleLevel.slot_num;
                }
                
                _buildingRef.max_slot_num = maxSlots;
            });
        }
        
        private void _calculateBuildingOrder()
        {
            if (marsBuildingRefCore?.refList == null)
                return;
            
            List<long> orderList = new List<long>(marsBuildingRefCore.refList.Count);
            marsBuildingRefCore.dealAllRef(_buildingRef =>
            {
                if (_buildingRef == null)
                    return;

                orderList.Add(_buildingRef.building_order);
            });
            
            orderList.Sort();
            List<long> numberedOrderList = new List<long>(orderList.Count);
            int orderNumber = 1;
            long prevOrder = long.MinValue;
            foreach (long order in orderList)
            {
                if (order <= 0)
                {
                    numberedOrderList.Add(order);
                    prevOrder = order;
                    continue;
                }

                if (order != prevOrder)
                {
                    numberedOrderList.Add(orderNumber);
                    prevOrder = order;
                    orderNumber++;
                }
                else
                {
                    numberedOrderList.Add(orderNumber - 1);
                }
            }
            
            marsBuildingRefCore.dealAllRef(_buildingRef =>
            {
                if (_buildingRef == null)
                    return;

                int index = orderList.IndexOf(_buildingRef.building_order);
                if (index >= 0 && index < numberedOrderList.Count)
                    _buildingRef.building_order_in_number = (int)numberedOrderList[index];
            });
        }
        
        private void _validateLevelSequence<T>(long _groupId, List<T> _levelList, string _typeName) where T : class
        {
            if (_levelList.Count == 0)
                return;

            for (int i = 0; i < _levelList.Count; i++)
            {
                int expectedLevel = i + 1;
                int actualLevel = 0;

                switch (_levelList[i])
                {
                    case MarsBuildingLevelRefObj buildingLevel:
                        actualLevel = buildingLevel.level;
                        break;
                    case MarsBuildingHomeLevelRefObj homeLevel:
                        actualLevel = homeLevel.level;
                        break;
                    case MarsBuildingSettleLevelRefObj settleLevel:
                        actualLevel = settleLevel.level;
                        break;
                    case MarsEquipmentLevelRefObj equipmentLevel:
                        actualLevel = equipmentLevel.level;
                        break;
                    case MarsEquipmentEnergyLevelRefObj energyLevel:
                        actualLevel = energyLevel.level;
                        break;
                    case MarsEquipmentFoodLevelRefObj foodLevel:
                        actualLevel = foodLevel.level;
                        break;
                    case MarsEquipmentLivingLevelRefObj livingLevel:
                        actualLevel = livingLevel.level;
                        break;
                    case MarsEquipmentHospitalLevelRefObj hospitalLevel:
                        actualLevel = hospitalLevel.level;
                        break;
                }

                if (actualLevel != expectedLevel)
                {
                    ALLog.Error($"Mars RefData 等级配置错误: {_typeName} GroupId={_groupId}, 期望等级={expectedLevel}, 实际等级={actualLevel}");
                }
            }
        }

        #region 属性展示

        public void getMarsBuildingLvlPropertyShowInfo(MarsBuildingRefObj _buildingRef, List<MarsLvlPropertyShowInfo> _lvlPropertyShowList, List<_IPropertyShow> _m_lAllPropertyList, bool _containsMarsPower = true)
        {
            _lvlPropertyShowList?.Clear();
            _m_lAllPropertyList?.Clear();
            if (_buildingRef == null)
                return;
            
            List<MarsLvlPropertyShowInfo> lvlPropertyInfoList = new List<MarsLvlPropertyShowInfo>();
            foreach (var levelRefObj in GRefdataCoreMgr.instance.getMarsBuildingLevelRefList(_buildingRef.upgrade_group_id))
            {
                if(levelRefObj == null)
                    continue;

                Dictionary<_IPropertyShow, long> levelPropertyDict = new Dictionary<_IPropertyShow, long>();
                levelRefObj.addPropertyToCollections(levelPropertyDict, _m_lAllPropertyList, _containsMarsPower);
                    
                _lvlPropertyShowList?.Add(new MarsLvlPropertyShowInfo()
                {
                    lvl = levelRefObj.level,
                    propertyValueDic = levelPropertyDict,
                });
            }
        }

        #endregion
    }
}
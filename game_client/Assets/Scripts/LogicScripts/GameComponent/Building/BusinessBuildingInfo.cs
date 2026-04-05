
using System;
using System.Collections.Generic;
using ALPackage;
using Common.BuildingObj;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class BusinessBuildingInfo
    {
        [NotNull] private readonly BusinessBuildingRefObj _m_baseRef;
        private BusinessBuildingLevelRefObj _m_levelRef;
        private BusinessBuildingLevelRefObj _m_nextLevelRef;
        private BusinessBuildingDevelopRefObj _m_newestDevelopRef; // 当前最新的一个业务配置
        [ItemNotNull, NotNull] private readonly List<BusinessBuildingProductRefObj> _m_productList; // 已经解锁的产品列表
        private long _m_employeeCount;
        private long _m_earningsPerS;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalEarningsDealer;
        [ItemNotNull, NotNull] private readonly List<HeroInfo> _m_placedHeroList;
        [NotNull] private readonly JudgeUnionBonusPart[] _m_bonusJudgeParts;
        // 用于记录现在要不要显示可驻扎红点，如果当前解锁到的槽位索引大于这个值，则显示红点
        private long _m_showedCanSettleHeroTipIndex = -1;
        // 用于记录现在要不要显示可解锁产品红点，如果当前解锁到的员工数量大于这个值，则显示红点
        private long _m_checkedCanUnlockProductEmployeeNum = -1;
        
        
        public BusinessBuildingInfo([NotNull] Building_Business _serverData, List<HeroInfo> _placedHeroList, bool _isDealNextFrame = true)
        {
            _m_baseRef = GRefdataCoreMgr.instance.businessBuildingRefCore.getRef(_serverData.getBuildingId());
            if (_m_baseRef == null)
                return;

            _m_bonusJudgeParts = new JudgeUnionBonusPart[]
            {
                new JudgeUnionBonusPart() { filterType = EBonusFilterType.BUILDING_ID, id = _m_baseRef.building_id },
                new JudgeUnionBonusPart() { filterType = EBonusFilterType.BUILDING_ATTR, id = (long)_m_baseRef.attr_type }
            };
            _m_productList = new List<BusinessBuildingProductRefObj>();
            _m_placedHeroList = _placedHeroList ?? new List<HeroInfo>();
            _m_recalEarningsDealer = new LazyNextFrameTaskDealer(new _RecalEarnings(this));
            update(_serverData, _isDealNextFrame);
        }
        
        
        [NotNull] public BusinessBuildingRefObj baseRef { get { return _m_baseRef; } }
        public BusinessBuildingLevelRefObj levelRef { get { return _m_levelRef; } }
        public BusinessBuildingLevelRefObj nextLevelRef { get { return _m_nextLevelRef; } }
        public long id { get { return _m_baseRef.building_id; } }
        public int level { get { return _m_levelRef?.level ?? 1; } }
        public bool levelMax { get { return _m_nextLevelRef == null; } }
        public long maxEmployeeNum { get { return _m_baseRef.employee_base_max_count + _m_baseRef.addition_employee_count_per_level * (level - 1); } }
        public long employeeNum { get { return _m_employeeCount; } }
        public long earningsPerS { get { return _m_earningsPerS; } }
        [ItemNotNull, NotNull] public List<HeroInfo> heroSlotList { get { return _m_placedHeroList; } }
        [ItemNotNull, NotNull] public List<BusinessBuildingProductRefObj> productList { get { return _m_productList; } }
        public JudgeUnionBonusPart[] bonusJudgeParts { get { return _m_bonusJudgeParts; } }


        public void update(Building_Business _serverData, bool _isDealNextFrame = true)
        {
            if (_serverData == null)
                return;
            
            _m_levelRef = GRefdataCoreMgr.instance.getBusinessBuildingLevelRef(_serverData.getBuildingId(), _serverData.getLvl());
            _m_nextLevelRef = GRefdataCoreMgr.instance.getBusinessBuildingLevelRef(_serverData.getBuildingId(), _serverData.getLvl() + 1);
            _m_employeeCount = _serverData.getEmployeeCount();
            _m_newestDevelopRef = GRefdataCoreMgr.instance.getNewestBusinessBuildingDevelopRef(_serverData.getBuildingId(), _serverData.getLvl());
            
            _m_productList.Clear();
            List<long> productIdList = _serverData.getProductList();
            if (productIdList != null)
            {
                foreach (long productId in productIdList)
                {
                    BusinessBuildingProductRefObj productRef = GRefdataCoreMgr.instance.businessBuildingProductRefCore.getRef(productId);
                    if (productRef == null)
                        continue;
                    
                    _m_productList.Add(productRef);
                }
            }
            
            recalEarnings(_isDealNextFrame);
        }
        public void updateEarnings(long _earnings)
        {
            _m_earningsPerS = _earnings;
        }
        public void addHero(long _heroId)
        {
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_heroId);
            if (heroInfo == null)
                return;
            
            _m_placedHeroList.Add(heroInfo);
            recalEarnings();
        }
        public void removeHero(long _heroId)
        {
            _m_placedHeroList.FindAndRemove(_heroInfo => _heroInfo.id == _heroId);
            recalEarnings();
        }
        public NPCommonCostItem getHirCost(int _hireNum)
        {
            if (_hireNum <= 0)
                return null;

            long totalBaseCost = 0;
            for (int i = 0; i < _hireNum; i++)
            {
                totalBaseCost += GRefdataCoreMgr.instance.getBuildingHireBaseCost(_m_employeeCount + 1 + i);
            }
            long totalCost = totalBaseCost * _m_baseRef.hire_cost_multiple;
            
            long reducePer = NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.BUILDING_EMPLOYEE_REDUCE_PER, _m_bonusJudgeParts);
            totalCost = (long) Math.Ceiling(totalCost * (1 - reducePer / 10000d));

            return new NPCommonCostItem(ENPItemType.CURRENCY, (int) ECurrency.SILVER, totalCost);
        }
        public void recalEarnings(bool _isDealNextFrame = true)
        {
            if(_isDealNextFrame)
                _m_recalEarningsDealer.setNeedDeal();
            else
                BuildingCommon.dealRecalculateBuildingEarnings(this, false);
        }
        public long getAllPlacedHeroEarningBonus()
        {
            long totalV = 0;
            for (int i = 0; i < _m_placedHeroList.Count; i++)
            {
                totalV += _m_placedHeroList[i].getBusinessSkillAddPropValue(baseRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
            }

            return totalV;
        }
        public long getTotalBaseEarningsPerS()
        {
            long outputBS = 0;
            outputBS += getEmployeeEarningsPerPerson() * employeeNum;
            outputBS += (long) Math.Ceiling((double) NPPlayer.instance.heroComponent.totalPower / 1000);
            return outputBS;
        }
        public int getTotalEarningBonus()
        {
            int outputP = BuildingCommon.calBusinessBuildingEarningsAddPer(this);
            return outputP;
        }
        public NPGGoIndex getCurrentResIndex()
        {
            NPGGoIndex resIndex = _m_levelRef?.res_index;
            if (resIndex == null || !resIndex.isValid())
                return _m_baseRef.res_index;
            
            return resIndex;
        }
        public long getEmployeeEarningsPerPerson()
        {
            return _m_baseRef.employee_earnings + NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.BUILDING_EMPLOYEE_PROFIT_ADD, _m_bonusJudgeParts);
        }
        public NPGTextureIndex getCurrentPreviewTexIndex()
        {
            return _getPreviewTexIndex(_m_levelRef);
        }
        public NPGTextureIndex getNextPreviewTexIndex()
        {
            return _getPreviewTexIndex(_m_nextLevelRef ?? _m_levelRef);
        }
        public BusinessBuildingVideoGroupRefObj getCurrentVideoGroupRef()
        {
            return _m_newestDevelopRef?.video_group_ref ?? _m_baseRef.video_group_ref;
        }
        public bool needShowSettleHeroTip()
        {
            List<long> unlockRequireList = _m_baseRef.hero_slot_employee_num_list;
            if (unlockRequireList == null)
                return false;
            
            long currentUnlockedSlotIndex = -1;
            for (int i = 0; i < unlockRequireList.Count; i++)
            {
                if (_m_employeeCount >= unlockRequireList[i])
                    currentUnlockedSlotIndex = i;
                else
                    break;
            }
            
            if (currentUnlockedSlotIndex <= _m_showedCanSettleHeroTipIndex)
                return false;
            
            return canSettleHero();
        }
        public bool needShowUnlockProductTip()
        {
            // 未满足研究解锁条件时，不显示解锁提示
            _NPPlayerConditionSerializeInfo productUnlockCondition = GRefdataCoreMgr.instance.npGeneral.building_product_unlock_condition;
            if (productUnlockCondition != null && !productUnlockCondition.isNoConditionOrEnable(null))
            {
                return false;
            }
            
            long currentMaxUnlockedEmployeeRequire = _getCurrentMaxUnlockableProductEmployeeRequire();
            return currentMaxUnlockedEmployeeRequire > _m_checkedCanUnlockProductEmployeeNum;
        }
        public void setShowedSettleHeroTipIndex()
        {
            List<long> unlockRequireList = _m_baseRef.hero_slot_employee_num_list;
            if (unlockRequireList == null)
                return;
            
            // Update to current highest unlocked slot index
            for (int i = 0; i < unlockRequireList.Count; i++)
            {
                if (_m_employeeCount >= unlockRequireList[i])
                    _m_showedCanSettleHeroTipIndex = i;
                else
                    break;
            }
        }
        public void setCheckedCanUnlockProductEmployeeNum()
        {
            long currentMaxUnlockedEmployeeRequire = _getCurrentMaxUnlockableProductEmployeeRequire();
            if (currentMaxUnlockedEmployeeRequire != -1)
                _m_checkedCanUnlockProductEmployeeNum = currentMaxUnlockedEmployeeRequire;
        }
        /// <summary>
        /// 可以驻扎大臣
        /// </summary>
        public bool canSettleHero()
        {
            // 没有空位了
            if (heroSlotList.Count >= baseRef.hero_slot_employee_num_list.Count ||
                baseRef.hero_slot_employee_num_list[heroSlotList.Count] > employeeNum)
                return false;

            NPSimpleUnlockRef heroSettleUnlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(GRefdataCoreMgr.instance.npGeneral.business_building_hero_settle_unlock_id);
            if (heroSettleUnlockRef != null && !heroSettleUnlockRef.isConditionEnable(null))
                return false;

            bool hasHero = false;
            NPPlayer.instance.heroComponent.dealAllHero(_heroInfo =>
            {
                if (_heroInfo?.heroRefObj == null)
                    return;
                            
                if (!_heroInfo.placeData.isPlaced && _heroInfo.heroRefObj.getCanPlaceInBuilding(baseRef.attr_type))
                    hasHero = true;
            });

            return hasHero;
        }
        /// <summary>
        /// 获取下一个解锁槽位所需的员工数量
        /// </summary>
        public long getNextSlotUnlockRequireNum()
        {
            List<long> unlockRequireList = _m_baseRef.hero_slot_employee_num_list;
            if (unlockRequireList == null)
                return -1;
            
            foreach (long num in unlockRequireList)
            {
                if (_m_employeeCount < num)
                    return num;
            }
            
            return -1; // 没有更多的解锁需求
        } 
        /// <summary>
        /// 检查是否所有槽位都已解锁
        /// </summary>
        /// <returns></returns>
        public bool isAllSlotUnlock()
        {
            List<long> unlockRequireList = _m_baseRef.hero_slot_employee_num_list;
            if (unlockRequireList == null)
                return true;

            foreach (long num in unlockRequireList)
            {
                if (_m_employeeCount < num)
                    return false;
            }

            return true; // 所有槽位都已解锁
        }
        [Pure]
        public bool isProductUnlocked(long _productId)
        {
            foreach (BusinessBuildingProductRefObj productRef in _m_productList)
            {
                if (productRef.id == _productId)
                    return true;
            }

            return false;
        }
        
        
        private NPGTextureIndex _getPreviewTexIndex(BusinessBuildingLevelRefObj _levelRef)
        {
            NPGTextureIndex index = _levelRef?.preview_tex_index;
            if (index == null || !index.isValid())
                return _m_baseRef.preview_tex_index;

            return index;
        }
        private long _getCurrentMaxUnlockableProductEmployeeRequire()
        {
            List<BusinessBuildingProductRefObj> allProductList = GRefdataCoreMgr.instance.getBusinessBuildingProductRefList(id);
            if (allProductList == null || allProductList.Count == 0)
                return -1;
            
            long currentMaxUnlockedEmployeeRequire = -1;
            foreach (BusinessBuildingProductRefObj productRef in allProductList)
            {
                if (_m_employeeCount >= productRef.employee_required)
                {
                    if (!isProductUnlocked(productRef.id) && productRef.employee_required > currentMaxUnlockedEmployeeRequire)
                        currentMaxUnlockedEmployeeRequire = productRef.employee_required;
                }
                else
                    break;
            }
            
            return currentMaxUnlockedEmployeeRequire;
        }


        private class _RecalEarnings : _IALBaseMonoTask
        {
            [NotNull] private readonly BusinessBuildingInfo _m_buildingInfo;
            
            
            public _RecalEarnings([NotNull] BusinessBuildingInfo _buildingInfo)
            {
                _m_buildingInfo = _buildingInfo;
            }


            public void deal()
            {
                BuildingCommon.dealRecalculateBuildingEarnings(_m_buildingInfo, true);
            }
        }
    }
}
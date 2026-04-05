
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        [NotNull] private readonly Dictionary<long, List<BusinessBuildingLevelRefObj>> _m_businessBuildingLevelDictionary = new Dictionary<long, List<BusinessBuildingLevelRefObj>>();
        [NotNull] private readonly Dictionary<long, List<FarmingBuildingLevelRefObj>> _m_farmingBuildingLevelDictionary = new Dictionary<long, List<FarmingBuildingLevelRefObj>>();
        [NotNull] private readonly Dictionary<long, List<BusinessBuildingDevelopRefObj>> _m_businessBuildingDevelopDictionary = new Dictionary<long, List<BusinessBuildingDevelopRefObj>>();
        [NotNull] private readonly Dictionary<long, List<BusinessBuildingProductRefObj>> _m_businessBuildingProductDictionary = new Dictionary<long, List<BusinessBuildingProductRefObj>>();

        public BusinessBuildingLevelRefObj getBusinessBuildingLevelRef(long _buildingId, int _level)
        {
            List<BusinessBuildingLevelRefObj> levelList = _m_businessBuildingLevelDictionary.GetValueOrDefault(_buildingId);
            return levelList?.SafeGet(_level - 1);
        }
        public FarmingBuildingLevelRefObj getFarmingBuildingLevelRefObj(long _buildingId, int _level)
        {
            FarmingBuildingRefObj buildingRef = farmingBuildingRefCore.getRef(_buildingId);
            if (buildingRef == null || _level > buildingRef.building_level_max)
                return null;
            
            List<FarmingBuildingLevelRefObj> levelList = _m_farmingBuildingLevelDictionary.GetValueOrDefault(_buildingId);
            FarmingBuildingLevelRefObj levelRef = null;
            for (int i = 0; i < levelList.Count; i++)
            {
                levelRef = levelList[i];
                if (levelRef.level > _level)
                {
                    levelRef = i <= 0 ? null : levelList[i - 1];
                    break;
                }
            }

            return levelRef;
        }
        /// <summary>
        /// 获取当前最新业务的配置
        /// </summary>
        public BusinessBuildingDevelopRefObj getNewestBusinessBuildingDevelopRef(long _buildingId, long _level)
        {
            List<BusinessBuildingDevelopRefObj> developList = _m_businessBuildingDevelopDictionary.GetValueOrDefault(_buildingId);
            if (developList == null || developList.Count <= 0)
                return null;

            BusinessBuildingDevelopRefObj developRef = null;
            for (int i = 0; i < developList.Count; i++)
            {
                developRef = developList[i];
                if (developRef.level_required > _level)
                {
                    developRef = i <= 0 ? null : developList[i - 1];
                    break;
                }
            }

            return developRef;
        }
        /// <summary>
        /// 获取建筑的产品列表
        /// </summary>
        public List<BusinessBuildingProductRefObj> getBusinessBuildingProductRefList(long _buildingId)
        {
            return _m_businessBuildingProductDictionary.GetValueOrDefault(_buildingId);
        }
        /// <summary>
        /// 获取建筑的业务列表
        /// </summary>
        public List<BusinessBuildingDevelopRefObj> getBusinessBuildingDevelopRefList(long _buildingId)
        {
            return _m_businessBuildingDevelopDictionary.GetValueOrDefault(_buildingId);
        }
        /// <summary>
        /// 获取雇佣员工的基础费用
        /// </summary>
        /// <remarks>
        /// 获取了之后对应建筑自己的消耗还有一个倍率，乘上才是最终的消耗
        /// </remarks>
        public long getBuildingHireBaseCost(long _employeeNumber)
        {
            BusinessBuildingHireCostRefObj hireCostRef = null;
            for (int i = 0; i < businessBuildingHireCostRefCore.refList.Count; i++)
            {
                hireCostRef = businessBuildingHireCostRefCore.refList[i];
                if (hireCostRef.employee_num > _employeeNumber)
                {
                    hireCostRef = i <= 0 ? null : businessBuildingHireCostRefCore.refList[i - 1];
                    break;
                }
            }
            
            if (hireCostRef == null)
                return 0;

            return (long)Math.Ceiling(
                hireCostRef.hire_cost_coeff_A / 10000d * _employeeNumber * _employeeNumber * _employeeNumber +
                hireCostRef.hire_cost_coeff_B / 10000d * _employeeNumber * _employeeNumber +
                hireCostRef.hire_cost_coeff_C / 10000d * _employeeNumber +
                hireCostRef.hire_cost_coeff_D / 10000d);
        }
        

        private void _initBuilding()
        {
            _fixClientRefdata();
            
            businessBuildingLevelRefCore?.dealAllRef(_levelRef =>
            {
                if (_levelRef == null)
                    return;

                List<BusinessBuildingLevelRefObj> levelList = _m_businessBuildingLevelDictionary.getValueDefinitely(_levelRef.building_id);
                levelList.Add(_levelRef);
            });
            farmingBuildingLevelRefCore?.dealAllRef(_levelRef =>
            {
                if (_levelRef == null)
                    return;

                List<FarmingBuildingLevelRefObj> levelList = _m_farmingBuildingLevelDictionary.getValueDefinitely(_levelRef.building_id);
                levelList.Add(_levelRef);
            });
            businessBuildingDevelopRefCore?.dealAllRef(_developRef =>
            {
                if (_developRef == null)
                    return;

                _developRef.video_group_ref = businessBuildingVideoGroupRefCore.getRef(_developRef.video_group_id);
                List<BusinessBuildingDevelopRefObj> developList = _m_businessBuildingDevelopDictionary.getValueDefinitely(_developRef.building_id);
                developList.Add(_developRef);
            });
            businessBuildingProductRefCore?.dealAllRef(_productRef =>
            {
                if (_productRef == null)
                    return;

                List<BusinessBuildingProductRefObj> productList = _m_businessBuildingProductDictionary.getValueDefinitely(_productRef.building_id);
                productList.Add(_productRef);
            });
            
            businessBuildingHireCostRefCore.refList.Sort();
            
            foreach (List<BusinessBuildingLevelRefObj> levelRefList in _m_businessBuildingLevelDictionary.Values)
            {
                levelRefList.Sort();
            }
            foreach (List<FarmingBuildingLevelRefObj> levelRefList in _m_farmingBuildingLevelDictionary.Values)
            {
                levelRefList.Sort();
            }
            foreach (List<BusinessBuildingDevelopRefObj> developRefList in _m_businessBuildingDevelopDictionary.Values)
            {
                developRefList.Sort();
            }
            foreach (List<BusinessBuildingProductRefObj> productRefList in _m_businessBuildingProductDictionary.Values)
            {
                productRefList.Sort();
            }
            
            
            NPGGoIndex overrideResIndex;
            NPGTextureIndex overridePreviewTexIndex;
            foreach ((long buildingId, List<BusinessBuildingLevelRefObj> levelRefList) in _m_businessBuildingLevelDictionary)
            {
                BusinessBuildingRefObj refObj = businessBuildingRefCore.getRef(buildingId);
                if (refObj == null)
                    continue;
                
                overrideResIndex = refObj.res_index;
                overridePreviewTexIndex = refObj.preview_tex_index;
                long overrideVideoGroupId = refObj.video_group_id;
                refObj.video_group_ref = businessBuildingVideoGroupRefCore.getRef(overrideVideoGroupId);
                foreach (BusinessBuildingLevelRefObj levelRef in levelRefList)
                {
                    if (overrideResIndex == null || 
                        (levelRef.override_res_index.isValid() && !BasicResIndexInfo.IsEqual(overrideResIndex, levelRef.override_res_index)))
                        overrideResIndex = levelRef.override_res_index;
                    if (overridePreviewTexIndex == null || 
                        (levelRef.override_preview_tex_index.isValid() && !BasicResIndexInfo.IsEqual(overridePreviewTexIndex, levelRef.override_preview_tex_index)))
                        overridePreviewTexIndex = levelRef.override_preview_tex_index;

                    levelRef.res_index = overrideResIndex;
                    levelRef.preview_tex_index = overridePreviewTexIndex;
                }
            }
            
            foreach ((long buildingId, List<FarmingBuildingLevelRefObj> levelRefList) in _m_farmingBuildingLevelDictionary)
            {
                FarmingBuildingRefObj refObj = farmingBuildingRefCore.getRef(buildingId);
                if (refObj == null)
                    continue;
                
                overrideResIndex = refObj.res_index;
                overridePreviewTexIndex = refObj.preview_tex_index;
                foreach (FarmingBuildingLevelRefObj levelRef in levelRefList)
                {
                    if (overrideResIndex == null || 
                        (levelRef.override_res_index.isValid() && !BasicResIndexInfo.IsEqual(overrideResIndex, levelRef.override_res_index)))
                        overrideResIndex = levelRef.override_res_index;
                    if (overridePreviewTexIndex == null || 
                        (levelRef.override_preview_tex_index.isValid() && !BasicResIndexInfo.IsEqual(overridePreviewTexIndex, levelRef.override_preview_tex_index)))
                        overridePreviewTexIndex = levelRef.override_preview_tex_index;
                
                    levelRef.res_index = overrideResIndex;
                    levelRef.preview_tex_index = overridePreviewTexIndex;
                }
            }
        }

        private void _fixClientRefdata()
        {
            // todo: 下面开始修复配置，来符合客户端的复杂结构。
            // todo: 后面如果要采用客户端的结构设计，这些代码都是多余的，修复配置主要是让配置更符合策划直觉和最初的设计。
            
            buildingRefCore.dealAllRef(_buildingRef =>
            {
                // 首先遍历 building 的配置，building 在客户端的结构来看，就只是个地基，不含有任何建筑功能。
                
                // 先检查这个建筑具有的功能
                BusinessBuildingRefObj businessRef = businessBuildingRefCore.getRef(_buildingRef.id);
                FarmingBuildingRefObj farmingRef = farmingBuildingRefCore.getRef(_buildingRef.id);
                if (businessRef != null && farmingRef != null)
                {
                    // 如果想要表现配置简单一点，就不允许同时存在两个功能，因为目前设计每个功能都有可能会影响到建筑的样式，除非废弃掉这一点，然后调整代码结构
                    ALLog.Error("一个建筑同时具有了两种功能，又要兼容升级可以变样式，需要采用客户端的复杂结构，联系 Coda 调整代码，配置也需要重新配。");
                    return;
                }

                if (businessRef == null && farmingRef == null)
                {
                    // 如果这个建筑两个功能都不具有，策划配置的资源，就是这个建筑最终的样子
                    _buildingRef.built_res_index = _buildingRef.res_index;
                }
                
                // 但如果这个建筑具有功能，策划配置的资源实际上是功能建筑的资源，而建筑资源其实只是个地基，下面处理这个问题
                if (businessRef != null)
                {
                    // 把已经挂有功能节点的建筑资源设置上，这部分不需要策划进行配置了
                    _buildingRef.built_res_index = GGameCommonInfo.instance.obj.business_building_base_res_index;
                    // 这时建筑总表的相关配置实际上是功能建筑的配置
                    businessRef.name = _buildingRef.name;
                    businessRef.desc = _buildingRef.building_desc;
                    businessRef.level_up_desc = _buildingRef.level_up_desc;
                    businessRef.res_index = _buildingRef.res_index;
                    businessRef.preview_tex_index = _buildingRef.preview_tex_index;
                }
                if (farmingRef != null)
                {
                    // 和上面类似的修正方法，这里不再赘述
                    _buildingRef.built_res_index = GGameCommonInfo.instance.obj.farming_building_base_res_index;
                    farmingRef.name = _buildingRef.name;
                    farmingRef.level_up_desc = _buildingRef.level_up_desc;
                    farmingRef.res_index = _buildingRef.res_index;
                    farmingRef.preview_tex_index = _buildingRef.preview_tex_index;
                }
            });
        }
    }
}
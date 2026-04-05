package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelAreaMgr;
import NPGameRes.Refs.Building.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 初始化建筑对象
 */
public class BuildingInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        ////////////////// ========== RefFarmingBuilding ========== //////////////////
        // 预整理：按 building_id 分组农田建筑等级数据
        Map<Long, _TLevelAreaMgr<RefFarmingBuildingLevel>> farmingLevelMap = new HashMap<>();
        List<RefFarmingBuildingLevel> farmingLevelList = RefFarmingBuildingLevel.getMgr().getList();
        for (int i = 0; i < farmingLevelList.size(); i++)
        {
            RefFarmingBuildingLevel ref = farmingLevelList.get(i);
            if (null == ref)
                continue;
            if (null == RefFarmingBuilding.getMgr().get(ref.building_id))
            {
                CommLog.error("BuildingInitDealer.dealInit - farming building ref not found: building_id={}", ref.building_id);
                continue;
            }
            farmingLevelMap.computeIfAbsent(ref.building_id, k -> new _TLevelAreaMgr<>())._initAddLevelData(ref);
        }
        List<RefFarmingBuilding> farmingList = RefFarmingBuilding.getMgr().getList();
        for (int i = 0; i < farmingList.size(); i++)
        {
            RefFarmingBuilding ref = farmingList.get(i);
            if (null == ref)
                continue;
            _TLevelAreaMgr<RefFarmingBuildingLevel> mgr = farmingLevelMap.get(ref.building_id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelAreaMgr<>());
        }

        ////////////////// ========== RefBusinessBuilding ========== //////////////////
        // 预整理：按 building_id 分组经营建筑等级数据
        Map<Long, _TLevelAreaMgr<RefBusinessBuildingLevel>> businessLevelMap = new HashMap<>();
        List<RefBusinessBuildingLevel> businessLevelList = RefBusinessBuildingLevel.getMgr().getList();
        for (int i = 0; i < businessLevelList.size(); i++)
        {
            RefBusinessBuildingLevel ref = businessLevelList.get(i);
            if (null == ref)
                continue;
            if (null == RefBusinessBuilding.getMgr().get(ref.building_id))
            {
                CommLog.error("BuildingInitDealer.dealInit - business building ref not found: building_id={}", ref.building_id);
                continue;
            }
            businessLevelMap.computeIfAbsent(ref.building_id, k -> new _TLevelAreaMgr<>())._initAddLevelData(ref);
        }
        List<RefBusinessBuilding> businessList = RefBusinessBuilding.getMgr().getList();
        for (int i = 0; i < businessList.size(); i++)
        {
            RefBusinessBuilding ref = businessList.get(i);
            if (null == ref)
                continue;
            _TLevelAreaMgr<RefBusinessBuildingLevel> mgr = businessLevelMap.get(ref.building_id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelAreaMgr<>());
        }
    	
    	////////////////// ========== RefBusinessBuildingHireCost ========== //////////////////
    	//经营建筑 消耗数据表
    	int maxEmployeeNum = 0;
    	RefBusinessBuildingHireCost maxEmployeeNumRef = null;
    	for(int i = 0; i < RefBusinessBuildingHireCost.getMgr().getList().size(); i++)
    	{
    		RefBusinessBuildingHireCost buildingHireCostRef = RefBusinessBuildingHireCost.getMgr().getList().get(i);
    		if(null == buildingHireCostRef)
    			continue;
    		
    		buildingHireCostRef.isMax = false;
    		
    		if(buildingHireCostRef.employee_num > maxEmployeeNum)
    		{
    			maxEmployeeNum = buildingHireCostRef.employee_num;
    			maxEmployeeNumRef = buildingHireCostRef;
    		}
    	}
    	if(null != maxEmployeeNumRef)
    	{
    		maxEmployeeNumRef.isMax = true;
    	}

		//经营建筑研发表
		Map<Long, List<RefBusinessBuildingDevelop>> developMap = new HashMap<>();
		for (RefBusinessBuildingDevelop refBusinessBuildingDevelop : RefBusinessBuildingDevelop.getMgr().getList())
		{
			developMap.computeIfAbsent(refBusinessBuildingDevelop.building_id, k -> new ArrayList<>()).add(refBusinessBuildingDevelop);
		}
		developMap.forEach((key, value) ->
        {
            RefBusinessBuilding buildingRef = RefBusinessBuilding.getMgr().get(key);
            if (null == buildingRef)
            {
                CommLog.error("init building develop ref fail, not find building-business:{} ref.", key);
                return;
            }

            buildingRef.setDevelopList(value);
        });
    }
}

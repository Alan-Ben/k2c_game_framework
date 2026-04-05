package NPGameRes.InitDealer;

import NPCommon.Game.WeightValueList;
import NPCommon.Game.WeightValueList.WeightValue;
import NPCommon.Log.CommLog;
import NPCommon.Util.Pair.WCGPairInt;
import NPEnum.EQuality;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;
import NPGameRes.GameObjs.Mars.MarsBattleQuality;
import NPGameRes.Refs.Mars.*;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class MarsInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
    	//前往火星
    	_TLevelMapMgr<RefMarsGoRoute> marsGoRouteMgr = new _TLevelMapMgr<>();
    	List<RefMarsGoRoute> goRouteRefList = RefMarsGoRoute.getMgr().getList();
    	for (int i = 0; i < goRouteRefList.size(); i++)
        {
    		RefMarsGoRoute ref = goRouteRefList.get(i);
    		if(null == ref)
    			continue;
    		
    		marsGoRouteMgr._initAddLevelData(ref);
        }
    	RefGeneral.Ref().setMarsGoRouteStageMapMgr(marsGoRouteMgr);

    	//火星建筑
    	List<RefMarsBuilding> buildingRefList = RefMarsBuilding.getMgr().getList();
    	List<RefMarsBuildingLevel> buildingLevelRefList = RefMarsBuildingLevel.getMgr().getList();
    	List<RefMarsBuildingSettleLevel> buildingSettleLevelRefList = RefMarsBuildingSettleLevel.getMgr().getList();
    	List<RefMarsBuildingEquipmentBelong> equipmentBelongRefList = RefMarsBuildingEquipmentBelong.getMgr().getList();
    	List<RefMarsEquipment> equipmentRefList = RefMarsEquipment.getMgr().getList();
    	List<RefMarsEquipmentLevel> equipmentLevelRefList = RefMarsEquipmentLevel.getMgr().getList();
    	List<RefMarsEquipmentEnergyLevel> equipmentEnergyLevelRefList = RefMarsEquipmentEnergyLevel.getMgr().getList();
    	List<RefMarsEquipmentFoodLevel> equipmentFoodLevelRefList = RefMarsEquipmentFoodLevel.getMgr().getList();
    	List<RefMarsEquipmentHospitalLevel> equipmentHospitalLevelRefList = RefMarsEquipmentHospitalLevel.getMgr().getList();
    	List<RefMarsEquipmentLivingLevel> equipmentLivingLevelRefList = RefMarsEquipmentLivingLevel.getMgr().getList();

    	// 预整理：按 group_id 分组建筑等级数据
    	HashMap<Integer, _TLevelMapMgr<RefMarsBuildingLevel>> buildingLevelMap = new HashMap<>();
    	for (int i = 0; i < buildingLevelRefList.size(); i++)
    	{
    		RefMarsBuildingLevel ref = buildingLevelRefList.get(i);
    		if (null == ref) continue;
    		buildingLevelMap.computeIfAbsent(ref.group_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
    	}
    	// 预整理：按 group_id 分组建筑结算等级数据
    	HashMap<Integer, _TLevelMapMgr<RefMarsBuildingSettleLevel>> buildingSettleLevelMap = new HashMap<>();
    	for (int i = 0; i < buildingSettleLevelRefList.size(); i++)
    	{
    		RefMarsBuildingSettleLevel ref = buildingSettleLevelRefList.get(i);
    		if (null == ref) continue;
    		buildingSettleLevelMap.computeIfAbsent(ref.group_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
    	}
    	// 预整理：按 group_id 分组建筑归属数据
    	HashMap<Integer, RefMarsBuildingEquipmentBelong> equipmentBelongByGroupMap = new HashMap<>();
    	for (int i = 0; i < equipmentBelongRefList.size(); i++)
    	{
    		RefMarsBuildingEquipmentBelong ref = equipmentBelongRefList.get(i);
    		if (null == ref) continue;
    		equipmentBelongByGroupMap.put(ref.group_id, ref);
    	}

    	//建筑等级
    	for(int i = 0; i < buildingRefList.size(); i++)
    	{
    		RefMarsBuilding ref = buildingRefList.get(i);
    		if(null == ref)
    			continue;

    		// 建筑等级直接从预分组 Map 中取
    		_TLevelMapMgr<RefMarsBuildingLevel> levelMapMgr = buildingLevelMap.get(ref.upgrade_group_id);
    		ref.setLevelMapMgr(levelMapMgr != null ? levelMapMgr : new _TLevelMapMgr<>());

    		// 建筑结算等级直接从预分组 Map 中取
    		_TLevelMapMgr<RefMarsBuildingSettleLevel> settleLevelMapMgr = buildingSettleLevelMap.get(ref.settle_group_id);
    		ref.setSettleLevelMapMgr(settleLevelMapMgr != null ? settleLevelMapMgr : new _TLevelMapMgr<>());

    		// 建筑部件数据直接从预分组 Map 中取
    		ref.equipmentBelongRef = equipmentBelongByGroupMap.get(ref.upgrade_group_id);

    		//条件数据
    		ArrayList<RefMarsBuildingCondition> conditionRefList = new ArrayList<>();
    		for(int j = 0; j < ref.build_condition_id_list.size(); j++)
    		{
    			RefMarsBuildingCondition condRef = RefMarsBuildingCondition.getMgr().get(ref.build_condition_id_list.get(j));
    			if(null == condRef)
    			{
    				CommLog.error("mars building:{} cond:{} can not find cond ref.", ref.id, ref.build_condition_id_list.get(j));
    				continue;
    			}
    			conditionRefList.add(condRef);
    		}
    		ref.conditionRefList = conditionRefList;
    	}
    	
    	//建筑等级数据
    	for(int i = 0; i < buildingLevelRefList.size(); i++)
    	{
    		RefMarsBuildingLevel ref = buildingLevelRefList.get(i);
    		if(null == ref)
    			continue;
    		
    		//条件数据
    		ArrayList<RefMarsBuildingCondition> conditionRefList = new ArrayList<>();
    		for(int j = 0; j < ref.upgrade_condition_id_list.size(); j++)
    		{
    			RefMarsBuildingCondition condRef = RefMarsBuildingCondition.getMgr().get(ref.upgrade_condition_id_list.get(j));
    			if(null == condRef)
    			{
    				CommLog.error("mars building lvl:{} cond:{} can not find upgrade cond ref.", ref.id, ref.upgrade_condition_id_list.get(j));
    				continue;
    			}
    			
    			conditionRefList.add(condRef);
    		}
    		ref.conditionRefList = conditionRefList;
    	}
    	
    	//部件归属数据
    	for(int i = 0; i < buildingRefList.size(); i++)
    	{
    		RefMarsBuilding buildingRef = buildingRefList.get(i);
    		if(null == buildingRef)
    			continue;
    		
    		RefMarsBuildingEquipmentBelong belongRef = buildingRef.equipmentBelongRef;
    		if(null == belongRef)
    			continue;

			//主要部件
    		ArrayList<RefMarsEquipment> buildingEquipmentRefList = new ArrayList<>();
			for(int j = 0; j < belongRef.main_equipment_id_list.size(); j++)
			{
				RefMarsEquipment equipmentRef = RefMarsEquipment.getMgr().get(belongRef.main_equipment_id_list.get(j));
				if(null == equipmentRef)
				{
					CommLog.error("mars belong:{} building belong main equip:{} can not find equip ref.", belongRef.Id(), belongRef.main_equipment_id_list.get(j));
					continue;
				}
				
				buildingEquipmentRefList.add(equipmentRef);
			}
			//其他部件
			for(int j = 0; j < belongRef.other_equipment_id_list.size(); j++)
			{
				RefMarsEquipment equipmentRef = RefMarsEquipment.getMgr().get(belongRef.other_equipment_id_list.get(j));
				if(null == equipmentRef)
				{
					CommLog.error("mars belong:{} building belong other equip:{} can not find equip ref.", belongRef.Id(), belongRef.other_equipment_id_list.get(j));
					continue;
				}
				
				buildingEquipmentRefList.add(equipmentRef);
			}
			buildingRef.equipmentRefList = buildingEquipmentRefList;
    	}
    	
    	//部件等级
    	// 预整理：按 group_id 分组部件各类型等级数据
    	HashMap<Integer, _TLevelMapMgr<RefMarsEquipmentLevel>> equipLevelMap = new HashMap<>();
    	for (int i = 0; i < equipmentLevelRefList.size(); i++)
    	{
    		RefMarsEquipmentLevel ref = equipmentLevelRefList.get(i);
    		if (null == ref) continue;
    		equipLevelMap.computeIfAbsent(ref.group_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
    	}
    	HashMap<Integer, _TLevelMapMgr<RefMarsEquipmentEnergyLevel>> equipEnergyLevelMap = new HashMap<>();
    	for (int i = 0; i < equipmentEnergyLevelRefList.size(); i++)
    	{
    		RefMarsEquipmentEnergyLevel ref = equipmentEnergyLevelRefList.get(i);
    		if (null == ref) continue;
    		equipEnergyLevelMap.computeIfAbsent(ref.group_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
    	}
    	HashMap<Integer, _TLevelMapMgr<RefMarsEquipmentFoodLevel>> equipFoodLevelMap = new HashMap<>();
    	for (int i = 0; i < equipmentFoodLevelRefList.size(); i++)
    	{
    		RefMarsEquipmentFoodLevel ref = equipmentFoodLevelRefList.get(i);
    		if (null == ref) continue;
    		equipFoodLevelMap.computeIfAbsent(ref.group_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
    	}
    	HashMap<Integer, _TLevelMapMgr<RefMarsEquipmentHospitalLevel>> equipHospitalLevelMap = new HashMap<>();
    	for (int i = 0; i < equipmentHospitalLevelRefList.size(); i++)
    	{
    		RefMarsEquipmentHospitalLevel ref = equipmentHospitalLevelRefList.get(i);
    		if (null == ref) continue;
    		equipHospitalLevelMap.computeIfAbsent(ref.group_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
    	}
    	HashMap<Integer, _TLevelMapMgr<RefMarsEquipmentLivingLevel>> equipLivingLevelMap = new HashMap<>();
    	for (int i = 0; i < equipmentLivingLevelRefList.size(); i++)
    	{
    		RefMarsEquipmentLivingLevel ref = equipmentLivingLevelRefList.get(i);
    		if (null == ref) continue;
    		equipLivingLevelMap.computeIfAbsent(ref.group_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
    	}

    	for(int i = 0; i < equipmentRefList.size(); i++)
    	{
    		RefMarsEquipment ref = equipmentRefList.get(i);
    		if(null == ref)
    			continue;

    		// 部件各类型等级直接从预分组 Map 中取
    		_TLevelMapMgr<RefMarsEquipmentLevel> levelMapMgr = equipLevelMap.get(ref.upgrade_group_id);
    		ref.setLevelMapMgr(levelMapMgr != null ? levelMapMgr : new _TLevelMapMgr<>());

    		_TLevelMapMgr<RefMarsEquipmentEnergyLevel> energyLevelMapMgr = equipEnergyLevelMap.get(ref.upgrade_group_id);
    		ref.setEnergyLevelMapMgr(energyLevelMapMgr != null ? energyLevelMapMgr : new _TLevelMapMgr<>());

    		_TLevelMapMgr<RefMarsEquipmentFoodLevel> foodLevelMapMgr = equipFoodLevelMap.get(ref.upgrade_group_id);
    		ref.setFoodLevelMapMgr(foodLevelMapMgr != null ? foodLevelMapMgr : new _TLevelMapMgr<>());

    		_TLevelMapMgr<RefMarsEquipmentHospitalLevel> hospitalLevelMapMgr = equipHospitalLevelMap.get(ref.upgrade_group_id);
    		ref.setHospitalLevelMapMgr(hospitalLevelMapMgr != null ? hospitalLevelMapMgr : new _TLevelMapMgr<>());

    		_TLevelMapMgr<RefMarsEquipmentLivingLevel> livingLevelMapMgr = equipLivingLevelMap.get(ref.upgrade_group_id);
    		ref.setLivingLevelMapMgr(livingLevelMapMgr != null ? livingLevelMapMgr : new _TLevelMapMgr<>());
    	}
    	
    	//火星居民-帮助
    	List<RefMarsPeopleRewardHelp> helpRewardRefList = RefMarsPeopleRewardHelp.getMgr().getList();
    	List<RefMarsPeopleChoiceHelp> helpChoiceRefList = RefMarsPeopleChoiceHelp.getMgr().getList();
    	for(int i = 0; i < helpRewardRefList.size(); i++)
    	{
    		RefMarsPeopleRewardHelp ref = helpRewardRefList.get(i);
    		if(null == ref)
    			continue;

    		ref.helpRef = null;
    		
    		RefMarsPeopleHelp helpRef = RefMarsPeopleHelp.getMgr().get(ref.id);
    		if(null == helpRef)
    		{
				CommLog.error("mars helpReward:{} can not find help ref.", ref.id);
    			continue;
    		}
    		else
    		{
    			ref.helpRef = helpRef;
    		}
    	}
    	for(int i = 0; i < helpChoiceRefList.size(); i++)
    	{
    		RefMarsPeopleChoiceHelp ref = helpChoiceRefList.get(i);
    		if(null == ref)
    			continue;
    		
    		ref.helpRef = null;
    		RefMarsPeopleHelp helpRef = RefMarsPeopleHelp.getMgr().get(ref.id);
    		if(null == helpRef)
    		{
				CommLog.error("mars choice help:{} can not find people help ref.", ref.id);
    		}
    		else
    		{
    			ref.helpRef = helpRef;
    		}
    		
    		ref.optionCount = ref.option_add_satisfaction_degree_list.size();
    	}
    	
    	//火星居民-事件
    	List<RefMarsEvent> eventRefList = RefMarsEvent.getMgr().getList();
    	List<RefMarsEventTriggerPer> eventTriggerPerRefList = RefMarsEventTriggerPer.getMgr().getList();
    	// 预整理：按 group_id 分组事件触发概率数据
    	HashMap<Integer, ArrayList<RefMarsEventTriggerPer>> triggerPerByGroupMap = new HashMap<>();
    	for (int i = 0; i < eventTriggerPerRefList.size(); i++)
    	{
    		RefMarsEventTriggerPer ref = eventTriggerPerRefList.get(i);
    		if (null == ref) continue;
    		triggerPerByGroupMap.computeIfAbsent(ref.group_id, k -> new ArrayList<>()).add(ref);
    	}
    	for(int i = 0; i < eventRefList.size(); i++)
    	{
    		RefMarsEvent ref = eventRefList.get(i);
    		if(null == ref)
    			continue;
    		ArrayList<RefMarsEventTriggerPer> list = triggerPerByGroupMap.get(ref.trigger_rand_group_id);
    		ref.triggerPerRefList = list != null ? list : new ArrayList<>();
    	}
    	
    	//火星科研
    	List<RefMarsTechnology> techRefList = RefMarsTechnology.getMgr().getList();
    	List<RefMarsTechnologyLevel> techLvlRefList = RefMarsTechnologyLevel.getMgr().getList();
    	// 预整理：按 technology_id 分组科研等级数据
    	HashMap<Long, _TLevelMapMgr<RefMarsTechnologyLevel>> techLevelMap = new HashMap<>();
    	for (int i = 0; i < techLvlRefList.size(); i++)
    	{
    		RefMarsTechnologyLevel ref = techLvlRefList.get(i);
    		if (null == ref) continue;
    		techLevelMap.computeIfAbsent(ref.technology_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
    	}
    	//火星科研配置
    	for(int i = 0; i < techRefList.size(); i++)
    	{
    		RefMarsTechnology techRef = techRefList.get(i);
    		if(null == techRef)
    			continue;
    		_TLevelMapMgr<RefMarsTechnologyLevel> levelMapMgr = techLevelMap.get(techRef.id);
    		techRef.setLevelMapMgr(levelMapMgr != null ? levelMapMgr : new _TLevelMapMgr<>());
    	}
    	//火星科研等级配置
    	for(int i = 0; i < techLvlRefList.size(); i++)
    	{
    		RefMarsTechnologyLevel techLvlRef = techLvlRefList.get(i);
    		if(null == techLvlRef)
    			continue;
    		
    		//挂载条件配置
    		ArrayList<RefMarsBuildingCondition> conditionRefList = new ArrayList<>();
    		for(int j = 0; j < techLvlRef.condition_id_list.size(); j++)
    		{
    			RefMarsBuildingCondition condRef = RefMarsBuildingCondition.getMgr().get(techLvlRef.condition_id_list.get(j));
    			if(null == condRef)
    			{
        			CommLog.error("mars tech lvl:{} can not find cond:{} ref.", techLvlRef.id, techLvlRef.condition_id_list.get(j));
    				continue;
    			}
    			
    			conditionRefList.add(condRef);
    		}
    		techLvlRef.conditionRefList = conditionRefList;
    	}
    	
    	//火星探索
    	List<RefMarsExploreLvl> exploreLvlRefList = RefMarsExploreLvl.getMgr().getList();
    	for(int i = 0; i < exploreLvlRefList.size(); i++)
    	{
    		RefMarsExploreLvl exploreLvlRef = exploreLvlRefList.get(i);
    		if(null == exploreLvlRef)
    			continue;
    		
    		//事件pos配置
    		ArrayList<RefMarsExplorePos> posRefList = new ArrayList<>();
    		for(int j = 0; j < exploreLvlRef.pos_list.size(); j++)
    		{
    			RefMarsExplorePos posRef = RefMarsExplorePos.getMgr().get(exploreLvlRef.pos_list.get(j));
    			if(null == posRef)
    			{
    				CommLog.error("mars explore:{} pos:{} not find pos ref.", exploreLvlRef.explore_level, exploreLvlRef.pos_list.get(j));
    				continue;
    			}
    			
    			posRefList.add(posRef);
    		}
			exploreLvlRef.posRefList = posRefList;
    		
    		//矿产pos配置
			ArrayList<RefMarsExplorePos> minePosRefList = new ArrayList<>();
    		for(int j = 0; j < exploreLvlRef.mine_pos_list.size(); j++)
    		{
    			RefMarsExplorePos posRef = RefMarsExplorePos.getMgr().get(exploreLvlRef.mine_pos_list.get(j));
    			if(null == posRef)
    			{
    				CommLog.error("mars explore:{} mine-pos:{} not find pos ref.", exploreLvlRef.explore_level, exploreLvlRef.mine_pos_list.get(j));
    				continue;
    			}
    			
    			minePosRefList.add(posRef);
    		}
			exploreLvlRef.minePosRefList = minePosRefList;

    		//矿产配置
			WeightValueList<RefMarsExploreLvl.MarsExploreMineRndInfo> mineIdList = new WeightValueList<RefMarsExploreLvl.MarsExploreMineRndInfo>();
    		for(int j = 0; j < exploreLvlRef.refresh_mine_list.getList().size(); j++)
    		{
    			WCGPairInt obj = exploreLvlRef.refresh_mine_list.getList().get(j);
    			if(null == obj)
    				continue;

				//构造权重对象
				RefMarsExploreLvl.MarsExploreMineRndInfo info = new RefMarsExploreLvl.MarsExploreMineRndInfo();
				info.mineLvl = obj.first();

				//判断新矿概率是否合法，如果长度不合法则返回默认5000
				if(null != exploreLvlRef.new_mine_per && exploreLvlRef.new_mine_per.size() > j) {
					info.newPer = exploreLvlRef.new_mine_per.get(j);
				}
				else {
					CommLog.error("mars_explore_lvl:{} new_mine_per size fail.", exploreLvlRef.explore_level);
					info.newPer = 5000;
				}
    			
    			mineIdList.add(info, obj.second());
    		}
    		exploreLvlRef.mineIdList = mineIdList;
    		
    		//品质配置
    		ArrayList<MarsBattleQuality> battleQualityList = new ArrayList<>();
    		for(int j = 0; j < exploreLvlRef.refresh_event_quality_list.getList().size(); j++)
    		{
    			WeightValue<EQuality> weiObj = exploreLvlRef.refresh_event_quality_list.getList().get(j);
    			
    			MarsBattleQuality qualityObj = new MarsBattleQuality();
    			qualityObj.setQuality(weiObj.value);
    			battleQualityList.add(qualityObj);
    		}
			//设置战力
			for(int j = 0; j < exploreLvlRef.battle_event_quality_solider_power_list.size(); j++)
			{
				if(j >= battleQualityList.size())
				{
					CommLog.error("mars_explore_lvl:{} battle_event_quality_solider_power_list size fail.", exploreLvlRef.explore_level);
					continue;
				}

				battleQualityList.get(j).setSoliderPower(exploreLvlRef.battle_event_quality_solider_power_list.get(j));
			}
			//设置兵力
			for(int j = 0; j < exploreLvlRef.battle_event_quality_solider_num_list.size(); j++)
			{
				if(j >= battleQualityList.size())
				{
					CommLog.error("mars_explore_lvl:{} battle_event_quality_solider_num_list size fail.", exploreLvlRef.explore_level);
					continue;
				}

				battleQualityList.get(j).setSoliderNum(exploreLvlRef.battle_event_quality_solider_num_list.get(j));
			}
    		//设置奖励
    		for(int j = 0; j < exploreLvlRef.battle_event_quality_reward_list.getCostItemListGroup().size(); j++)
    		{
    			if(j >= battleQualityList.size())
    			{
    				CommLog.error("mars_explore_lvl:{} battle_event_quality_reward_list size fail.", exploreLvlRef.explore_level);
    				continue;
    			}
    			
    			battleQualityList.get(j).getItemList().clear();
    			battleQualityList.get(j).getItemList().addAll(exploreLvlRef.battle_event_quality_reward_list.getCostItemListGroup().get(j).getCostItemList());
    		}
    		exploreLvlRef.battleQualityList = battleQualityList;
    	}
    	
    	//火星探索-battle事件
    	List<RefMarsExploreEventBattle> exploreEventBattleRefList = RefMarsExploreEventBattle.getMgr().getList();
    	HashMap<EQuality, WeightValueList<RefMarsExploreEventBattle>> marsQualityBattleEventMap = new HashMap<>();
    	for(int i = 0; i < exploreEventBattleRefList.size(); i++)
    	{
    		RefMarsExploreEventBattle exploreEventBattleRef = exploreEventBattleRefList.get(i);
    		if(null == exploreEventBattleRef)
    			continue;
    		
    		WeightValueList<RefMarsExploreEventBattle> obj = marsQualityBattleEventMap.computeIfAbsent(exploreEventBattleRef.quality, k -> new WeightValueList<RefMarsExploreEventBattle>());
    		obj.add(exploreEventBattleRef, exploreEventBattleRef.refresh_wei);
    	}
    	RefGeneral.Ref().marsQualityBattleEventMap = marsQualityBattleEventMap;
    	
    	//火星探索-矿产数据
    	List<RefMarsExploreMine> mineRefList = RefMarsExploreMine.getMgr().getList();
    	HashMap<Integer, WeightValueList<RefMarsExploreMine>> marsLvlMineMap = new HashMap<>();
    	for(int i = 0; i < mineRefList.size(); i++)
    	{
    		RefMarsExploreMine mineRef = mineRefList.get(i);
    		if(null == mineRef)
    			continue;
    		
    		WeightValueList<RefMarsExploreMine> obj = marsLvlMineMap.computeIfAbsent(mineRef.mine_lvl, k -> new WeightValueList<RefMarsExploreMine>());
    		obj.add(mineRef, mineRef.res_type_refresh_wei);
    	}
    	RefGeneral.Ref().marsLvlMineMap = marsLvlMineMap;

        //火星移民分档处理
        WeightValueList<RefMarsImmigrationPer> marsMarsImmigrationPerWeiObj = new WeightValueList<>();
        List<RefMarsImmigrationPer> marsImmigrationPerRefList = RefMarsImmigrationPer.getMgr().getList();
        for(int i = 0; i < marsImmigrationPerRefList.size(); i++)
        {
        	RefMarsImmigrationPer ref = marsImmigrationPerRefList.get(i);
        	if(null == ref)
        		continue;
        	
        	RefMarsImmigrationPer preRef = RefMarsImmigrationPer.getMgr().get((ref.id - 1));
        	if(null != preRef)
        	{
        		ref.minNumPer = preRef.can_gain_perple_num_per;
        	}
        	
        	marsMarsImmigrationPerWeiObj.add(ref, ref.wei);
        }
        RefGeneral.Ref().marsMarsImmigrationPerWeiObj = marsMarsImmigrationPerWeiObj;
    }
}

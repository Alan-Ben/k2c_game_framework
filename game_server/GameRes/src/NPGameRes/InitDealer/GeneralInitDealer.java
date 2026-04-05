package NPGameRes.InitDealer;

import Common.HeroObj.Hero_ArenaShowInfo;
import Common.HeroObj.Hero_ArenaShowList;
import Common.MarsEnum.EMarsExploreEventType;
import Common.MarsEnum.EMarsPeopleHelpType;
import NPCommon.Game.EChildBirthRes;
import NPCommon.Game.WeightValueList;
import NPCommon.Util.Pair.WCGPairMarsEventTypeInt;
import NPGameRes.GameObjs.Arena.ArenaHeroObj;
import NPGameRes.Refs.Common.RefProAdd;
import NPGameRes.Refs.Dinner.RefDinnerJoinCost;
import NPGameRes.Refs.Grave.RefGraveType;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;

public class GeneralInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
    	//家人CG权重（本地构建后原子替换，避免热更新时并发问题）
        WeightValueList<Boolean> consortCallRandCgPer = new WeightValueList<>();
        consortCallRandCgPer.add(false, RefGeneral.Ref().consort_call_rand_cg_per.first());
        consortCallRandCgPer.add(true, RefGeneral.Ref().consort_call_rand_cg_per.second());
        RefGeneral.Ref().consortCallRandCgPer = consortCallRandCgPer;

        //家人生子权重（本地构建后原子替换，避免热更新时并发问题）
        WeightValueList<EChildBirthRes> consortCallRandPerWeight = new WeightValueList<>();
        for(int i = 0; i < EChildBirthRes.values().length; i++)
        {
        	if(i < RefGeneral.Ref().consort_call_rand_per.size())
        	{
        		consortCallRandPerWeight.add(EChildBirthRes.values()[i], RefGeneral.Ref().consort_call_rand_per.get(i));
        	}
        }
        RefGeneral.Ref().consortCallRandPerWeight = consortCallRandPerWeight;
        
        //家人领悟概率
        List<RefProAdd> proAddRefList = RefProAdd.getMgr().getList();
        HashMap<Integer, ArrayList<RefProAdd>> proAddRefGroupMap = new HashMap<>();
        for(int i = 0; i < proAddRefList.size(); i++)
        {
        	RefProAdd ref = proAddRefList.get(i);
        	if(null == ref)
        		continue;
        	
        	proAddRefGroupMap.computeIfAbsent(ref.group_id, k -> new ArrayList<>()).add(ref);
        }
        HashMap<Integer, WeightValueList<Integer>> proAddMap = new HashMap<>();
        HashMap<Integer, WeightValueList<Integer>> proAddConsortNotFullMap = new HashMap<>();
        proAddRefGroupMap.forEach((k, v) -> 
        {
        	for(int i = 0; i < v.size(); i++)
        	{
        		proAddMap.computeIfAbsent(k, groupId -> new WeightValueList<>()).add(v.get(i).add, v.get(i).random_weight);
        		if(i < v.size() - 1)
        		{
        			proAddConsortNotFullMap.computeIfAbsent(k, groupId -> new WeightValueList<>()).add(v.get(i).add, v.get(i).random_weight);
        		}
        	}
        });
        RefGeneral.Ref().proAddMap = proAddMap;
        RefGeneral.Ref().proAddConsortNotFullMap = proAddConsortNotFullMap;
        
        //宴会数据
        RefGeneral.Ref().dinnerNpcJoinCostRef = RefDinnerJoinCost.getMgr().get(RefGeneral.Ref().dinner_npc_join_cost_id);
        
        //杰出者大厅系统
        HashSet<Long> graveTitleIdSet = new HashSet<>();
        for(int i = 0; i < RefGraveType.getMgr().getList().size(); i++)
        {
        	RefGraveType ref = RefGraveType.getMgr().getList().get(i);
        	if(null == ref)
        		continue;
        	
        	graveTitleIdSet.addAll(ref.player_title_id_list);
        }
        RefGeneral.Ref().graveTitleIdList.addAll(graveTitleIdSet);
        
        //火星探索事件权重
        ArrayList<WCGPairMarsEventTypeInt> marsExploreRefreshEventTypeWeiList = RefGeneral.Ref().mars_explore_refresh_event_type_wei_list;
        WeightValueList<EMarsExploreEventType> marsExploreRefreshEventTypeWeiObjList = new WeightValueList<>();
        for(int i = 0; i < marsExploreRefreshEventTypeWeiList.size(); i++)
        {
        	WCGPairMarsEventTypeInt obj = marsExploreRefreshEventTypeWeiList.get(i);
        	if(null == obj)
        		continue;
        	
        	marsExploreRefreshEventTypeWeiObjList.add(obj.getType(), obj.getValue());
        }
        RefGeneral.Ref().marsExploreRefreshEventTypeWeiList = marsExploreRefreshEventTypeWeiObjList;
        
        //求助帮助类型权重类型
        WeightValueList<EMarsPeopleHelpType> marsHelpTypeWeiObj = new WeightValueList<>();
        int marsHelpTypeLength = EMarsPeopleHelpType.EMarsPeopleHelpType_Length;
        for(int i = 0; i < marsHelpTypeLength; i++)
        {
        	EMarsPeopleHelpType type = EMarsPeopleHelpType.EMarsPeopleHelpType_FromInt(i);
        	if(null == type || EMarsPeopleHelpType.NONE == type)
        		continue;
        	
        	marsHelpTypeWeiObj.add(type, 1000);
        }
        RefGeneral.Ref().marsHelpTypeWeiObj = marsHelpTypeWeiObj;

        //us arena大臣列表
        ArrayList<ArenaHeroObj> arenaHeroObjList = RefGeneral.Ref().arena_us_hero_list.getArenaHeroList();
        Hero_ArenaShowList arenaShowList = new Hero_ArenaShowList();
        for(int i = 0; i < arenaHeroObjList.size(); i++)
        {
            ArenaHeroObj obj = arenaHeroObjList.get(i);
            if(null == obj)
                continue;

            Hero_ArenaShowInfo info = new Hero_ArenaShowInfo();
            info.setHeroId(obj.getHeroId());
            info.setSkinId(obj.getSkinId());
            info.setLevel(obj.getLevel());
            info.setPower(obj.getPower());

            arenaShowList.addHeroList(info);
        }
        RefGeneral.Ref().arenaUsHeroList = arenaShowList;
    }
}

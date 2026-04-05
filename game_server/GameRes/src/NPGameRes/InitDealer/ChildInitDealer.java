package NPGameRes.InitDealer;

import NPCommon.Game.WeightValueList;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.Child.RefChildAttr;
import NPGameRes.Refs.Child.RefChildCareer;
import NPGameRes.Refs.Child.RefChildQuality;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;
import java.util.List;

public class ChildInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
    	//////////////////========== RefChildAttr 学徒职业权重 ========== //////////////////
    	List<RefChildAttr> attrRefList = RefChildAttr.getMgr().getList();
    	List<RefChildCareer> careerRefList = RefChildCareer.getMgr().getList();
    	for(int i = 0; i < attrRefList.size(); i++)
    	{
    		RefChildAttr attrRef = attrRefList.get(i);
    		if(null == attrRef)
    			continue;
    		
    		WeightValueList<RefChildCareer> weiObjList = new WeightValueList<>();
    		for(int j = 0; j < careerRefList.size(); j++)
        	{
        		RefChildCareer careerRef = careerRefList.get(j);
        		if(null == careerRef)
        			continue;
        		
        		if(attrRef.career_rand_group == careerRef.career_group_id)
        		{
        			weiObjList.add(careerRef, careerRef.career_rand_wei);
        		}
        	}
    		attrRef.careerWeiList = weiObjList;
    	}
    	
    	//////////////////========== RefChildQuality 学徒品质 ========== //////////////////
    	for(int i = 0; i < RefChildQuality.getMgr().getList().size(); i++)
    	{
    		RefChildQuality qualityRef = RefChildQuality.getMgr().getList().get(i);
    		if(null == qualityRef)
    			continue;
    	
    		ArrayList<Integer> addBonusStepList = new ArrayList<>();
    		
    		//最大等级
    		int maxLvl = 0;
    		for(int j = 0; j < qualityRef.step_lvl.size(); j++)
    		{
    			if(qualityRef.step_lvl.get(j) > maxLvl)
    				maxLvl = qualityRef.step_lvl.get(j);
    		}
    		qualityRef.maxLvl = maxLvl;
    		
    		//计算需要增加基础收益的等级列表
    		double perLvl = 1.0f * qualityRef.maxLvl * RefGeneral.Ref().child_cal_unit_per / 10000f;
    		if(perLvl <= 0 || perLvl >= qualityRef.maxLvl)
    		{
    			CommLog.error("RefChildQuality init addBonusStepList fail, maxLvl:{} per:{} perLvl:{}"
    					, qualityRef.maxLvl, RefGeneral.Ref().child_cal_unit_per, perLvl);
    		}
    		else
    		{
    			int curCount = 1;
    			int curLvl = (int) Math.ceil(perLvl * curCount);
    			while(curLvl < qualityRef.maxLvl)
    			{
    				curLvl = (int) Math.ceil(perLvl * curCount);
    				addBonusStepList.add(curLvl);
    				
    				curCount++;
    			}
    		}
    		qualityRef.addBonusStepList = addBonusStepList;
    		
    		//CommLog.error("====> {} | {} | {} | {}", qualityRef.id, qualityRef.maxLvl, perLvl, CommonFunc.list2String(qualityRef.addBonusStepList));
    	}
    }
}

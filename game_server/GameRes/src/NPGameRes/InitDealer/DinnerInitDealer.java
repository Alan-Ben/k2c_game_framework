package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Dinner.RefDinnerJoinCost;
import NPGameRes.Refs.Dinner.RefDinnerPermit;
import NPGameRes.Refs.Dinner.RefDinnerType;
import NPGameRes.Refs.RefPlayerFixedCd;

/**
 * 初始化宴会对象
 */
public class DinnerInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
    	//====== RefDinnerPermit
    	for(int i = 0; i < RefDinnerPermit.getMgr().getList().size(); i++)
    	{
    		RefDinnerPermit ref = RefDinnerPermit.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		RefDinnerType dinnerRef = RefDinnerType.getMgr().get(ref.dinner_id);
    		if(null == dinnerRef)
    		{
    			CommLog.error("RefDinnerPermit:{} RefDinnerType:{} init related ref fail, not find RefDinnerType.", ref.permit_type, ref.dinner_id);
    			continue;
    		}
    		ref.dinnerRef = dinnerRef;
    	}
    	
    	//====== RefDinnerJoinCost
    	for(int i = 0; i < RefDinnerJoinCost.getMgr().getList().size(); i++)
    	{
    		RefDinnerJoinCost ref = RefDinnerJoinCost.getMgr().getList().get(i);
    		if(null == ref)
    			continue;
    		
    		if(ref.fixed_cd_id > 0)
    		{
    			RefPlayerFixedCd fixedCdRef = RefPlayerFixedCd.getMgr().get(ref.fixed_cd_id);
    			if(null == fixedCdRef)
    			{
    				CommLog.error("RefDinnerJoinCost:{} fixedCd:{} init fixedCdRef fail, not find RefPlayerFixedCd.", ref.id, ref.fixed_cd_id);
    				continue;
    			}
    			
    			ref.fixedCdRef = fixedCdRef;
    		}
    	}
    }
}

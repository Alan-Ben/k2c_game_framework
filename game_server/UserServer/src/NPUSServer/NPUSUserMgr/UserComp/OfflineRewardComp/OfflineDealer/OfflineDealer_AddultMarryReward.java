package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.ChildObj.Adult_MarryReward;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPCommon.NPCommon_ItemInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.MarriedAdultInfo;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;

public class OfflineDealer_AddultMarryReward extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.ADULT_MARRY_REWARD;
    }

    @Override
    public boolean isValid()
    {
        return true;
    }

    @Override
    public boolean syncToClient()
    {
        return true;
    }

    @Override
    protected void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context)
    {
    }
    
    /**
     * 创建子嗣联姻离线奖励数据
     * 
     * @param _marriedAdult
     * @param _context
     */
    public static void addAdultMarryReward(MarriedAdultInfo _marriedAdult, NPCommon_ItemInfo _marriedItem, NPPlayerContext _context)
    {
    	//离线数据
    	Adult_MarryReward offlineAdultMarryRewardObj = new Adult_MarryReward();
		offlineAdultMarryRewardObj.setMarriedInfo(_marriedAdult.toMarriedProto());
		
		//构造对应奖励数据
        if(null != _marriedItem)
        {
        	//离线奖励展示数据
        	offlineAdultMarryRewardObj.setItem(_marriedItem);
        }
		
		//创建离线数据
        _marriedAdult.getUserData().getOfflineRewardComponent().addReward(EOfflineRewardEnum.ADULT_MARRY_REWARD
        		, null
        		, null
        		, offlineAdultMarryRewardObj.makePackage()
        		, _context);
    }
}

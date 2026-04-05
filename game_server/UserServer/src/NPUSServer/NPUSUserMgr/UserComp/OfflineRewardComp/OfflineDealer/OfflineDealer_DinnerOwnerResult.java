package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.DinnerObj.Dinner_ResultInfo;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 发送宴会主人结算数据
 * @author mj
 *
 */
public class OfflineDealer_DinnerOwnerResult extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.DINNER_OWNER_RESULT;
	}

	@Override
	public boolean isValid() 
	{
		return true;
	}

	@Override
	public boolean syncToClient() 
	{
		return false;
	}

	@Override
	public void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context)
	{
		if(null == _info.getOfflineData())
		{
			return;
		}
		
		try
		{
			//开宴玩家结算数据
			Dinner_ResultInfo obj = new Dinner_ResultInfo();
			obj.readPackage(_info.getOfflineData());
			
			//记录开宴奖励
			_info.getUserData().getDinnerComponent().addOwnerReward(obj, _context);
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

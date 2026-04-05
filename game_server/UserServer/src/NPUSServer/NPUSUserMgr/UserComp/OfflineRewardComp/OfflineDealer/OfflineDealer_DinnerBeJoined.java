package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.Common_Long;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 开宴玩家增加宴会交互记录
 * @author mj
 *
 */
public class OfflineDealer_DinnerBeJoined extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.DINNER_BE_JOINED;
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
			Common_Long obj = new Common_Long();
			obj.readPackage(_info.getOfflineData());
			
			//开宴玩家增加宴会交互记录
			_info.getUserData().getDinnerComponent().addLastEachLog(false, obj.getValue(), _context);
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

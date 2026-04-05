package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_AdultMarryApplyInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 发送宴会主人结算数据
 * @author mj
 *
 */
public class OfflineDealer_AdultMarryAcceptPersonApply extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.ADULT_MARRY_ACCEPT_PERSON_APPLY;
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
			ServerObj_AdultMarryApplyInfo obj = new ServerObj_AdultMarryApplyInfo();
			obj.readPackage(_info.getOfflineData());
			
			_info.getUserData().getChildComponent().getToMeMarryApplyMgr().add(obj, _context);
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

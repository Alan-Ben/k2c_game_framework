package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_AdultMarriedInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 发送宴会主人结算数据
 * @author mj
 *
 */
public class OfflineDealer_AdultMarryAgreeServerApply extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.ADULT_MARRY_AGREE_SERVER_APPLY;
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
			ServerObj_AdultMarriedInfo obj = new ServerObj_AdultMarriedInfo();
			obj.readPackage(_info.getOfflineData());
			
			UnmarryAdultInfo adult = _info.getUserData().getChildComponent().getAdultMgr().lookupUnmarryAdult(obj.getAdultId());
			if(null == adult)
			{
				return;
			}
			
			//检查子嗣状态，此时只能是 空闲/服务器请求 状态
			if(adult.isApplyPerson())
			{
				return;
			}
			
			//设置子嗣空闲
			adult.setIdle(true, _context);
			
			//设置子嗣
			_info.getUserData().getChildComponent().getAdultMgr().beAgreedMatchMarriedAdult(obj, _context);
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

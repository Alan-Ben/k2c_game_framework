package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_PlayerGmCommand;
import NPCommon.GMCommand.GmCommandMgr;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GMCommand.UsCmdPlayerExecutor;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 玩家命令
 * @author mj
 *
 */
public class OfflineDealer_GM extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.GM;
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
			//领取奖励
			Offline_PlayerGmCommand obj = new Offline_PlayerGmCommand();
			obj.readPackage(_info.getOfflineData());

			//执行GM命令
			UsCmdPlayerExecutor execContext = new UsCmdPlayerExecutor(_info.getUserData());
			GmCommandMgr.getInstance().run(execContext, obj.getCommand(), _context, (_bSucc, _result) ->
			{

			});
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

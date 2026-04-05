package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_FriendRemove;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 发送宴会主人结算数据
 * @author mj
 *
 */
public class OfflineDealer_FriendRemove extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.FRIEND_REMOVE;
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
			ServerObj_FriendRemove obj = new ServerObj_FriendRemove();
			obj.readPackage(_info.getOfflineData());
			
			 //移除好友
			_info.getUserData().getFriendComponent().getFriendMgr().removeFriend(obj.getSendCid(), _context);
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

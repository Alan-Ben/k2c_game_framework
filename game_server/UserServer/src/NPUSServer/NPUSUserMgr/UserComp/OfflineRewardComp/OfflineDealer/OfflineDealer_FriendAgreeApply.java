package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_FriendAgree;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 发送宴会主人结算数据
 * @author mj
 *
 */
public class OfflineDealer_FriendAgreeApply extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.FRIEND_AGREE_APPLY;
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
			ServerObj_FriendAgree obj = new ServerObj_FriendAgree();
			obj.readPackage(_info.getOfflineData());
			
			 //加入好友
			_info.getUserData().getFriendComponent().getFriendMgr().addFriend(obj.getAgreeCid(), false, _context);
            //移除申请
			_info.getUserData().getFriendComponent().getFriendApplyMgr().removeApply(obj.getAgreeCid(), _context);
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

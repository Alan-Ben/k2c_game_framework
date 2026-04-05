package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_GuildMarsHelpSuc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp._IGuildMarsHelp;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.USLog;

/**
 * 发送宴会主人结算数据
 * @author mj
 *
 */
public class OfflineDealer_GuildMarsHelpSuc extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.GUILD_MARS_HELP_SUC;
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
			Offline_GuildMarsHelpSuc obj = new Offline_GuildMarsHelpSuc();
			obj.readPackage(_info.getOfflineData());
			 
			_IGuildMarsHelp help = _info.getUserData().getMarsComponent().getGuildMarsHelpMgr().beDealedGuildHelpSuc(obj.getObjType(), obj.getObjId(), obj.getHelpId(), obj.getHelpSecs());
			if(null != help)
			{
				//推送数据
				_info.getUserData().sendMsgToGC(
						US2GCWriter_042_GuildRelatedOp.make_053_OnMyMarsHelpDealed(obj.getHelpId(), 
								obj.getDealedCid(), obj.getDealedCount(), obj.getDealLimit(), 
								obj.getObjType(), obj.getObjId()));
			}
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

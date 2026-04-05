package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.MailObj.Mail_Data;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_GuildBoxSettleList;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 联盟宝箱-玩家下发领取邮件
 * @author mj
 *
 */
public class OfflineDealer_GuildBoxDispatch extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.GUILD_BOX_DISPATCH;
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
			//解析结算数据
			ServerObj_GuildBoxSettleList objList = new ServerObj_GuildBoxSettleList();
			objList.readPackage(_info.getOfflineData());
			
			//结算所有宝箱数据
			_info.getUserData().getGuildBoxComponent().dispatchAll(objList, _context);
	
			//存在可以发送的奖励物品
			if(!_context.getCollector().isEmpty())
			{
				//构造邮件数据
				Mail_Data maiData = new Mail_Data();
				maiData.setMailRefId(RefGeneral.Ref().guild_box_dispatch_mail_id);
				maiData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(_context.getCollector().getAllItemList()));
	            
				_info.getUserData().getMailComponent().addMail(maiData, _context);
			}
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_GuildMarsHelpAutoDeal;
import NPCommon.CommonObj.NPCommonCostItem;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

/**
 * 发送宴会主人结算数据
 * @author mj
 *
 */
public class OfflineDealer_GuildMarsHelpAutoDeal extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum() 
	{
		return EOfflineRewardEnum.GUILD_MARS_HELP_AUTO_DEAL;
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
			Offline_GuildMarsHelpAutoDeal obj = new Offline_GuildMarsHelpAutoDeal();
			obj.readPackage(_info.getOfflineData());
			 
			//处理帮助请求
	        int dealCount = obj.getDealedCount();
	        //自动领取帮助奖励
	        long cdCount = _info.getUserData().getFixedCdComponent().getItemCount(RefGeneral.Ref().guild_mars_help_deal_reward_fixed_cd_id);
	        long realCount = Math.min(dealCount, cdCount);
	        if(realCount > 0)
	        {
	        	//扣除次数
	        	if(!_info.getUserData().getFixedCdComponent().spendItem(RefGeneral.Ref().guild_mars_help_deal_reward_fixed_cd_id, realCount, _context))
	        		return;
	        	
	        	NPCommonCostItem gainItem = RefGeneral.Ref().guild_mars_help_deal_reward_item.duplicate();
	        	gainItem.setCount(gainItem.getCount() * realCount);
	        	_info.getUserData().gainItem(gainItem, _context);
	        }
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}
}

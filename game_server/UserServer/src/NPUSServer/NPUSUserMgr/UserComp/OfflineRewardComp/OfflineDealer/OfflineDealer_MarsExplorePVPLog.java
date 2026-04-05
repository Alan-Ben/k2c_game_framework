package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_MarsExplorePVPLog;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

public class OfflineDealer_MarsExplorePVPLog extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.MARS_EXPLORE_PVP_LOG;
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
    protected void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context)
    {
		if(null == _info.getOfflineData())
		{
			return;
		}
		
		try
		{
			ServerObj_MarsExplorePVPLog obj = new ServerObj_MarsExplorePVPLog();
			obj.readPackage(_info.getOfflineData());

            _info.getUserData().getMarsExploreComponent().getPVPLogObj()
                    .addStartLog(obj.getLogType(), obj.getCreatedAt(), obj.getLogData());
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
    }
}

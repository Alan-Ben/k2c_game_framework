package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_MarsMineSettle;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

public class OfflineDealer_MarsMineTeamSettle extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.MARS_MINE_TEAM_SETTLE;
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
			ServerObj_MarsMineSettle obj = new ServerObj_MarsMineSettle();
			obj.readPackage(_info.getOfflineData());

			//更新队伍状态
			MarsExploreTeam team = _info.getUserData().getMarsExploreComponent().getTeamMgr().lookup(obj.getTeamId());
			if(null == team)
			{
				USLog.error(_info.getUserData().getUSServer(), "player:{} team:{} not find team for MARS_MINE_TEAM_SETTLE.", _info.getUserData().getCid(), obj.getTeamId());
				return;
			}

			//设置队伍损耗数量
			team.setLossValue(obj.getLossValue());
			
			//设置采集状态完成，进入返程状态
			team.getStateMachine().setCollectDone(obj, NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_COLLECT_END));

			//记录提交资源
			team.getCollectResultMgr().submitResult(obj.getRefId(), obj.getNum());
			
			//增加采矿数量记录
			_info.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_TEAM_COLLECT_SUM, obj.getNum(), _context);
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
    }
}

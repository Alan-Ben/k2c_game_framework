package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_MarsTeam_OccupyMineResult;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.USLog;

public class OfflineDealer_MarsTeamOccupyResult extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.MARS_TEAM_OCCUPY_RESULT;
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
			//读取离线数据
			ServerObj_MarsTeam_OccupyMineResult obj = new ServerObj_MarsTeam_OccupyMineResult();
			obj.readPackage(_info.getOfflineData());

			//不论是否在线都通过本机制进行处理
			NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK);

			//移除火星矿数据
			//移除数据需要处理的是另外的操作
//			if(obj.getIsEnd())
//			{
//				_info.getUserData().getMarsMineComponent().removeMine(obj.getMineInstanceId(), context);
//			}
			
			//更新火星队伍数据
			MarsExploreTeam team = _info.getUserData().getMarsExploreComponent().getTeamMgr().lookup(obj.getTeamId());
			if(null != team)
			{
				//更新战损
				team.setLossValue(obj.getTeamLossValue());

				//判断胜负，失败则直接遣返
				if(!obj.getIsWin())
				{
					//直接进入返回状态
					team.getStateMachine().sendbackTeam(obj.getBattleTimeMs(), context);
				}
				else
				{
					//胜利进入采集状态
					team.getStateMachine().enterCollect(obj, context);

					//更新占领计数
					_info.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_EXPLORER_OCCUPY_MINE_NUM, 1, context);
				}
			}
		
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
    }
}

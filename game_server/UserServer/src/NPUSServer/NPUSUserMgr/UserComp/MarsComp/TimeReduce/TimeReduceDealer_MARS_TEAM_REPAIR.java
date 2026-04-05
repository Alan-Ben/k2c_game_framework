package NPUSServer.NPUSUserMgr.UserComp.MarsComp.TimeReduce;

import Common.MarsEnum.EMarsBagItemUseTimeType;
import Common.MarsEnum.EMarsExploreTeamState;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState.ExploreTeamState_REPAIR;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState._AExploreTeamState;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;

public class TimeReduceDealer_MARS_TEAM_REPAIR extends _ATimeReduceDealer
{
	@Override
	public EMarsBagItemUseTimeType getType() {return EMarsBagItemUseTimeType.MARS_TEAM_REPAIR;}

	@Override
	protected Result _checkObj(NPUSUserData _userData, long _objId) 
	{
		MarsExploreTeam team = _userData.getMarsExploreComponent().getTeamMgr().lookup(_objId);
		if(null == team)
			return MarsErr.MARS_EXPLORE_TEAM_NOT_FOUND;
		
		//刷新队伍状态，检查队伍是否还在维修状态
		team.getStateMachine().refreshState();
		if(EMarsExploreTeamState.REPAIR != team.getStateMachine().getCurState().getStateType())
			return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
		
		return Result.SUCC;
	}

	@Override
	protected Result _deal(NPUSUserData _userData, long _objId, int _secs, NPPlayerContext _context) 
	{
		MarsExploreTeam team = _userData.getMarsExploreComponent().getTeamMgr().lookup(_objId);
		
		_AExploreTeamState state = team.getStateMachine().getCurState();
		if(!(state instanceof ExploreTeamState_REPAIR))
			return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
			
		ExploreTeamState_REPAIR repairState = (ExploreTeamState_REPAIR) state;
		int realUsedSecs = repairState.reduceSecs(_secs, _context);
		//存在实际减少的时长，换算成分钟（不满1分钟算1分钟）
		if(realUsedSecs > 0)
		{
			//推送数据，用于客户端计算物品互助时长
			pushHelpSecsChg(_userData, _objId, repairState.getExtData().getItemHelpSecs());
			//成功减少时长
			countUsedMins(_userData, realUsedSecs, _context);
			//累计队伍维修实际使用分钟，用于成就
			countTeamRepairUsedMins(_userData, realUsedSecs, _context);
		}
		//获取溢出补偿道具
		gainOverChange(_userData, (_secs - realUsedSecs), _context);
		
		return Result.SUCC;
	}
	
	/**
	 * 累计队伍维修实际使用分钟，用于成就
	 * @param _userData
	 * @param _usedSecs
	 * @param _context
	 */
	public void countTeamRepairUsedMins(NPUSUserData _userData, long _usedSecs, NPPlayerContext _context) 
	{
		if(_usedSecs <= 0)
			return;

		//换算成分钟
		long mins = (long) (Math.ceil(_usedSecs / 60f));
		//记录总互助时长
		_userData.getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_TEAM_REPAIR_TIME_REDUCED_MIN_SUM, mins, _context);
	}
}

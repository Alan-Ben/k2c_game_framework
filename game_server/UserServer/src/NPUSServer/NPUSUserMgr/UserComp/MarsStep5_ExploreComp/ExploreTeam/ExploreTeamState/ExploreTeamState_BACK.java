package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.MarsEnum.EMarsExploreTeamState;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import USDB.Bo.PlayerMarsExploreTeamBO;


/**
 * 火星队伍状态 - 返程中
 * @author mj
 *
 */
public class ExploreTeamState_BACK extends _AExploreTeamState
{
	public ExploreTeamState_BACK(MarsExploreTeam _team, PlayerMarsExploreTeamBO _bo)
	{
		super(_team, _bo);
	}
	public ExploreTeamState_BACK(MarsExploreTeam _team, long _startTimeMS, long _keepTimeMS, long _targetPos)
	{
		//采集由于可能被攻击，因此返回的起始时间使用当前时间
		super(_team, _startTimeMS, _keepTimeMS, _targetPos);
	}
    /**
     * 采集状态切换到返程中
     * @param _preState 上一个采集状态
     */
    public ExploreTeamState_BACK(ExploreTeamState_COLLECT _preState)
    {
        //采集由于可能被攻击，因此返回的起始时间使用当前时间
        super(_preState.getTeam(), _preState.getStateEndMs(), _preState.getExtData().getMarchTimeMS(), _preState.getExtData().getPos());
    }
    /**
     * 等待集结状态切换到返程中
     * @param _preState 上一个等待集结状态
     */
    public ExploreTeamState_BACK(ExploreTeamState_WAIT_RALLY _preState)
    {
        //等待集结状态无额外扩展数据，返程参数直接沿用基础状态字段
        super(_preState.getTeam(), _preState.getStateEndMs(), _preState.getKeepTimeMS(), _preState.getTargetPos());
    }

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.BACK;
	}
	
	@Override
	protected	void _loadExtData(PlayerMarsExploreTeamBO _bo)
	{
	}
	
	@Override
	public _IALProtocolStructure getExtData()
	{
		return null;
	}

	@Override
	public boolean canEnter(_AExploreTeamState _state) 
	{
		//只能从 战斗/Boss战斗/行军/等待集结/采集 状态开启
		return EMarsExploreTeamState.BATTLE == _state.getStateType()
				|| EMarsExploreTeamState.BOSS_BATTLE == _state.getStateType()
				|| EMarsExploreTeamState.MARCH == _state.getStateType()
				|| EMarsExploreTeamState.WAIT_RALLY == _state.getStateType()
				|| EMarsExploreTeamState.COLLECT == _state.getStateType()
				;
	}

	@Override
	protected void _onEnter() 
	{
	}

	@Override
	public boolean canQuit(_AExploreTeamState _targetState)
	{
		return CommonFunc.getNowTimeMS() >= getStateEndMs();
	}

	@Override
	protected void _onQuit() 
	{
		getTeam().onBackDone();
	}

	@Override
	public _AExploreTeamState getNextState() 
	{
		return new ExploreTeamState_IDLE(getTeam());
	}
    /**
     * 服务器启动后的一次性状态校验
     * 默认不处理，具体状态按需重载
     */
    @Override
    public void onSInitedCheck() { }

}

package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.MarsEnum.EMarsExploreTeamState;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import USDB.Bo.PlayerMarsExploreTeamBO;

/**
 * 火星队伍状态 - 错误中（需要手动处理）
 * @author mj
 *
 */
public class ExploreTeamState_ERROR extends _AExploreTeamState
{
	public ExploreTeamState_ERROR(MarsExploreTeam _team)
	{
		super(_team, 0, 0, 0);
	}
	public ExploreTeamState_ERROR(MarsExploreTeam _team, PlayerMarsExploreTeamBO _bo)
	{
		super(_team, _bo);
	}

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.ERROR;
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
		//任何状态都可以进入
		return true;
	}

	@Override
	protected void _onEnter() 
	{
	}

	@Override
	public boolean canQuit(_AExploreTeamState _targetState)
	{
		//只能手动处理退出该状态
		return false;
	}

	@Override
	protected void _onQuit() 
	{
	}

	@Override
	public _AExploreTeamState getNextState() 
	{
		return null;
	}
    /**
     * 服务器启动后的一次性状态校验
     * 默认不处理，具体状态按需重载
     */
    @Override
    public void onSInitedCheck() { }

}

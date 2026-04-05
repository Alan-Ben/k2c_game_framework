package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.MarsEnum.EMarsExploreTeamState;
import NPGameRes.Refs.Mars.RefMarsExploreTeam;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsExploreTeamBO;

/**
 * 火星队伍状态 - 未解锁
 * @author mj
 *
 */
public class ExploreTeamState_NONE extends _AExploreTeamState
{
	public ExploreTeamState_NONE(MarsExploreTeam _team)
	{
		super(_team, 0, 0, 0);
	}

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.NONE;
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
		//只能解锁一次
		return false;
	}

	@Override
	protected void _onEnter() 
	{
	}

	@Override
	public boolean canQuit(_AExploreTeamState _targetState)
	{
		RefMarsExploreTeam ref = RefMarsExploreTeam.getMgr().get(getTeam().getTeamId());
		if(null == ref)
		{
			USLog.error(getTeam().getUSServer(), "player:{} team:{} quit none fail, not find ref.", getTeam().getCid(), getTeam().getTeamId());
			return false;
		}
		
		if(!NPPlayerConditionDealerMgr.IsEnable(ref.unlock_cond, getTeam().getUserData(), null))
			return false;
		
		return true;
	}

	@Override
	protected void _onQuit() 
	{
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

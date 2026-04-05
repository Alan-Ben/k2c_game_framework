package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_Collect;
import Common.ServerObj.ServerObj_MarsMine;
import Common.ServerObj.ServerObj_MarsTeam_OccupyMineResult;
import NPCommon.ErrMain.MarsErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import USDB.Bo.PlayerMarsExploreTeamBO;

import java.nio.ByteBuffer;

/**
 * 火星队伍状态 - 采集中
 * @author mj
 *
 */
public class ExploreTeamState_COLLECT extends _AExploreTeamState
{
	private MarsTeamState_Collect _m_edExtraData;

	public ExploreTeamState_COLLECT(MarsExploreTeam _team, PlayerMarsExploreTeamBO _bo)
	{
		super(_team, _bo);
	}

	/**
	 * 来自行军状态的转变
	 */
	public ExploreTeamState_COLLECT(MarsExploreTeam _team, long _startTimeMS, long _keepTimeMS, long _targetPos, ServerObj_MarsTeam_OccupyMineResult _occupyMineResult)
	{
		super(_team, _startTimeMS, _keepTimeMS, _targetPos);

		_m_edExtraData = new MarsTeamState_Collect();
		_m_edExtraData.setMineInstanceId(_occupyMineResult.getMineInstanceId());
		_m_edExtraData.setMarchTimeMS(_keepTimeMS);
		_m_edExtraData.setPos(_targetPos);
		_m_edExtraData.setStartCollectMs(_occupyMineResult.getStartCollectMs());
		_m_edExtraData.setEndCollectMs(_occupyMineResult.getEndCollectMs());
	}
	
	/**
	 * 需要重载积累的状态截至时间
	 * 
	 * 返回值说明：
	 		0 - 尚未确认采集结束时间，此时不能退出状态
	 */
	@Override
	public long getStateEndMs() 
	{
		return _m_edExtraData.getEndCollectMs();
	}

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.COLLECT;
	}
	
	@Override
	protected	void _loadExtData(PlayerMarsExploreTeamBO _bo)
	{
		_m_edExtraData = new MarsTeamState_Collect();

		if(null != _bo.getExtData())
		{
			ByteBuffer buff = ByteBuffer.wrap(_bo.getExtData());
			_m_edExtraData.readPackage(buff);
		}
	}
	
	@Override
	public MarsTeamState_Collect getExtData()
	{
		return _m_edExtraData;
	}
	
	@Override
	public boolean canEnter(_AExploreTeamState _state) 
	{
		//只能从行军状态开启
		return EMarsExploreTeamState.MARCH == _state.getStateType();
	}

	@Override
	protected void _onEnter() 
	{
		MarsMineSystem.GetMarsMine(getTeam().getUSServer(), _m_edExtraData.getMineInstanceId(), 
				new _ICallBackIntT<ServerObj_MarsMine>() 
		{
			@Override
			public void onRunOver(int _err, ServerObj_MarsMine _mine) 
			{
				if(MarsErr.MARS_MINE_NOT_FOUND.getCode() == _err)
				{
					//直接回城
					getTeam().getStateMachine().sendbackTeam(CommonFunc.getNowTimeMS(), NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));
				}
			}
		});
	}

	@Override
	protected void _onQuit() 
	{
	}

	@Override
	public boolean canQuit(_AExploreTeamState _targetState)
	{
		if(null != _targetState && _targetState.getStateType() == EMarsExploreTeamState.BACK)
			return true;

		return _m_edExtraData.getEndCollectMs() > 0 && CommonFunc.getNowTimeMS() > _m_edExtraData.getEndCollectMs();
	}

	@Override
	public _AExploreTeamState getNextState() 
	{
		return new ExploreTeamState_BACK(this);
	}
    /**
     * 服务器启动后的一次性状态校验
     * 默认不处理，具体状态按需重载
     */
    @Override
    public void onSInitedCheck() { }
}

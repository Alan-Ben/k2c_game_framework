package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import AllRpcData.US_Service.Mars.MarsRallySInitedCheck;
import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_March;
import Common.ServerObj.ServerObj_MarsMine;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.GuildRallyErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.PlayerMarsExploreTeamBO;

import java.nio.ByteBuffer;

/**
 * 火星队伍状态 - 行军中
 * @author mj
 *
 */
public class ExploreTeamState_MARCH extends _AExploreTeamState
{
	//行军结束后的容错等待时间（毫秒）
	private static final long S_INITED_CHECK_TOLERANCE_MS = 30 * 1000L;

	//额外数据
	private MarsTeamState_March _m_edExtraData;
	private EMarsExploreTeamState _m_eTargetState;
	
	public ExploreTeamState_MARCH(MarsExploreTeam _team, PlayerMarsExploreTeamBO _bo)
	{
		super(_team, _bo);
	}
	
	public ExploreTeamState_MARCH(MarsExploreTeam _team, long _startTimeMS, long _keepTimeMS, long _targetPos, MarsTeamState_March _extData)
	{
		super(_team, _startTimeMS, _keepTimeMS, _targetPos);
    	
    	_m_edExtraData = new MarsTeamState_March();
    	_m_edExtraData.readPackage(_extData.makePackage());

		_m_eTargetState = EMarsExploreTeamState.EMarsExploreTeamState_FromInt(_m_edExtraData.getTargetState());
	}

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.MARCH;
	}

	@Override
	protected	void _loadExtData(PlayerMarsExploreTeamBO _bo)
	{
		_m_edExtraData = new MarsTeamState_March();

		if(null != _bo.getExtData())
		{
			ByteBuffer buff = ByteBuffer.wrap(_bo.getExtData());
			_m_edExtraData.readPackage(buff);
		}

		_m_eTargetState = EMarsExploreTeamState.EMarsExploreTeamState_FromInt(_m_edExtraData.getTargetState());
	}

	@Override
	public MarsTeamState_March getExtData()
	{
		return _m_edExtraData;
	}

	@Override
	public boolean canEnter(_AExploreTeamState _state)
	{
		//只能从空闲状态开启
		return EMarsExploreTeamState.IDLE == _state.getStateType();
	}

	@Override
	protected void _onEnter() 
	{
	}

	@Override
	public boolean canQuit(_AExploreTeamState _targetState)
	{
		//完成行军
		return CommonFunc.getNowTimeMS() > getStateEndMs();
	}

	@Override
	protected void _onQuit() 
	{
	}

	@Override
	public _AExploreTeamState getNextState() 
	{
		if(EMarsExploreTeamState.BATTLE == _m_eTargetState)
		{
			return new ExploreTeamState_BATTLE(this);
		}
		else if(EMarsExploreTeamState.BOSS_BATTLE == _m_eTargetState)
		{
			return new ExploreTeamState_BOSS_BATTLE(this);
		}
		else if(EMarsExploreTeamState.COLLECT == _m_eTargetState)
		{
			//return new ExploreTeamState_COLLECT(this);
			//采集不做处理，等待服务器内部机制处理
			return null;
		}
		
		return null;
	}

	/**
	 * 启动后行军状态二次校验：处理WAIT_RALLY与COLLECT两类目标状态，避免跨服启动不同步导致状态卡住
	 */
	@Override
	public void onSInitedCheck()
	{
        //判断时间是否达到需要检测的时间
		long nowMs = CommonFunc.getNowTimeMS();
		if(nowMs <= getStateEndMs() + S_INITED_CHECK_TOLERANCE_MS)
			return;

		final long stateSerialize = getStateSerialize();

		if(EMarsExploreTeamState.WAIT_RALLY == _m_eTargetState)
		{
			_handleWaitRallySInitedCheck(stateSerialize);
		}
        else if(EMarsExploreTeamState.COLLECT == _m_eTargetState)
		{
			_handleCollectSInitedCheck(stateSerialize);
		}
	}

	/**
	 * WAIT_RALLY启动后校验：超时后向联盟所在服确认集结是否还存在
	 */
	private void _handleWaitRallySInitedCheck(final long _stateSerialize)
	{
		final long guildId = getTeam().getUserData().getGuildComponent().getGuildId();
		final long rallyId = _m_edExtraData.getInstanceId();

		// guildId或rallyId异常时直接重置，避免队伍状态卡死
		if(guildId <= 0 || rallyId <= 0)
		{
			getTeam().resetIdleState(_stateSerialize, NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));
			return;
		}

		int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(guildId);

		MarsRallySInitedCheck rpc = new MarsRallySInitedCheck();
		rpc.req().setCid(getTeam().getCid());
		rpc.req().setGuildId(guildId);
		rpc.req().setRallyId(rallyId);

		getTeam().getUSServer().rpc2us().requestToRepeat(guildUsId, rpc,
				new _ARpcCallBack<MarsRallySInitedCheck>()
				{
					@Override
					public void call_back(int _errCode, MarsRallySInitedCheck _rpc)
					{
						if(_errCode == CommErr.SYS_ERR.getCode())
						{
							USLog.error(getTeam().getUSServer(),
									"ExploreTeamState_MARCH.onSInitedCheck - query rally rpc fail: cid={}, teamId={}, guildId={}, rallyId={}, err={}",
									getTeam().getCid(), getTeam().getTeamId(), guildId, rallyId, _errCode);
							return;
						}

						if(_errCode == GuildRallyErr.RALLY_NOT_FOUND.getCode()
								|| _errCode == GuildErr.MEMBER_NOT_FOUND.getCode())
						{
							getTeam().resetIdleState(_stateSerialize, NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));
						}
					}
				},
				3,
				() -> USLog.error(getTeam().getUSServer(),
						"ExploreTeamState_MARCH.onSInitedCheck - send rally check rpc fail: cid={}, teamId={}, guildId={}, rallyId={}",
						getTeam().getCid(), getTeam().getTeamId(), guildId, rallyId));
	}

	/**
	 * COLLECT启动后校验：超时后确认矿点仍存在且归属当前玩家
	 */
	private void _handleCollectSInitedCheck(final long _stateSerialize)
	{
		final long mineInstanceId = _m_edExtraData.getInstanceId();
		MarsMineSystem.GetMarsMine(getTeam().getUSServer(), mineInstanceId, new _ICallBackIntT<ServerObj_MarsMine>()
		{
			@Override
			public void onRunOver(int _err, ServerObj_MarsMine _mineObj)
			{
				//系统错误不做处理，避免做了错误重置
				if(_err == CommErr.SYS_ERR.getCode())
				{
					USLog.error(getTeam().getUSServer(),
							"ExploreTeamState_MARCH.onSInitedCheck - check mine owner rpc fail: cid={}, teamId={}, mineInstanceId={}, err={}",
							getTeam().getCid(), getTeam().getTeamId(), mineInstanceId, _err);
					return;
				}

				//矿不存在或者非本玩家拥有则开始重置
				boolean needResetIdle = false;
				if(_err == MarsErr.MARS_MINE_NOT_FOUND.getCode())
				{
					needResetIdle = true;
				}
				else if(_err == 0)
				{
					needResetIdle = (_mineObj == null) || (_mineObj.getCid() != getTeam().getCid());
				}

				if(needResetIdle)
				{
					getTeam().resetIdleState(_stateSerialize, NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));
				}
			}
		});
	}
}

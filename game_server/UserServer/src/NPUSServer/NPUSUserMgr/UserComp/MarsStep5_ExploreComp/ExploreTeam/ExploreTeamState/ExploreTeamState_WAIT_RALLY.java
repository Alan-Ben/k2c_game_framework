package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import ALBasicProtocolPack._IALProtocolStructure;
import AllRpcData.US_Service.Mars.MarsRallySInitedCheck;
import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_WaitRally;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.GuildRallyErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.PlayerMarsExploreTeamBO;

/**
 * 火星队伍状态 - 采集中
 * @author mj
 *
 */
public class ExploreTeamState_WAIT_RALLY extends _AExploreTeamState
{
    //等待集结超时后的容错等待时间（毫秒）
    private static final long S_INITED_CHECK_TOLERANCE_MS = 30 * 1000L;

    // 额外数据：仅持久化集结id用于重启校验
    private MarsTeamState_WaitRally _m_edExtraData;

	public ExploreTeamState_WAIT_RALLY(MarsExploreTeam _team, PlayerMarsExploreTeamBO _bo)
	{
		super(_team, _bo);
	}

	/**
	 * 来自行军状态的转变
	 */
	public ExploreTeamState_WAIT_RALLY(MarsExploreTeam _team, long _startTimeMS, long _keepTimeMS, long _targetPos)
	{
		super(_team, _startTimeMS, _keepTimeMS, _targetPos);

        _m_edExtraData = new MarsTeamState_WaitRally();
	}

    /**
     * 来自行军状态的转变（带集结id）
     */
    public ExploreTeamState_WAIT_RALLY(MarsExploreTeam _team, long _startTimeMS, long _keepTimeMS, long _targetPos, long _rallyId)
    {
        super(_team, _startTimeMS, _keepTimeMS, _targetPos);

        _m_edExtraData = new MarsTeamState_WaitRally();
        _m_edExtraData.setRallyId(_rallyId);
    }

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.WAIT_RALLY;
	}
	
	@Override
	protected	void _loadExtData(PlayerMarsExploreTeamBO _bo)
	{
        _m_edExtraData = new MarsTeamState_WaitRally();

        if(null != _bo.getExtData())
        {
            // 兼容历史数据：旧结构前8字节同样是rallyId
            _m_edExtraData.readPackage(java.nio.ByteBuffer.wrap(_bo.getExtData()));
        }
	}
	
	@Override
	public _IALProtocolStructure getExtData()
	{
		return _m_edExtraData;
	}

    /**
     * 绑定集结ID，重复绑定同一ID按成功处理
     */
    public Result bindRallyId(long _rallyId)
    {
        if (_rallyId <= 0) {
            return CommErr.PARAM_ERROR;
        }

        long curRallyId = _m_edExtraData.getRallyId();
        if (curRallyId > 0) {
            if (curRallyId == _rallyId) {
                return Result.SUCC;
            }

            return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
        }

        _m_edExtraData.setRallyId(_rallyId);
        return Result.SUCC;
    }
	
	@Override
	public boolean canEnter(_AExploreTeamState _state) 
	{
		//只能从行军状态开启
		return EMarsExploreTeamState.IDLE == _state.getStateType()
                || EMarsExploreTeamState.MARCH == _state.getStateType();
	}

	@Override
	protected void _onEnter() 
	{
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

        return false;
	}

	@Override
	public _AExploreTeamState getNextState() 
	{
		return null;
	}
    /**
     * 服务器启动后的WAIT_RALLY状态校验
     * 超时后执行保护性回收，避免队伍状态长期卡住
     */
    @Override
    public void onSInitedCheck()
    {
        long nowMs = CommonFunc.getNowTimeMS();
        if(nowMs <= getStateEndMs() + S_INITED_CHECK_TOLERANCE_MS)
            return;

        long guildId = getTeam().getUserData().getGuildComponent().getGuildId();
        long rallyId = _m_edExtraData.getRallyId();

        // 无法构造校验参数时不做重置，避免误回收
        if(guildId <= 0 || rallyId <= 0)
            return;

        final long stateSerialize = getStateSerialize();
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
                                    "ExploreTeamState_WAIT_RALLY.onSInitedCheck - query rally rpc fail: cid={}, teamId={}, guildId={}, rallyId={}, err={}",
                                    getTeam().getCid(), getTeam().getTeamId(), guildId, rallyId, _errCode);
                            return;
                        }

                        if(_errCode == GuildRallyErr.RALLY_NOT_FOUND.getCode()
                                || _errCode == GuildErr.MEMBER_NOT_FOUND.getCode())
                        {
                            getTeam().resetIdleState(stateSerialize, NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));
                        }
                    }
                },
                3,
                () -> USLog.error(getTeam().getUSServer(),
                        "ExploreTeamState_WAIT_RALLY.onSInitedCheck - send rally check rpc fail: cid={}, teamId={}, guildId={}, rallyId={}",
                        getTeam().getCid(), getTeam().getTeamId(), guildId, rallyId));
    }
}

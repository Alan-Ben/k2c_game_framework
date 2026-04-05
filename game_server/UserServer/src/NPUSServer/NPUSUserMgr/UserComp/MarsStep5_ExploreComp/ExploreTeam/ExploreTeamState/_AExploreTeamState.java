package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import ALBasicCommon.ALSerializeMaker;
import ALBasicProtocolPack._IALProtocolStructure;
import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.Mars_TeamState;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import USDB.Bo.PlayerMarsExploreTeamBO;

public abstract class _AExploreTeamState 
{
	//队伍数据
	private MarsExploreTeam _m_etExploreTeam;

    //状态内存序列号
    private long _m_lStateSerialize;
	//状态开启时间
    private long _m_lStateStartMs;
	//状态持续时长
    private long _m_iKeepTimeMS;
    //存储移动的起始或目标位置，方便客户端展示
    private long _m_lTargetPos;
	
	public _AExploreTeamState(MarsExploreTeam _team, long _startTimeMS, long _keepTimeMS, long _targetPos)
	{
		_m_etExploreTeam = _team;
        _m_lStateSerialize = ALSerializeMaker.makeNewSerialize();
        _m_lStateStartMs = _startTimeMS;
        _m_iKeepTimeMS = _keepTimeMS;
        _m_lTargetPos = _targetPos;
	}
    public _AExploreTeamState(MarsExploreTeam _team, PlayerMarsExploreTeamBO _bo)
    {
        _m_etExploreTeam = _team;

        _m_lStateSerialize = ALSerializeMaker.makeNewSerialize();
        _m_lStateStartMs = _bo.getCurStateStartMs();
        _m_iKeepTimeMS = _bo.getCurStateKeepTimeMS();
        _m_lTargetPos = _bo.getTargetPos();

        //加载额外数据
        _loadExtData(_bo);
    }
    
    public MarsExploreTeam getTeam() {return _m_etExploreTeam;}

    public long getStateSerialize() {return _m_lStateSerialize;}
    public long getStateStartMs() {return _m_lStateStartMs;}
    public long getKeepTimeMS() {return _m_iKeepTimeMS;}
    public long getTargetPos() {return _m_lTargetPos;}
    
    public long getStateEndMs() {return _m_lStateStartMs + _m_iKeepTimeMS;}
    
    /**
     * 获得状态类型
     * @return 状态id
     */
    abstract public EMarsExploreTeamState getStateType();
    /**
     * 加载额外数据
     * @param _bo
     */
    abstract protected	void _loadExtData(PlayerMarsExploreTeamBO _bo);
    /**
     * 获取额外数据
     * @return
     */
    abstract public _IALProtocolStructure getExtData();
    /**
     * 判断是否能够退出
     * @return
     */
    abstract public boolean canQuit(_AExploreTeamState _targetState);
    /**
     * 离开本状态处理
     */
    abstract protected void _onQuit();
    /**
     * 判断能否进入状态
     * @param _preState 上一个状态
     * @return boolean
     */
    abstract public boolean canEnter(_AExploreTeamState _preState);
    /**
     * 进入本状态处理
     */
    abstract protected void _onEnter();
    /**
     * 获取下一个目标状态
     * @return
     */
    public abstract _AExploreTeamState getNextState();

    /**
     * 服务器启动后的一次性状态校验
     * 默认不处理，具体状态按需重载
     */
    public abstract void onSInitedCheck();

    /**
     * 退出状态
     */
    protected void _quit() 
    {
    	_onQuit();
	}
    /**
     * 进入状态
     */
    protected void _enter() 
    {
		_onEnter();
	}

    /**
     * 构造状态数据
     * @return
     */
    public Mars_TeamState toProto()
    {
    	Mars_TeamState proto = new Mars_TeamState();
    	proto.setState(getStateType());
        proto.setStateSerialize(getStateSerialize());
    	proto.setStateStartMs(getStateStartMs());
    	proto.setStateEndMs(getStateEndMs());
    	proto.setPos(getTargetPos());

        //构造数据不带入协议号
        _IALProtocolStructure exData = getExtData();
        if(null != exData)
            proto.setExData(exData.makePackage());
    	
    	return proto;
    }
}

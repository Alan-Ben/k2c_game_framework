package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import Common.MarsEnum.EMarsExploreTeamState;
import Common.ServerObj.ServerObj_MarsMineSettle;
import Common.ServerObj.ServerObj_MarsTeam_OccupyMineResult;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Mars.RefMarsExplorePos;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsExploreTeamBO;

/**
 * 火星探险队伍状态机
 * @author mj
 *
 */
public class ExploreTeamStateMachine 
{
	//队伍数据
	private MarsExploreTeam _m_etExploreTeam;
	
    //队伍当前状态
    private _AExploreTeamState _m_csCurState;
    
    public ExploreTeamStateMachine(MarsExploreTeam _team)
    {
    	_m_etExploreTeam = _team;
    	
    	//默认是未解锁状态
    	_m_csCurState = new ExploreTeamState_NONE(_m_etExploreTeam);
    }
    
    public MarsExploreTeam getTeam() {return _m_etExploreTeam;}
	public NPUSUserData getUserData() {return getTeam().getUserData();}
    
    public _AExploreTeamState getCurState() {return _m_csCurState;}
    
    public boolean isIdle() {return EMarsExploreTeamState.IDLE == _m_csCurState.getStateType();}

    /**
     * 服务器启动后的状态校验入口
     */
    public void onSInitedCheck()
    {
        getUserData().lockUser();

        try
        {
            _m_csCurState.onSInitedCheck();
        }
        finally
        {
            getUserData().unlockUser();
        }
    }
    
    /**
     * 加载状态数据
     * @param _bo
     * @return
     */
    public void _loadFromBo(PlayerMarsExploreTeamBO _bo) 
    {
    	_m_csCurState = getStateHandler(_bo);
	}
    
    /**
     * 根据当前数据获取对应的状态数据
     * @param _bo
     * @return
     */
    public _AExploreTeamState getStateHandler(PlayerMarsExploreTeamBO _bo)
    {
    	EMarsExploreTeamState curStateType = EMarsExploreTeamState.EMarsExploreTeamState_FromInt(_bo.getCurState());

        if(curStateType == null)
        {
          USLog.error(getTeam().getUSServer(), "player:{} team:{} _stage:{} mars explore team stage get handler fail.",
              getTeam().getCid(), getTeam().getTeamId(), _bo.getCurState());
          return new ExploreTeamState_ERROR(getTeam(), _bo);
        }

    	_AExploreTeamState curState = null;
    	switch(curStateType)
    	{
    		case NONE:
    			curState = new ExploreTeamState_NONE(getTeam());
    			break;
    		case ERROR:
    			curState = new ExploreTeamState_ERROR(getTeam(), _bo);
    			break;
    		case IDLE:
    			curState = new ExploreTeamState_IDLE(getTeam(), _bo);
    			break;
    		case MARCH:
    			curState = new ExploreTeamState_MARCH(getTeam(), _bo);
    			break;
          case WAIT_RALLY:
                curState = new ExploreTeamState_WAIT_RALLY(getTeam(), _bo);
                break;
    		case BACK:
    			curState = new ExploreTeamState_BACK(getTeam(), _bo);
    			break;
    		case REPAIR:
    			curState = new ExploreTeamState_REPAIR(getTeam(), _bo);
    			break;
    		case COLLECT:
    			curState = new ExploreTeamState_COLLECT(getTeam(), _bo);
    			break;
    		default: //异常状态，通过GM命令进行修复
    		{
    			USLog.error(getTeam().getUSServer(), "player:{} team:{} _stage:{} mars explore team stage get handler fail.", 
    					getTeam().getCid(), getTeam().getTeamId(), curStateType);
    			curState =  new ExploreTeamState_ERROR(getTeam(), _bo);
    		}
    	}
		
		return curState;
    }
    
    /**
     * 刷新当前队伍状态
     */
    public void refreshState()
    {
    	getUserData().lockUser();
    	
    	try
    	{
            //判断玩家数据是否加载完成，未完成则直接返回
            if(!getUserData().isLoaded())
                return ;

    		//需要做循环保护，避免死循环
    		int i = 0;
    		
    		do
    		{
    			//当前状态不能退出
    			if(!_m_csCurState.canQuit(null))
    				break;
    			
    			//检查下一个状态
    			_AExploreTeamState nextState = _m_csCurState.getNextState();
        		if(null == nextState)
        			break;
        		
        		//检查下一个状态是否能够进入
        		if(!nextState.canEnter(_m_csCurState))
        			break;

				i++;

				//切换状态
        		_m_csCurState._quit();
        		
        		_m_csCurState = nextState;
        		_m_csCurState._enter();
        		
        		//如果包换
    			if(i > 100)
    			{
    				USLog.error(getUserData().getUSServer(), "player:{} team:{} mars team refresh state fail.", getTeam().getCid(), getTeam().getTeamId());
    				_m_csCurState = new ExploreTeamState_ERROR(_m_etExploreTeam);
    				break;
    			}
    			
    		} while(true);
    		
    		//保存数据（未解锁状态的数据不需要保存）
    		if(EMarsExploreTeamState.NONE != _m_csCurState.getStateType())
    		{
    			getTeam()._saveAll();
    		}

			//数据有变动则推送状态变更
			if(i > 0)
			{
				//推送队伍状态数据
				getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_058_OnExploreTeamState(getTeam()));
			}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 切换状态
     * @param _targetState
     * @return
     */
    public boolean transState(_AExploreTeamState _targetState)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//刷新状态
    		refreshState();
    		
    		//检查当前状态是否可以退出
    		if(!_m_csCurState.canQuit(_targetState))
    			return false;
    		
    		//检查目标状态是否可以进入
    		if(!_targetState.canEnter(_m_csCurState))
    			return false;
    		
    		//执行原状态退出
    		_m_csCurState._quit();
    		
    		_m_csCurState = _targetState;
    		_m_csCurState._enter();
    		
    		//更新数据
    		getTeam()._saveAll();

			//推送队伍状态数据
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_058_OnExploreTeamState(getTeam()));

    		return true;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 设置空闲状态
     */
    public void setIdle()
    {
    	getUserData().lockUser();
    	
    	try
    	{
			//执行原状态退出
			_m_csCurState._quit();

			_m_csCurState = new ExploreTeamState_IDLE(getTeam());
			_m_csCurState._enter();

			//更新数据
			getTeam()._saveAll();

			//推送队伍状态数据
			getUserData().sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_058_OnExploreTeamState(_m_etExploreTeam));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

	/**
	 * 进入采集状态
	 * @param _context
	 * @return
	 */
	public Result enterCollect(ServerObj_MarsTeam_OccupyMineResult _occupyMineResult, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			//获取当前队伍状态，判断是否行军状态，如非行军状态则报错
			if(_m_csCurState.getStateType() != EMarsExploreTeamState.MARCH)
				return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;

			//检查对应的地点的配置
			RefMarsExplorePos posRef = RefMarsExplorePos.getMgr().get(_m_csCurState.getTargetPos());
			if(null == posRef)
				return CommErr.REF_NOT_FOUND;

			//目标状态
			ExploreTeamState_COLLECT targetState =
					new ExploreTeamState_COLLECT(_m_etExploreTeam, _occupyMineResult.getStartCollectMs(), _m_csCurState.getKeepTimeMS(), _m_csCurState.getTargetPos()
							, _occupyMineResult);
			//尝试切换状态
			if(!transState(targetState))
			{
				return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
			}

			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 设置队伍采集状态完成
	 * @param _settleObj
	 * @param _context
	 */
	public void setCollectDone(ServerObj_MarsMineSettle _settleObj, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			//检查状态，如果还处于该矿的采集状态，则直接进入采矿结束状态
			_AExploreTeamState state = getCurState();
			if((state instanceof ExploreTeamState_COLLECT))
			{
				ExploreTeamState_COLLECT collectState = (ExploreTeamState_COLLECT) state;
				if(collectState.getExtData().getMineInstanceId() == _settleObj.getId())
				{
					//直接回城
					sendbackTeam(_settleObj.getEndCollectMs(), _context);
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}


	/**
	 * 从行军进入遣返状态
	 * @param _context
	 * @return
	 */
	public Result sendbackTeam(long _backStartTimeMS, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
        //获取当前队伍状态，判断是否 行军/等待集结/采集 状态，如否则报错
        if(_m_csCurState.getStateType() != EMarsExploreTeamState.MARCH
                && _m_csCurState.getStateType() != EMarsExploreTeamState.WAIT_RALLY
				&& _m_csCurState.getStateType() != EMarsExploreTeamState.COLLECT)
				return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;

			//检查对应的地点的配置
			RefMarsExplorePos posRef = RefMarsExplorePos.getMgr().get(_m_csCurState.getTargetPos());
			if(null == posRef)
				return CommErr.REF_NOT_FOUND;

			//目标状态
			ExploreTeamState_BACK targetState =
					new ExploreTeamState_BACK(_m_etExploreTeam, _backStartTimeMS, _m_csCurState.getKeepTimeMS(), _m_csCurState.getTargetPos());
			//尝试切换状态
			if(!transState(targetState))
			{
				return MarsErr.MARS_EXPLORE_TEAM_STATE_ERROR;
			}

			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}

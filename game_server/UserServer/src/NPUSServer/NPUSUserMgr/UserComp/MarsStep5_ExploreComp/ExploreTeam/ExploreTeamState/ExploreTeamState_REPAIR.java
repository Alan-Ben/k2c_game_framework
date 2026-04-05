package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.GuildEnum.EGuildMarsHelpObjType;
import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_Repair;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp._IGuildMarsHelp;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import USDB.Bo.PlayerMarsExploreTeamBO;

import java.nio.ByteBuffer;

/**
 * 火星队伍状态 - 维修中
 * @author mj
 *
 */
public class ExploreTeamState_REPAIR extends _AExploreTeamState implements _IGuildMarsHelp
{
	private MarsTeamState_Repair _m_edExtraData;
	
	public ExploreTeamState_REPAIR(MarsExploreTeam _team, PlayerMarsExploreTeamBO _bo)
	{
		super(_team, _bo);
	}
	
	public ExploreTeamState_REPAIR(MarsExploreTeam _team, long _startTimeMS, long _keepSecs, MarsTeamState_Repair _extData)
	{
		super(_team, _startTimeMS, _keepSecs, 0);
		
		_m_edExtraData = new MarsTeamState_Repair();
		_m_edExtraData.readPackage(_extData.makePackage());
	}
	
	/**
	 * 获取修复完成时间
	 * @return
	 */
	public long getRepairEndTime()
	{
		return getStateEndMs() - (_m_edExtraData.getGuildHelpSecs() * 1000) - (_m_edExtraData.getItemHelpSecs() * 1000);
	}

	@Override
	public EMarsExploreTeamState getStateType() 
	{
		return EMarsExploreTeamState.REPAIR;
	}
	
	@Override
	protected	void _loadExtData(PlayerMarsExploreTeamBO _bo)
	{
		_m_edExtraData = new MarsTeamState_Repair();

		if(null != _bo.getExtData())
		{
			ByteBuffer buff = ByteBuffer.wrap(_bo.getExtData());
			_m_edExtraData.readPackage(buff);
		}
		
		//当前已经处在了修复状态，需要注册公会求助
		getTeam().getUserData().getMarsComponent().getGuildMarsHelpMgr().regMarsHelpObj(this);
	}
	
	@Override
	public MarsTeamState_Repair getExtData()
	{
		return _m_edExtraData;
	}
	
	@Override
	public boolean canEnter(_AExploreTeamState _state) 
	{
		//只能从空闲状态进行转换
		return _state.getStateType() == EMarsExploreTeamState.IDLE;
	}

	@Override
	protected void _onEnter() 
	{
		//注册后注册公会求助数据
		getTeam().getUserData().getMarsComponent().getGuildMarsHelpMgr().regMarsHelpObj(this);
	}

	@Override
	public boolean canQuit(_AExploreTeamState _targetState)
	{
		// 允许主动取消维修，转换到 IDLE 状态
        if (null != _targetState
                && _targetState.getStateType() == EMarsExploreTeamState.IDLE
                && _m_edExtraData.getIsCancel())
            return true;

		//已经设置完成 或 当前时间超过维修截至时间
		return _m_edExtraData.getIsDone() || CommonFunc.getNowTimeMS() >= getRepairEndTime();
	}

	@Override
	protected void _onQuit()
	{
        // 只有在非取消维修的情况下，才会扣除损失值
        if (!_m_edExtraData.getIsCancel())
		    getTeam().reduceLossValue(_m_edExtraData.getRepairNum());

		//重置公会求助数据
		getTeam().getUserData().getMarsComponent().getGuildMarsHelpMgr().unsetHelp(this);
		//注销公会求助数据
		getTeam().getUserData().getMarsComponent().getGuildMarsHelpMgr().unregMarsHelpObj(this);
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

	//--------------------------- 求助对象接口方法 ---------------------------//
	@Override
	public EGuildMarsHelpObjType getObjType() 
	{
		return EGuildMarsHelpObjType.TEAM_REPAIR;
	}
	
	@Override
	public long getObjId() 
	{
		return getTeam().getTeamId();
	}

	@Override
	public _IALProtocolStructure makeExt() 
	{
		return null;
	}

	@Override
	public long getHelpId() 
	{
		return _m_edExtraData.getGuildHelpId();
	}

	@Override
	public int getHelpSecs() 
	{
		return _m_edExtraData.getGuildHelpSecs();
	}

	@Override
	public boolean canSendHelp(EGuildMarsHelpObjType _objType)
	{
		return getHelpId() <= 0;
	}
	
	@Override
	public void setSendHelp(long _helpId) 
	{
		getTeam().getUserData().lockUser();
		
		try
		{
			_m_edExtraData.setGuildHelpId(_helpId);

			//仅保存额外信息
			getTeam()._saveExData();
		}
		finally
		{
			getTeam().getUserData().unlockUser();
		}
	}

	@Override
	public void updateHelpSecs(long _helpId, int _helpSecs) 
	{
		getTeam().getUserData().lockUser();
		
		try
		{
			//检查请求ID
			if(_m_edExtraData.getGuildHelpId() != _helpId)
				return;
			
			//检查互助时长（只大不小）
			if(_helpSecs <= _m_edExtraData.getGuildHelpSecs())
				return;
			
			//更新互助时长（秒）
			_m_edExtraData.setGuildHelpSecs(_helpSecs);
			//仅保存额外信息
			getTeam()._saveExData();
		}
		finally
		{
			getTeam().getUserData().unlockUser();
		}
	}
	
	/**
	 * 设置修复完成
	 */
	public void setDone()
	{
		getTeam().getUserData().lockUser();
		
		try
		{
			_m_edExtraData.setIsDone(true);
			
			//仅保存额外信息
			getTeam()._saveExData();
		}
		finally
		{
			getTeam().getUserData().unlockUser();
		}
	}

	/**
	 * 设置修复取消
	 */
	public void setCancel()
	{
		getTeam().getUserData().lockUser();

		try
		{
			_m_edExtraData.setIsCancel(true);

			//仅保存额外信息
			getTeam()._saveExData();
		}
		finally
		{
			getTeam().getUserData().unlockUser();
		}
	}
	
	/**
	 * 加速时长
	 * @param _reduceSecs
	 * @param _context
	 * @return
	 */
	public int reduceSecs(int _reduceSecs, NPPlayerContext _context)
	{
		getTeam().getUserData().lockUser();
		
		try
		{
			//计算并更新数据
			long remainMs = getRepairEndTime() - CommonFunc.getNowTimeMS();
			//无需加速
			if(remainMs <= 0)
				return 0;
			
			//计算加速时长
			long realReduceMs = Math.min((_reduceSecs * 1000), remainMs);
			int realReduceSecs = (int) Math.ceil(realReduceMs / 1000f);
			realReduceSecs = Math.max(realReduceSecs, 0);
			//更新加速时长（秒）
			if(realReduceSecs > 0)
			{
				_m_edExtraData.setItemHelpSecs(_m_edExtraData.getItemHelpSecs() + realReduceSecs);
				getTeam()._saveExData();
			}
			
			return realReduceSecs;
		}
		finally
		{
			getTeam().getUserData().unlockUser();
		}
	}
}

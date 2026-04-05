package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent;

import Common.MarsEnum.EMarsExploreEventType;
import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_March;
import Common.ServerObj.ServerObj_MarsExploreEvent_Boss;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Mars.RefMarsExploreEventBoss;
import NPGameRes.Refs.Mars.RefMarsExplorePos;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp.MarsCalculate;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState.ExploreTeamState_MARCH;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import USDB.Bo.PlayerMarsExploreEventBO;

import java.nio.ByteBuffer;

/**
 * boss事件，固定pos刷新战斗
 * @author mj
 *
 */
public class MarsExploreEvent_BOSS extends _AMarsExploreEventInfo
{
	//boss事件配置
	private RefMarsExploreEventBoss _m_ref;
	//事件数据
	private ServerObj_MarsExploreEvent_Boss _m_edExtraData;
	
	public MarsExploreEvent_BOSS(NPUSUserData _userData, PlayerMarsExploreEventBO _bo) 
	{
		super(_userData, _bo);
		
		_m_ref = RefMarsExploreEventBoss.getMgr().get(getEventId());
		
		_m_edExtraData = new ServerObj_MarsExploreEvent_Boss();
		if(null != getBo().getExtData())
		{
			ByteBuffer buff = ByteBuffer.wrap(getBo().getExtData());
			_m_edExtraData.readPackage(buff);
		}
	}
	
	public MarsExploreEvent_BOSS(NPUSUserData _userData, PlayerMarsExploreEventBO _bo, RefMarsExploreEventBoss _ref) 
	{
		super(_userData, _bo);
		
		_m_ref = _ref;
		
		_m_edExtraData = new ServerObj_MarsExploreEvent_Boss();
	}
	
	public RefMarsExploreEventBoss getRef() {return _m_ref;}

	@Override
	public EMarsExploreEventType getEventType() 
	{
		return EMarsExploreEventType.BOSS;
	}

	@Override
	public Result dealStart(MarsExploreTeam _team, NPPlayerContext _context) 
	{
		getUserData().lockUser();
		
		try
		{
			//已完成
			if(isDone())
	        {
	        	return MarsErr.MARS_EXPLORE_EVENT_OCCUOIED;
	        }
			
			//检查对应的地点的配置
			RefMarsExplorePos posRef = RefMarsExplorePos.getMgr().get(getPos());
			if(null == posRef)
			{
				return CommErr.REF_NOT_FOUND;
			}
			
			//用于队伍的额外数据
			MarsTeamState_March extData = new MarsTeamState_March();
			extData.setInstanceId(getId());
			extData.setTargetState(EMarsExploreTeamState.BOSS_BATTLE.ordinal());
			//目标状态
			ExploreTeamState_MARCH targetState = new ExploreTeamState_MARCH(_team, CommonFunc.getNowTimeMS(), MarsCalculate.calMarchTimeMS(_team.getUserData(), posRef), getPos(), extData);
			//尝试切换状态
			if(!_team.getStateMachine().transState(targetState))
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
	 * 检查是否可以更新下一个事件
	 * @return
	 */
	public boolean checkUpdateNextEvent()
	{
		return null != _m_ref && isDone() && _m_edExtraData.getHadReward();
	}
	
	/**
	 * 获取完成奖励
	 * @param _context
	 * @return
	 */
	public Result getDoneReward(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(null == _m_ref)
				return CommErr.REF_NOT_FOUND;
			
	        //检查事件是否完成
	        if(!isDone())
	        	return MarsErr.MARS_EXPLORE_EVENT_NOT_FOUND;
	        
	        //是否领取奖励
	        if(_m_edExtraData.getHadReward())
	        	return MarsErr.MARS_EXPLORE_EVENT_HAD_REWARD;

            //检查每日刷新次数限制
            long fixedCdId = RefGeneral.Ref().mars_explore_daily_refresh_fixed_cd;
            if(fixedCdId > 0)
            {
                boolean consumeSucc = getUserData().spendItem(ENPItemType.FIXED_CD, fixedCdId, 1, _context);
                if(!consumeSucc)
                {
                    return CommErr.OP_DISABLE;
                }
            }
	        
	        //更新已领奖数据
	        _m_edExtraData.setHadReward(true);
	        setExtData(_m_edExtraData);
	        
	        //领取奖励
	        getUserData().gainItemList(_m_ref.reward_item_list, _context);
	        
	        //完成事件处理
	        _onDone(_context);
	        
	        //自动更新下一个事件
	        getUserData().getMarsExploreComponent().getEventMgr().checkNextBossEvent(_context);
			
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}

package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.MarsEnum.EMarsExploreEventType;
import Common.MarsEnum.EMarsExploreTeamState;
import Common.MarsObj.MarsTeamState_March;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Mars.RefMarsExploreLvl;
import NPGameRes.Refs.Mars.RefMarsExplorePos;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp.MarsCalculate;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.ExploreTeamState.ExploreTeamState_MARCH;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsExploreEventBO;

import java.util.ArrayList;

public class MarsExploreEvent_BATTLE extends _AMarsExploreEventInfo
{
	public MarsExploreEvent_BATTLE(NPUSUserData _userData, PlayerMarsExploreEventBO _bo) 
	{
		super(_userData, _bo);
	}

	@Override
	public EMarsExploreEventType getEventType() 
	{
		return EMarsExploreEventType.BATTLE;
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
			extData.setTargetState(EMarsExploreTeamState.BATTLE.ordinal());
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
	 * 获取完成奖励
	 * @param _context
	 * @return
	 */
	public Result getDoneReward(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
	        //检查事件是否完成
	        if(!isDone())
	        	return MarsErr.MARS_EXPLORE_EVENT_NOT_FOUND;
	        
	        //检查当时的探索等级配置
	        RefMarsExploreLvl exploreLvlRef = RefMarsExploreLvl.getMgr().get(getExploreLvl());
	        if(null == exploreLvlRef)
	        {
	        	USLog.error(getUSServer(), "player:{} mars explore lvl:{} mars get done reward can not get ref on remove.", getCid(), getExploreLvl());
	        	return CommErr.REF_NOT_FOUND;
	        }
	        
	        //移除事件数据
	        _del();
	        
	        //领取奖励
	        ArrayList<NPCommonCostItem> itemList = exploreLvlRef.getQualityRewardItemList(getQuality());
	        if(null == itemList)
	        {
	        	USLog.error(getUSServer(), "player:{} exploreLvl:{} quality:{} mars battle can not get quality reward.", getCid(), exploreLvlRef.explore_level, getQuality());
	        }
	        else
	        {
	        	getUserData().gainItemList(itemList, _context);
	        }
	        
	        //完成事件处理
	        _onDone(_context);

	        //增加探索计数
	        ALSynTaskManager.getInstance().regTask(()->
	        {
	        	//GOB-6415 【BUG-1】火星探索-领取探索奖励，奖励列表含有升级奖励 
	        	//https://www.teambition.com/task/690da3824cb154b47d59aef0
	        	NPPlayerContext incrNumContext = NPPlayerContext.createNew(_context);
		        getUserData().getMarsExploreComponent().getExploreInfo().incrExploreNum(incrNumContext);
	        });
	        
	        return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}

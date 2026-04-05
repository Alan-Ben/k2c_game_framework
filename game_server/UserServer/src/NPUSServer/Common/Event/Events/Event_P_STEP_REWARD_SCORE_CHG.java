package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
阶段奖励分数变更
****/
@EventDesc(id=56,name="P_STEP_REWARD_SCORE_CHG",params={"SET_ID","SCORE"})
public class Event_P_STEP_REWARD_SCORE_CHG extends _ALogicEventBase
{
	public static final int ID =56;
	public Event_P_STEP_REWARD_SCORE_CHG(_IContext _context,long _SET_ID,long _SCORE)
	{
		super(_context);
		
		set_SET_ID(_SET_ID);
		set_SCORE(_SCORE);

	}
	public Event_P_STEP_REWARD_SCORE_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//当前阶段Set配置ID
	public void set_SET_ID(long _SET_ID){ setParamValue(0, _SET_ID); }
	public long get_SET_ID(){ return getParamValue(0); }
	//当前分数
	public void set_SCORE(long _SCORE){ setParamValue(1, _SCORE); }
	public long get_SCORE(){ return getParamValue(1); }

}

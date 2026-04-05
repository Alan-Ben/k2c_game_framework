package ActivitiesV02.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
2048总分变化
****/
@EventDesc(id=2001,name="P_NUM_MERGE_TOTAL_SCORE_CHG",params={"SCORE"})
public class Event_P_NUM_MERGE_TOTAL_SCORE_CHG extends _ALogicEventBase
{
	public static final int ID =2001;
	public Event_P_NUM_MERGE_TOTAL_SCORE_CHG(_IContext _context,long _SCORE)
	{
		super(_context);
		
		set_SCORE(_SCORE);

	}
	public Event_P_NUM_MERGE_TOTAL_SCORE_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//分数
	public void set_SCORE(long _SCORE){ setParamValue(0, _SCORE); }
	public long get_SCORE(){ return getParamValue(0); }

}

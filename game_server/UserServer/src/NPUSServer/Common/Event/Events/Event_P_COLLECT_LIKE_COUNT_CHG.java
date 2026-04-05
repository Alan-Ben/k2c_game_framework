package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
集赞次数变更
****/
@EventDesc(id=28,name="P_COLLECT_LIKE_COUNT_CHG",params={"LIKE_COUNT"})
public class Event_P_COLLECT_LIKE_COUNT_CHG extends _ALogicEventBase
{
	public static final int ID =28;
	public Event_P_COLLECT_LIKE_COUNT_CHG(_IContext _context,long _LIKE_COUNT)
	{
		super(_context);
		
		set_LIKE_COUNT(_LIKE_COUNT);

	}
	public Event_P_COLLECT_LIKE_COUNT_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//点赞次数
	public void set_LIKE_COUNT(long _LIKE_COUNT){ setParamValue(0, _LIKE_COUNT); }
	public long get_LIKE_COUNT(){ return getParamValue(0); }

}

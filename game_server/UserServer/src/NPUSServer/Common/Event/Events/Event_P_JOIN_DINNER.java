package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
参加宴会
****/
@EventDesc(id=13,name="P_JOIN_DINNER",params={"DINNER_TYPE"})
public class Event_P_JOIN_DINNER extends _ALogicEventBase
{
	public static final int ID =13;
	public Event_P_JOIN_DINNER(_IContext _context,long _DINNER_TYPE)
	{
		super(_context);
		
		set_DINNER_TYPE(_DINNER_TYPE);

	}
	public Event_P_JOIN_DINNER(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//宴会类型
	public void set_DINNER_TYPE(long _DINNER_TYPE){ setParamValue(0, _DINNER_TYPE); }
	public long get_DINNER_TYPE(){ return getParamValue(0); }

}

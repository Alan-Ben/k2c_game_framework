package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
开启宴会
****/
@EventDesc(id=61,name="P_START_DINNER",params={})
public class Event_P_START_DINNER extends _ALogicEventBase
{
	public static final int ID =61;
	public Event_P_START_DINNER(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_START_DINNER(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
游历
****/
@EventDesc(id=16,name="P_TRAVEL_EVENT",params={})
public class Event_P_TRAVEL_EVENT extends _ALogicEventBase
{
	public static final int ID =16;
	public Event_P_TRAVEL_EVENT(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_TRAVEL_EVENT(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

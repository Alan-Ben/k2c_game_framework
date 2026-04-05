package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
子嗣联姻
****/
@EventDesc(id=30,name="P_ADULT_MARRY",params={})
public class Event_P_ADULT_MARRY extends _ALogicEventBase
{
	public static final int ID =30;
	public Event_P_ADULT_MARRY(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_ADULT_MARRY(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

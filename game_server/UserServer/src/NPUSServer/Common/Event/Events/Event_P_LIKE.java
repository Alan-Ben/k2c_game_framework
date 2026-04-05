package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
点赞
****/
@EventDesc(id=29,name="P_LIKE",params={})
public class Event_P_LIKE extends _ALogicEventBase
{
	public static final int ID =29;
	public Event_P_LIKE(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_LIKE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
火星探索事件完成
****/
@EventDesc(id=64,name="P_MARS_EVENT_DONE",params={})
public class Event_P_MARS_EVENT_DONE extends _ALogicEventBase
{
	public static final int ID =64;
	public Event_P_MARS_EVENT_DONE(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_MARS_EVENT_DONE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

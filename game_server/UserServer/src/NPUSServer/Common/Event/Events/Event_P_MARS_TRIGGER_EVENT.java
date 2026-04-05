package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
火星触发事件
****/
@EventDesc(id=55,name="P_MARS_TRIGGER_EVENT",params={"EVENT_ID"})
public class Event_P_MARS_TRIGGER_EVENT extends _ALogicEventBase
{
	public static final int ID =55;
	public Event_P_MARS_TRIGGER_EVENT(_IContext _context,long _EVENT_ID)
	{
		super(_context);
		
		set_EVENT_ID(_EVENT_ID);

	}
	public Event_P_MARS_TRIGGER_EVENT(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//事件配置ID
	public void set_EVENT_ID(long _EVENT_ID){ setParamValue(0, _EVENT_ID); }
	public long get_EVENT_ID(){ return getParamValue(0); }

}

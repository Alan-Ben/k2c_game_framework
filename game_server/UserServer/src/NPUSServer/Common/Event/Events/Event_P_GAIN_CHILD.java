package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
获得子嗣
****/
@EventDesc(id=59,name="P_GAIN_CHILD",params={"COUNT"})
public class Event_P_GAIN_CHILD extends _ALogicEventBase
{
	public static final int ID =59;
	public Event_P_GAIN_CHILD(_IContext _context,long _COUNT)
	{
		super(_context);
		
		set_COUNT(_COUNT);

	}
	public Event_P_GAIN_CHILD(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//数量
	public void set_COUNT(long _COUNT){ setParamValue(0, _COUNT); }
	public long get_COUNT(){ return getParamValue(0); }

}

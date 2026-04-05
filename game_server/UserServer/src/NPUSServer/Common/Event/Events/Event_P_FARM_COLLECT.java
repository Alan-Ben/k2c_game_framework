package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
农田收获
****/
@EventDesc(id=8,name="P_FARM_COLLECT",params={})
public class Event_P_FARM_COLLECT extends _ALogicEventBase
{
	public static final int ID =8;
	public Event_P_FARM_COLLECT(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_FARM_COLLECT(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

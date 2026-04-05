package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
午间副本派遣伙伴
****/
@EventDesc(id=62,name="P_MIDDAY_ATTACK",params={})
public class Event_P_MIDDAY_ATTACK extends _ALogicEventBase
{
	public static final int ID =62;
	public Event_P_MIDDAY_ATTACK(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_MIDDAY_ATTACK(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

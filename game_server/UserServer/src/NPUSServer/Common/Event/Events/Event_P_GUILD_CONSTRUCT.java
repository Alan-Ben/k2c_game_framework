package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
公会建设
****/
@EventDesc(id=35,name="P_GUILD_CONSTRUCT",params={})
public class Event_P_GUILD_CONSTRUCT extends _ALogicEventBase
{
	public static final int ID =35;
	public Event_P_GUILD_CONSTRUCT(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_GUILD_CONSTRUCT(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

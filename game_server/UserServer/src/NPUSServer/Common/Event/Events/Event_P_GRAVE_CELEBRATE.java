package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
杰出者大厅点赞
****/
@EventDesc(id=50,name="P_GRAVE_CELEBRATE",params={})
public class Event_P_GRAVE_CELEBRATE extends _ALogicEventBase
{
	public static final int ID =50;
	public Event_P_GRAVE_CELEBRATE(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_GRAVE_CELEBRATE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

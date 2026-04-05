package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
聊天频道发言
****/
@EventDesc(id=26,name="P_SEND_CHAT_TO_ROOM",params={})
public class Event_P_SEND_CHAT_TO_ROOM extends _ALogicEventBase
{
	public static final int ID =26;
	public Event_P_SEND_CHAT_TO_ROOM(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_SEND_CHAT_TO_ROOM(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

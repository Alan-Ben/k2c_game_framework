package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
处理政务
****/
@EventDesc(id=22,name="P_DEAL_ANECDOTE",params={})
public class Event_P_DEAL_ANECDOTE extends _ALogicEventBase
{
	public static final int ID =22;
	public Event_P_DEAL_ANECDOTE(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_DEAL_ANECDOTE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

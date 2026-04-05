package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
关卡前进一波次
****/
@EventDesc(id=12,name="P_CHAPTER_FORWARD",params={})
public class Event_P_CHAPTER_FORWARD extends _ALogicEventBase
{
	public static final int ID =12;
	public Event_P_CHAPTER_FORWARD(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_CHAPTER_FORWARD(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

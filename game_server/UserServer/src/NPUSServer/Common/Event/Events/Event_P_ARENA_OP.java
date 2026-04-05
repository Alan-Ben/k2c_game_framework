package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
竞技场操作
****/
@EventDesc(id=65,name="P_ARENA_OP",params={"NUM"})
public class Event_P_ARENA_OP extends _ALogicEventBase
{
	public static final int ID =65;
	public Event_P_ARENA_OP(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_ARENA_OP(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//数量
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}

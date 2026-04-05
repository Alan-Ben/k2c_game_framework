package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
竞技场指定谈判次数
****/
@EventDesc(id=45,name="P_ARENA_SELECT_ATTACK",params={"NUM"})
public class Event_P_ARENA_SELECT_ATTACK extends _ALogicEventBase
{
	public static final int ID =45;
	public Event_P_ARENA_SELECT_ATTACK(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_ARENA_SELECT_ATTACK(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//次数
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}

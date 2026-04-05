package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
竞技场击败对手大臣
****/
@EventDesc(id=17,name="P_ARENA_DEFEAT_HERO",params={"NUM"})
public class Event_P_ARENA_DEFEAT_HERO extends _ALogicEventBase
{
	public static final int ID =17;
	public Event_P_ARENA_DEFEAT_HERO(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_ARENA_DEFEAT_HERO(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//数量
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}

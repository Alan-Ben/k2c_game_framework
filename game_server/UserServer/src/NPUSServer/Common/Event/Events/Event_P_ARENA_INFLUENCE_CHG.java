package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
竞技场影响力变更
****/
@EventDesc(id=7,name="P_ARENA_INFLUENCE_CHG",params={"NUM"})
public class Event_P_ARENA_INFLUENCE_CHG extends _ALogicEventBase
{
	public static final int ID =7;
	public Event_P_ARENA_INFLUENCE_CHG(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_ARENA_INFLUENCE_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//影响力变更值
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}

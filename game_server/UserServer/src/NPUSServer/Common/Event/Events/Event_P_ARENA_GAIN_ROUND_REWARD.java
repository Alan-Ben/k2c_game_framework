package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
获取比武场连胜宝箱
****/
@EventDesc(id=47,name="P_ARENA_GAIN_ROUND_REWARD",params={})
public class Event_P_ARENA_GAIN_ROUND_REWARD extends _ALogicEventBase
{
	public static final int ID =47;
	public Event_P_ARENA_GAIN_ROUND_REWARD(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_ARENA_GAIN_ROUND_REWARD(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

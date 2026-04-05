package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
领取每日奖励
****/
@EventDesc(id=48,name="P_DRAW_DAILY_REWARD",params={})
public class Event_P_DRAW_DAILY_REWARD extends _ALogicEventBase
{
	public static final int ID =48;
	public Event_P_DRAW_DAILY_REWARD(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_DRAW_DAILY_REWARD(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

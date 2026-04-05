package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
太空寻宝每日钻石产出领取
****/
@EventDesc(id=51,name="P_TREASURE_HUNT_DRAW_DAILY_GEM",params={})
public class Event_P_TREASURE_HUNT_DRAW_DAILY_GEM extends _ALogicEventBase
{
	public static final int ID =51;
	public Event_P_TREASURE_HUNT_DRAW_DAILY_GEM(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_TREASURE_HUNT_DRAW_DAILY_GEM(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

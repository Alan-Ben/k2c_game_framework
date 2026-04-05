package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
消耗背包物品
****/
@EventDesc(id=57,name="P_CONSUME_BAG_ITEM",params={"ID","COUNT"})
public class Event_P_CONSUME_BAG_ITEM extends _ALogicEventBase
{
	public static final int ID =57;
	public Event_P_CONSUME_BAG_ITEM(_IContext _context,long _ID,long _COUNT)
	{
		super(_context);
		
		set_ID(_ID);
		set_COUNT(_COUNT);

	}
	public Event_P_CONSUME_BAG_ITEM(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//物品ID
	public void set_ID(long _ID){ setParamValue(0, _ID); }
	public long get_ID(){ return getParamValue(0); }
	//数量
	public void set_COUNT(long _COUNT){ setParamValue(1, _COUNT); }
	public long get_COUNT(){ return getParamValue(1); }

}

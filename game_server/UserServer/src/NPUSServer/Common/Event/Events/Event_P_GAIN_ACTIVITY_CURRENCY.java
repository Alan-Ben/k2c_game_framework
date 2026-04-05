package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
获取活动货币
****/
@EventDesc(id=37,name="P_GAIN_ACTIVITY_CURRENCY",params={"ITEM_ID","NUM"})
public class Event_P_GAIN_ACTIVITY_CURRENCY extends _ALogicEventBase
{
	public static final int ID =37;
	public Event_P_GAIN_ACTIVITY_CURRENCY(_IContext _context,long _ITEM_ID,long _NUM)
	{
		super(_context);
		
		set_ITEM_ID(_ITEM_ID);
		set_NUM(_NUM);

	}
	public Event_P_GAIN_ACTIVITY_CURRENCY(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//活动货币ID
	public void set_ITEM_ID(long _ITEM_ID){ setParamValue(0, _ITEM_ID); }
	public long get_ITEM_ID(){ return getParamValue(0); }
	//数量
	public void set_NUM(long _NUM){ setParamValue(1, _NUM); }
	public long get_NUM(){ return getParamValue(1); }

}

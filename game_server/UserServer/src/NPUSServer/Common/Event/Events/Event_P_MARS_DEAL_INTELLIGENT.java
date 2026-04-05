package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
火星系统-处理AI决策
****/
@EventDesc(id=52,name="P_MARS_DEAL_INTELLIGENT",params={"INTELLIGENT_ID"})
public class Event_P_MARS_DEAL_INTELLIGENT extends _ALogicEventBase
{
	public static final int ID =52;
	public Event_P_MARS_DEAL_INTELLIGENT(_IContext _context,long _INTELLIGENT_ID)
	{
		super(_context);
		
		set_INTELLIGENT_ID(_INTELLIGENT_ID);

	}
	public Event_P_MARS_DEAL_INTELLIGENT(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//决策ID
	public void set_INTELLIGENT_ID(long _INTELLIGENT_ID){ setParamValue(0, _INTELLIGENT_ID); }
	public long get_INTELLIGENT_ID(){ return getParamValue(0); }

}

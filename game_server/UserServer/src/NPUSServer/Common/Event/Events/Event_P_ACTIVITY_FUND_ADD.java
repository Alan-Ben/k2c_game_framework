package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
活动基金新增
****/
@EventDesc(id=74,name="P_ACTIVITY_FUND_ADD",params={})
public class Event_P_ACTIVITY_FUND_ADD extends _ALogicEventBase
{
	public static final int ID =74;
	public Event_P_ACTIVITY_FUND_ADD(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_ACTIVITY_FUND_ADD(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

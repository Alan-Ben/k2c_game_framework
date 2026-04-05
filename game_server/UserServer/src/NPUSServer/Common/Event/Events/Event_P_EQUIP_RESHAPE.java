package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
装备重塑
****/
@EventDesc(id=6,name="P_EQUIP_RESHAPE",params={})
public class Event_P_EQUIP_RESHAPE extends _ALogicEventBase
{
	public static final int ID =6;
	public Event_P_EQUIP_RESHAPE(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_EQUIP_RESHAPE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}

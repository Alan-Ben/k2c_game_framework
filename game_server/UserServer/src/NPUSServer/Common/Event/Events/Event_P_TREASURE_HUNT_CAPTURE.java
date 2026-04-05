package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
太空寻宝捕捉
****/
@EventDesc(id=42,name="P_TREASURE_HUNT_CAPTURE",params={"NUM"})
public class Event_P_TREASURE_HUNT_CAPTURE extends _ALogicEventBase
{
	public static final int ID =42;
	public Event_P_TREASURE_HUNT_CAPTURE(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_TREASURE_HUNT_CAPTURE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//次数
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}

package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
火星实力最高值新增
****/
@EventDesc(id=54,name="P_MARS_MAX_POWER_UP",params={"ADD","NUM"})
public class Event_P_MARS_MAX_POWER_UP extends _ALogicEventBase
{
	public static final int ID =54;
	public Event_P_MARS_MAX_POWER_UP(_IContext _context,long _ADD,long _NUM)
	{
		super(_context);
		
		set_ADD(_ADD);
		set_NUM(_NUM);

	}
	public Event_P_MARS_MAX_POWER_UP(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//新增数量
	public void set_ADD(long _ADD){ setParamValue(0, _ADD); }
	public long get_ADD(){ return getParamValue(0); }
	//当前数量
	public void set_NUM(long _NUM){ setParamValue(1, _NUM); }
	public long get_NUM(){ return getParamValue(1); }

}

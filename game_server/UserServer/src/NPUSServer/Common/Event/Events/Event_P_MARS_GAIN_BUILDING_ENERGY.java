package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
火星-能源厂收取能源次数
****/
@EventDesc(id=70,name="P_MARS_GAIN_BUILDING_ENERGY",params={"ADD"})
public class Event_P_MARS_GAIN_BUILDING_ENERGY extends _ALogicEventBase
{
	public static final int ID =70;
	public Event_P_MARS_GAIN_BUILDING_ENERGY(_IContext _context,long _ADD)
	{
		super(_context);
		
		set_ADD(_ADD);

	}
	public Event_P_MARS_GAIN_BUILDING_ENERGY(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//新增数量
	public void set_ADD(long _ADD){ setParamValue(0, _ADD); }
	public long get_ADD(){ return getParamValue(0); }

}

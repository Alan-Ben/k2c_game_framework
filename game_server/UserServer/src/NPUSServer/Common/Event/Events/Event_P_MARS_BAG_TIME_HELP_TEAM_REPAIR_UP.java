package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
火星队伍-维修所维修加速时长（分钟）
****/
@EventDesc(id=69,name="P_MARS_BAG_TIME_HELP_TEAM_REPAIR_UP",params={"ADD"})
public class Event_P_MARS_BAG_TIME_HELP_TEAM_REPAIR_UP extends _ALogicEventBase
{
	public static final int ID =69;
	public Event_P_MARS_BAG_TIME_HELP_TEAM_REPAIR_UP(_IContext _context,long _ADD)
	{
		super(_context);
		
		set_ADD(_ADD);

	}
	public Event_P_MARS_BAG_TIME_HELP_TEAM_REPAIR_UP(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//新增数量
	public void set_ADD(long _ADD){ setParamValue(0, _ADD); }
	public long get_ADD(){ return getParamValue(0); }

}

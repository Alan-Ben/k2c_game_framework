package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
联盟-获得公会经验
****/
@EventDesc(id=73,name="P_GUILD_EXP_GAIN",params={"ADD"})
public class Event_P_GUILD_EXP_GAIN extends _ALogicEventBase
{
	public static final int ID =73;
	public Event_P_GUILD_EXP_GAIN(_IContext _context,long _ADD)
	{
		super(_context);
		
		set_ADD(_ADD);

	}
	public Event_P_GUILD_EXP_GAIN(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//新增数量
	public void set_ADD(long _ADD){ setParamValue(0, _ADD); }
	public long get_ADD(){ return getParamValue(0); }

}

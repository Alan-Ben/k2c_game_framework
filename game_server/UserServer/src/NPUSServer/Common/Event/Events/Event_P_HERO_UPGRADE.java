package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
提升骑士等级
****/
@EventDesc(id=5,name="P_HERO_UPGRADE",params={"HERO_ID","LEVEL_NUM"})
public class Event_P_HERO_UPGRADE extends _ALogicEventBase
{
	public static final int ID =5;
	public Event_P_HERO_UPGRADE(_IContext _context,long _HERO_ID,long _LEVEL_NUM)
	{
		super(_context);
		
		set_HERO_ID(_HERO_ID);
		set_LEVEL_NUM(_LEVEL_NUM);

	}
	public Event_P_HERO_UPGRADE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//骑士ID
	public void set_HERO_ID(long _HERO_ID){ setParamValue(0, _HERO_ID); }
	public long get_HERO_ID(){ return getParamValue(0); }
	//提升等级数量
	public void set_LEVEL_NUM(long _LEVEL_NUM){ setParamValue(1, _LEVEL_NUM); }
	public long get_LEVEL_NUM(){ return getParamValue(1); }

}

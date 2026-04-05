package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
获取副本币
****/
@EventDesc(id=36,name="P_GAIN_DUNGEON_COIN",params={"NUM"})
public class Event_P_GAIN_DUNGEON_COIN extends _ALogicEventBase
{
	public static final int ID =36;
	public Event_P_GAIN_DUNGEON_COIN(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_GAIN_DUNGEON_COIN(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//数量
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}

package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
藏品提升等级
****/
@EventDesc(id=44,name="P_EQUIP_UPGRADE",params={"EQUIP_ID","NUM"})
public class Event_P_EQUIP_UPGRADE extends _ALogicEventBase
{
	public static final int ID =44;
	public Event_P_EQUIP_UPGRADE(_IContext _context,long _EQUIP_ID,long _NUM)
	{
		super(_context);
		
		set_EQUIP_ID(_EQUIP_ID);
		set_NUM(_NUM);

	}
	public Event_P_EQUIP_UPGRADE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//藏品ID
	public void set_EQUIP_ID(long _EQUIP_ID){ setParamValue(0, _EQUIP_ID); }
	public long get_EQUIP_ID(){ return getParamValue(0); }
	//次数
	public void set_NUM(long _NUM){ setParamValue(1, _NUM); }
	public long get_NUM(){ return getParamValue(1); }

}

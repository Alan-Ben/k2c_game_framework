package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
经营建筑提升等级
****/
@EventDesc(id=43,name="P_BUSINESS_UPGRADE",params={"BUILDING_ID"})
public class Event_P_BUSINESS_UPGRADE extends _ALogicEventBase
{
	public static final int ID =43;
	public Event_P_BUSINESS_UPGRADE(_IContext _context,long _BUILDING_ID)
	{
		super(_context);
		
		set_BUILDING_ID(_BUILDING_ID);

	}
	public Event_P_BUSINESS_UPGRADE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//建筑ID
	public void set_BUILDING_ID(long _BUILDING_ID){ setParamValue(0, _BUILDING_ID); }
	public long get_BUILDING_ID(){ return getParamValue(0); }

}

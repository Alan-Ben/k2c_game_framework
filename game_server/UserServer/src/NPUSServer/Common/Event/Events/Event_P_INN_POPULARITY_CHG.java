package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
旅店人气变更
****/
@EventDesc(id=40,name="P_INN_POPULARITY_CHG",params={"POPULARITY"})
public class Event_P_INN_POPULARITY_CHG extends _ALogicEventBase
{
	public static final int ID =40;
	public Event_P_INN_POPULARITY_CHG(_IContext _context,long _POPULARITY)
	{
		super(_context);
		
		set_POPULARITY(_POPULARITY);

	}
	public Event_P_INN_POPULARITY_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//人气值
	public void set_POPULARITY(long _POPULARITY){ setParamValue(0, _POPULARITY); }
	public long get_POPULARITY(){ return getParamValue(0); }

}

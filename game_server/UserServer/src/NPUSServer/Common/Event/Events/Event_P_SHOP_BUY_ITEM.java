package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
商店购买物品
****/
@EventDesc(id=49,name="P_SHOP_BUY_ITEM",params={"SHOP_ID"})
public class Event_P_SHOP_BUY_ITEM extends _ALogicEventBase
{
	public static final int ID =49;
	public Event_P_SHOP_BUY_ITEM(_IContext _context,long _SHOP_ID)
	{
		super(_context);
		
		set_SHOP_ID(_SHOP_ID);

	}
	public Event_P_SHOP_BUY_ITEM(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//商店ID
	public void set_SHOP_ID(long _SHOP_ID){ setParamValue(0, _SHOP_ID); }
	public long get_SHOP_ID(){ return getParamValue(0); }

}

package ActivitiesV02.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
2048新方块生成
****/
@EventDesc(id=2003,name="P_NUM_MERGE_NEW_BLOCK",params={"ID","COUNT"})
public class Event_P_NUM_MERGE_NEW_BLOCK extends _ALogicEventBase
{
	public static final int ID =2003;
	public Event_P_NUM_MERGE_NEW_BLOCK(_IContext _context,long _ID,long _COUNT)
	{
		super(_context);
		
		set_ID(_ID);
		set_COUNT(_COUNT);

	}
	public Event_P_NUM_MERGE_NEW_BLOCK(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//方块等级
	public void set_ID(long _ID){ setParamValue(0, _ID); }
	public long get_ID(){ return getParamValue(0); }
	//数量
	public void set_COUNT(long _COUNT){ setParamValue(1, _COUNT); }
	public long get_COUNT(){ return getParamValue(1); }

}

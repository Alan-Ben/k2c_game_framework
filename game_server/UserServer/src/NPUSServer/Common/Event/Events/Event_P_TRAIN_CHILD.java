package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
培养子嗣
****/
@EventDesc(id=11,name="P_TRAIN_CHILD",params={"CHILD_ID","TRAIN_NUM","TRAIN_EXP","HERO_EXP"})
public class Event_P_TRAIN_CHILD extends _ALogicEventBase
{
	public static final int ID =11;
	public Event_P_TRAIN_CHILD(_IContext _context,long _CHILD_ID,long _TRAIN_NUM,long _TRAIN_EXP,long _HERO_EXP)
	{
		super(_context);
		
		set_CHILD_ID(_CHILD_ID);
		set_TRAIN_NUM(_TRAIN_NUM);
		set_TRAIN_EXP(_TRAIN_EXP);
		set_HERO_EXP(_HERO_EXP);

	}
	public Event_P_TRAIN_CHILD(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//子嗣实例ID
	public void set_CHILD_ID(long _CHILD_ID){ setParamValue(0, _CHILD_ID); }
	public long get_CHILD_ID(){ return getParamValue(0); }
	//培养次数
	public void set_TRAIN_NUM(long _TRAIN_NUM){ setParamValue(1, _TRAIN_NUM); }
	public long get_TRAIN_NUM(){ return getParamValue(1); }
	//培养经验
	public void set_TRAIN_EXP(long _TRAIN_EXP){ setParamValue(2, _TRAIN_EXP); }
	public long get_TRAIN_EXP(){ return getParamValue(2); }
	//大臣经验
	public void set_HERO_EXP(long _HERO_EXP){ setParamValue(3, _HERO_EXP); }
	public long get_HERO_EXP(){ return getParamValue(3); }

}

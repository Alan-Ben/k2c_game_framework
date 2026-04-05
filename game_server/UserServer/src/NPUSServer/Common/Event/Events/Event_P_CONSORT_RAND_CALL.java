package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
妃子随机宠幸
****/
@EventDesc(id=10,name="P_CONSORT_RAND_CALL",params={"CONSORT_ID","CHARM_POINT_NUM"})
public class Event_P_CONSORT_RAND_CALL extends _ALogicEventBase
{
	public static final int ID =10;
	public Event_P_CONSORT_RAND_CALL(_IContext _context,long _CONSORT_ID,long _CHARM_POINT_NUM)
	{
		super(_context);
		
		set_CONSORT_ID(_CONSORT_ID);
		set_CHARM_POINT_NUM(_CHARM_POINT_NUM);

	}
	public Event_P_CONSORT_RAND_CALL(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//情人ID
	public void set_CONSORT_ID(long _CONSORT_ID){ setParamValue(0, _CONSORT_ID); }
	public long get_CONSORT_ID(){ return getParamValue(0); }
	//获得的加护力点数
	public void set_CHARM_POINT_NUM(long _CHARM_POINT_NUM){ setParamValue(1, _CHARM_POINT_NUM); }
	public long get_CHARM_POINT_NUM(){ return getParamValue(1); }

}

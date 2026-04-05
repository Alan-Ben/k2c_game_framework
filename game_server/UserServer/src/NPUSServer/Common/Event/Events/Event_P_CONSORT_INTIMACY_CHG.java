package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
妃子亲密度变更
****/
@EventDesc(id=39,name="P_CONSORT_INTIMACY_CHG",params={"CONSORT_ID","INTIMACY"})
public class Event_P_CONSORT_INTIMACY_CHG extends _ALogicEventBase
{
	public static final int ID =39;
	public Event_P_CONSORT_INTIMACY_CHG(_IContext _context,long _CONSORT_ID,long _INTIMACY)
	{
		super(_context);
		
		set_CONSORT_ID(_CONSORT_ID);
		set_INTIMACY(_INTIMACY);

	}
	public Event_P_CONSORT_INTIMACY_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//妃子ID
	public void set_CONSORT_ID(long _CONSORT_ID){ setParamValue(0, _CONSORT_ID); }
	public long get_CONSORT_ID(){ return getParamValue(0); }
	//亲密度
	public void set_INTIMACY(long _INTIMACY){ setParamValue(1, _INTIMACY); }
	public long get_INTIMACY(){ return getParamValue(1); }

}

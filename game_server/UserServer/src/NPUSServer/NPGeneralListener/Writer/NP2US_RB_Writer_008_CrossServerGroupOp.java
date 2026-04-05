package NPUSServer.NPGeneralListener.Writer;


import NP2US_RB.p008_ScheduleOp.NP2US_RB_008_001_ChgCrossServerGroup;
import NP2US_RB.p008_ScheduleOp.NP2US_RB_008_010_PushSchedule;

public class NP2US_RB_Writer_008_CrossServerGroupOp
{
	public static NP2US_RB_008_001_ChgCrossServerGroup make_001_ChgCrossServerGroup()
	{
		NP2US_RB_008_001_ChgCrossServerGroup proto = new NP2US_RB_008_001_ChgCrossServerGroup();
		return proto;
	}

    public static NP2US_RB_008_010_PushSchedule make_010_PushSchedule()
    {
    	NP2US_RB_008_010_PushSchedule proto = new NP2US_RB_008_010_PushSchedule();
        return proto;
    }
}

package NPUSServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.NPGeneralListener.RequestDispather.p008_CrossServerGroupOp.NPUSGeneralRequest_008_001_ChgCrossServerGroup;
import NPUSServer.NPGeneralListener.RequestDispather.p008_CrossServerGroupOp.NPUSGeneralRequest_008_002_NoticeScheduleCanSendReward;
import NPUSServer.NPGeneralListener.RequestDispather.p008_CrossServerGroupOp.NPUSGeneralRequest_008_010_PushSchedule;


public class NPUsGeneral_008_RequestDispatcher_CrossServerGroupOp extends NPRequestDispatcher
{
    public static void init(NPUSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPUSGeneralRequest_008_001_ChgCrossServerGroup(_dispather.getUSServer()));
        _dispather.regHandler(new NPUSGeneralRequest_008_002_NoticeScheduleCanSendReward(_dispather.getUSServer()));
        _dispather.regHandler(new NPUSGeneralRequest_008_010_PushSchedule(_dispather.getUSServer()));
    }
}

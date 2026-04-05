package NPScheduleServer.NPGeneralListener.RequestDispather.p001_CrossServerGroupOp;

import Common.ScheduleObj.Schedule_UsPushData;
import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_011_GetUsActivityScheduleList;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.NPGeneralListener.RequestDispather.Writer.NP2SS_RB_Writer_001_CrossServerGroupOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.List;

public class NPSSGeneralRequest_001_011_GetUsActivityScheduleList extends NPRequestDealer<NP2SS_R_001_011_GetUsActivityScheduleList>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2SS_R_001_011_GetUsActivityScheduleList _msg)
    {
        List<Schedule_UsPushData> scheduleList = ActivityScheduleDataMgr.getInstance().makeUsScheduleHadPushList(_msg.getUsId());

        _commiter.commitSucRes(NP2SS_RB_Writer_001_CrossServerGroupOp.make_011_GetUsActivityScheduleList(scheduleList));
    }
}

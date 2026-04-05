package NPScheduleServer.NPGeneralListener.RequestDispather.p001_CrossServerGroupOp;

import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_013_CmdAddActivitySchedule;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.NPGeneralListener.RequestDispather.Writer.NP2SS_RB_Writer_001_CrossServerGroupOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPSSGeneralRequest_001_013_CmdAddActivitySchedule extends NPRequestDealer<NP2SS_R_001_013_CmdAddActivitySchedule>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2SS_R_001_013_CmdAddActivitySchedule _msg)
    {
        if (_msg.getIsGmAdd())
        {
            ActivityScheduleDataMgr.getInstance().createNewSchedule(_msg.getScheduleInfo());
        }else
        {
            ActivityScheduleDataMgr.getInstance().chgScheduleData(_msg.getScheduleInfo());
        }

    	_commiter.commitSucRes(NP2SS_RB_Writer_001_CrossServerGroupOp.make_013_CmdAddActivitySchedule());
    }
}

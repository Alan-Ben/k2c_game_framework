package NPScheduleServer.NPGeneralListener.RequestDispather.p001_CrossServerGroupOp;

import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_016_ReportUsActivitySchedulePlaying;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.NPGeneralListener.RequestDispather.Writer.NP2SS_RB_Writer_001_CrossServerGroupOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPSSGeneralRequest_001_016_ReportUsActivitySchedulePlaying extends NPRequestDealer<NP2SS_R_001_016_ReportUsActivitySchedulePlaying>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2SS_R_001_016_ReportUsActivitySchedulePlaying _msg)
    {
    	ActivityScheduleDataMgr.getInstance().setUsPlaying(_msg.getScheduleId(), _msg.getUsGroupId(), _msg.getUsId());
    	
    	_commiter.commitSucRes(NP2SS_RB_Writer_001_CrossServerGroupOp.make_016_ReportUsActivitySchedulePlaying());
    }
}

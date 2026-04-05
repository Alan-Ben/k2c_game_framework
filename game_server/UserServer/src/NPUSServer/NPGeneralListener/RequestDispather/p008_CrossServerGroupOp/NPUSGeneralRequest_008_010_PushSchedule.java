package NPUSServer.NPGeneralListener.RequestDispather.p008_CrossServerGroupOp;

import NP2US_R.p008_ScheduleOp.NP2US_R_008_010_PushSchedule;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_008_CrossServerGroupOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPUSGeneralRequest_008_010_PushSchedule extends _ABasicGeneralRequestDealer<NP2US_R_008_010_PushSchedule>
{
    public NPUSGeneralRequest_008_010_PushSchedule(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2US_R_008_010_PushSchedule _msg)
    {
        USLog.info(getUSServer(), "schedule:{} usGroup:{} push to US."
                , _msg.getUsSchedule().getScheduleId(), _msg.getUsSchedule().getUsGroupId());
    	
    	getUSServer().getUsActivityScheduleMgr().onScheduleInfoPush(_msg.getUsSchedule());
    	
    	_commiter.commitSucRes(NP2US_RB_Writer_008_CrossServerGroupOp.make_010_PushSchedule());
    }
}

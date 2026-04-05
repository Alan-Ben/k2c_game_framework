package NPUSServer.NPGeneralListener.RequestDispather.p008_CrossServerGroupOp;

import NP2US_R.p008_ScheduleOp.NP2US_R_008_002_NoticeScheduleCanSendReward;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_008_CrossServerGroupOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPUSGeneralRequest_008_002_NoticeScheduleCanSendReward extends _ABasicGeneralRequestDealer<NP2US_R_008_002_NoticeScheduleCanSendReward>
{
    public NPUSGeneralRequest_008_002_NoticeScheduleCanSendReward(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2US_R_008_002_NoticeScheduleCanSendReward _msg)
    {
        getUSServer().getUsActivityScheduleMgr().setScheduleCanSendReward(_msg.getScheduleId());

        _receiver.commitSucRes(NP2US_RB_Writer_008_CrossServerGroupOp.make_001_ChgCrossServerGroup());
    }
}

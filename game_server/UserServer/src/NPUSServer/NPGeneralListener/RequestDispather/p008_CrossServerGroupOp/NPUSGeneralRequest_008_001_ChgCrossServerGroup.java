package NPUSServer.NPGeneralListener.RequestDispather.p008_CrossServerGroupOp;

import NP2US_R.p008_ScheduleOp.NP2US_R_008_001_ChgCrossServerGroup;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_008_CrossServerGroupOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPUSGeneralRequest_008_001_ChgCrossServerGroup extends _ABasicGeneralRequestDealer<NP2US_R_008_001_ChgCrossServerGroup>
{
    public NPUSGeneralRequest_008_001_ChgCrossServerGroup(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2US_R_008_001_ChgCrossServerGroup _msg)
    {
        getUSServer().getLocalCrossServerGroupMgr().setPrepareChangeGroupInfo(_msg.getGroupInfo());

        _receiver.commitSucRes(NP2US_RB_Writer_008_CrossServerGroupOp.make_001_ChgCrossServerGroup());
    }
}

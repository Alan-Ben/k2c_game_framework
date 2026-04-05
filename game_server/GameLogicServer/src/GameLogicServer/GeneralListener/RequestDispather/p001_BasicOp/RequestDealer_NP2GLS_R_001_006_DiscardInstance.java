package GameLogicServer.GeneralListener.RequestDispather.p001_BasicOp;

import GameLogicServer.GeneralListener.RB_Writer.GOM2CD_RB_Writer_001_DataOp;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_006_DiscardInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2GLS_R_001_006_DiscardInstance extends NPRequestDealer<NP2GLS_R_001_006_DiscardInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2GLS_R_001_006_DiscardInstance _msg)
    {
        GroupInstanceMgr.getInstance().tryDiscard(_msg.getInstanceId());

        _committer.commitSucRes(GOM2CD_RB_Writer_001_DataOp.make_006_DiscardInstance());
    }
}

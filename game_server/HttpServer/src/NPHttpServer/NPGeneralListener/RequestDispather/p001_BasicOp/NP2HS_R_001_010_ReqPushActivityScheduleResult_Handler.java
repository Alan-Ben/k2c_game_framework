package NPHttpServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import NP2HS_R.p001_HSOp.NP2HS_R_001_010_ReqPushActivityScheduleResult;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPHttpServer.NPGeneralListener.Writer.NP2HS_RB_Writer_001_PSOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NP2HS_R_001_010_ReqPushActivityScheduleResult_Handler extends NPRequestDealer<NP2HS_R_001_010_ReqPushActivityScheduleResult>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2HS_R_001_010_ReqPushActivityScheduleResult _msg)
    {
    	//TODO：推送协议到后台
    	
        _receiver.commitSucRes(NP2HS_RB_Writer_001_PSOp.make_010_RetPushActivityScheduleResult());
    }
}

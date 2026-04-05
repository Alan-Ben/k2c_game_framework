package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_003_CommonOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_008_ReqGetBoxInfo;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_008_RetGetBoxInfo;

public class RequestDealer_NP2US_R_003_008_ReqGetBoxInfo extends _ABasicGeneralRequestDealer<NP2US_R_003_008_ReqGetBoxInfo>
{
    public RequestDealer_NP2US_R_003_008_ReqGetBoxInfo(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_008_ReqGetBoxInfo _msg)
    {
        CommBoxInfo boxInfo = getUSServer().getCommBoxMgr().lookupBox(_msg.getInstanceId());
        if (null == boxInfo)
        {
            _committer.commitSucRes(new NP2US_RB_003_008_RetGetBoxInfo());
            return;
        }

        _committer.commitSucRes(NP2US_RB_Writer_003_CommonOp.make_008_RetGetBoxInfo(
                boxInfo.getRef().id, boxInfo.checkStatus(_msg.getCid()), boxInfo.getGainedCidList()));
    }
}

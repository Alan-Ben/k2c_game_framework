package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_010_ReqRemoveBoxGainedCid;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_010_RetRemoveBoxGainedCid;

public class RequestDealer_NP2US_R_003_010_ReqRemoveBoxGainedCid extends _ABasicGeneralRequestDealer<NP2US_R_003_010_ReqRemoveBoxGainedCid>
{
    public RequestDealer_NP2US_R_003_010_ReqRemoveBoxGainedCid(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_010_ReqRemoveBoxGainedCid _msg)
    {
        CommBoxInfo boxInfo = getUSServer().getCommBoxMgr().lookupBox(_msg.getInstanceId());
        if (null != boxInfo)
        {
            boxInfo.clearGainedCid(_msg.getCid());
        }

        _committer.commitSucRes(new NP2US_RB_003_010_RetRemoveBoxGainedCid());
    }
}

package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_006_ReqRankFixedLike;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_006_RetRankFixedLike;

public class RequestDealer_NP2US_R_003_006_ReqRankFixedLike extends _ABasicGeneralRequestDealer<NP2US_R_003_006_ReqRankFixedLike>
{
    public RequestDealer_NP2US_R_003_006_ReqRankFixedLike(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_006_ReqRankFixedLike _msg)
    {
        getUSServer().getRankFixedObjDataListMgr().ensureObj(_msg.getRankFixedId()).incrLike(_msg.getKey(), _msg.getIsCross());
        _committer.commitSucRes(new NP2US_RB_003_006_RetRankFixedLike());
    }
}

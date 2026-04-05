package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_013_ReqUnBanCid;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_013_RetUnBanCid;

public class RequestDealer_NP2US_R_003_013_ReqUnBanCid extends _ABasicGeneralRequestDealer<NP2US_R_003_013_ReqUnBanCid>
{
    public RequestDealer_NP2US_R_003_013_ReqUnBanCid(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_013_ReqUnBanCid _msg)
    {
        getUSServer().getPlayerFreezeMgr().freeze(_msg.getCid(), 0);
        _committer.commitSucRes(new NP2US_RB_003_013_RetUnBanCid());
    }
}

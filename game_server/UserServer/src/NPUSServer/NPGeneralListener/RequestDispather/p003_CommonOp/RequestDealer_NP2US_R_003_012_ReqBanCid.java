package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_012_ReqBanCid;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_012_RetBanCid;

public class RequestDealer_NP2US_R_003_012_ReqBanCid extends _ABasicGeneralRequestDealer<NP2US_R_003_012_ReqBanCid>
{
    public RequestDealer_NP2US_R_003_012_ReqBanCid(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_012_ReqBanCid _msg)
    {
        getUSServer().getPlayerFreezeMgr().freeze(_msg.getCid(), _msg.getFreezeTimeMs());
        //如果玩家在线将玩家强制下线
        getUSServer().getUsUserMgr().forceKickUser(_msg.getCid());
        _committer.commitSucRes(new NP2US_RB_003_012_RetBanCid());
    }
}

package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPCommon.ErrMain.CommErr;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_001_ReqSendProtocol;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_001_ReqSendProtocol;

public class RequestDealer_NP2US_R_003_001_ReqSendProtocol extends _ABasicGeneralRequestDealer<NP2US_R_003_001_ReqSendProtocol>
{
    public RequestDealer_NP2US_R_003_001_ReqSendProtocol(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_001_ReqSendProtocol _msg)
    {
        for(int i = 0; i < _msg.getCidList().size(); i++)
        {
            NPUSUserData userData = getUSServer().getUsUserMgr().lookupCacheUserData(_msg.getCidList().get(i));
            if(null == userData)
            {
                _committer.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
                return;
            }

            userData.safeCall(() ->
            {
                userData.sendMsgToGC(_msg.get_buffer_Protocol());
            });
        }

        _committer.commitSucRes(new NP2US_RB_003_001_ReqSendProtocol());
    }
}

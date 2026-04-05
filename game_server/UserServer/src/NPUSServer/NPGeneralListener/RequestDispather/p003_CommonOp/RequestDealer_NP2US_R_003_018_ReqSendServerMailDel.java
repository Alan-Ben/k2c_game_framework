package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_018_ReqSendServerMailDel;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_018_RetSendServerMailDel;

public class RequestDealer_NP2US_R_003_018_ReqSendServerMailDel extends _ABasicGeneralRequestDealer<NP2US_R_003_018_ReqSendServerMailDel>
{
    public RequestDealer_NP2US_R_003_018_ReqSendServerMailDel(NPUserServer _server) 
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_018_ReqSendServerMailDel _msg)
    {
    	getUSServer().getAllServerMailTemplateMgr().delMailTemplate(_msg.getPhpMailId());
    	
    	_committer.commitSucRes(new NP2US_RB_003_018_RetSendServerMailDel());
    }
}

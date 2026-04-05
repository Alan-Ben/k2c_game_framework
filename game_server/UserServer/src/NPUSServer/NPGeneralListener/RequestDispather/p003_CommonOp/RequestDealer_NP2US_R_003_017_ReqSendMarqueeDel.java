package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_017_ReqSendMarqueeDel;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_017_RetSendMarqueeDel;

public class RequestDealer_NP2US_R_003_017_ReqSendMarqueeDel extends _ABasicGeneralRequestDealer<NP2US_R_003_017_ReqSendMarqueeDel>
{
    public RequestDealer_NP2US_R_003_017_ReqSendMarqueeDel(NPUserServer _server) 
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_017_ReqSendMarqueeDel _msg)
    {
    	getUSServer().getMarqueeMgr().cmdDelPHPMarquee(_msg.getPhpId());

    	_committer.commitSucRes(new NP2US_RB_003_017_RetSendMarqueeDel());
    }
}

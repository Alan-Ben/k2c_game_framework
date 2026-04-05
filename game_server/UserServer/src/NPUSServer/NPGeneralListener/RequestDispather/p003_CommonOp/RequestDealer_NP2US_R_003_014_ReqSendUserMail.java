package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_014_ReqSendUserMail;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_014_RetSendUserMail;

public class RequestDealer_NP2US_R_003_014_ReqSendUserMail extends _ABasicGeneralRequestDealer<NP2US_R_003_014_ReqSendUserMail>
{
    public RequestDealer_NP2US_R_003_014_ReqSendUserMail(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_014_ReqSendUserMail _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.REFRESH_MAIL);
        MailSystem.addFormMail(getUSServer(), _msg.getCid(), _msg.getMailData(), context);
        _committer.commitSucRes(new NP2US_RB_003_014_RetSendUserMail());
    }
}

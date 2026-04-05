package NPUSServer.NPUserMsgDispather.p009_Mail;

import GC2GS.p009_MailOp.GC2GS_009_009_ReqSetReadOver;
import NPCommon.ErrMain.MailErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;

public class MsgDealer_GC2GS_009_009_ReqSetReadOver extends NPUserMsgDealer<GC2GS_009_009_ReqSetReadOver>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_009_ReqSetReadOver _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        MailInfo npMailInfo = userData.getMailComponent().lookupMail(_msg.getMailUid());
        if (null == npMailInfo)
        {
            _commiter.commitFailRes(MailErr.MAIL_NOT_FOUND.getCode());
            return;
        }
        
        npMailInfo.setReadOver();
        
        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_009_RetSetReadOver());
    }
}

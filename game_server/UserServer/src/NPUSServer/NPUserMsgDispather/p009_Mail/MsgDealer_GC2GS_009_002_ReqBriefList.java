package NPUSServer.NPUserMsgDispather.p009_Mail;

import GC2GS.p009_MailOp.GC2GS_009_002_ReqBriefList;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;

public class MsgDealer_GC2GS_009_002_ReqBriefList extends NPUserMsgDealer<GC2GS_009_002_ReqBriefList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_002_ReqBriefList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_002_RetMailBriefList(userData));
    }
}

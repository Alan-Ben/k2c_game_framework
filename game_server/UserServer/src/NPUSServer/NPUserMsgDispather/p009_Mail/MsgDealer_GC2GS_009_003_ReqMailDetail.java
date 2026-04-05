package NPUSServer.NPUserMsgDispather.p009_Mail;

import GC2GS.p009_MailOp.GC2GS_009_003_ReqMailDetail;
import NPCommon.ErrMain.MailErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;

public class MsgDealer_GC2GS_009_003_ReqMailDetail extends NPUserMsgDealer<GC2GS_009_003_ReqMailDetail>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_003_ReqMailDetail _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        MailInfo npMailInfo = userData.getMailComponent().lookupMail(_msg.getMailUid());
        if (null == npMailInfo)
        {
            _commiter.commitFailRes(MailErr.MAIL_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MAIL_READ_ONE);
        npMailInfo.markRead(context);

        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_003_RetMailDetail(npMailInfo));

        //返回可能获得的奖励信息
        userData.sendMsgToGC(context.getCollector().toProto());
    }
}

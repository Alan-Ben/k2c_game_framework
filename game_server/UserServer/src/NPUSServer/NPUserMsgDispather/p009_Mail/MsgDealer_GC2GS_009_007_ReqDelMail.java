package NPUSServer.NPUserMsgDispather.p009_Mail;

import GC2GS.p009_MailOp.GC2GS_009_007_ReqDelMail;
import NPCommon.ErrMain.MailErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;

public class MsgDealer_GC2GS_009_007_ReqDelMail extends NPUserMsgDealer<GC2GS_009_007_ReqDelMail>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_007_ReqDelMail _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        MailInfo npMailInfo = userData.getMailComponent().lookupMail(_msg.getMailUid());
        if (null == npMailInfo)
        {
            _commiter.commitFailRes(MailErr.MAIL_NOT_FOUND.getCode());
            return;
        }
        //有附件未领取，不能删除
        if (npMailInfo.getHasItem() && !npMailInfo.getHasTaken())
        {
            _commiter.commitFailRes(MailErr.MAIL_DEL_HAS_ITEM.getCode());
            return;
        }
        if (npMailInfo.getIsLocked())
        {
            _commiter.commitFailRes(MailErr.MAIL_DEL_LOCKED.getCode());
            return;
        }
        //必读邮件未读取，不能删除
        if (npMailInfo.getIsMustRead() && !npMailInfo.getIsRead())
        {
            _commiter.commitFailRes(MailErr.MAIL_DEL_MUST_READ.getCode());
            return;
        }
        //必读邮件未读取，没有读完，不能删除
        if (npMailInfo.getIsMustRead() && !npMailInfo.getIsReadOver())
        {
            _commiter.commitFailRes(MailErr.MAIL_DEL_MUST_READ_OVER.getCode());
            return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DEL_MAIL);
        userData.getMailComponent().deleteMail(npMailInfo, context);
        
        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_007_RetDelMail(npMailInfo));
    }
}

package NPUSServer.NPUserMsgDispather.p009_Mail;

import GC2GS.p009_MailOp.GC2GS_009_004_ReqTakeMailItems;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.MailErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;
import USLOGDB.OptBo.Opt009004MailTakeBO;

public class MsgDealer_GC2GS_009_004_ReqTakeMailItems extends NPUserMsgDealer<GC2GS_009_004_ReqTakeMailItems>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_004_ReqTakeMailItems _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        MailInfo npMailInfo = userData.getMailComponent().lookupMail(_msg.getMailUid());
        if (null == npMailInfo)
        {
            _commiter.commitFailRes(MailErr.MAIL_NOT_FOUND.getCode());
            return;
        }
        if (!npMailInfo.getHasItem())
        {
            _commiter.commitFailRes(MailErr.MAIL_TAKE_NOT_HAS_ITEM.getCode());
            return;
        }
        
        if (npMailInfo.getHasTaken())
        {
            _commiter.commitFailRes(MailErr.MAIL_TAKE_ALREADY_TAKEN.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GAIN_MAIL_REWARD);
        //调用邮件获取附件处理
        if (!npMailInfo.takeMailAttach(context))
        {
            _commiter.commitFailRes(MailErr.MAIL_TAKE_HAS_ERROR.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_004_RetTakeMailItems(npMailInfo));

        userData.sendMsgToGC(context.getCollector().toProto());

        BM bmObj = getUSServer().getBM();
        //操作日志
        Opt009004MailTakeBO optBo = new Opt009004MailTakeBO();
        optBo.setMailUid(bmObj, _msg.getMailUid());
        optBo.setMailId(bmObj, npMailInfo.getMailRefId());
        optBo.setMailPhp(bmObj, npMailInfo.getMailPhpId());
        userData.logEvent(optBo, context);
    }
}

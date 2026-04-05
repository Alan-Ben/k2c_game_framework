package NPUSServer.NPUserMsgDispather.p009_Mail;

import GC2GS.p009_MailOp.GC2GS_009_005_ReqSetMailLockState;
import NPCommon.ErrMain.MailErr;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;

public class MsgDealer_GC2GS_009_005_ReqSetMailLockState extends NPUserMsgDealer<GC2GS_009_005_ReqSetMailLockState>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_005_ReqSetMailLockState _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        if (_msg.getIsLocked() && userData.getMailComponent().getLockedCount() >= RefGeneral.Ref().mail_max_lock_num)
        {
            _commiter.commitFailRes(MailErr.MAIL_LOCK_MAX.getCode());
            return;
        }
        
        MailInfo npMailInfo = userData.getMailComponent().lookupMail(_msg.getMailUid());
        if (null == npMailInfo)
        {
            _commiter.commitFailRes(MailErr.MAIL_NOT_FOUND.getCode());
            return;
        }

        npMailInfo.setLockState(_msg.getIsLocked());
        
        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_005_RetSetMailLockState(npMailInfo));
    }
}

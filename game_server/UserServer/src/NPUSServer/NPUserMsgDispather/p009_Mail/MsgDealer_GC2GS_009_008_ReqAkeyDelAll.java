package NPUSServer.NPUserMsgDispather.p009_Mail;

import GC2GS.p009_MailOp.GC2GS_009_008_ReqAkeyDelAll;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;

import java.util.ArrayList;
import java.util.List;

public class MsgDealer_GC2GS_009_008_ReqAkeyDelAll extends NPUserMsgDealer<GC2GS_009_008_ReqAkeyDelAll>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_008_ReqAkeyDelAll _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        ArrayList<Long> delIdList = new ArrayList<>();
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DEL_ALL_MAIL);
        List<MailInfo> mailList = userData.getMailComponent().getMailList();
        for (MailInfo mailInfo : mailList)
        {
            //已收藏邮件不删除
            if (mailInfo.getIsLocked())
                continue;
            
            //未读邮件不删除
            if(!mailInfo.getIsRead())
            	continue;
            
            //必读邮件未读完整不删除
            if (mailInfo.getIsMustRead() && !mailInfo.getIsReadOver())
            	continue;
            
            //未领取的邮件不删除
            if (mailInfo.getHasItem() && !mailInfo.getHasTaken())
                continue;
            
            //删除邮件
            userData.getMailComponent().deleteMail(mailInfo, context);
            //记录ID
            delIdList.add(mailInfo.getUid());
        }
        
        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_008_RetAKeyDelAll(delIdList));
    }
}

package NPUSServer.NPUserMsgDispather.p009_Mail;

import Common.MailObj.Mail_TitleInfo;
import GC2GS.p009_MailOp.GC2GS_009_010_ReqMailTitleInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;

import java.util.ArrayList;

public class MsgDealer_GC2GS_009_010_ReqMailTitleInfo extends NPUserMsgDealer<GC2GS_009_010_ReqMailTitleInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_010_ReqMailTitleInfo _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        ArrayList<Mail_TitleInfo> mailTitleList = new ArrayList<>();
        for (Long uid : _msg.getMailUidList())
        {
            MailInfo mailInfo = userData.getMailComponent().lookupMail(uid);
            if (null == mailInfo)
            {
                continue;
            }
            
            mailTitleList.add(mailInfo.to_Mail_TitleInfo());
        }
        
        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_010_RetMailTitleInfo(mailTitleList));
    }
}

package NPUSServer.NPUserMsgDispather.p009_Mail;

import GC2GS.p009_MailOp.GC2GS_009_006_ReqAKeyTakeAll;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;

import java.util.ArrayList;
import java.util.List;

public class MsgDealer_GC2GS_009_006_ReqAKeyTakeAll extends NPUserMsgDealer<GC2GS_009_006_ReqAKeyTakeAll>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_009_006_ReqAKeyTakeAll _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MAIL_TAKE_ALL);

        ArrayList<Long> mailUidList = new ArrayList<>();
        List<MailInfo> mailList = userData.getMailComponent().getMailList();
        for (MailInfo mailInfo : mailList)
        {
            //GOB-6664【优化-1】邮件一键领取后，无法领取引流页下发的奖励邮件
            //https://www.teambition.com/task/691440591c36eb4fb494d1bd
            //说明：必读邮件如果未读则不领取
            if (mailInfo.getIsMustRead() && !mailInfo.getIsRead())
            {
                continue;
            }

            //设置已读
            mailInfo.markRead(context);
            
            //尝试领取未领取的邮件，函数内有判断
            if(mailInfo.takeMailAttach(context))
            {
            	mailUidList.add(mailInfo.getUid());
            }
        }

        _commiter.commitSucRes(US2GCWriter_009_MailOp.make_006_RetAKeyTakeAll(context, mailUidList));

        //返回可能获得的奖励信息
        userData.sendMsgToGC(context.getCollector().toProto());
    }
}

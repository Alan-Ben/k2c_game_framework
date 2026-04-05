package NPUSServer.NPUserMsgDispather.Write;

import Common.MailObj.Mail_TitleInfo;
import GS2GC.p009_MailOp.*;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;

import java.util.ArrayList;

public class US2GCWriter_009_MailOp
{
    public static GS2GC_009_002_RetMailBriefList make_002_RetMailBriefList(NPUSUserData _userData)
    {
    	GS2GC_009_002_RetMailBriefList proto = new GS2GC_009_002_RetMailBriefList();
    	_userData.getMailComponent().fillBriefList(proto.getBriefList());
    	
    	return proto;
    }
    
    public static GS2GC_009_003_RetMailDetail make_003_RetMailDetail(MailInfo _mailInfo)
    {
    	GS2GC_009_003_RetMailDetail proto = new GS2GC_009_003_RetMailDetail();
    	proto.setMailDetail(_mailInfo.to_Mail_DetailInfo());
    	
    	return proto;
    }
    
    public static GS2GC_009_004_RetTakeMailItems make_004_RetTakeMailItems(MailInfo _mailInfo)
    {
    	GS2GC_009_004_RetTakeMailItems proto = new GS2GC_009_004_RetTakeMailItems();
    	proto.setMailUid(_mailInfo.getUid());
    	
    	return proto;
    }
    
    public static GS2GC_009_005_RetSetMailLockState make_005_RetSetMailLockState(MailInfo _mailInfo)
    {
    	GS2GC_009_005_RetSetMailLockState proto = new GS2GC_009_005_RetSetMailLockState();
    	proto.setMailUid(_mailInfo.getUid());
    	proto.setIsLocked(_mailInfo.getIsLocked());
    	
    	return proto;
    }
    
    public static GS2GC_009_006_RetAKeyTakeAll make_006_RetAKeyTakeAll(NPPlayerContext _context, ArrayList<Long> _mustReadMailList)
    {
    	GS2GC_009_006_RetAKeyTakeAll proto = new GS2GC_009_006_RetAKeyTakeAll();
    	proto.getGainItemList().addAll(_context.getCollector().toProto().getItemList());
    	proto.getMustReadMailList().addAll(_mustReadMailList);
    	
    	return proto;
    }
    
    public static GS2GC_009_007_RetDelMail make_007_RetDelMail(MailInfo _mailInfo)
    {
    	GS2GC_009_007_RetDelMail proto = new GS2GC_009_007_RetDelMail();
    	proto.setMailUid(_mailInfo.getUid());
    	
    	return proto;
    }
    
    public static GS2GC_009_008_RetAKeyDelAll make_008_RetAKeyDelAll(ArrayList<Long> _delIdList)
    {
    	GS2GC_009_008_RetAKeyDelAll proto = new GS2GC_009_008_RetAKeyDelAll();
    	proto.getDelIdList().addAll(_delIdList);
    	
    	return proto;
    }
    
    public static GS2GC_009_009_RetSetReadOver make_009_RetSetReadOver()
    {
    	GS2GC_009_009_RetSetReadOver proto = new GS2GC_009_009_RetSetReadOver();
    	
    	return proto;
    }
    
    public static GS2GC_009_010_RetMailTitleInfo make_010_RetMailTitleInfo(ArrayList<Mail_TitleInfo> _mailTitleList)
    {
    	GS2GC_009_010_RetMailTitleInfo proto = new GS2GC_009_010_RetMailTitleInfo();
    	proto.getTitleList().addAll(_mailTitleList);
    	
    	return proto;
    }

    public static GS2GC_009_051_OnMailAdded make_051_OnMailAdded(MailInfo _mailInfo)
    {
    	GS2GC_009_051_OnMailAdded proto = new GS2GC_009_051_OnMailAdded();
    	proto.setTitleInfo(_mailInfo.to_Mail_TitleInfo());
    	proto.setBriefInfo(_mailInfo.to_Mail_BriefInfo());
    	return proto;
    }

    public static GS2GC_009_052_OnMailRemoved make_052_OnMailRemoved(MailInfo _mailInfo)
    {
    	GS2GC_009_052_OnMailRemoved proto = new GS2GC_009_052_OnMailRemoved();
    	proto.setMailUid(_mailInfo.getUid());
    	proto.setIsRead(_mailInfo.getIsRead());
    	
    	return proto;
    }
    
    public static GS2GC_009_053_OnMailLockedUpdated make_053_OnMailLockedUpdated(MailInfo _mailInfo)
    {
    	GS2GC_009_053_OnMailLockedUpdated proto = new GS2GC_009_053_OnMailLockedUpdated();
    	proto.setMailUid(_mailInfo.getUid());
    	proto.setIsLocked(_mailInfo.getIsLocked());
    	
    	return proto;
    }
    
    public static GS2GC_009_054_OnMailReaded make_054_OnMailReaded(MailInfo _mailInfo)
    {
    	GS2GC_009_054_OnMailReaded proto = new GS2GC_009_054_OnMailReaded();
    	proto.addMailUidList(_mailInfo.getUid());
    	
    	return proto;
    }
    
    public static GS2GC_009_055_OnMailRewardTaken make_055_OnMailRewardTaken(MailInfo _mailInfo)
    {
    	GS2GC_009_055_OnMailRewardTaken proto = new GS2GC_009_055_OnMailRewardTaken();
    	proto.addMailUidList(_mailInfo.getUid());
    	
    	return proto;
    }
    
    public static GS2GC_009_057_OnMailReadOver make_057_OnMailReadOver(MailInfo _mailInfo)
    {
    	GS2GC_009_057_OnMailReadOver proto = new GS2GC_009_057_OnMailReadOver();
    	proto.setMailUid(_mailInfo.getUid());
    	
    	return proto;
    }

    public static GS2GC_009_058_OnMailExpiredSecChg make_058_OnMailExpiredSecChg(MailInfo _mailInfo)
    {
    	GS2GC_009_058_OnMailExpiredSecChg proto = new GS2GC_009_058_OnMailExpiredSecChg();
    	proto.setMailUid(_mailInfo.getUid());
    	proto.setExpiredTimeSec(_mailInfo.getExpiredTs());
    	
    	return proto;
    }
}

package NPUSServer.GMCommand.Cmds;

import Common.MailEnum.EMailExtType;
import Common.MailObj.Mail_Data;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.StringFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Mail.RefMail;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.UserComp.MailComp.MailInfo;

import java.util.List;


@ACommander(comment = "邮件相关命令", name = "mail")
public class CmdMail extends UsCmdBase
{
    @ACommand(comment = "发送php邮件，无配置Id(cid)")
    public String sendPhpMail(long _cid)
    {
        if (_cid <= 0)
        {
            if (null != getOwner())
            {
                _cid = getOwner().getCid();
            } else
            {
                return "invalid cid:" + _cid;
            }
        }
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(1);
        mailData.setSenderId(1);
        mailData.setTitle("php mail title replace:{0} " + CommonFunc.randomInt(1000));
        mailData.setContent("php mail test content {0}-{1}");
        mailData.getContentReplace().add("replace1");
        mailData.getContentReplace().add("replace2");
        mailData.getItemList().addItemList(new NPCommon_ItemInfo(ENPItemType.CURRENCY.ordinal(), ECurrency.GEM.ordinal(), 1, null));
        mailData.getItemList().addItemList(new NPCommon_ItemInfo(ENPItemType.CURRENCY.ordinal(), ECurrency.SILVER.ordinal(), 1, null));
        mailData.setIsMustRead(true);
        MailSystem.addMail(getUserServer(), _cid, mailData, getContext());
        return "ok";
    }

    @ACommand(comment = "发送测试邮件,指定配表id(cid,配表id,是否必读)")
    public String sendMail(long _cid, long _refId, boolean _isMustRead)
    {
        if (_cid <= 0)
        {
            if (null != getOwner())
            {
                _cid = getOwner().getCid();
            } else
            {
                return "invalid cid:" + _cid;
            }
        }
        RefMail refMail = RefMail.getMgr().get(_refId);
        if (null == refMail)
        {
            return "can not find RefMail for id:" + _refId;
        }
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(_refId);
        mailData.getContentReplace().add("replace1");
        mailData.getContentReplace().add("replace2");
        mailData.getItemList().addItemList(new NPCommon_ItemInfo(ENPItemType.CURRENCY.ordinal(), ECurrency.GEM.ordinal(), 2, null));
        mailData.getItemList().addItemList(new NPCommon_ItemInfo(ENPItemType.CURRENCY.ordinal(), ECurrency.SILVER.ordinal(), 2, null));
        mailData.setIsMustRead(_isMustRead);
        
        mailData.setExType(EMailExtType.TEST_ITEM_LIST.ordinal());
        NPCommon_ItemList testItemList = new NPCommon_ItemList();
        testItemList.addItemList(new NPCommon_ItemInfo(ENPItemType.ICON.ordinal(), 1001, 1, null));
        testItemList.addItemList(new NPCommon_ItemInfo(ENPItemType.ICON_BGK.ordinal(), 1001, 1, null));
        mailData.setExData(testItemList.makePackage());

        MailSystem.addMail(getUserServer(), _cid, mailData, getContext());
        return "ok";
    }

    @ACommand(comment = "发送测试邮件,指定配表id和奖励(cid,配表id,是否必读，奖励列表)")
    public String sendAttachMail(long _cid, long _refId, boolean _isMustRead, String _itemList)
    {
        if (_cid <= 0)
        {
            if (null != getOwner())
            {
                _cid = getOwner().getCid();
            } else
            {
                return "invalid cid:" + _cid;
            }
        }
        RefMail refMail = RefMail.getMgr().get(_refId);
        if (null == refMail)
        {
            return "can not find RefMail for id:" + _refId;
        }
        List<NPCommonCostItem> itemList = StringFunc.listFromString(_itemList, () -> new NPCommonCostItem());
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(_refId);
        mailData.getContentReplace().add("replace1");
        mailData.getContentReplace().add("replace2");
        for (NPCommonCostItem item : itemList)
        {
            mailData.getItemList().addItemList(item.toProto());
        }
        mailData.setIsMustRead(_isMustRead);
        MailSystem.addMail(getUserServer(), _cid, mailData, getContext());
        return "ok";
    }

    @ACommand(comment = "发送测试邮件没有附件,指定配表id（cid,配表id）")
    public String sendTxtMail(long _cid, long _refId)
    {
        if (_cid <= 0)
        {
            if (null != getOwner())
            {
                _cid = getOwner().getCid();
            } else
            {
                return "invalid cid:" + _cid;
            }
        }
        RefMail refMail = RefMail.getMgr().get(_refId);
        if (null == refMail)
        {
            return "can not find RefMail for id:" + _refId;
        }
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(_refId);
        mailData.getContentReplace().add("replace1");
        mailData.getContentReplace().add("replace2");
        mailData.setIsMustRead(false);
        MailSystem.addMail(getUserServer(), _cid, mailData, getContext());
        return "ok";
    }

    @ACommand(comment = "邮件列表")
    public String list()
    {
        List<MailInfo> mailList = getOwner().getMailComponent().getMailList();
        StringBuilder sb = new StringBuilder();
        for (MailInfo mailInfo : mailList)
        {
            sb.append(mailInfo.toString()).append("\n");
        }
        return sb.toString();
    }

    @ACommand(comment = "发送php必读邮件，指定文本(cid,标题，文本)")
    public String sendPhpTxt(long _cid, String _title, String _context)
    {
        if (_cid <= 0)
        {
            if (null != getOwner())
            {
                _cid = getOwner().getCid();
            } else
            {
                return "invalid cid:" + _cid;
            }
        }
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(1);
        mailData.setSenderId(1);
        mailData.setTitle(_title);
        mailData.setContent(_context);
        mailData.getContentReplace().add("replace1");
        mailData.getContentReplace().add("replace2");
        mailData.getItemList().addItemList(new NPCommon_ItemInfo(ENPItemType.CURRENCY.ordinal(), ECurrency.GEM.ordinal(), 1, null));
        mailData.getItemList().addItemList(new NPCommon_ItemInfo(ENPItemType.CURRENCY.ordinal(), ECurrency.SILVER.ordinal(), 1, null));
        mailData.setIsMustRead(true);
        MailSystem.addMail(getUserServer(), _cid, mailData, getContext());
        return "ok";
    }

}

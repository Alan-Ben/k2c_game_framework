package MGClient.Cmd.Cmds;

import GC2GS.p002_InitOp.GC2GS_002_014_ReqMailStatInfo;
import GC2GS.p009_MailOp.*;
import GS2GC.p002_InitOp.GS2GC_002_014_RetMailStatInfo;
import GS2GC.p009_MailOp.*;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "邮件命令", name = "mail")
public class CmdMail extends CmdBase
{
    @Command(comment = "获取邮件列表列表")
    public void list()
    {
        GC2GS_009_002_ReqBriefList proto = new GC2GS_009_002_ReqBriefList();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_009_002_RetMailBriefList>(GS2GC_009_002_RetMailBriefList.class)
        {
            @Override
            public void handle(GS2GC_009_002_RetMailBriefList _ret)
            {

            }
        });
    }

    @Command(comment = "获取邮件系统状态")
    public void stat()
    {
        GC2GS_002_014_ReqMailStatInfo proto = new GC2GS_002_014_ReqMailStatInfo();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_002_014_RetMailStatInfo>(GS2GC_002_014_RetMailStatInfo.class)
        {
            @Override
            public void handle(GS2GC_002_014_RetMailStatInfo _ret)
            {

            }
        });
    }

    @Command(comment = "查看邮件详情(邮件id)")
    public void detail(long mailUid)
    {
        GC2GS_009_003_ReqMailDetail proto = new GC2GS_009_003_ReqMailDetail();
        proto.setMailUid(mailUid);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_009_003_RetMailDetail>(GS2GC_009_003_RetMailDetail.class)
        {
            @Override
            public void handle(GS2GC_009_003_RetMailDetail _ret)
            {

            }
        });
    }

    @Command(comment = "领取邮件物品(邮件id)")
    public void take(long mailUid)
    {
        GC2GS_009_004_ReqTakeMailItems proto = new GC2GS_009_004_ReqTakeMailItems();
        proto.setMailUid(mailUid);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_009_004_RetTakeMailItems>(GS2GC_009_004_RetTakeMailItems.class)
        {
            @Override
            public void handle(GS2GC_009_004_RetTakeMailItems _ret)
            {

            }
        });
    }

    @Command(comment = "领取邮件物品(邮件id,是否锁定)")
    public void lock(long mailUid, boolean isLock)
    {
        GC2GS_009_005_ReqSetMailLockState proto = new GC2GS_009_005_ReqSetMailLockState();
        proto.setMailUid(mailUid);
        proto.setIsLocked(isLock);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_009_005_RetSetMailLockState>(GS2GC_009_005_RetSetMailLockState.class)
        {
            @Override
            public void handle(GS2GC_009_005_RetSetMailLockState _ret)
            {

            }
        });
    }

    @Command(comment = "领取全部")
    public void takeAll()
    {
        GC2GS_009_006_ReqAKeyTakeAll proto = new GC2GS_009_006_ReqAKeyTakeAll();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_009_006_RetAKeyTakeAll>(GS2GC_009_006_RetAKeyTakeAll.class)
        {
            @Override
            public void handle(GS2GC_009_006_RetAKeyTakeAll _ret)
            {

            }
        });
    }

    @Command(comment = "删除邮件(邮件id)")
    public void del(long mailId)
    {
        GC2GS_009_007_ReqDelMail proto = new GC2GS_009_007_ReqDelMail();
        proto.setMailUid(mailId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_009_007_RetDelMail>(GS2GC_009_007_RetDelMail.class)
        {
            @Override
            public void handle(GS2GC_009_007_RetDelMail _ret)
            {

            }
        });
    }

    @Command(comment = "删除全部邮件")
    public void delAll()
    {
        GC2GS_009_008_ReqAkeyDelAll proto = new GC2GS_009_008_ReqAkeyDelAll();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_009_008_RetAKeyDelAll>(GS2GC_009_008_RetAKeyDelAll.class)
        {
            @Override
            public void handle(GS2GC_009_008_RetAKeyDelAll _ret)
            {

            }
        });
    }
}

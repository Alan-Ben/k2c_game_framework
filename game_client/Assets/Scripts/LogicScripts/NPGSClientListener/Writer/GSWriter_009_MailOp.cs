using GC2GS.p009_MailOp;

namespace GOE
{
    public static class GSWriter_009_MailOp
    {

        public static GC2GS_009_002_ReqBriefList make_002_ReqBriefList()
        {
            GC2GS_009_002_ReqBriefList protocol = new GC2GS_009_002_ReqBriefList();
            return protocol;
        }

        public static GC2GS_009_003_ReqMailDetail make_003_ReqMailDetail(long _mailUid)
        {
            GC2GS_009_003_ReqMailDetail protocol = new GC2GS_009_003_ReqMailDetail();
            protocol.setMailUid(_mailUid);
            return protocol;
        }

        public static GC2GS_009_004_ReqTakeMailItems make_004_ReqTakeMailItems(long _mailUid)
        {
            GC2GS_009_004_ReqTakeMailItems protocol = new GC2GS_009_004_ReqTakeMailItems();
            protocol.setMailUid(_mailUid);
            return protocol;
        }

        public static GC2GS_009_005_ReqSetMailLockState make_005_ReqSetMailLockState(long _mailUid,bool _isLocked)
        {
            GC2GS_009_005_ReqSetMailLockState protocol = new GC2GS_009_005_ReqSetMailLockState();
            protocol.setMailUid(_mailUid);
            protocol.setIsLocked(_isLocked);
            return protocol;
        }

        public static GC2GS_009_006_ReqAKeyTakeAll make_006_ReqAKeyTakeAll()
        {
            GC2GS_009_006_ReqAKeyTakeAll protocol = new GC2GS_009_006_ReqAKeyTakeAll();
            return protocol;
        }

        public static GC2GS_009_007_ReqDelMail make_007_ReqDelMail(long _mailUid)
        {
            GC2GS_009_007_ReqDelMail protocol = new GC2GS_009_007_ReqDelMail();
            protocol.setMailUid(_mailUid);
            return protocol;
        }

        public static GC2GS_009_008_ReqAkeyDelAll make_008_ReqAkeyDelAll()
        {
            GC2GS_009_008_ReqAkeyDelAll protocol = new GC2GS_009_008_ReqAkeyDelAll();
            return protocol;
        }
        public static GC2GS_009_009_ReqSetReadOver make_009_ReqSetReadOver(long _mailUid)
        {
            GC2GS_009_009_ReqSetReadOver protocol = new GC2GS_009_009_ReqSetReadOver();
            protocol.setMailUid(_mailUid);
            return protocol;
        }

        public static GC2GS_009_010_ReqMailTitleInfo make_010_ReqMailTitleInfo(long _mailUid)
        {
            GC2GS_009_010_ReqMailTitleInfo protocol = new GC2GS_009_010_ReqMailTitleInfo();
            protocol.addMailUidList(_mailUid);
            return protocol;
        }
    }
}

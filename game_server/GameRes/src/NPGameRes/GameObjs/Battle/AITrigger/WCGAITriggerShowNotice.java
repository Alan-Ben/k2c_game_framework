package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle.WCGBattleSpecialNoticeInfo;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

public class WCGAITriggerShowNotice extends _AWCGBasicAITrigger
{
    private WCGBattleSpecialNoticeInfo _m_lShowNoticeInfo;

    public WCGBattleSpecialNoticeInfo NoticeInfo()
    {
        return _m_lShowNoticeInfo;
    }

    @Override
    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SHOW_NOTICE;
    }

    public static WCGAITriggerShowNotice read(String _infoStr)
    {
        WCGAITriggerShowNotice obj = new WCGAITriggerShowNotice();

//        obj._m_lShowNoticeInfo = new WCGBattleSpecialNoticeInfo();
//        obj._m_lShowNoticeInfo.readIndex(_infoStr,"");

        return obj;
    }
}
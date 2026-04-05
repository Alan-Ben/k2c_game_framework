package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle.WCGBattleSpecialNoticeInfo;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;


public class WCGAITriggerShowSingleNotice extends _AWCGBasicAITrigger
{
    private int _m_eRelationType;
    private WCGBattleSpecialNoticeInfo _m_lShowNoticeInfo;

    public int RelationType()
    {
        return _m_eRelationType;
    }

    public WCGBattleSpecialNoticeInfo NoticeInfo()
    {
        return _m_lShowNoticeInfo;
    }

    @Override
    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SHOW_SINGLE_NOTICE;
    }

    public static WCGAITriggerShowSingleNotice read(String _infoStr)
    {
        WCGAITriggerShowSingleNotice obj = new WCGAITriggerShowSingleNotice();

//        String[] strs = WCGCommonFunc.charSplit(_infoStr, ':',2);
//
//        obj._m_eRelationType = WCGResCommon.readRelationBitValue(strs[0]);
//        obj._m_lShowNoticeInfo = new WCGBattleSpecialNoticeInfo();
//        obj._m_lShowNoticeInfo.readIndex(strs[1],"");

        return obj;
    }
}
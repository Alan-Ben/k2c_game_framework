package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle.WCGBattleSpecialNoticeInfo;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

public class WCGAITriggerShowAddNotice extends _AWCGBasicAITrigger
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

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SHOW_ADD_NOTICE;
    }

    public static WCGAITriggerShowAddNotice read(String _infoStr)
    {
        WCGAITriggerShowAddNotice obj = new WCGAITriggerShowAddNotice();

//        String[] strs = WCGCommonFunc.charSplit(_infoStr, ':');
//
//        obj._m_eRelationType = WCGResCommon.readRelationBitValue(strs[0]);
//        obj._m_lShowNoticeInfo = new WCGBattleSpecialNoticeInfo();
//        obj._m_lShowNoticeInfo.readIndex(strs[1],"");

        return obj;
    }
}
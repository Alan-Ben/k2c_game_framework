package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerSpecialType;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

public class WCGAITriggerSpecial extends _AWCGBasicAITrigger
{
    private EWCGAITriggerSpecialType _m_eType;

    public EWCGAITriggerSpecialType Type()
    {
        return _m_eType;
    }

    @Override
    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SPECIAL;
    }

    public static WCGAITriggerSpecial read(String _infoStr)
    {
        WCGAITriggerSpecial obj = new WCGAITriggerSpecial();

        obj._m_eType = (EWCGAITriggerSpecialType.valueOf(_infoStr.toUpperCase().trim()));
        return obj;
    }
}
package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

public class WCGAITriggerStartOperation extends _AWCGBasicAITrigger
{
    private int _m_iOpId;

    public int OpId()
    {
        return _m_iOpId;
    }

    @Override
    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.START_OP;
    }

    public static WCGAITriggerStartOperation read(String _infoStr)
    {
        WCGAITriggerStartOperation obj = new WCGAITriggerStartOperation();

        obj._m_iOpId = Integer.parseInt(_infoStr.trim());
        return obj;
    }
}
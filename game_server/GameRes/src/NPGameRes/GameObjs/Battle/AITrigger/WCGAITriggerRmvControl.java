package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

//增加阵营资源
public class WCGAITriggerRmvControl extends _AWCGBasicAITrigger
{
    //资源枚举
    private long _m_eCTLType;

    public long CTLType()
    {
        return _m_eCTLType;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.RMV_CONTROL;
    }


    public static WCGAITriggerRmvControl read(String _str)
    {
        WCGAITriggerRmvControl effectObj = new WCGAITriggerRmvControl();
        effectObj._m_eCTLType = WCGResCommon.readControlType(_str);

        return effectObj;
    }
}
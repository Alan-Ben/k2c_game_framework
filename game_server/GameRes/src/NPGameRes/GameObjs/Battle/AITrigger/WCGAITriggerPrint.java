package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

//ai本体触发效果
public class WCGAITriggerPrint extends _AWCGBasicAITrigger
{
    private String _m_sText;

    public String text()
    {
        return _m_sText;
    }

    @Override
    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.PRINT;
    }

    public static WCGAITriggerPrint read(String _infoStr)
    {
        WCGAITriggerPrint obj = new WCGAITriggerPrint();

        //创建对象
        obj._m_sText = _infoStr;

        return obj;
    }
}

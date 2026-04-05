package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;
import WCGCommon.Enum.NPEnum.EWCGActorTipType;


//ai本体触发效果
public class WCGAITriggerShowTip extends _AWCGBasicAITrigger
{
    private EWCGActorTipType _m_eTipType;
    private long _m_lBasicTime;
    private WCGVariableGroupObj _m_vgVariableGroupObj;

    public EWCGActorTipType tipType()
    {
        return _m_eTipType;
    }

    public long basicTime()
    {
        return _m_lBasicTime;
    }

    public WCGVariableGroupObj variableGroupObj()
    {
        return _m_vgVariableGroupObj;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SHOW_TIP;
    }

    public static WCGAITriggerShowTip read(String _infoStr)
    {
        WCGAITriggerShowTip obj = new WCGAITriggerShowTip();

        int splitPos = _infoStr.indexOf(':');
        if (0 >= splitPos)
            return null;

        //读取第一个字段：条件类型
        String typeStr = _infoStr.substring(0, splitPos);
        //读取类型枚举
        obj._m_eTipType = EWCGActorTipType.valueOf(typeStr.toUpperCase().trim());
        //读取时间
        int timePos = _infoStr.indexOf(':', splitPos + 1);
        //读取时间字符串
        if (timePos == -1)
        {
            //读取剩余字符串
            String timeStr = _infoStr.substring(splitPos + 1);
            try
            {
                obj._m_lBasicTime = Long.parseLong(timeStr);
            } catch (Exception e)
            {
                obj._m_lBasicTime = 0;
            }
        } else
        {
            String timeStr = _infoStr.substring(splitPos + 1, timePos);
            try
            {
                obj._m_lBasicTime = Long.parseLong(timeStr);
            } catch (Exception e)
            {
                obj._m_lBasicTime = 0;
            }

            //读取剩余字符串
            String vaStr = _infoStr.substring(timePos + 1);
            //读取高级公式
            obj._m_vgVariableGroupObj = WCGVariableGroupObj.readVariableGroup(vaStr, "show_tip err: ");
        }

        return obj;
    }
}

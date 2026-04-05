package NPGameRes.GameObjs.Battle.AITrigger;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;


/// <summary>
/// AI 触发效果： 开启对应的定时AI，并在指定时间后执行（时间为毫秒）
/// </summary>
public class WCGAITriggeStartTimer extends _AWCGBasicAITrigger
{

    private long _m_lTimerAiId;//定时AI id

    private int _m_iDelayMs;//指定时间（时间为毫秒）

    public long TimerID()
    {
        return _m_lTimerAiId;
    }

    public int DelayMS()
    {
        return _m_iDelayMs;
    }

    @Override
    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.START_TIMER;
    }

    public static WCGAITriggeStartTimer read(String _infoStr)
    {
        WCGAITriggeStartTimer obj = new WCGAITriggeStartTimer();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');

        if (strs.length < 2)
        {
            ALServerLog.Error("ai START_TIMER配置错误 正确配置: 枚举:对应的定时AI:指定时间");
            return null;
        }
        try
        {
            obj._m_lTimerAiId = Integer.parseInt(strs[0].trim());
            obj._m_iDelayMs = Integer.parseInt(strs[1].trim());
            return obj;
        } catch (Exception e)
        {
            ALServerLog.Error("ai START_TIMER配置错误 正确配置: 枚举:对应的定时AI:指定时间");
            return null;
        }
    }

}

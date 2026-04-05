package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.EWCGTeamTriggerType;

public class WCGTeamTriggerCompleteBattleTask extends _AWCGBasicTeamTrigger
{
    private long _m_lBattleTaskId;//任务Id

    public long battleTaskId()
    {
        return _m_lBattleTaskId;
    }

    public EWCGTeamTriggerType triggerType()
    {
        return EWCGTeamTriggerType.COMPLETE_BATTLE_TASK;
    }

    public static WCGTeamTriggerCompleteBattleTask read(String _str)
    {
        if (null == _str || _str.isEmpty())
        {
            CommLog.error("效果配置错误 - WCGTeamTriggerCompleteBattleTask example: enum:taskId， Error Str: " + _str);
            return null;
        }

        try
        {
            WCGTeamTriggerCompleteBattleTask effectObj = new WCGTeamTriggerCompleteBattleTask();
            effectObj._m_lBattleTaskId = Long.parseLong(_str);
            return effectObj;
        } catch (Exception ex)
        {
            CommLog.error("效果配置错误 - WCGTeamTriggerCompleteBattleTask example: enum:taskId， Error Str: " + _str);
            return null;
        }
    }
}
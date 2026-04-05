package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

/// <summary>
/// AI 触发效果：设置指定的队伍输
/// </summary>
public class WCGAITriggerTeamLose extends _AWCGBasicAITrigger
{

    private int _m_iTeamId;

    public int TeamID()
    {
        return _m_iTeamId;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.TEAM_LOSE;
    }

    public static WCGAITriggerTeamLose read(String _infoStr)
    {
        WCGAITriggerTeamLose obj = new WCGAITriggerTeamLose();
        try
        {
            obj._m_iTeamId = Integer.parseInt(_infoStr.trim());
            return obj;
        } catch (Exception e)
        {
            CommLog.error("ai team_lose配置错误,正确配置:teamId", e);
            return null;
        }
    }
}

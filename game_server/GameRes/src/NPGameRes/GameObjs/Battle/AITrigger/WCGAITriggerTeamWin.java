package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;


/// <summary>
/// AI 触发效果：设置指定的队伍赢
/// </summary>
public class WCGAITriggerTeamWin extends _AWCGBasicAITrigger
{

    private int _m_iTeamId;

    public int TeamID()
    {
        return _m_iTeamId;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.TEAM_WIN;
    }

    public static WCGAITriggerTeamWin read(String _infoStr)
    {
        WCGAITriggerTeamWin obj = new WCGAITriggerTeamWin();
        try
        {
            obj._m_iTeamId = Integer.parseInt(_infoStr.trim());
            return obj;
        } catch (Exception e)
        {
            CommLog.error("ai team_win配置错误,正确配置:teamId", e);
            return null;
        }
    }
}
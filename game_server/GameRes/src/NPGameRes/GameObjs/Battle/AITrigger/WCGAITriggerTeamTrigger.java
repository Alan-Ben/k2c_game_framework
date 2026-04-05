package NPGameRes.GameObjs.Battle.AITrigger;

import NPGameRes.GameObjs.Battle.WCGTeamTriggerObj;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

import java.util.List;

/// <summary>
/// AI 触发效果：设置指定的队伍赢
/// </summary>
public class WCGAITriggerTeamTrigger extends _AWCGBasicAITrigger
{

    private List<WCGTeamTriggerObj> _m_ttTeamTriggerInfo;

    public List<WCGTeamTriggerObj> teamTrigger()
    {
        return _m_ttTeamTriggerInfo;
    }

    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.TEAM_TRIGGER;
    }

    public static WCGAITriggerTeamTrigger read(String _infoStr)
    {
        WCGAITriggerTeamTrigger obj = new WCGAITriggerTeamTrigger();
        obj._m_ttTeamTriggerInfo = WCGTeamTriggerObj.readTriggerList(_infoStr);

        return obj;
    }
}

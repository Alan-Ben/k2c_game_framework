package NPGameRes.GameObjs.Battle.Condition;

import NPGameRes.GameObjs.Battle.WCGTeamConditionGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionTeamCond extends _AWCGBasicSingleCondition
{
    private WCGTeamConditionGroupObj _m_aABConidtionGroupObj;

    public WCGTeamConditionGroupObj TeamConidtionGroupObj()
    {
        return _m_aABConidtionGroupObj;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.TEAM_COND;
    }

    public static WCGConditionTeamCond readCond(String _str)
    {
        WCGConditionTeamCond cond = new WCGConditionTeamCond();

        cond._m_aABConidtionGroupObj = WCGTeamConditionGroupObj.readConditionGroupList(_str, "");

        return cond;
    }
}

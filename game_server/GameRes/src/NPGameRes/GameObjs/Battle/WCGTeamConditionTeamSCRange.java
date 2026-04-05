package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.EWCGTeamConditionType;

//队伍中符合单体条件的Actor数量范围
public class WCGTeamConditionTeamSCRange extends _AWCGBasicTeamCondition
{
    private WCGSingleConditionGroupObj _m_singleConditionGroupObj;
    private WCGIntRange _m_intRange;

    public WCGSingleConditionGroupObj ConditionGroupObj()
    {
        return _m_singleConditionGroupObj;
    }

    public WCGIntRange Range()
    {
        return _m_intRange;
    }

    public EWCGTeamConditionType conditionType()
    {
        return EWCGTeamConditionType.TEAM_S_R;
    }

    public static WCGTeamConditionTeamSCRange readCond(String _str)
    {
        WCGTeamConditionTeamSCRange cond = new WCGTeamConditionTeamSCRange();

        try
        {
            int pos = _str.indexOf(':');
            String strMin = _str.substring(0, pos);
            _str = _str.substring(pos + 1);
            pos = _str.indexOf(':');
            String strMax = _str.substring(0, pos);
            _str = _str.substring(pos + 1);
            cond._m_singleConditionGroupObj = WCGSingleConditionGroupObj.readConditionGroupList(_str, "TEAM_S_R");
            cond._m_intRange = new WCGIntRange(strMin, strMax);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - TEAM_A_T_C example: condition-min-max Error Str: " + _str, e);
            return null;
        }
    }
}
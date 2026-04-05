package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.WCGResCommon;
import WCGCommon.Enum.NPEnum.EWCGTeamConditionType;

//队伍中符合单体条件的Actor数量范围
public class WCGTeamConditionAllActorSCRange extends _AWCGBasicTeamCondition
{
    private WCGSingleConditionGroupObj _m_singleConditionGroupObj;
    private int _m_iRelationValue;
    private WCGIntRange _m_intRange;

    public WCGSingleConditionGroupObj ConditionGroupObj()
    {
        return _m_singleConditionGroupObj;
    }

    public int relationValue()
    {
        return _m_iRelationValue;
    }

    public WCGIntRange Range()
    {
        return _m_intRange;
    }

    public EWCGTeamConditionType conditionType()
    {
        return EWCGTeamConditionType.ALL_ACT_S_R;
    }

    public static WCGTeamConditionAllActorSCRange readCond(String _str)
    {
        WCGTeamConditionAllActorSCRange cond = new WCGTeamConditionAllActorSCRange();

        try
        {
            int pos = _str.indexOf(':');
            String strMin = _str.substring(0, pos);
            _str = _str.substring(pos + 1);

            pos = _str.indexOf(':');
            String strMax = _str.substring(0, pos);
            _str = _str.substring(pos + 1);

            pos = _str.indexOf(':');
            String relationValueStr = _str.substring(0, pos);
            _str = _str.substring(pos + 1);

            cond._m_singleConditionGroupObj = WCGSingleConditionGroupObj.readConditionGroupList(_str, "teamAllACTSR");
            cond._m_iRelationValue = WCGResCommon.readRelationBitValue(relationValueStr);
            cond._m_intRange = new WCGIntRange(strMin, strMax);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - ALL_ACT_S_R example: ALL_ACT_S_R:min:max:relation:condition Error Str: " + _str);
            return null;
        }
    }
}
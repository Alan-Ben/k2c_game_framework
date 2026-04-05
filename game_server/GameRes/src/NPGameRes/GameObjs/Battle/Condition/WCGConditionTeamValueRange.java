package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGFloatRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;
import WCGCommon.Enum.NPEnum.EWCGTeamValueType;

public class WCGConditionTeamValueRange extends _AWCGBasicSingleCondition
{
    private EWCGTeamValueType _m_eValueType;

    private WCGFloatRange _m_fRange;

    public EWCGTeamValueType ValueType()
    {
        return _m_eValueType;
    }

    public WCGFloatRange Range()
    {
        return _m_fRange;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.TEAM_V_R;
    }

    public static WCGConditionTeamValueRange readCond(String _str)
    {
        WCGConditionTeamValueRange cond = new WCGConditionTeamValueRange();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        //逐个判断
        if (strs.length < 3)
        {
            CommLog.error("条件配置错误 - team_pro_range example: enum:property:min:max Error Str: " + _str);
            return null;
        }

        try
        {
            cond._m_eValueType = EWCGTeamValueType.valueOf(strs[0].toUpperCase().trim());
            cond._m_fRange = new WCGFloatRange(strs[1], strs[2]);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - team_pro_range example: enum:property:min:max Error Str: " + _str);
            return null;
        }
    }
}

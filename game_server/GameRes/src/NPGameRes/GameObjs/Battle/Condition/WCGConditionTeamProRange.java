package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGFloatRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.ENPTeamPropertyType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionTeamProRange extends _AWCGBasicSingleCondition
{
    private ENPTeamPropertyType _m_ePropertyType;

    private WCGFloatRange _m_lRange;

    public ENPTeamPropertyType PropertyType()
    {
        return _m_ePropertyType;
    }

    public WCGFloatRange Range()
    {
        return _m_lRange;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.TEAM_P_R;
    }

    public static WCGConditionTeamProRange readCond(String _str)
    {
        WCGConditionTeamProRange cond = new WCGConditionTeamProRange();

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
            cond._m_ePropertyType = ENPTeamPropertyType.valueOf(strs[0].toUpperCase().trim());
            cond._m_lRange = new WCGFloatRange(strs[1], strs[2]);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - team_pro_range example: enum:property:min:max Error Str: " + _str);
            return null;
        }
    }
}

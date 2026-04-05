package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.ENPTeamPropertyType;
import WCGCommon.Enum.NPEnum.EWCGTeamConditionType;


public class WCGTeamConditionTeamProRange extends _AWCGBasicTeamCondition
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

    public EWCGTeamConditionType conditionType()
    {
        return EWCGTeamConditionType.TEAM_P_R;
    }

    public static WCGTeamConditionTeamProRange readCond(String _str)
    {
        WCGTeamConditionTeamProRange cond = new WCGTeamConditionTeamProRange();

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

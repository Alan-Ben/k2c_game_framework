package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.EWCGTeamValueType;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

public class WCGTeamVariableAvgTeamValue extends _AWCGBasicTeamVariableObj
{

    /**
     * 属性类型枚举
     */
    private EWCGTeamValueType _m_eValueType;

    public EWCGTeamValueType ValueType()
    {
        return _m_eValueType;
    }

    protected WCGTeamVariableAvgTeamValue()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.AVG_TEAM_VALUE;
    }


    public static WCGTeamVariableAvgTeamValue readVariable(String _str)
    {
        WCGTeamVariableAvgTeamValue variableObj = new WCGTeamVariableAvgTeamValue();

        try
        {
            variableObj._m_eValueType = EWCGTeamValueType.valueOf(_str.toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Value example: enum:value_type Error Str: " + _str);
            return null;
        }
    }
}

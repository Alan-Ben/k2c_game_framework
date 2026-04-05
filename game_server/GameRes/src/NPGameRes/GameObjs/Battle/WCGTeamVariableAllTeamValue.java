package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.EWCGTeamValueType;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

/// <summary>
/// 高级公式变量-TEAM_PRO - 队伍属性  格式如下：枚举@对象类型@队伍值类型
/// </summary>
public class WCGTeamVariableAllTeamValue extends _AWCGBasicTeamVariableObj
{
    /**
     * 属性类型枚举
     */
    private EWCGTeamValueType _m_eValueType;

    public EWCGTeamValueType ValueType()
    {
        return _m_eValueType;
    }

    protected WCGTeamVariableAllTeamValue()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.ALL_TEAM_VALUE;
    }


    public static WCGTeamVariableAllTeamValue readVariable(String _str)
    {
        WCGTeamVariableAllTeamValue variableObj = new WCGTeamVariableAllTeamValue();

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

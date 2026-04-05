package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.EWCGTeamValueType;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;


/// <summary>
/// 高级公式变量-TEAM_PRO - 队伍属性  格式如下：枚举@对象类型@队伍值类型
/// </summary>
public class WCGTeamVariableAllCampValue extends _AWCGBasicTeamVariableObj
{

    /**
     * 属性类型枚举
     */
    private EWCGTeamValueType _m_eValueType;

    public EWCGTeamValueType ValueType()
    {
        return _m_eValueType;
    }

    protected WCGTeamVariableAllCampValue()
    {
    }

    /******************
     * 获取条件类型
     */
    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.ALL_CAMP_VALUE;
    }


    public static WCGTeamVariableAllCampValue readVariable(String _str)
    {
        WCGTeamVariableAllCampValue variableObj = new WCGTeamVariableAllCampValue();

        try
        {
            variableObj._m_eValueType = EWCGTeamValueType.valueOf(_str.trim().toUpperCase());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Value example: enum:value_type Error Str: " + _str);
            return null;
        }
    }
}

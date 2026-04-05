package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.ENPTeamPropertyType;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

/// <summary>
/// 高级公式变量-TEAM_VALUE - 队伍值    格式如下：枚举@队伍属性类型
/// </summary>
public class WCGTeamVariableTeamPro extends _AWCGBasicTeamVariableObj
{
    /**
     * 属性类型枚举
     */
    private ENPTeamPropertyType _m_ePropertyType;

    public ENPTeamPropertyType PropertyType()
    {
        return _m_ePropertyType;
    }

    protected WCGTeamVariableTeamPro()
    {
        _m_ePropertyType = ENPTeamPropertyType.NONE;
    }

    /******************
     * 获取条件类型
     */
    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.TEAM_PRO;
    }

    public static WCGTeamVariableTeamPro readVariable(String _str)
    {
        WCGTeamVariableTeamPro variableObj = new WCGTeamVariableTeamPro();
        try
        {
            variableObj._m_ePropertyType = ENPTeamPropertyType.valueOf(_str.toUpperCase().trim());
            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Pro example: enum:target:property Error Str: " + _str);
            return null;
        }
    }

}

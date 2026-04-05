package NPGameRes.GameObjs.Battle.Variable;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.ENPTeamPropertyType;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

/// <summary>
/// 高级公式变量-TEAM_VALUE - 队伍值    格式如下：枚举@对象类型@队伍属性类型
/// </summary>
public class WCGVariableTeamPro extends _AWCGBasicVariableObj
{
    /**
     * 对象类型
     */
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * 属性类型枚举
     */
    private ENPTeamPropertyType _m_ePropertyType;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public ENPTeamPropertyType PropertyType()
    {
        return _m_ePropertyType;
    }

    protected WCGVariableTeamPro()
    {
        _m_eTargetType = EWCGEffectTargetType.NONE;
        _m_ePropertyType = ENPTeamPropertyType.NONE;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public WCGCommon.Enum.NPEnum.EWCGVariableType variableType()
    {
        return EWCGVariableType.TEAM_PRO;
    }

    public static WCGVariableTeamPro readVariable(String _str)
    {
        WCGVariableTeamPro variableObj = new WCGVariableTeamPro();
        // 解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        // 逐个判断
        if (strs.length < 2)
        {
            ALServerLog.Error("Error Format for Variable - Pro example: enum:target:property Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            variableObj._m_ePropertyType = ENPTeamPropertyType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("Error Format for Variable - Pro example: enum:target:property Error Str: " + _str);
            return null;
        }
    }

}

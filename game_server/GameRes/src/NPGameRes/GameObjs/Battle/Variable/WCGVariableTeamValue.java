package NPGameRes.GameObjs.Battle.Variable;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGTeamValueType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

/// <summary>
/// 高级公式变量-TEAM_PRO - 队伍属性  格式如下：枚举@对象类型@队伍值类型
/// </summary>
public class WCGVariableTeamValue extends _AWCGBasicVariableObj
{
    /**
     * 对象类型
     */
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * 属性类型枚举
     */
    private EWCGTeamValueType _m_eValueType;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public EWCGTeamValueType ValueType()
    {
        return _m_eValueType;
    }

    protected WCGVariableTeamValue()
    {
        _m_eTargetType = EWCGEffectTargetType.NONE;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public WCGCommon.Enum.NPEnum.EWCGVariableType variableType()
    {
        return EWCGVariableType.TEAM_VALUE;
    }

    public static WCGVariableTeamValue readVariable(String _str)
    {
        WCGVariableTeamValue variableObj = new WCGVariableTeamValue();

        // 解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        // 逐个判断
        if (strs.length < 2)
        {
            ALServerLog.Error("Error Format for Variable - Value example: enum:target:value_type Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            variableObj._m_eValueType = EWCGTeamValueType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("Error Format for Variable - Value example: enum:target:value_type Error Str: " + _str);
            return null;
        }
    }
}

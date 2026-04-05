package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGCampPropertyType;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

/// <summary>
/// 高级公式变量-TEAM_PRO - 队伍属性  格式如下：枚举@对象类型@队伍值类型
/// </summary>
public class WCGTeamVariableCampValue extends _AWCGBasicTeamVariableObj
{

    /**
     * 对象类型
     */
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * 属性类型枚举
     */
    private EWCGCampPropertyType _m_eValueType;

    private int _m_iCampId = 0;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public EWCGCampPropertyType ValueType()
    {
        return _m_eValueType;
    }

    public int CampID()
    {
        return _m_iCampId;
    }


    protected WCGTeamVariableCampValue()
    {
        _m_eTargetType = EWCGEffectTargetType.NONE;
    }

    /******************
     * 获取条件类型
     */
    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.CAMP_PRO;
    }


    public static WCGTeamVariableCampValue readVariable(String _str)
    {
        WCGTeamVariableCampValue variableObj = new WCGTeamVariableCampValue();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            CommLog.error("高级公式配置错误 - Value example: enum@target@value_type Error Str: " + _str);
            return null;
        }

        try
        {
            try
            {
                variableObj._m_iCampId = Integer.parseInt(strs[0].trim());
            } catch (Exception e)
            {
                variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            }
            variableObj._m_eValueType = EWCGCampPropertyType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Value example: enum@target@value_type Error Str: " + _str, e);
            return null;
        }
    }
}
package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGCampPropertyType;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;


/// <summary>
/// 高级公式变量-TEAM_PRO - 队伍属性  格式如下：枚举@对象类型@队伍值类型
/// </summary>
public class WCGVariableCampValue extends _AWCGBasicVariableObj
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


    protected WCGVariableCampValue()
    {
        _m_eTargetType = EWCGEffectTargetType.NONE;
    }

    /******************
     * 获取条件类型
     */
    public EWCGVariableType variableType()
    {
        return EWCGVariableType.CAMP_PRO;
    }


    public static WCGVariableCampValue readVariable(String _str)
    {
        WCGVariableCampValue variableObj = new WCGVariableCampValue();

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
            variableObj._m_iCampId = 0;
            try
            {
                variableObj._m_iCampId = Integer.parseInt(strs[0].trim());
            } catch (Exception e)
            {
                variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());

            }
            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Value example: enum@target@value_type Error Str: " + _str, e);
            return null;
        }
    }
}
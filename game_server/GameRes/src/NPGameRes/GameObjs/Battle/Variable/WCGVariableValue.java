package NPGameRes.GameObjs.Battle.Variable;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGValueType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

public class WCGVariableValue extends _AWCGBasicVariableObj
{
    /**
     * 对象类型
     */
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * 属性类型枚举
     */
    private EWCGValueType _m_eValueType;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public EWCGValueType ValueType()
    {
        return _m_eValueType;
    }

    protected WCGVariableValue()
    {
        _m_eTargetType = EWCGEffectTargetType.NONE;
        _m_eValueType = EWCGValueType.NONE;
    }

    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    @Override
    public WCGCommon.Enum.NPEnum.EWCGVariableType variableType()
    {
        return EWCGVariableType.VALUE;
    }


    public static WCGVariableValue readVariable(String _str)
    {
        WCGVariableValue variableObj = new WCGVariableValue();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            ALServerLog.Error("Error Format for Variable - Value example: enum:target:value_type Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            variableObj._m_eValueType = EWCGValueType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("Error Format for Variable - Value example: enum:target:value_type Error Str: " + _str);
            return null;
        }
    }


}
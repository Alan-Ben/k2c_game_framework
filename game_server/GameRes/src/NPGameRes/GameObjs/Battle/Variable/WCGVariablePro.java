package NPGameRes.GameObjs.Battle.Variable;

import ALServerLog.ALServerLog;
import NPCommon.Enum.NPCommonEnum.ENPPropertyType;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

public class WCGVariablePro extends _AWCGBasicVariableObj
{
    /**
     * 对象类型
     */
    private EWCGEffectTargetType _m_eTargetType;
    /**
     * 属性类型枚举
     */
    private ENPPropertyType _m_ePropertyType;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public ENPPropertyType PropertyType()
    {
        return _m_ePropertyType;
    }

    protected WCGVariablePro()
    {
        _m_eTargetType = EWCGEffectTargetType.NONE;
        _m_ePropertyType = ENPPropertyType.NONE;
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
        return EWCGVariableType.PRO;
    }


    public static WCGVariablePro readVariable(String _str)
    {
        WCGVariablePro variableObj = new WCGVariablePro();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, '@');
        //逐个判断
        if (strs.length < 2)
        {
            ALServerLog.Error("Error Format for Variable - Pro example: enum:target:property Error Str: " + _str);
            return null;
        }

        try
        {
            variableObj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            variableObj._m_ePropertyType = ENPPropertyType.valueOf(strs[1].toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("Error Format for Variable - Pro example: enum:target:property Error Str: " + _str);
            return null;
        }
    }


}
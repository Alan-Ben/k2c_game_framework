package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;


public class WCGVariableOwnerValue extends _AWCGBasicVariableObj
{
    /**
     * 对象类型1
     */
    private EWCGEffectTargetType _m_eOwnerTargetType;
    //参数信息
    private WCGVariableGroupObj _m_vVariableInfo;

    public EWCGEffectTargetType ownerTargetType()
    {
        return _m_eOwnerTargetType;
    }

    public WCGVariableGroupObj variableInfo()
    {
        return _m_vVariableInfo;
    }

    protected WCGVariableOwnerValue()
    {
        _m_eOwnerTargetType = EWCGEffectTargetType.NONE;
        _m_vVariableInfo = null;
    }

    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    public EWCGVariableType variableType()
    {
        return EWCGVariableType.OWNER_V;
    }


    @SuppressWarnings("unused")
    private static char[] _g_sparr = new char[]{'@'};

    public static WCGVariableOwnerValue readVariable(String _str)
    {
        WCGVariableOwnerValue variableObj = new WCGVariableOwnerValue();

        //解析字符串
        int index = _str.indexOf("@");
        //逐个判断
        if (index <= -1)
        {
            CommLog.error("高级公式配置错误 - Pro example: enum:target:variable Error Str: " + _str);
            return null;
        }

        try
        {
            String targetTypeStr = _str.substring(0, index);
            variableObj._m_eOwnerTargetType = EWCGEffectTargetType.valueOf(targetTypeStr.toUpperCase().trim());

            String variableStr = _str.substring(index + 1);
            variableObj._m_vVariableInfo = WCGVariableGroupObj.readVariableGroup(variableStr, "owV");
            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - Pro example: enum:target:variable Error Str: " + _str);
            return null;
        }
    }
}

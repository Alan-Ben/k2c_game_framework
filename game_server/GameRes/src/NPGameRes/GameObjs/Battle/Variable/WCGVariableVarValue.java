package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGEffectVariableType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;


//指定类型的队伍资源
public class WCGVariableVarValue extends _AWCGBasicVariableObj
{


    private EWCGEffectVariableType _m_eType;

    public EWCGEffectVariableType vType()
    {
        return _m_eType;
    }

    public EWCGVariableType variableType()
    {
        return EWCGVariableType.VAR_VALUE;
    }


    public static WCGVariableVarValue readVariable(String _str)
    {
        WCGVariableVarValue variableObj = new WCGVariableVarValue();

        try
        {
            variableObj._m_eType = EWCGEffectVariableType.valueOf(_str.toUpperCase().trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - VAR_VALUE example: enum:varType Error Str: " + _str, e);
            return null;
        }
    }

}
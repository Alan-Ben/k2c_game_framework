package NPGameRes.GameObjs.Battle.Variable;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGVariableType;


public class WCGVariableBattleValue extends _AWCGBasicVariableObj
{
    /**
     * 具体上限值
     */
    private int _m_iIndex;

    public int index()
    {
        return _m_iIndex;
    }

    protected WCGVariableBattleValue()
    {
        _m_iIndex = 0;
    }

    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    public EWCGVariableType variableType()
    {
        return EWCGVariableType.BATTLE_VALUE;
    }


    public static WCGVariableBattleValue readVariable(String _str)
    {
        WCGVariableBattleValue variableObj = new WCGVariableBattleValue();

        try
        {
            variableObj._m_iIndex = Integer.parseInt(_str.trim());

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - BATTLE_VALUE  example: enum:index Error Str: " + _str, e);
            return null;
        }
    }
}
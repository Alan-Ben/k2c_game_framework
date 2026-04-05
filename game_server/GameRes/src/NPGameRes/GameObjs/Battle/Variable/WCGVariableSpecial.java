package NPGameRes.GameObjs.Battle.Variable;

import ALServerLog.ALServerLog;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGSpecialVariableType;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

/// /// <summary>
/// 高级公式变量-SPECIAL - 特殊变量类型   格式如下：枚举@特殊变量类型
/// </summary>
public class WCGVariableSpecial extends _AWCGBasicVariableObj
{

    /**
     * 特殊参数类型
     */
    private EWCGSpecialVariableType _m_eSpecialVariableType;

    public EWCGSpecialVariableType SpecialVariableType()
    {
        return _m_eSpecialVariableType;
    }

    protected WCGVariableSpecial()
    {
        _m_eSpecialVariableType = EWCGSpecialVariableType.NONE;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public WCGCommon.Enum.NPEnum.EWCGVariableType variableType()
    {
        return EWCGVariableType.SPECIAL;
    }

    public static WCGVariableSpecial readVariable(String _str)
    {
        WCGVariableSpecial variableObj = new WCGVariableSpecial();

        try
        {
            variableObj._m_eSpecialVariableType = EWCGSpecialVariableType.valueOf(_str.toUpperCase().trim());
            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("Error Format for Variable - Value example: enum@EWCGSpecialVariableType Error Str: "
                    + _str);
            return null;
        }
    }

}

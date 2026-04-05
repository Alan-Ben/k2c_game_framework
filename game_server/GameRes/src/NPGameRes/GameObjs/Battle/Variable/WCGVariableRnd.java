package NPGameRes.GameObjs.Battle.Variable;

import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

public class WCGVariableRnd extends _AWCGBasicVariableObj
{
    /**
     * 具体上限值
     */
    private int _m_iMaxValue;

    public int maxValue()
    {
        return _m_iMaxValue;
    }

    protected WCGVariableRnd()
    {
        _m_iMaxValue = 0;
    }

    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    @Override
    public EWCGVariableType variableType()
    {
        return EWCGVariableType.RND;
    }


    public static WCGVariableRnd readVariable(String _str)
    {
        WCGVariableRnd variableObj = new WCGVariableRnd();

        //解析字符串
        variableObj._m_iMaxValue = Integer.parseInt(_str.trim());

        return variableObj;
    }
}

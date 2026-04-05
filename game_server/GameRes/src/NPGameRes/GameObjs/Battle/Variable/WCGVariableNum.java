package NPGameRes.GameObjs.Battle.Variable;

import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

public class WCGVariableNum extends _AWCGBasicVariableObj
{
    /**
     * 具体数字
     */
    private long _m_value;

    public long Value()
    {
        return _m_value;
    }

    protected WCGVariableNum()
    {
        _m_value = 0;
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
        return EWCGVariableType.NUM;
    }

    public static WCGVariableNum readVariable(String _str)
    {
        WCGVariableNum variableObj = new WCGVariableNum();

        //解析字符串
        variableObj._m_value = Long.parseLong(_str.trim());
        return variableObj;
    }


}
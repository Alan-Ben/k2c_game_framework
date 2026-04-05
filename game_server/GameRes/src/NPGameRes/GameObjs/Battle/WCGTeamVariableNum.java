package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

public class WCGTeamVariableNum extends _AWCGBasicTeamVariableObj
{
    /**
     * 具体数字
     */
    private int _m_value;

    public int Value()
    {
        return _m_value;
    }

    protected WCGTeamVariableNum()
    {
        _m_value = 0;
    }

    /******************
     * 获取条件类型
     */
    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.NUM;
    }


    public static WCGTeamVariableNum readVariable(String _str)
    {
        WCGTeamVariableNum variableObj = new WCGTeamVariableNum();

        //解析字符串
        variableObj._m_value = Integer.parseInt(_str.trim());

        return variableObj;
    }
}

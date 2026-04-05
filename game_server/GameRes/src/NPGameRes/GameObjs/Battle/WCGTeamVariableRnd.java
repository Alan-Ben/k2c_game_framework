package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;


public class WCGTeamVariableRnd extends _AWCGBasicTeamVariableObj
{
    /**
     * 具体上限值
     */
    private int _m_iMaxValue;

    public int maxValue()
    {
        return _m_iMaxValue;
    }

    protected WCGTeamVariableRnd()
    {
        _m_iMaxValue = 0;
    }

    /******************
     * 获取条件类型
     */
    public EWCGTeamVariableType variableType()
    {
        return EWCGTeamVariableType.RND;
    }


    public static WCGTeamVariableRnd readVariable(String _str)
    {
        WCGTeamVariableRnd variableObj = new WCGTeamVariableRnd();

        //解析字符串
        variableObj._m_iMaxValue = Integer.parseInt(_str.trim());

        return variableObj;
    }
}
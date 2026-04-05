package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_S_RND extends _ANPBasicPlayerVariableObj
{
    /**
     * 具体上限值
     */
    private int _m_iMaxValue;

    public int maxValue()
    {
        return _m_iMaxValue;
    }

    protected NPPlayerVariable_S_RND()
    {
        _m_iMaxValue = 0;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.S_RND;
    }


    public static NPPlayerVariable_S_RND readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_S_RND variableObj = new NPPlayerVariable_S_RND();

        String maxVS = _reader.readItem('@');
        if (null == maxVS)
        {
            ALServerLog.Error("NPPlayerVariable_S_RND format err: " + _reader.getSrcString());
            return null;
        }

        //解析字符串
        variableObj._m_iMaxValue = Integer.parseInt(maxVS);

        return variableObj;
    }
}

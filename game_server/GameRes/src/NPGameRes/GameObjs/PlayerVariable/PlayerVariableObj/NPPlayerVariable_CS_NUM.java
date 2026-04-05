package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_NUM extends _ANPBasicPlayerVariableObj
{
    /**
     * 具体数字
     */
    private int _m_value;

    public int Value()
    {
        return _m_value;
    }

    protected NPPlayerVariable_CS_NUM()
    {
        _m_value = 0;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_NUM;
    }

    public static NPPlayerVariable_CS_NUM readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_NUM variableObj = new NPPlayerVariable_CS_NUM();

        String valueS = _reader.readItem('@');
        if (null == valueS)
        {
            ALServerLog.Error("NPPlayerVariable_CS_NUM format err: " + _reader.getSrcString());
            return null;
        }

        //解析字符串
        variableObj._m_value = Integer.parseInt(valueS);

        return variableObj;
    }
}

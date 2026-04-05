package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerValueType;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_VALUE extends _ANPBasicPlayerVariableObj
{
    /**
     * 属性类型枚举
     */
    private ENPPlayerValueType _m_eValueType;

    public ENPPlayerValueType ValueType()
    {
        return _m_eValueType;
    }

    protected NPPlayerVariable_CS_VALUE()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_VALUE;
    }


    public static NPPlayerVariable_CS_VALUE readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_VALUE variableObj = new NPPlayerVariable_CS_VALUE();

        try
        {
            String valueTS = _reader.readItem('@');

            variableObj._m_eValueType = ENPPlayerValueType.valueOf(valueTS.toUpperCase());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式配置错误 - Value example: enum:value_type Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

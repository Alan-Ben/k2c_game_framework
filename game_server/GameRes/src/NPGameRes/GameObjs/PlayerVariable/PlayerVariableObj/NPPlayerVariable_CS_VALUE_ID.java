package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerIdValueType;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_VALUE_ID extends _ANPBasicPlayerVariableObj
{
    /**
     * 类型枚举
     */
    private ENPPlayerIdValueType _m_eIdValueType;
    private long _m_lId;

    public ENPPlayerIdValueType getValueType()
    {
        return _m_eIdValueType;
    }

    public long getId()
    {
        return _m_lId;
    }

    protected NPPlayerVariable_CS_VALUE_ID()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_VALUE_ID;
    }


    public static NPPlayerVariable_CS_VALUE_ID readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_VALUE_ID variableObj = new NPPlayerVariable_CS_VALUE_ID();

        //解析字符串
        String typeS = _reader.readItem('@');
        String idS = _reader.readItem('@');
        //逐个判断
        if (null == typeS || null == idS)
        {
            CommLog.error("高级公式配置错误 - CS_VALUE_ID example: enum:idType:id Error Str: " + _reader.getSrcString());
            return null;
        }

        try
        {
            variableObj._m_eIdValueType = ENPPlayerIdValueType.valueOf(typeS.toUpperCase());
            variableObj._m_lId = Long.parseLong(idS);

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - CS_VALUE_ID example: enum:idType:id Error Str: " + _reader.getSrcString(), e);
            return null;
        }
    }
}

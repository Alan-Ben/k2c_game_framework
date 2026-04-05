package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_PROPERTY extends _ANPBasicPlayerVariableObj
{
    /**
     * 属性类型枚举
     */
    private ENPPlayerPropertyType _m_ePropertyType;

    public ENPPlayerPropertyType PropertyType()
    {
        return _m_ePropertyType;
    }

    protected NPPlayerVariable_CS_PROPERTY()
    {
        _m_ePropertyType = ENPPlayerPropertyType.NONE;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_PROPERTY;
    }

    public static NPPlayerVariable_CS_PROPERTY readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_PROPERTY variableObj = new NPPlayerVariable_CS_PROPERTY();
        try
        {
            String propertyTypeS = _reader.readItem('@');

            variableObj._m_ePropertyType = ENPPlayerPropertyType.valueOf(propertyTypeS.toUpperCase());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式配置错误 - Pro example: enum:property Error Str: " + _reader.getSrcString());
            return null;
        }
    }

}

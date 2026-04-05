package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_PARAM extends _ANPBasicPlayerVariableObj
{
    /**
     * 类型枚举
     */
    private ENPPlayerParam _m_eParamType;

    public ENPPlayerParam getParamType()
    {
        return _m_eParamType;
    }

    protected NPPlayerVariable_CS_PARAM()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_PARAM;
    }


    public static NPPlayerVariable_CS_PARAM readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_PARAM variableObj = new NPPlayerVariable_CS_PARAM();

        try
        {
            String paramS = _reader.readItem('@');

            variableObj._m_eParamType = ENPPlayerParam.valueOf(paramS.toUpperCase());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式配置错误 - Value example: enum:param_type Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

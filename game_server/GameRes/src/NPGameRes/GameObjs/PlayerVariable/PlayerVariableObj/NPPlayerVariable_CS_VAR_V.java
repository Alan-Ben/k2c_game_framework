package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPEnum.ENPPlayerVariableVarType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_VAR_V extends _ANPBasicPlayerVariableObj
{
    private ENPPlayerVariableVarType _m_eType;

    public ENPPlayerVariableVarType vType()
    {
        return _m_eType;
    }

    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_VAR_V;
    }


    public static NPPlayerVariable_CS_VAR_V readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_VAR_V variableObj = new NPPlayerVariable_CS_VAR_V();

        try
        {
            String typeS = _reader.readItem('@');

            variableObj._m_eType = ENPPlayerVariableVarType.valueOf(typeS.toUpperCase());

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式配置错误 - VAR_VALUE example: enum:varType Error Str: " + _reader.getSrcString());
            return null;
        }
    }

}

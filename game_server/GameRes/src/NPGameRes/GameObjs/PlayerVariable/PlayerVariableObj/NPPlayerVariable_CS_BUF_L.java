package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_BUF_L extends _ANPBasicPlayerVariableObj
{
    private long _m_lBufId;//buffId

    public long bufId()
    {
        return _m_lBufId;
    }

    protected NPPlayerVariable_CS_BUF_L()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_BUF_L;
    }


    public static NPPlayerVariable_CS_BUF_L readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_BUF_L variableObj = new NPPlayerVariable_CS_BUF_L();

        try
        {
            String bufIdS = _reader.readItem('@');

            variableObj._m_lBufId = Long.parseLong(bufIdS);

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式——卡牌状态——配置错误 - BUF_L   example: enum:bufId Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

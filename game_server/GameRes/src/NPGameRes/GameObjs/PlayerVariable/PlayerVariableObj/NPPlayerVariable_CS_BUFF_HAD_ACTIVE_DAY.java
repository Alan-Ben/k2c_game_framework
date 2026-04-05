package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY extends _ANPBasicPlayerVariableObj
{
    private long _m_lBuffId;//buffId

    public long buffId()
    {
        return _m_lBuffId;
    }

    protected NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_BUFF_HAD_ACTIVE_DAY;
    }


    public static NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY variableObj = new NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY();

        try
        {
            String bufIdS = _reader.readItem('@');

            variableObj._m_lBuffId = Long.parseLong(bufIdS);

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("读取NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY失败，数据格式错误 src:{}", _reader.getSrcString());
            return null;
        }
    }
}

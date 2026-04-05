package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import Common.PlayerEnum.EPlayerEventRecordType;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_EVENT_RECORD extends _ANPBasicPlayerVariableObj
{
    /**
     * 类型枚举
     */
    private EPlayerEventRecordType _m_eParamType;
    private long _m_lSubId;

    public EPlayerEventRecordType paramType()
    {
        return _m_eParamType;
    }

    public long subId()
    {
        return _m_lSubId;
    }

    protected NPPlayerVariable_CS_EVENT_RECORD()
    {
        _m_eParamType = EPlayerEventRecordType.NONE;
        _m_lSubId = -1;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_EVENT_RECORD;
    }


    public static NPPlayerVariable_CS_EVENT_RECORD readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_EVENT_RECORD variableObj = new NPPlayerVariable_CS_EVENT_RECORD();

        try
        {
            //事件记录枚举
            String paramS = _reader.readItem('@');
            variableObj._m_eParamType = EPlayerEventRecordType.valueOf(paramS.toUpperCase());

            //二级ID
            String subIdS = _reader.readItem('@');
            if (null != subIdS)
            {
                variableObj._m_lSubId = Long.parseLong(subIdS);
            } else //如果未指定，则默认使用全部sub的
            {
                variableObj._m_lSubId = -1;
            }

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式配置错误 - Value example: ENPPlayerEventRecordType Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

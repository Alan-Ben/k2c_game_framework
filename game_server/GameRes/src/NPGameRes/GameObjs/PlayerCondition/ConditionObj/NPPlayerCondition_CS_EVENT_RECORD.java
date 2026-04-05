package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import Common.PlayerEnum.EPlayerEventRecordType;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 玩家计数 cs_record:ENPPlayerRecordParam:min_value:max_value
 * @author mark
 */
public class NPPlayerCondition_CS_EVENT_RECORD extends _ANPBasicPlayerCondition
{
    private EPlayerEventRecordType _m_eType;
    private long _m_lSubId;

    private long _m_lMinValue = -1; // -1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public EPlayerEventRecordType type()
    {
        return _m_eType;
    }

    public long subId()
    {
        return _m_lSubId;
    }

    public long minValue()
    {
        return _m_lMinValue;
    }

    public long maxValue()
    {
        return _m_lMaxValue;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_EVENT_RECORD;
    }

    public static NPPlayerCondition_CS_EVENT_RECORD readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_EVENT_RECORD cond = new NPPlayerCondition_CS_EVENT_RECORD();

        String typeS = _reader.readItem();
        String subIdS = _reader.readItem();
        String minVS = _reader.readItem();

        if (null == typeS || null == subIdS || null == minVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_EVENT_RECORD[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_eType = EPlayerEventRecordType.valueOf(typeS.toUpperCase());
        cond._m_lSubId = Long.parseLong(subIdS);
        cond._m_lMinValue = Long.parseLong(minVS);

        String maxVS = _reader.readItem();
        if (null != maxVS)
            cond._m_lMaxValue = Long.parseLong(maxVS);

        return cond;
    }
}

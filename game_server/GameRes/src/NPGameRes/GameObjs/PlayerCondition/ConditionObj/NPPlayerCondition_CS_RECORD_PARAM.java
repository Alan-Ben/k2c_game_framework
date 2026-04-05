package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 玩家计数 cs_record:ENPPlayerRecordParam:min_value:max_value
 * @author mark
 */
public class NPPlayerCondition_CS_RECORD_PARAM extends _ANPBasicPlayerCondition
{
    private ENPPlayerRecordParam _m_eRecord;
    private long _m_lMinValue = -1; // -1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public ENPPlayerRecordParam record()
    {
        return _m_eRecord;
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
        return ENPPlayerConditionType.CS_RECORD_PARAM;
    }

    public static NPPlayerCondition_CS_RECORD_PARAM readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_RECORD_PARAM cond = new NPPlayerCondition_CS_RECORD_PARAM();

        String playerValueS = _reader.readItem();
        String minVS = _reader.readItem();

        if (null == playerValueS || null == minVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_RECORD_PARAM[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_eRecord = ENPPlayerRecordParam.valueOf(playerValueS.toUpperCase());
        cond._m_lMinValue = Long.parseLong(minVS);

        String maxVS = _reader.readItem();
        if (null != maxVS)
            cond._m_lMaxValue = Long.parseLong(maxVS);

        return cond;
    }
}

package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 玩家计数 cs_record:ENPPlayerRecordParam:min_value:max_value
 * @author mark
 */
public class NPPlayerCondition_CS_QUEST_COUNT extends _ANPBasicPlayerCondition
{
    private long _m_lQuestId;
    private long _m_lMinValue = -1; // -1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public long questId()
    {
        return _m_lQuestId;
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
        return ENPPlayerConditionType.CS_QUEST_COUNT;
    }

    public static NPPlayerCondition_CS_QUEST_COUNT readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_QUEST_COUNT cond = new NPPlayerCondition_CS_QUEST_COUNT();

        String questS = _reader.readItem();
        String minVS = _reader.readItem();

        if (null == questS || null == minVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_QUEST_COUNT[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lQuestId = Long.parseLong(questS);
        cond._m_lMinValue = Long.parseLong(minVS);

        String maxVS = _reader.readItem();
        if (null != maxVS)
            cond._m_lMaxValue = Long.parseLong(maxVS);

        return cond;
    }
}

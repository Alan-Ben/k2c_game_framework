package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 玩家计数 cs_record:ENPPlayerRecordParam:min_value:max_value
 * @author mark
 */
public class NPPlayerCondition_CS_QUEST_DOING extends _ANPBasicPlayerCondition
{
    private long _m_lQuestId;
    private long _m_lQuestStep;
    private long _m_lQuestStepTarget;

    public long questId()
    {
        return _m_lQuestId;
    }

    public long questStep()
    {
        return _m_lQuestStep;
    }

    public long questStepTarget()
    {
        return _m_lQuestStepTarget;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_QUEST_DOING;
    }

    public static NPPlayerCondition_CS_QUEST_DOING readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_QUEST_DOING cond = new NPPlayerCondition_CS_QUEST_DOING();

        String questS = _reader.readItem();

        if (null == questS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_VALUE[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lQuestId = Long.parseLong(questS);

        String questStepS = _reader.readItem();
        if (null != questStepS)
            cond._m_lQuestStep = Long.parseLong(questStepS);

        String questStepTargetS = _reader.readItem();
        if (null != questStepTargetS)
            cond._m_lQuestStepTarget = Long.parseLong(questStepTargetS);

        return cond;
    }
}

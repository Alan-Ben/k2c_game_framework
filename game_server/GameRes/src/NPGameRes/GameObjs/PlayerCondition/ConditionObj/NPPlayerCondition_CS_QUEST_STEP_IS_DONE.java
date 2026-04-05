package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 是否完成任务 CS_QUEST_STEP_IS_DONE:quest_id:step_id
 * @author keith
 */
public class NPPlayerCondition_CS_QUEST_STEP_IS_DONE extends _ANPBasicPlayerCondition
{
    private long _m_lQuestId;
    private long _m_lStepId;

    public long questId()
    {
        return _m_lQuestId;
    }

    public long stepId()
    {
        return _m_lStepId;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_QUEST_STEP_IS_DONE;
    }

    public static NPPlayerCondition_CS_QUEST_STEP_IS_DONE readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_QUEST_STEP_IS_DONE cond = new NPPlayerCondition_CS_QUEST_STEP_IS_DONE();

        String questS = _reader.readItem(':');

        if (null == questS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_VALUE[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lQuestId = Long.parseLong(questS);

        String questStepS = _reader.readItem(':');
        if (null != questStepS)
            cond._m_lStepId = Long.parseLong(questStepS);

        return cond;
    }
}

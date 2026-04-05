package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 章节是否通过指定关卡 CS_CHAPTER_STAGE_PASSED:stageId
 */
public class NPPlayerCondition_CS_CHAPTER_STAGE_PASSED extends _ANPBasicPlayerCondition
{
    private long _m_stageId;

    public long getStageId()
    {
        return _m_stageId;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_CHAPTER_STAGE_PASSED;
    }

    public static NPPlayerCondition_CS_CHAPTER_STAGE_PASSED readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_CHAPTER_STAGE_PASSED cond = new NPPlayerCondition_CS_CHAPTER_STAGE_PASSED();

        //读取关卡id
        String rawStageId = _reader.readItem();
        if (null == rawStageId)
        {
            CommLog.error("Can not read str for rawStageId str:{}", _reader.getSrcString());
            return null;
        }
        cond._m_stageId = Long.parseLong(rawStageId);

        return cond;
    }
}

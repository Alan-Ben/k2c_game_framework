package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 修改玩家计数 s_record_incr:ENPPlayerRecordParam:计数的变化数值
 * @author mark
 */
public class NPPlayerEffect_S_QUEST_COUNTER_CHG extends _ANPPlayerEffectInfo
{
    private long _m_lQuestId;
    private long _m_lQuestStep;
    private long _m_lQuestStepTarget;
    private long _m_lCount;
    private ENCounterDealType _m_eDealType = ENCounterDealType.ADD;

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

    public long count()
    {
        return _m_lCount;
    }

    public ENCounterDealType dealType()
    {
        return _m_eDealType;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_QUEST_COUNTER_CHG;
    }

    public static NPPlayerEffect_S_QUEST_COUNTER_CHG readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_CHG_QUEST_STEP_TARGET_COUNTER[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String questS = _reader.readItem(':');
            String questStepS = _reader.readItem(':');
            String questStepTargetS = _reader.readItem(':');
            String countS = _reader.readItem(':');
            String dealTypeS = _reader.readItem(':');

            if (null == questS
                    || null == questStepS
                    || null == questStepTargetS
                    || null == countS)
            {
                ALServerLog.Error("can not get effect S_CHG_QUEST_STEP_TARGET_COUNTER[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_QUEST_COUNTER_CHG obj = new NPPlayerEffect_S_QUEST_COUNTER_CHG();
            obj._m_lQuestId = Long.valueOf(questS);
            obj._m_lQuestStep = Long.valueOf(questStepS);
            obj._m_lQuestStepTarget = Long.valueOf(questStepTargetS);
            obj._m_lCount = Long.valueOf(countS);

            if (null != dealTypeS)
            {
                obj._m_eDealType = ENCounterDealType.valueOf(dealTypeS.toUpperCase());
            }

            return obj;
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

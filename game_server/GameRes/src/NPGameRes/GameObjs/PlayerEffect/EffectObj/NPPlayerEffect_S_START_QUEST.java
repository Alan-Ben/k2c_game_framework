package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 开启任务（continue_secs是持续时间，为0表示走配置） s_start_quest:quest_id:continue_secs
 * @author mark
 */
public class NPPlayerEffect_S_START_QUEST extends _ANPPlayerEffectInfo
{
    private long _m_lQuestId;

    public long questId()
    {
        return _m_lQuestId;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_START_QUEST;
    }

    public static NPPlayerEffect_S_START_QUEST readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_START_QUEST[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String questIdS = _reader.readItem(':');

            if (null == questIdS)
            {
                ALServerLog.Error("can not get effect S_START_QUEST[" + _reader.getSrcString() + "]");
                return null;
            }


            NPPlayerEffect_S_START_QUEST obj = new NPPlayerEffect_S_START_QUEST();
            obj._m_lQuestId = Long.parseLong(questIdS);

            return obj;
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

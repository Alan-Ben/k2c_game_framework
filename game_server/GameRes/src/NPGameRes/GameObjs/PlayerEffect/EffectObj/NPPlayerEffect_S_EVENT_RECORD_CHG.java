package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import Common.PlayerEnum.EPlayerEventRecordType;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 修改玩家计数 s_record_incr:ENPPlayerRecordParam:计数的变化数值
 * @author mark
 */
public class NPPlayerEffect_S_EVENT_RECORD_CHG extends _ANPPlayerEffectInfo
{
    private EPlayerEventRecordType _m_eType;
    private long _m_lSubId;
    private long _m_lCount;
    private ENCounterDealType _m_eDealType = ENCounterDealType.ADD;

    public EPlayerEventRecordType type()
    {
        return _m_eType;
    }

    public long subId()
    {
        return _m_lSubId;
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
        return ENPPlayerEffectType.S_EVENT_RECORD_CHG;
    }

    public static NPPlayerEffect_S_EVENT_RECORD_CHG readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_EVENT_RECORD_CHG[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String typeS = _reader.readItem(':');
            String subIdS = _reader.readItem(':');
            String countS = _reader.readItem(':');
            String dealTypeS = _reader.readItem(':');

            if (null == typeS
                    || null == subIdS
                    || null == countS)
            {
                ALServerLog.Error("can not get effect S_EVENT_RECORD_CHG[" + _reader.getSrcString() + "]");
                return null;
            }


            NPPlayerEffect_S_EVENT_RECORD_CHG obj = new NPPlayerEffect_S_EVENT_RECORD_CHG();
            obj._m_eType = EPlayerEventRecordType.valueOf(typeS.toUpperCase());
            obj._m_lSubId = Long.valueOf(subIdS);
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

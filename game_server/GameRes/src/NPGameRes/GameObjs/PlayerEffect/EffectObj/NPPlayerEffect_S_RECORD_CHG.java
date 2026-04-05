package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerEffectType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 修改玩家计数 s_record_incr:ENPPlayerRecordParam:计数的变化数值
 * @author mark
 */
public class NPPlayerEffect_S_RECORD_CHG extends _ANPPlayerEffectInfo
{
    private ENPPlayerRecordParam _m_eRecord;
    private long _m_lCount;
    private ENCounterDealType _m_eDealType = ENCounterDealType.ADD;

    public ENPPlayerRecordParam record()
    {
        return _m_eRecord;
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
        return ENPPlayerEffectType.S_RECORD_CHG;
    }

    public static NPPlayerEffect_S_RECORD_CHG readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_RECORD_CHG[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String recordS = _reader.readItem(':');
            String countS = _reader.readItem(':');
            String dealTypeS = _reader.readItem(':');

            if (null == recordS
                    || null == countS)
            {
                ALServerLog.Error("can not get effect S_RECORD_CHG[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_RECORD_CHG obj = new NPPlayerEffect_S_RECORD_CHG();
            obj._m_eRecord = ENPPlayerRecordParam.valueOf(recordS.toUpperCase());
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

package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPEnum.ENPServerIDDealType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 修改玩家计数 s_record_incr:ENPPlayerRecordParam:计数的变化数值
 * @author mark
 */
public class NPPlayerEffect_S_DEAL_ID extends _ANPPlayerEffectInfo
{
    private ENPServerIDDealType _m_eDealType;
    private long _m_lDealId;

    public ENPServerIDDealType getDealType()
    {
        return _m_eDealType;
    }

    public long getDealId()
    {
        return _m_lDealId;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_DEAL_ID;
    }

    public static NPPlayerEffect_S_DEAL_ID readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_DEAL_ID[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String dealTypeS = _reader.readItem(':');
            String idS = _reader.readItem(':');

            if (null == dealTypeS
                    || null == idS)
            {
                ALServerLog.Error("can not get effect S_DEAL_ID[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_DEAL_ID obj = new NPPlayerEffect_S_DEAL_ID();
            obj._m_eDealType = ENPServerIDDealType.valueOf(dealTypeS.toUpperCase());
            obj._m_lDealId = Long.valueOf(idS);

            return obj;
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

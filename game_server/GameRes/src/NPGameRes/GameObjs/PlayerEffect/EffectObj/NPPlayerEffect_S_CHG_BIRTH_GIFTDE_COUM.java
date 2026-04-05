package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 修改卷王子嗣次数 S_CHG_BIRTH_GIFTDE_COUM:数量（:ENCounterDealType默认ADD）
 */
public class NPPlayerEffect_S_CHG_BIRTH_GIFTDE_COUM extends _ANPPlayerEffectInfo
{
    private long _m_lCount;
    // 操作类型，默认ADD
    private ENCounterDealType _m_eDealType = ENCounterDealType.ADD;

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
        return ENPPlayerEffectType.S_CHG_BIRTH_GIFTDE_COUM;
    }

    public static NPPlayerEffect_S_CHG_BIRTH_GIFTDE_COUM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_CHG_BIRTH_GIFTDE_COUM[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String countS = _reader.readItem(':');
            String dealTypeS = _reader.readItem(':');

            if (null == countS)
            {
                ALServerLog.Error("can not get effect S_CHG_BIRTH_GIFTDE_COUM[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_CHG_BIRTH_GIFTDE_COUM obj = new NPPlayerEffect_S_CHG_BIRTH_GIFTDE_COUM();
            obj._m_lCount = Long.parseLong(countS);

            if (null != dealTypeS)
            {
                obj._m_eDealType = ENCounterDealType.valueOf(dealTypeS.toUpperCase());
            }

            return obj;
        }
        catch (Exception e)
        {
            CommLog.error("", e);
            return null;
        }
    }
}

package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 给予LAZY_CD（CdId,数量）S_GAIN_LAZY_CD:CdId:数量
 */
public class NPPlayerEffect_S_GAIN_LAZY_CD extends _ANPPlayerEffectInfo
{
    private int _m_cdId;
    private int _m_num;

    public int getCdId()
    {
        return _m_cdId;
    }

    public int getNum()
    {
        return _m_num;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_LAZY_CD;
    }

    public static NPPlayerEffect_S_GAIN_LAZY_CD readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_LAZY_CD[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String rawCdId = _reader.readItem(':');
            String rawNum = _reader.readItem(':');

            if (rawCdId == null || null == rawNum)
            {
                ALServerLog.Error("can not get effect S_GAIN_LAZY_CD[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_GAIN_LAZY_CD obj = new NPPlayerEffect_S_GAIN_LAZY_CD();
            obj._m_cdId = Integer.parseInt(rawCdId);
            obj._m_num = Integer.parseInt(rawNum);
            return obj;
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

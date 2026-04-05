package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 为随机建筑招募X位员工（数量） S_RND_GAIN_X_BUSINESS_WORKERS:数量
 */
public class NPPlayerEffect_S_RND_GAIN_X_BUSINESS_WORKERS extends _ANPPlayerEffectInfo
{
    private int _m_num = 0;

    public int getNum()
    {
        return _m_num;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_RND_GAIN_X_BUSINESS_WORKERS;
    }

    public static NPPlayerEffect_S_RND_GAIN_X_BUSINESS_WORKERS readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_X_CONSORT_CHARM_FROM[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String rawNum = _reader.readItem();

            if (rawNum == null)
            {
                ALServerLog.Error("can not get effect S_RND_GAIN_X_BUSINESS_WORKERS[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_RND_GAIN_X_BUSINESS_WORKERS obj = new NPPlayerEffect_S_RND_GAIN_X_BUSINESS_WORKERS();
            obj._m_num = Integer.parseInt(rawNum);
            return obj;
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

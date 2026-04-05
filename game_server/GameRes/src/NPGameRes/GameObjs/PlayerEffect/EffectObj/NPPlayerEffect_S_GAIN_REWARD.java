package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

import java.util.ArrayList;

public class NPPlayerEffect_S_GAIN_REWARD extends _ANPPlayerEffectInfo
{
    private ArrayList<Long> _m_alRewardIdList;

    public NPPlayerEffect_S_GAIN_REWARD()
    {
        _m_alRewardIdList = new ArrayList<>();
    }

    public ArrayList<Long> getRewardIdList()
    {
        return _m_alRewardIdList;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_REWARD;
    }

    public static NPPlayerEffect_S_GAIN_REWARD readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_REWARD[" + _reader.getSrcString() + "]");
            return null;
        }

        NPPlayerEffect_S_GAIN_REWARD effect = new NPPlayerEffect_S_GAIN_REWARD();

        String idS = _reader.readItem('#');
        while (null != idS)
        {
            try
            {
                effect._m_alRewardIdList.add(Long.parseLong(idS));
            } catch (Exception e)
            {
                e.printStackTrace();
            }

            idS = _reader.readItem('#');
        }

        return effect;
    }
}

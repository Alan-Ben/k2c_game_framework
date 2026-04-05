package NPGameRes.Refs.Parse;

import NPCommon.RefData._IParseFromStringable;
import NPGameRes.GameObjs.PlayerEffect.NPPlayerEffectSerializeInfo;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

import java.util.ArrayList;

public class NPPlayerEffectListParse implements _IParseFromStringable
{
    private ArrayList<_ANPPlayerEffectInfo> _m_alPlayerEffectInfoList = new ArrayList<>();

    public boolean isEmpty()
    {
        return _m_alPlayerEffectInfoList.isEmpty();
    }

    public ArrayList<_ANPPlayerEffectInfo> getPlayerEffectList()
    {
        return _m_alPlayerEffectInfoList;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue == null || sValue.trim().isEmpty())
            return true;

        ArrayList<NPPlayerEffectSerializeInfo> list = NPPlayerEffectSerializeInfo.readEffectList(sValue);
        if (null == list)
            return true;

        for (int i = 0; i < list.size(); i++)
        {
            NPPlayerEffectSerializeInfo info = list.get(i);
            if (null == info)
                continue;

            if (null != info.effectInfo())
                _m_alPlayerEffectInfoList.add(info.effectInfo());
        }

        return true;
    }
}

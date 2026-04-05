package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 对政务指定刷新组进行刷新 S_ANECDOTE_REFRESH:刷新组id
 */
public class NPPlayerEffect_S_ANECDOTE_REFRESH extends _ANPPlayerEffectInfo
{
    private int _m_groupId = 0;

    public int getGroupId()
    {
        return _m_groupId;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_ANECDOTE_REFRESH;
    }

    public static NPPlayerEffect_S_ANECDOTE_REFRESH readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_ANECDOTE_REFRESH[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String rawGroupId = _reader.readItem();

            if (rawGroupId == null)
            {
                ALServerLog.Error("can not get effect S_ANECDOTE_REFRESH[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_ANECDOTE_REFRESH obj = new NPPlayerEffect_S_ANECDOTE_REFRESH();
            obj._m_groupId = Integer.parseInt(rawGroupId);
            return obj;
        } catch (Exception e)
        {
            CommLog.error("", e);
            return null;
        }
    }
}

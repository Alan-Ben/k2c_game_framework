package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

/**
 * 给予未拥有的妃子好感度（妃子id,数量） S_GAIN_X_CONSORT_LIKE_FROM:妃子id（高级公式）:数量（高级公式）
 */
public class NPPlayerEffect_S_GAIN_X_CONSORT_LIKE_FROM extends _ANPPlayerEffectInfo
{
	//妃子ID
    private long _m_lConsortId;
    //好感度数值高级公式
    private NPPlayerVariableGroupObj _m_voValueObj;

    public long getConsortId() {return _m_lConsortId;}
    public NPPlayerVariableGroupObj getValueObj() {return _m_voValueObj;}

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_X_CONSORT_LIKE_FROM;
    }

    public static NPPlayerEffect_S_GAIN_X_CONSORT_LIKE_FROM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_X_CONSORT_LIKE_FROM[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String consortIdStr = _reader.readItem(':');
            String valueStr = _reader.readItem(':');

            if (consortIdStr == null || valueStr == null)
            {
                ALServerLog.Error("can not get effect S_GAIN_X_CONSORT_LIKE_FROM[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_GAIN_X_CONSORT_LIKE_FROM obj = new NPPlayerEffect_S_GAIN_X_CONSORT_LIKE_FROM();
            obj._m_lConsortId = Long.valueOf(consortIdStr);
            obj._m_voValueObj = NPPlayerVariableGroupObj.readVariable(valueStr);
            
            return obj;
            
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

/**
 * 给予已拥有的妃子魅力值（妃子id,数量） S_GAIN_X_CONSORT_CHARM_FROM:妃子id（高级公式）:数量（高级公式）
 */
public class NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_FROM extends _ANPPlayerEffectInfo
{
    private NPPlayerVariableGroupObj _m_consortVariableObj;
    private NPPlayerVariableGroupObj _m_countVariableObj;

    public NPPlayerVariableGroupObj getConsortVariableObj()
    {
        return _m_consortVariableObj;
    }

    public NPPlayerVariableGroupObj getCountVariableObj()
    {
        return _m_countVariableObj;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_X_CONSORT_CHARM_FROM;
    }

    public static NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_FROM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_X_CONSORT_CHARM_FROM[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String rawConsortVariableObj = _reader.readItem(':');
            String rawCountVariableObj = _reader.readItem(':');

            if (rawConsortVariableObj == null || rawCountVariableObj == null)
            {
                ALServerLog.Error("can not get effect S_GAIN_X_CONSORT_CHARM_FROM[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_FROM obj = new NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_FROM();
            obj._m_consortVariableObj = NPPlayerVariableGroupObj.readVariable(rawConsortVariableObj);
            obj._m_countVariableObj = NPPlayerVariableGroupObj.readVariable(rawCountVariableObj);
            return obj;
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

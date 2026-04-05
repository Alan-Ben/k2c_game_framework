package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

/**
 * 给予已拥有的情人加护力点（情人id，数量，倍数） S_GAIN_X_CONSORT_CHARM_POINT_FROM:情人id（高级公式）:数量（高级公式）(:倍数，默认1)
 */
public class NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_POINT_FROM extends _ANPPlayerEffectInfo
{
    private NPPlayerVariableGroupObj _m_consortVariableObj;
    private NPPlayerVariableGroupObj _m_countVariableObj;
    
    //倍数，默认1
    private int _m_iMultiple = 1;

    public NPPlayerVariableGroupObj getConsortVariableObj()
    {
        return _m_consortVariableObj;
    }

    public NPPlayerVariableGroupObj getCountVariableObj()
    {
        return _m_countVariableObj;
    }
    
    public int getMultiple()
    {
    	return _m_iMultiple;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_X_CONSORT_CHARM_POINT_FROM;
    }

    public static NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_POINT_FROM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_X_CONSORT_INTIMACY_FROM[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String rawConsortVariableObj = _reader.readItem(':');
            String rawCountVariableObj = _reader.readItem(':');

            if (rawConsortVariableObj == null || rawCountVariableObj == null)
            {
                ALServerLog.Error("can not get effect S_GAIN_X_CONSORT_CHARM_POINT_FROM[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_POINT_FROM obj = new NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_POINT_FROM();
            obj._m_consortVariableObj = NPPlayerVariableGroupObj.readVariable(rawConsortVariableObj);
            obj._m_countVariableObj = NPPlayerVariableGroupObj.readVariable(rawCountVariableObj);
            
            obj._m_iMultiple = 1;
            String rawMultipleObj = _reader.readItem(':');
            if(null != rawMultipleObj)
            {
            	obj._m_iMultiple = Integer.parseInt(rawMultipleObj);
            }
            
            return obj;
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

/**
 * 给予未成年子嗣增加经验（子嗣实例id，数量，倍数） S_GAIN_X_CHILD_EXP_POINT_FROM:未成年id（高级公式）:数量（高级公式）(:倍数，默认1)
 */
public class NPPlayerEffect_S_GAIN_X_CHILD_EXP_POINT_FROM extends _ANPPlayerEffectInfo
{
    private NPPlayerVariableGroupObj _m_childVariableObj;
    private NPPlayerVariableGroupObj _m_countVariableObj;
    
    //倍数，默认1
    private int _m_iMultiple = 1;

    public NPPlayerVariableGroupObj getChildVariableObj()
    {
        return _m_childVariableObj;
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
        return ENPPlayerEffectType.S_GAIN_X_CHILD_EXP_POINT_FROM;
    }

    public static NPPlayerEffect_S_GAIN_X_CHILD_EXP_POINT_FROM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_X_CHILD_EXP_POINT_FROM[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String rawChildVariableObj = _reader.readItem(':');
            String rawCountVariableObj = _reader.readItem(':');

            if (rawChildVariableObj == null || rawCountVariableObj == null)
            {
                ALServerLog.Error("can not get effect S_GAIN_X_CHILD_EXP_POINT_FROM[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_GAIN_X_CHILD_EXP_POINT_FROM obj = new NPPlayerEffect_S_GAIN_X_CHILD_EXP_POINT_FROM();
            obj._m_childVariableObj = NPPlayerVariableGroupObj.readVariable(rawChildVariableObj);
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

package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.EEffectPlayerValueType;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

/**
 * 增加政务事件数量
 */
public class NPPlayerEffect_S_ADD_P_V_FORM extends _ANPPlayerEffectInfo
{
	private EEffectPlayerValueType _m_valueType = EEffectPlayerValueType.NONE;
	private NPPlayerVariableGroupObj _m_variableObj;
	
	public EEffectPlayerValueType getValueType() {return _m_valueType;}
	public NPPlayerVariableGroupObj getVarObj() {return _m_variableObj;}
	
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_ADD_P_V_FORM;
    }

    public static NPPlayerEffect_S_ADD_P_V_FORM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_ADD_P_V_FORM[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String typeS = _reader.readItem(':');
            String varS = _reader.readItem(':');

            if (null == typeS
                    || null == varS)
            {
                ALServerLog.Error("can not get effect S_ADD_P_V_FORM[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_ADD_P_V_FORM obj = new NPPlayerEffect_S_ADD_P_V_FORM();
            obj._m_valueType = EEffectPlayerValueType.valueOf(typeS.toUpperCase());
            obj._m_variableObj = NPPlayerVariableGroupObj.readVariable(varS);

            return obj;
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

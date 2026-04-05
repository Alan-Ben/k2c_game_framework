package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.EEffectPlayerValueType;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 增加政务事件数量
 */
public class NPPlayerEffect_S_ADD_P_V extends _ANPPlayerEffectInfo
{
	private EEffectPlayerValueType _m_valueType = EEffectPlayerValueType.NONE;
    private int _m_num;

	public EEffectPlayerValueType getValueType() {return _m_valueType;}
    public int getNum() {return _m_num;}

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_ADD_P_V;
    }

    public static NPPlayerEffect_S_ADD_P_V readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_ADD_P_V[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String typeS = _reader.readItem(':');
            String numS = _reader.readItem(':');

            if (null == typeS
                    || null == numS)
            {
                ALServerLog.Error("can not get effect S_ADD_P_V[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_ADD_P_V obj = new NPPlayerEffect_S_ADD_P_V();
            obj._m_valueType = EEffectPlayerValueType.valueOf(typeS.toUpperCase());
            obj._m_num = Integer.parseInt(numS);

            return obj;
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

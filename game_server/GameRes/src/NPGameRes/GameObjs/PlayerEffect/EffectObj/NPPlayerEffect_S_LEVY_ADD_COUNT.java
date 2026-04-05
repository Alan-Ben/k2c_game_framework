package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import Common.LevyEnum.ELevy_Type;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 解锁征收
 */
public class NPPlayerEffect_S_LEVY_ADD_COUNT extends _ANPPlayerEffectInfo
{
	//征收类型
	private ELevy_Type _m_eType = ELevy_Type.NONE;
	//增加数量
	private int _m_iAddCount;
	
	public ELevy_Type getType() {return _m_eType;}
	public int getAddCount() {return _m_iAddCount;}
	
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_LEVY_ADD_COUNT;
    }

    public static NPPlayerEffect_S_LEVY_ADD_COUNT readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_LEVY_ADD_COUNT[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            //征收类型
            String typeS = _reader.readItem(':');
            //是否重置征收数据
            String varS = _reader.readItem(':');

            if (null == typeS || null == varS)
            {
                ALServerLog.Error("can not get effect S_LEVY_ADD_COUNT[" + _reader.getSrcString() + "]");
                return null;
            }
            
            NPPlayerEffect_S_LEVY_ADD_COUNT obj = new NPPlayerEffect_S_LEVY_ADD_COUNT();
            obj._m_eType = ELevy_Type.valueOf(typeS.toUpperCase());
            obj._m_iAddCount = Integer.valueOf(varS);
            
            return obj;
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

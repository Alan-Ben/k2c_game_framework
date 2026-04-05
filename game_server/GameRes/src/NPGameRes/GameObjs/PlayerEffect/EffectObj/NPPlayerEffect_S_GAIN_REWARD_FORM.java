package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

public class NPPlayerEffect_S_GAIN_REWARD_FORM extends _ANPPlayerEffectInfo
{
	//奖励数据（高级公式）
    private NPPlayerVariableGroupObj _m_voRewardVarObj;
    //奖励倍数（高级公式）
    private NPPlayerVariableGroupObj _m_voMultipleVarObj; 

    public NPPlayerEffect_S_GAIN_REWARD_FORM()
    {
    }

    public NPPlayerVariableGroupObj getRewardObj() {return _m_voRewardVarObj;}
    public NPPlayerVariableGroupObj getMultipleVarObj() {return _m_voMultipleVarObj;}

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_REWARD_FORM;
    }

    public static NPPlayerEffect_S_GAIN_REWARD_FORM readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_REWARD_FORM[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String rawRewardVariableObj = _reader.readItem(':');
            String rawMultipleVariableObj = _reader.readItem(':');

            if (rawRewardVariableObj == null || rawMultipleVariableObj == null)
            {
                ALServerLog.Error("can not get effect S_GAIN_REWARD_FORM[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_GAIN_REWARD_FORM obj = new NPPlayerEffect_S_GAIN_REWARD_FORM();
            obj._m_voRewardVarObj = NPPlayerVariableGroupObj.readVariable(rawRewardVariableObj);
            obj._m_voMultipleVarObj = NPPlayerVariableGroupObj.readVariable(rawMultipleVariableObj);
            
            return obj;
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}

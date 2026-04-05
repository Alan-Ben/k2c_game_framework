package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGPosEffectType;

import java.util.ArrayList;
import java.util.List;

public class WCGPosEffectDoTrigger extends _AWCGPosEffectInfo
{
    private List<Long> _m_lTrigger_effect_list;//触发效果id列表

    public List<Long> trigger_effect_list()
    {
        return _m_lTrigger_effect_list;
    }

    public EWCGPosEffectType effectType()
    {
        return EWCGPosEffectType.TRIGGER_EFFECT;
    }

    public WCGPosEffectDoTrigger()
    {
        _m_lTrigger_effect_list = new ArrayList<Long>();
    }

    public static WCGPosEffectDoTrigger readVariable(String _str)
    {
        WCGPosEffectDoTrigger effectObj = new WCGPosEffectDoTrigger();
        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');

        for (int i = 0; i < strs.length; ++i)
        {
            try
            {
                effectObj.trigger_effect_list().add(Long.parseLong(strs[i].trim()));
            } catch (Exception e)
            {
                CommLog.error("位置效果配置错误 - TRIGGER_EFFECT example: enum:triggerId:triggerId.... Error Str: " + _str, e);
                continue;
            }
        }

        return effectObj;
    }
}
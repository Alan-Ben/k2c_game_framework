package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGEffectSerializeInfo;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGEffectType;

//ai本体触发效果
public class WCGAITriggerSelfEffect extends _AWCGBasicAITrigger
{
    private WCGEffectSerializeInfo _m_sEffectInfo;//效果信息

    public WCGEffectSerializeInfo EffectInfo()
    {
        return _m_sEffectInfo;
    }

    @Override
    public EWCGAITriggerType triggerType()
    {
        return EWCGAITriggerType.SELF_EFFECT;
    }

    public static WCGAITriggerSelfEffect read(String _infoStr)
    {
        WCGAITriggerSelfEffect obj = new WCGAITriggerSelfEffect();

        String[] strs = CommonFunc.charSplit(_infoStr, ':', 3);

        if (strs.length < 3)
            return null;

        //创建对象
        WCGEffectSerializeInfo effectInfo = new WCGEffectSerializeInfo();
        obj._m_sEffectInfo = effectInfo;
        effectInfo.effect_type = EWCGEffectType.valueOf(strs[0].toUpperCase().trim());
        effectInfo.target_type = EWCGEffectTargetType.valueOf(strs[1].toUpperCase().trim());
        effectInfo.effect_info_str = strs[2];

        return obj;
    }
}
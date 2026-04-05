package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefSkillTriggerEffect;

import java.util.List;

public class WCGSkillTriggerEffect implements _IWCGRndItem
{
    public int trigger_effect_delay_time;//触发延迟时间  毫秒
    public List<Long> trigger_effect_list;//触发效果id列表，对应WCGPosEffectRefObj表id
    public int trigger_pos_effect_delay_time;//触发延迟时间  毫秒
    public List<Long> trigger_pos_effect_list;//触发效果id列表
    public int chance;//触发概率

    /**
     * 随机概率
     */
    public int rndChance()
    {
        return chance;
    }

    public void adapt(RefSkillTriggerEffect refTriggerEffect)
    {
        this.trigger_effect_delay_time = refTriggerEffect.trigger_effect_delay_time;
        this.trigger_effect_list = refTriggerEffect.trigger_effect_list;
        this.trigger_pos_effect_delay_time = refTriggerEffect.trigger_pos_effect_delay_time;
        this.trigger_pos_effect_list = refTriggerEffect.trigger_pos_effect_list;
        this.chance = refTriggerEffect.chance;
    }
}

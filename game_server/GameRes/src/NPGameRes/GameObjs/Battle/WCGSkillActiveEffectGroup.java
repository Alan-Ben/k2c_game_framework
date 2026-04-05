package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefSkillActiveEffect;
import NPGameRes.Refs.Battle.RefSkillActiveEffectGroup;

public class WCGSkillActiveEffectGroup extends _ATWCGRndController<WCGSkillActiveEffect>
{
    public int group_id;//组ID
    public int group_chance;//组概率
    public int trigger_time;//效果触发时间
    public WCGBothConditionGroupObj trigger_both_cond;//触发的双向条件
    public boolean is_atk_suc;//用于是否攻击成功的判断

    /***************
     * 默认值
     **/
    public WCGSkillActiveEffect defaultValue()
    {
        return null;
    }

    public String toString()
    {
        return String.format("效果组:%d,触发时间:%d"
                , group_id
                , trigger_time);
    }

    public void adapt(RefSkillActiveEffectGroup ref)
    {
        this.group_id = ref.group_id;
        this.group_chance = ref.chance;
        this.trigger_time = ref.trigger_time;
        this.trigger_both_cond = WCGBothConditionGroupObj.readConditionGroupList(ref.trigger_both_cond);
        this.is_atk_suc = ref.is_atk_suc;

        for (RefSkillActiveEffect refActiveEffect : RefSkillActiveEffect.getMgr().getList())
        {
            if (refActiveEffect.id == ref.id
                    && refActiveEffect.level == ref.level
                    && refActiveEffect.group_id == ref.group_id)
            {
                WCGSkillActiveEffect obj = new WCGSkillActiveEffect();
                obj.adapt(refActiveEffect);
                this.addItem(obj);
            }

        }

    }
}
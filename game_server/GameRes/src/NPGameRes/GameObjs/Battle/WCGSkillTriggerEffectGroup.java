package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefSkillTriggerEffect;
import NPGameRes.Refs.Battle.RefSkillTriggerEffectGroup;
import WCGCommon.Enum.NPEnum.EWCGSkillTriggerType;

public class WCGSkillTriggerEffectGroup extends _ATWCGRndController<WCGSkillTriggerEffect>
{
    public EWCGSkillTriggerType trigger_type;//触发条件
    public long cd_group;//cd 组
    public int cdTime;//技能CD 毫秒

    public int group_id;//组ID
    public int group_chance;//组概率

    /***************
     * 默认值
     **/
    public WCGSkillTriggerEffect defaultValue()
    {
        return null;
    }

    public void adapt(RefSkillTriggerEffectGroup ref)
    {
        this.trigger_type = ref.trigger_type;
        this.cd_group = ref.cd_group;
        this.cdTime = ref.cdTime;
        this.group_id = ref.group_id;
        this.group_chance = ref.chance;

        for (RefSkillTriggerEffect refEffect : RefSkillTriggerEffect.getMgr().getList())
        {
            if (refEffect.id == ref.id && refEffect.level == ref.level && refEffect.group_id == ref.group_id)
            {
                WCGSkillTriggerEffect obj = new WCGSkillTriggerEffect();
                obj.adapt(refEffect);
                this.addItem(obj);
            }
        }

    }

}
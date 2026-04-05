package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import WCGCommon.Enum.NPEnum.EWCGSkillTriggerType;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "skill_trigger_effect_group", isSingletonKey = false)
public class RefSkillTriggerEffectGroup extends RefBase
{
    private static RefListContainer<RefSkillTriggerEffectGroup> _g_mgr = new RefListContainer<RefSkillTriggerEffectGroup>();

    public static RefListContainer<RefSkillTriggerEffectGroup> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefSkillTriggerEffectGroup> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefSkillTriggerEffectGroup>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSkillTriggerEffectGroup newRef = (RefSkillTriggerEffectGroup) _newRef;
        id = newRef.id;
        level = newRef.level;
        trigger_type = newRef.trigger_type;
        cd_group = newRef.cd_group;
        cdTime = newRef.cdTime;
        group_id = newRef.group_id;
        chance = newRef.chance;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id * 10000 + (level * 100) + group_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public int id;
    public int level;
    public EWCGSkillTriggerType trigger_type;//触发条件
    public long cd_group;//cd 组
    public int cdTime;//技能CD 毫秒

    public int group_id;//组ID
    public int chance;//组概率

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

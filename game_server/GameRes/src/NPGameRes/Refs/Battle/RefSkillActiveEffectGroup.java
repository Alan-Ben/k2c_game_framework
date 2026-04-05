package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "skill_active_effect_group", isSingletonKey = false)
public class RefSkillActiveEffectGroup extends RefBase
{
    private static RefListContainer<RefSkillActiveEffectGroup> _g_mgr = new RefListContainer<RefSkillActiveEffectGroup>();

    public static RefListContainer<RefSkillActiveEffectGroup> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefSkillActiveEffectGroup> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefSkillActiveEffectGroup>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSkillActiveEffectGroup newRef = (RefSkillActiveEffectGroup) _newRef;
        id = newRef.id;
        level = newRef.level;
        group_id = newRef.group_id;
        chance = newRef.chance;
        trigger_time = newRef.trigger_time;
        trigger_both_cond = newRef.trigger_both_cond;
        is_atk_suc = newRef.is_atk_suc;
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
        return 0;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//触发条件
    public int level;//cd 组

    public int group_id;//组概率
    public int chance;//组ID

    public int trigger_time;//技能CD 毫秒
    public String trigger_both_cond;//触发本集合的双向判断条件
    public boolean is_atk_suc;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

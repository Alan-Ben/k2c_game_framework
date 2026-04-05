package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "skill_trigger_effect", isSingletonKey = false)
public class RefSkillTriggerEffect extends RefBase
{
    private static RefListContainer<RefSkillTriggerEffect> _g_mgr = new RefListContainer<RefSkillTriggerEffect>();

    public static RefListContainer<RefSkillTriggerEffect> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefSkillTriggerEffect> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefSkillTriggerEffect>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSkillTriggerEffect newRef = (RefSkillTriggerEffect) _newRef;
        id = newRef.id;
        level = newRef.level;
        trigger_effect_delay_time = newRef.trigger_effect_delay_time;
        trigger_effect_list = newRef.trigger_effect_list;
        chance = newRef.chance;
        group_id = newRef.group_id;
        trigger_pos_effect_delay_time = newRef.trigger_pos_effect_delay_time;
        trigger_pos_effect_list = newRef.trigger_pos_effect_list;
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
    public int id;
    public int level;
    public int trigger_effect_delay_time;//触发延迟时间  毫秒
    public ArrayList<Long> trigger_effect_list = new ArrayList<Long>();//触发效果id列表
    public int chance;//触发概率
    public int group_id;
    public int trigger_pos_effect_delay_time;
    public ArrayList<Long> trigger_pos_effect_list;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "skill_active_effect", isSingletonKey = false)
public class RefSkillActiveEffect extends RefBase
{
    private static RefListContainer<RefSkillActiveEffect> _g_mgr = new RefListContainer<RefSkillActiveEffect>();

    public static RefListContainer<RefSkillActiveEffect> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefSkillActiveEffect> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefSkillActiveEffect>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSkillActiveEffect newRef = (RefSkillActiveEffect) _newRef;
        id = newRef.id;
        level = newRef.level;
        group_id = newRef.group_id;
        self_sfx_Id = newRef.self_sfx_Id;
        target_sfx_Id = newRef.target_sfx_Id;
        bullet_id = newRef.bullet_id;
        active_effect_delay_time = newRef.active_effect_delay_time;
        active_effect_list = newRef.active_effect_list;
        chance = newRef.chance;
        active_pos_effect_list = newRef.active_pos_effect_list;
        active_pos_effect_delay_time = newRef.active_pos_effect_delay_time;
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
    //使用技能id，等级，以及group划分，一个group内随机触发效果
    public long id;//技能ID
    public int level;//技能等级
    public int group_id;

    public ArrayList<Long> self_sfx_Id;
    public ArrayList<Long> target_sfx_Id;
    public long bullet_id;
    public int active_effect_delay_time;
    public ArrayList<Long> active_effect_list = new ArrayList<Long>();//触发效果id列表
    public int chance;

    public ArrayList<Long> active_pos_effect_list;
    public int active_pos_effect_delay_time;
    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

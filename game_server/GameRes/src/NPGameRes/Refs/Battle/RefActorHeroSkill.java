package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import WCGCommon.Enum.NPEnum.EWCGEffectRangeType;
import WCGCommon.Enum.NPEnum.EWCGSkillTrackingMethod;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "actor_hero_skill", isSingletonKey = false)
public class RefActorHeroSkill extends RefBase
{
    private static RefListContainer<RefActorHeroSkill> _g_mgr = new RefListContainer<RefActorHeroSkill>();

    public static RefListContainer<RefActorHeroSkill> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefActorHeroSkill> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefActorHeroSkill>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActorHeroSkill newRef = (RefActorHeroSkill) _newRef;
        actor_id = newRef.actor_id;
        actor_lvl = newRef.actor_lvl;
        skill_id = newRef.skill_id;
        cast_range = newRef.cast_range;
        range_type = newRef.range_type;
        width = newRef.width;
        height = newRef.height;
        tracking_method = newRef.tracking_method;
        init_effect = newRef.init_effect;
        condition_list = newRef.condition_list;
        condition_show_list = newRef.condition_show_list;
        tar_info = newRef.tar_info;
        angle = newRef.angle;
        cast_min_range = newRef.cast_min_range;
        horizontal_off_set = newRef.horizontal_off_set;
        vertical_off_set = newRef.vertical_off_set;
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
    public long actor_id;
    public int actor_lvl;
    public long skill_id;
    public int cast_range;
    public EWCGEffectRangeType range_type;
    public int width;//动作时长 毫秒
    public int height; //释放距离
    public EWCGSkillTrackingMethod tracking_method;
    public ArrayList<Long> init_effect;
    public String condition_list;
    public String condition_show_list;
    public String tar_info;
    public float angle;
    public int cast_min_range;
    public float horizontal_off_set;
    public float vertical_off_set;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

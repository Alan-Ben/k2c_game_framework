package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import WCGCommon.Enum.NPEnum.EWCGModelType;

import java.util.ArrayList;

/**
 * @author scott
 * 角  色  信  息
 */
@RefTable(tableName = "actor_level", isSingletonKey = false)
public class RefActorLevel extends RefBase
{
    private static RefListContainer<RefActorLevel> _g_mgr = new RefListContainer<RefActorLevel>();

    public static RefListContainer<RefActorLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefActorLevel> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefActorLevel>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActorLevel newRef = (RefActorLevel) _newRef;
        id = newRef.id;
        min_level = newRef.min_level;
        model = newRef.model;
        model_type = newRef.model_type;
        model_scale = newRef.model_scale;
        lvl_basic_property = newRef.lvl_basic_property;
        addition_property = newRef.addition_property;
        team_property = newRef.team_property;
        evolve_actor = newRef.evolve_actor;
        ai_id = newRef.ai_id;
        defence_ai_id = newRef.defence_ai_id;
        attackSkillId = newRef.attackSkillId;
        skillIdList = newRef.skillIdList;
        skill_info_list = newRef.skill_info_list;
        main_city_skill_list = newRef.main_city_skill_list;
        main_city_skill_list_show = newRef.main_city_skill_list_show;
        link_effect_list = newRef.link_effect_list;
        building_ing = newRef.building_ing;
        deathSkillId = newRef.deathSkillId;
        birth_skill_id = newRef.birth_skill_id;
        deploy_skill_id = newRef.deploy_skill_id;
        recycle_skill_id = newRef.recycle_skill_id;
        ex_att_id = newRef.ex_att_id;
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
        return id * 10000 + min_level;
    }

    public long id; //角色id
    public int min_level;
    public String model;//模型资源
    public EWCGModelType model_type;
    public int model_scale;

    public String lvl_basic_property;//到达这个等级的基础属性加成
    public String addition_property;//之后每级提升会增加的属性属性加成
    public String team_property;

    public long evolve_actor;  //进化角色

    public int ai_id;//ai id
    public int defence_ai_id;

    public long attackSkillId;//默认攻击id
    public ArrayList<Long> skillIdList;//其他技能id

    public String skill_info_list;//其他技能id

    public String main_city_skill_list;

    public String main_city_skill_list_show;

    public ArrayList<Long> link_effect_list;//关联的特效列表  用于特效预加载

    public String building_ing;  //生产前提不满足时的建筑单位 

    public long deathSkillId;

    public long birth_skill_id;
    public long deploy_skill_id;
    public long recycle_skill_id;

    public ArrayList<Long> ex_att_id = new ArrayList<Long>();
}

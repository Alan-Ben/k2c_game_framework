package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;
import NPGameRes.GameObjs.Battle.WCGActorSkill;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "mob_ref")
public class RefMapMobRef extends RefBase
{
    private static RefTableContainer<RefMapMobRef> _g_mgr = new RefTableContainer<RefMapMobRef>();

    public static RefTableContainer<RefMapMobRef> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefMapMobRef> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefMapMobRef>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMapMobRef newRef = (RefMapMobRef) _newRef;
        id = newRef.id;
        pet_id = newRef.pet_id;
        actor_id = newRef.actor_id;
        star = newRef.star;
        add_level = newRef.add_level;
        quality = newRef.quality;
        skin_id = newRef.skin_id;
        death_effect_id = newRef.death_effect_id;
        birth_effect_id = newRef.birth_effect_id;
        addition_property = newRef.addition_property;
        team_property = newRef.team_property;
        new_ai_id = newRef.new_ai_id;
        new_defence_ai_id = newRef.new_defence_ai_id;
        birth_buf_list = newRef.birth_buf_list;
        skill_info_list = newRef.skill_info_list;
        active_skill_info = newRef.active_skill_info;
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
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id;
    public long pet_id;                //卡牌Id
    public long actor_id;               //在卡牌和军队Id都无效的时候使用actorId，使用单位的时候，等级、星级、品质都将失去意义
    public short star;
    public int add_level;
    public EQuality quality;
    public long skin_id;
    public ArrayList<Long> death_effect_id;
    public ArrayList<Long> birth_effect_id;
    public String addition_property;
    public String team_property;
    public int new_ai_id;
    public int new_defence_ai_id;
    public String birth_buf_list;
    public ArrayList<WCGActorSkill> skill_info_list = new ArrayList<>();// 其他技能等级列表
    public ArrayList<WCGActorSkill> active_skill_info = new ArrayList<>();// 主动技能等级列表

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

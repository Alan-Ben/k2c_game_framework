package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.Battle.WCGActorSkill;
import WCGCommon.Enum.NPEnum.EWCGBuffType;
import WCGCommon.Enum.NPEnum.EWCGDmgType;
import WCGCommon.Enum.NPEnum.EWCGSkillTriggerType;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "buff")
public class RefBuff extends RefBase
{
    private static RefTableContainer<RefBuff> _g_mgr = new RefTableContainer<RefBuff>();

    public static RefTableContainer<RefBuff> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefBuff> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBuff>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBuff newRef = (RefBuff) _newRef;
        id = newRef.id;
        sfx_id = newRef.sfx_id;
        buff_type = newRef.buff_type;
        group_id = newRef.group_id;
        group_level = newRef.group_level;
        property = newRef.property;
        team_property = newRef.team_property;
        camp_property = newRef.camp_property;
        max_layers = newRef.max_layers;
        max_layers_effect = newRef.max_layers_effect;
        addtion_skill_list = newRef.addtion_skill_list;
        appear_effect = newRef.appear_effect;
        disappear_effect = newRef.disappear_effect;
        remove_effect = newRef.remove_effect;
        cannot_remove = newRef.cannot_remove;
        immunity_magic = newRef.immunity_magic;
        immunity_physic = newRef.immunity_physic;
        immunity_control = newRef.immunity_control;
        ban_heal = newRef.ban_heal;
        immunity_chaos = newRef.immunity_chaos;
        immunity_debuff = newRef.immunity_debuff;
        immunity_buff = newRef.immunity_buff;
        trigger_type = newRef.trigger_type;
        max_trigger_count = newRef.max_trigger_count;
        interval_check_time = newRef.interval_check_time;
        interval_effect_list = newRef.interval_effect_list;
        max_interval_count = newRef.max_interval_count;
        broken_effect_list = newRef.broken_effect_list;
        taunt = newRef.taunt;
        hide_blood = newRef.hide_blood;
        cannot_select = newRef.cannot_select;
        ignore_collide = newRef.ignore_collide;
        shield_type_list = newRef.shield_type_list;
        shield_variable = newRef.shield_variable;
        parry_type_list = newRef.parry_type_list;
        parry_count = newRef.parry_count;
        change_team = newRef.change_team;
        anim_trigger = newRef.anim_trigger;
        comp_show_list = newRef.comp_show_list;
        comp_hide_list = newRef.comp_hide_list;
        target_trigger_effect = newRef.target_trigger_effect;
        cannot_crit = newRef.cannot_crit;
        ignore_immunity_buff = newRef.ignore_immunity_buff;
        trigger_switch_actor = newRef.trigger_switch_actor;
        target_switch_actor = newRef.target_switch_actor;
        silence = newRef.silence;
        trigger_effect_list = newRef.trigger_effect_list;
        is_main_path = newRef.is_main_path;
        can_pick = newRef.can_pick;
        is_force_walk = newRef.is_force_walk;
        is_prior_tar = newRef.is_prior_tar;
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
    public Long id;

    public String sfx_id;

    public EWCGBuffType buff_type;

    public Long group_id;

    public int group_level;

    public String property;//BUff 增加的属性

    public String team_property;

    public String camp_property;

    public int max_layers;//最大层数

    public ArrayList<Long> max_layers_effect;  //最大层数触发效果

    public ArrayList<WCGActorSkill> addtion_skill_list;  //buff附带技能

    public ArrayList<Long> appear_effect;//buff出现触发效果

    public ArrayList<Long> disappear_effect;//buff消失触发效果

    public ArrayList<Long> remove_effect;  //被外部移除触发效果

    public boolean cannot_remove;

    public boolean immunity_magic;  //免疫魔法伤害

    public boolean immunity_physic;//免疫物理伤害

    public boolean immunity_control; //免疫控制效果

    public boolean ban_heal;//禁止受到治疗

    public boolean immunity_chaos;//免疫混沌伤害

    public boolean immunity_debuff;//免疫debuff

    public boolean immunity_buff;//免疫buff

    public EWCGSkillTriggerType trigger_type; //buff 被动触发类型


    public int max_trigger_count;//最大触发次数

    public int interval_check_time;//定时检测间隔

    public ArrayList<Long> interval_effect_list;//定时检测触发效果

    public int max_interval_count;//定时检测触发效果次数上限

    public ArrayList<Long> broken_effect_list;//buff被破坏的效果

    public boolean taunt;//是否嘲讽buff

    public boolean hide_blood;//隐藏血条

    public boolean cannot_select;

    public boolean ignore_collide;

    public ArrayList<EWCGDmgType> shield_type_list;

    public String shield_variable;//护盾值高级公式

    public ArrayList<EWCGDmgType> parry_type_list;

    public int parry_count;

    public int change_team;

    public String anim_trigger;

    public ArrayList<String> comp_show_list;

    public ArrayList<String> comp_hide_list;

    public boolean target_trigger_effect;

    public boolean cannot_crit;

    public boolean ignore_immunity_buff;

    public boolean trigger_switch_actor;

    public boolean target_switch_actor;

    public boolean silence;

    public String trigger_effect_list;

    public boolean is_main_path;

    public boolean can_pick;

    public boolean is_force_walk;

    public boolean is_prior_tar;


    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

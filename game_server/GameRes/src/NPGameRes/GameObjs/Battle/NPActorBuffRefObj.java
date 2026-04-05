package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.NPActorProperty.NPPropertyModifier;
import NPGameRes.Refs.Battle.RefBuff;
import WCGCommon.Enum.NPEnum.EWCGBuffType;

import java.util.List;

public class NPActorBuffRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return id;
    }

    public long id;

//    public WCGBuffSFXIdInfoObj sfx_id;

    public EWCGBuffType buff_type;

    public boolean trigger_switch_actor = false;//触发效果的触发者是否更改为buf的附加者

    public boolean target_switch_actor = false;//触发的目标是否更换为buf的附加者

    public long group_id;

    public int group_level;

    public NPPropertyModifier property;//BUff 增加的属性

    public NPTeamPropertyModifier team_property;//BUff 增加的队伍属性

    public WCGCampPropertyModifier camp_property;//BUff 增加的队伍属性

    public int max_layers;//最大层数

    public List<Long> max_layers_effect;  //最大层数触发效果

    public List<WCGActorSkill> addtion_skill_list;  //buff附带技能

    public List<Long> appear_effect;//buff出现触发效果

    public List<Long> disappear_effect;//buff消失触发效果

    public List<Long> remove_effect;  //被外部移除触发效果

    public boolean cannot_remove; //不可以被驱散

    public boolean immunity_magic;  //免疫魔法伤害

    public boolean immunity_physic;//免疫物理伤害

    public boolean immunity_control; //免疫控制效果

    public boolean ban_heal;//禁止受到治疗

    public boolean immunity_chaos;//免疫混沌伤害

    public boolean immunity_debuff;//免疫debuff

    public boolean immunity_buff;//免疫buff

    public boolean silence;//沉默

    public boolean ignore_immunity_buff;//忽略 免疫BUFF

    public boolean taunt;//是否嘲讽buff

    public boolean is_main_path;//是否主路径buff


    public boolean hide_blood;//隐藏血条

    public boolean cannot_select;

    public boolean ignore_collide;//忽略碰撞


    public boolean can_pick;//是否可偷取

    public boolean is_force_walk;//是否强制移动

    public boolean is_prior_tar;//是否优先此目标


    public List<WCGBuffTriggerTypeAndEffectInfo> trigger_effect_list;//被动触发相关信息


    public int max_trigger_count;//最大触发次数

    public int interval_check_time;//定时检测间隔

    public List<Long> interval_effect_list;//定时检测触发效果

    public int max_interval_count;//定时检测触发效果次数上限

    public int shield_type;

    public WCGVariableGroupObj shield_variable;//护盾值高级公式

    public int parry_type;

    public int parry_count;

    public List<Long> broken_effect_list;//buff被破坏的效果

    public int change_team;//变换的group id

    public String anim_trigger;

    public List<String> comp_show_list;
    public List<String> comp_hide_list;

    public String change_model;//模型变化

    public boolean cannot_crit;//不可被暴击

    public int special_type;//特效类型标记

    /**************
     * 获取对应层数的特效Id
     **/
//    public long getLayerSfxId(int _layer)
//    {
//        return Mathf.Min(sfx_id.allySfxIdList.size(), _layer);
//    }
    public void adapt(RefBuff refBuff)
    {
        this.id = refBuff.id;

        this.buff_type = refBuff.buff_type;

        this.trigger_switch_actor = refBuff.trigger_switch_actor;

        this.target_switch_actor = refBuff.target_switch_actor;

        this.group_id = refBuff.group_id;

        this.group_level = refBuff.group_level;

        this.property = NPPropertyModifier.readPropertyModifier(refBuff.property, "property");

        this.team_property = NPTeamPropertyModifier.readPropertyModifier(refBuff.team_property, "team_property");

        this.camp_property = WCGCampPropertyModifier.readPropertyModifier(refBuff.camp_property, "camp_property");

        this.max_layers = refBuff.max_layers;//最大层数

        this.max_layers_effect = refBuff.max_layers_effect;//最大层数触发效果

        this.addtion_skill_list = refBuff.addtion_skill_list;//buff附带技能

        this.appear_effect = refBuff.appear_effect;//buff出现触发效果

        this.disappear_effect = refBuff.disappear_effect;//buff消失触发效果

        this.remove_effect = refBuff.remove_effect;//被外部移除触发效果

        this.cannot_remove = refBuff.cannot_remove;//是否可以被外部移除

        this.immunity_magic = refBuff.immunity_magic;//免疫魔法伤害

        this.immunity_physic = refBuff.immunity_physic;//免疫物理伤害

        this.immunity_control = refBuff.immunity_control;//免疫控制效果

        this.ban_heal = refBuff.ban_heal;//禁止受到治疗

        this.immunity_chaos = refBuff.immunity_chaos;//免疫混沌伤害

        this.immunity_debuff = refBuff.immunity_debuff;//免疫debuff

        this.immunity_buff = refBuff.immunity_buff;//免疫buff

        this.taunt = refBuff.taunt;//是否嘲讽buff

        this.hide_blood = refBuff.hide_blood; //隐藏血条

        this.cannot_select = refBuff.cannot_select;

        this.ignore_collide = refBuff.ignore_collide;

        this.can_pick = refBuff.can_pick;//buff被动触发类型

        this.is_force_walk = refBuff.is_force_walk;//buff被动触发类型

        this.is_prior_tar = refBuff.is_prior_tar;//buff被动触发类型

        this.max_trigger_count = refBuff.max_trigger_count;//最大触发次数

        this.interval_check_time = refBuff.interval_check_time;//定时检测间隔

        this.interval_effect_list = refBuff.interval_effect_list;//定时检测触发效果

        this.max_interval_count = refBuff.max_interval_count;//定时检测触发效果次数上限

        for (int i = 0; i < refBuff.shield_type_list.size(); i++)
        {
            this.shield_type |= 1 << refBuff.shield_type_list.get(i).ordinal();
        }

        this.shield_variable = WCGVariableGroupObj.readVariableGroup(refBuff.shield_variable, "护盾高级公式错误： ");//护盾值高级公式

        for (int i = 0; i < refBuff.parry_type_list.size(); i++)
        {
            this.parry_type |= 1 << refBuff.parry_type_list.get(i).ordinal();
        }

        this.parry_count = refBuff.parry_count;

        this.broken_effect_list = refBuff.broken_effect_list;//buff被破坏的效果

        this.change_team = refBuff.change_team;

        this.anim_trigger = refBuff.anim_trigger;

        this.comp_show_list = refBuff.comp_show_list;
        this.comp_hide_list = refBuff.comp_hide_list;
        this.cannot_crit = refBuff.cannot_crit;

        this.ignore_immunity_buff = refBuff.ignore_immunity_buff;

        this.silence = refBuff.silence;
        this.trigger_effect_list = WCGBuffTriggerTypeAndEffectInfo._readBuffTriggerList(refBuff.trigger_effect_list);
        this.is_main_path = refBuff.is_main_path;

    }
}

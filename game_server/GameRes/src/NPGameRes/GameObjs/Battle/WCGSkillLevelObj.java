package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.AITrigger.WCGAITriggerObj;
import NPGameRes.GameObjs.NPActorProperty.NPPropertyModifier;
import NPGameRes.Refs.Battle.RefSkillActiveEffectGroup;
import NPGameRes.Refs.Battle.RefSkillLevel;
import NPGameRes.Refs.Battle.RefSkillTriggerEffectGroup;
import WCGCommon.Enum.NPEnum.EWCGSkillRollingTimeType;

import java.util.ArrayList;
import java.util.List;


public class WCGSkillLevelObj
{
    public long id;
    public int minLevel;//技能等级
    public String action_name;//技能动作名
    public int action_time;//动作时长 毫秒
    public int cast_distance; //释放距离  厘米

    public long cd_group;//cd 组
    public int cdTime;//技能CD 毫秒
    public NPPropertyModifier skill_property;  //技能增加属性
    public NPPropertyModifier skill_property_up; //技能增加属性等级改变值
    public NPTeamPropertyModifier team_property;//增加的队伍属性
    public List<WCGSkillAudioTimeLine> aduio_time_Line_List = new ArrayList<>(); //时间轴-播放音效集合
    public List<WCGSkillActiveEffectGroup> active_effect_list_groups = new ArrayList<>();//主动触发效果组，直接由技能id释放触发的效果
    public List<WCGSkillTriggerEffectGroup> trigger_effect_list_groups = new ArrayList<>();//被动触发效果组
    public int aura_interval;//光环间隔时间
    public List<Long> aura_sfx_id;//光环特效id
    public List<WCGAITriggerObj> interval_trigger = new ArrayList<WCGAITriggerObj>();//光环触发事件
    public boolean is_tar_pos;//攻击目标是位置还是战斗单位，true->POS   false->actor

    public boolean relation_atk_speed;//是否与攻击速度关联
    public boolean cd_rel_atk_speed;//cd是否与攻速关联

    public List<Long> skill_use_effect;//技能使用时触发的效果队列
    public int skill_rolling_time;//前摇时间 毫秒
    public List<Long> skill_break_effect_list;//技能被打断时触发的效果列表
    public EWCGSkillRollingTimeType skill_rolling_ui_type;//前摇时间UI展示类型枚举


    //通过组ID获取主动技能效果组
    public WCGSkillActiveEffectGroup getActiveEffectGroupByGroupId(int _groupId)
    {
        if (active_effect_list_groups == null)
            return null;

        WCGSkillActiveEffectGroup tmp = null;
        for (int i = 0; i < active_effect_list_groups.size(); i++)
        {
            tmp = active_effect_list_groups.get(i);
            if (null == tmp)
                continue;

            if (tmp.group_id == _groupId)
                return tmp;
        }
        return null;
    }


    //通过组ID获取被动技能效果组
    public WCGSkillTriggerEffectGroup getTriggerEffectGroupByGroupId(int _groupId)
    {
        if (trigger_effect_list_groups == null)
            return null;
        WCGSkillTriggerEffectGroup tmp = null;
        for (int i = 0; i < trigger_effect_list_groups.size(); i++)
        {
            tmp = trigger_effect_list_groups.get(i);
            if (null == tmp)
                continue;

            if (tmp.group_id == _groupId)
                return tmp;
        }
        return null;
    }

    public void adapt(RefSkillLevel refSkill)
    {
        this.id = refSkill.id;
        this.minLevel = refSkill.minLevel;
        this.action_name = refSkill.action_name;
        this.action_time = refSkill.action_time;
        this.cast_distance = refSkill.cast_distance;

        this.cd_group = refSkill.cd_group;
        this.cdTime = refSkill.cdTime;
        this.skill_property = NPPropertyModifier.readPropertyModifier(refSkill.skill_property, "skill_property");
        this.skill_property_up = NPPropertyModifier.readPropertyModifier(refSkill.skill_property_up, "skill_property_up");
        this.team_property = NPTeamPropertyModifier.readPropertyModifier(refSkill.team_property, "team_property");
        this.aduio_time_Line_List = WCGSkillAudioTimeLine.readSkillAudioTimeLine(refSkill.audio_timeline);

        this.aura_interval = refSkill.aura_interval;
        this.aura_sfx_id = refSkill.aura_sfx_id;
        this.interval_trigger = WCGAITriggerObj.readTriggerList(refSkill.interval_trigger);
        this.relation_atk_speed = refSkill.relation_atk_speed;
        this.cd_rel_atk_speed = refSkill.cd_rel_atk_speed;
        this.skill_rolling_time = refSkill.skill_rolling_time;
        this.skill_break_effect_list = refSkill.skill_break_effect_list;
        this.skill_rolling_ui_type = refSkill.skill_rolling_ui_type;
        this.skill_use_effect = refSkill.skill_use_effect;

        if (this.minLevel <= 0)
        {
            System.out.println("///////////////////   Error   //////////////////////////");
            System.out.println("Skill Level: " + this.id + " minLevel <= 0 !!!!");
            System.out.println("////////////////////////////////////////////////////////");
        }

        //read active_effect_list;
        for (RefSkillTriggerEffectGroup ref : RefSkillTriggerEffectGroup.getMgr().getList())
        {
            if (ref.id == refSkill.id && ref.level == this.minLevel)
            {
                WCGSkillTriggerEffectGroup group = new WCGSkillTriggerEffectGroup();
                group.adapt(ref);
                this.trigger_effect_list_groups.add(group);
            }
        }


        for (RefSkillActiveEffectGroup ref : RefSkillActiveEffectGroup.getMgr().getList())
        {
            if (ref.id == refSkill.id && ref.level == this.minLevel)
            {
                WCGSkillActiveEffectGroup group = new WCGSkillActiveEffectGroup();
                group.adapt(ref);
                this.active_effect_list_groups.add(group);
            }
        }

    }
}


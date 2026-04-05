package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefTeamSkill;
import WCGCommon.Enum.NPEnum.EWCGTeamSkillClass;
import WCGCommon.Enum.NPEnum.EWCGTeamSkillEffectType;
import WCGCommon.Enum.NPEnum.EWCGTeamSkillSignType;

import java.util.List;

public class WCGTeamSkillRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return id;
    }

    public long id;//id

    public String skill_icon;

    public int radius;//技能半径  厘米

    public boolean default_use;//默认在战场中

    public List<WCGTeamTriggerObj> trigger_list;

    public int init_cd;//初始CD,表示该技能刚进入战斗时候的CD
    public List<Integer> cd_list;//cd列表

    public int min_cd_ms;//最小cd  无法用资源重置

    public List<WCGActorCost> reset_cost;//重置cd消耗金币

    public String name;//名字
    public String desc;//描述

    public int use_num;//使用次数  -1代表无限使用 0代表没有使用此时 >=1标示具体使用次数，使用一次，这个次数会减1 当使用次数为0是，删除该指挥官技能

    public boolean is_can_be_replece;//是否可被替换

    public EWCGTeamSkillClass skill_class;//技能类型
    public EWCGTeamSkillEffectType skill_effect_type;//指挥官技能效果枚举
    public long max_radius;//显示范围(外半径)

    public float horizontal_off_set;//指挥官技能的水平偏移量
    public float vertical_off_set;//指挥官技能的垂直偏移量
    public WCGBothConditionGroupObj target_select_contition;//目标选择条件
    public boolean show_target_select;//是否进行目标标记展示
    public boolean show_dispatch_area;//是否展示放兵区域赋值指示

    public EWCGTeamSkillSignType sign_type;//指挥官技能标记类型


    public void adapt(RefTeamSkill ref)
    {
        this.id = ref.id;
        this.skill_icon = ref.skill_icon;
        this.radius = ref.radius;
        this.default_use = ref.default_use;
        this.trigger_list = WCGTeamTriggerObj.readTriggerList(ref.trigger_list);
        this.init_cd = ref.init_cd;
        this.cd_list = ref.cd_list;
        this.min_cd_ms = ref.min_cd_ms;
        this.reset_cost = WCGActorCost.readCostList(ref.reset_cost);
        this.name = ref.name;
        this.desc = ref.desc;
        this.use_num = ref.use_num;
        this.is_can_be_replece = ref.is_can_be_replece;
        this.skill_class = ref.skill_class;
        this.skill_effect_type = ref.skill_effect_type;
        this.max_radius = ref.max_radius;

        this.target_select_contition = WCGBothConditionGroupObj.readConditionGroupList(ref.target_select_contition);
        this.show_target_select = ref.show_target_select;
        this.show_dispatch_area = ref.show_dispatch_area;
        this.sign_type = ref.sign_type;


    }
}


package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefActorHeroSkill;
import WCGCommon.Enum.NPEnum.EWCGEffectRangeType;
import WCGCommon.Enum.NPEnum.EWCGSkillTrackingMethod;

import java.util.List;

public class WCGHeroSkill
{
    public long actor_id;//卡牌静态ID
    public int actor_lvl;//卡牌等级
    public long skill_id;//对应的WCGSkillRefObj Id
    public String tar_info;//检索目标的处理字符串
    public WCGSingleConditionGroupObj condition_list;//释放技能的条件(能否释放使用这个来判断)
    public WCGSingleConditionGroupObj condition_show_list;//用于判断英雄技能是否在头顶展示（能否显示使用这个来判断）
    public List<Long> init_effect;
    public int cast_range;//释放范围,外圈半径
    public int cast_min_range;//释放范围,内圈半径
    public EWCGEffectRangeType range_type;//范围类型
    public int width;//范围宽度
    public int height;//范围长度
    public EWCGSkillTrackingMethod tracking_method;//技能追踪方式
    public float angle;//扇形角度

    public float horizontal_off_set;//英雄技能的水平偏移量
    public float vertical_off_set;//英雄技能的垂直偏移量

    public void adapt(RefActorHeroSkill ref)
    {
        this.actor_id = ref.actor_id;
        this.actor_lvl = ref.actor_lvl;
        this.skill_id = ref.skill_id;
        this.cast_range = ref.cast_range;
        this.cast_min_range = ref.cast_min_range;
        this.range_type = ref.range_type;
        this.width = ref.width;
        this.height = ref.height;
        this.tracking_method = ref.tracking_method;
        this.init_effect = ref.init_effect;
        this.condition_list = WCGSingleConditionGroupObj.readConditionGroupList(ref.condition_list, "读取 WCGHeroSkill.condition_list 失败");
        this.condition_show_list = WCGSingleConditionGroupObj.readConditionGroupList(ref.condition_show_list, "读取 WCGHeroSkill.condition_show_list 失败");
        this.tar_info = ref.tar_info;
        this.angle = ref.angle;
        this.horizontal_off_set = ref.horizontal_off_set;
        this.vertical_off_set = ref.vertical_off_set;
    }
}

package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefEffect;
import WCGCommon.Enum.NPEnum.EWCGEffectRangeSelectPos;
import WCGCommon.Enum.NPEnum.EWCGEffectRangeType;
import WCGCommon.Enum.NPEnum.EWCGSortCondition;

import java.util.List;

public class WCGEffectRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return id;
    }

    public long id;//效果id
    public List<Long> sfx_id;//目标特效
    public int relation_type;//作用目标关系类型
    public boolean is_target_rel;//是否使用目标进行关系判断
    public EWCGEffectRangeType range_type;//范围类型
    public EWCGEffectRangeSelectPos range_pos;//范围选择位置
    public int width;//范围宽度  (扇形表示半角的正弦值)  矩形表示宽度
    public int height;//范围长度   厘米
    public EWCGSortCondition sort_condition;//选择目标按照这个条件进行排序（目标排序条件）
    public int effect_target_count;//效果作用目标上限

    public WCGBothConditionGroupObj condition_info_list;//筛选条件列表

    public List<WCGEffectSerializeInfo> effect_info_list;//效果信息列表

    public boolean select_all;  //选择所有目标
    public boolean without_target;  //是否剔除目标

    public boolean use_crit;
    public boolean use_dodge;
    public int effect_info_list_chance;//effect_info_list触发的概率

    public void adapt(RefEffect ref)
    {
        this.id = ref.id;
        this.sfx_id = ref.sfx_id;
        for (int i = 0; i < ref.relation_type.size(); i++)
        {
            this.relation_type |= 1 << ref.relation_type.get(i).ordinal();
        }

        this.is_target_rel = ref.is_target_rel;

        this.range_type = ref.range_type;
        this.range_pos = ref.range_pos;

        this.width = ref.width;
        this.height = ref.height;
        this.sort_condition = ref.sort_condition;
        this.effect_target_count = ref.effect_target_count;
        this.condition_info_list = WCGBothConditionGroupObj.readConditionGroupList(ref.condition_info_list);
        this.effect_info_list = WCGEffectSerializeInfo.readEffectList(ref.effect_info_list);
        this.select_all = ref.select_all;
        this.without_target = ref.without_target;
        this.use_crit = ref.use_crit;
        this.use_dodge = ref.use_dodge;
        this.effect_info_list_chance = ref.effect_info_list_chance;

    }
}
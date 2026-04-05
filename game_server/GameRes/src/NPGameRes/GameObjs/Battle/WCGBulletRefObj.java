package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefBullet;
import WCGCommon.Enum.NPEnum.EWCGBulletType;

import java.util.ArrayList;
import java.util.List;

public class WCGBulletRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return id;
    }

    public long id;

    public List<Long> sfx_id;//特效名称
    public long hit_sfx_id;

    public int speed;//子弹速度  厘米/秒
    public int width;//子弹宽度

    public EWCGBulletType bullet_type;
    public int bullet_length;//子弹距离  厘米

    public WCGBothConditionGroupObj end_condition;
    public WCGBothConditionGroupObj check_tar_enable_condition; //目标确认条件

    public ArrayList<Long> range_effect_list;//经过区域产生效果

    public int active_effect_delay_time;//触发延迟时间  毫秒
    public List<Long> active_effect_list;//触发效果id列表
    public int active_pos_effect_delay_time;//触发延迟时间  毫秒
    public List<Long> active_pos_effect_list;//位置效果列表
    public int max_target_quantity;//允许命中的目标最大数量


    //是否在达到最大击中数后销毁子弹
    public boolean max_hit_end;
    //是否在达到最大击中时触发active效果
    public boolean max_hit_trigger;
    //是否在轨迹结束的时候以width作为宽度涵盖半径内的所有单位
    public boolean end_circle;

    public void adapt(RefBullet ref)
    {
        this.id = ref.id;
        this.sfx_id = ref.sfx_id;
        this.hit_sfx_id = ref.hit_sfx_id;
        this.speed = ref.speed;
        this.width = ref.width;
        this.bullet_type = ref.bullet_type;
        this.bullet_length = ref.bullet_length;
        this.range_effect_list = ref.range_effect_list;
        this.active_effect_delay_time = ref.active_effect_delay_time;
        this.active_effect_list = ref.active_effect_list;
        this.max_target_quantity = ref.max_target_quantity;
        this.end_condition = WCGBothConditionGroupObj.readConditionGroupList(ref.end_condition);
        this.check_tar_enable_condition = WCGBothConditionGroupObj.readConditionGroupList(ref.check_tar_enable_condition);
        this.active_pos_effect_delay_time = ref.active_pos_effect_delay_time;
        this.active_pos_effect_list = ref.active_pos_effect_list;

        this.max_hit_end = ref.max_hit_end;
        this.max_hit_trigger = ref.max_hit_trigger;
        this.end_circle = ref.end_circle;

    }
}
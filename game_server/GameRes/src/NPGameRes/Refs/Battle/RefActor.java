package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;
import NPEnum.EQuality;
import WCGCommon.Enum.NPEnum.*;

import java.util.ArrayList;

/**
 * 角  色  信  息
 * @author scott
 */
@RefTable(tableName = "actor_ref")
public class RefActor extends RefBase
{
    private static RefListContainer<RefActor> _g_mgr = new RefListContainer<RefActor>();

    public static RefListContainer<RefActor> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefActor> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefActor>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActor newRef = (RefActor) _newRef;
        id = newRef.id;
        race_id = newRef.race_id;
        name = newRef.name;
        blood_pos_y = newRef.blood_pos_y;
        operation_pos_y = newRef.operation_pos_y;
        introduce = newRef.introduce;
        icon = newRef.icon;
        quality = newRef.quality;
        move_type = newRef.move_type;
        actor_type = newRef.actor_type;
        view_range = newRef.view_range;
        special_type = newRef.special_type;
        property = newRef.property;
        team_property = newRef.team_property;
        always_no_select = newRef.always_no_select;
        ignore_collide = newRef.ignore_collide;
        width = newRef.width;
        length = newRef.length;
        weight = newRef.weight;
        radius = newRef.radius;
        actor_unit_type = newRef.actor_unit_type;
        attack_range = newRef.attack_range;
        actor_event_type = newRef.actor_event_type;
        birth_delay_create_time_ms = newRef.birth_delay_create_time_ms;
        seat_actor_id = newRef.seat_actor_id;
        seat_actor_level = newRef.seat_actor_level;
        kill_notification = newRef.kill_notification;
        actor_camera_height = newRef.actor_camera_height;
        blood_length = newRef.blood_length;
        blood_widgth = newRef.blood_widgth;
        patrol_radius = newRef.patrol_radius;
        is_ground_building = newRef.is_ground_building;
        working_buf = newRef.working_buf;
        def_rang_show = newRef.def_rang_show;
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

    public long id;
    public long race_id;//种族ID
    public String name;//名字
    public int blood_pos_y;//角色血条显示高度
    public int operation_pos_y;//角色头顶操作区域显示高度
    public String introduce;//介绍
    public String icon;//图标
    public EQuality quality;//品质
    public EWCGMoveType move_type;//是否空中单位
    public EWCGActorType actor_type;//角色类型
    public int view_range;//视野范围  厘米
    public ArrayList<EWCGActorSpecialType> special_type;//特效类型标记
//    public ENPPetElementType element_type;//角色的属性类型
    public String property;  //角色基础属性对象
    public String team_property;
    public boolean always_no_select;
    public boolean ignore_collide;

    public int width;//单位占据宽  格子数
    public int length;//单位占据长  格子数
    public int weight;//单位重量
    public int radius;//单位半径   厘米

    public EWCGActorUnitType actor_unit_type;
    public int attack_range;
    public EWCGActorEventType actor_event_type;
    public long birth_delay_create_time_ms;
    public long seat_actor_id;
    public int seat_actor_level;
    public boolean kill_notification;
    public float actor_camera_height;
    public float blood_length;
    public float blood_widgth;
    public int patrol_radius;
    public boolean is_ground_building;
    public ArrayList<Long> working_buf;
    public int def_rang_show = 2000;
}

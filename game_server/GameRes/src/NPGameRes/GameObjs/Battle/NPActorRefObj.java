package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.NPActorProperty.NPPropertyModifier;
import NPGameRes.Refs.Battle.RefActor;
import NPGameRes.Refs.Battle.RefActorLevel;
import WCGCommon.Enum.NPEnum.*;

import java.util.ArrayList;
import java.util.List;

public class NPActorRefObj extends _IALBasicRefObj
{
    @Override
    public long _refId()
    {
        return id;
    }

    public long id;
    public long race_id;//种族ID
    public EWCGMoveType move_type;//是否空中单位
    public EWCGActorType actor_type;//角色类型
    public EWCGActorEventType actor_event_type;//角色事件类型
    public EWCGActorUnitType actor_unit_type;//角色分类类型

    public List<EWCGActorSpecialType> special_type_list;//特效类型标记
    public int special_type;//特效类型标记
    public boolean is_ground_building;//是否地面的建筑单位，此类单位可行走，不占用行走区域

//    public ENPPetElementType element_type;//角色的属性类型
    public NPPropertyModifier property;  //角色基础属性
    public NPTeamPropertyModifier team_property;//增加的队伍属性
    public boolean ignore_collide;//忽略单位碰撞
    public boolean kill_notification;//是否有击杀通知

    public int width;//单位占据宽  格子数
    public int length;//单位占据长  格子数
    public int view_range;//视野半径  厘米

    public int weight;//单位重量
    public int radius;//单位半径   厘米
    public int patrol_radius;//单位子对象巡逻的半径   厘米
    public int def_rang_show;//建筑防御范围

    public int cost_population;//占用人口

    public boolean can_get;//玩家能否获得 默认空为FALSE 

    //下面参数客户端使用
    public boolean always_hide_blood;//是否永久隐藏血条
    public boolean always_no_select;//是否永久无法选择
    public String name;//名字Id（语言表）
    public String introduce;//简介
    public String details;//详情
    public String card_info_location;//展示该单位的特点(对应语言表的key)
    public String icon;//图标
    public String small_icon;//小图标
    public String blood_icon;//血条图标，用于展示该兵种的定位(1,2,3本兵)
    public float actor_camera_height; //角色相机高度
    public float blood_length;// 血条长度比例
    public float blood_widgth;// 血条宽度比例
    public int blood_pos_y;//角色血条显示高度
    public int operation_pos_y;//角色头顶操作区域显示高度
    public Vector3 bullet_offset;//子弹发射偏移
    public Vector3 hit_offset;//子弹命中偏移

    public int foot_height;//脚底高度
    public int head_height;//头顶高度
    public int center_height;//重心高度

    public long birth_sfx_id;//出生特效Id
    public long birth_delay_create_time_ms;//出生时延迟创建的时间（毫秒）
    public long seat_actor_id;//出生时占位对象的Id和等级信息
    public int seat_actor_level;
    public boolean science_or_not;//该Actor的相关操作是否显示在科技面板
    public int operation_priority;  // 用于操作的推荐排序

    public long death_sfx_id;

    public EWCGOperationGridType operation_grid_type;//头顶操作条样式

    public int attack_range;//攻击范围, 整形(单位:厘米,显示单位:米,取小数点后一位,向上取整)
    public long production_complete;//单位生产完成音效
    public long appearance_sound;//单位出场音效
    public long dead_sound;//单位死亡音效
    public EWCGHitMaterialType weapon_material_Type;//武器材质 
    public EWCGDefenseMaterialType body_material_Type;//身体材质

    public long buildingtouch_sound;  //单位点击音效
    public float sfx_size;//SFX缩放比例

    public List<Long> working_buf;//建筑单位生产时需要添加的Buff列表

    public boolean minimap_not_show; //是否在小地图上不显示
    public boolean minimap_fog_show; //小地图迷雾下也显示

    public ArrayList<NPActorLevel> level_info;//等级信息列表
    private ArrayList<NPSkinPropertyRefObj> skin_property_list = new ArrayList<NPSkinPropertyRefObj>();//皮肤相关加成信息列表
    private int _m_LevelFullNeedChipNum;

    //获取皮肤参数对象
    public NPSkinPropertyRefObj getSkinPropertyInfo(long _skinPropertyID)
    {
        if (skin_property_list == null)
            return null;

        NPSkinPropertyRefObj tarRefInfo;
        for (int i = 0; i < skin_property_list.size(); i++)
        {
            tarRefInfo = skin_property_list.get(i);

            if (tarRefInfo == null)
                continue;

            if (tarRefInfo.skin_id == _skinPropertyID)
                return tarRefInfo;
        }

        return null;
    }

    //增加皮肤属性信息
    public void addSkinPropertyRefInfo(List<NPSkinPropertyRefObj> _skinPropertyRefList)
    {
        if (_skinPropertyRefList == null)
            return;

        if (skin_property_list == null)
            skin_property_list = new ArrayList<NPSkinPropertyRefObj>();

        NPSkinPropertyRefObj tempSkinRef;

        for (int i = 0; i < _skinPropertyRefList.size(); i++)
        {
            tempSkinRef = _skinPropertyRefList.get(i);

            if (tempSkinRef == null || tempSkinRef.actor_id != id)
                continue;

            skin_property_list.add(tempSkinRef);
        }
    }

    public int getLevelFullNeedChipNum()
    {
        return _m_LevelFullNeedChipNum;
    }


    //获取等级内的最大等级信息
    public NPActorLevel GetMaxLevelInfo(int _lv)
    {
        if (_lv <= 0)
            _lv = 1;

        NPActorLevel actorLevel = null;
        NPActorLevel maxLevelInfo = null;
        for (int i = 0; i < level_info.size(); i++)
        {
            actorLevel = level_info.get(i);
            if (actorLevel.min_level == _lv)
            {
                return actorLevel;
            }

            if (actorLevel.min_level < _lv)
            {
                if (null == maxLevelInfo || maxLevelInfo.min_level < actorLevel.min_level)
                    maxLevelInfo = actorLevel;
            }
        }
        return maxLevelInfo;
    }

    private RefActor _m_ref = null;

    public void adapt(RefActor ref)
    {
        _m_ref = ref;
        this.id = ref.id;
        this.race_id = ref.race_id;//种族ID
        this.name = ref.name;//名字
        this.blood_pos_y = ref.blood_pos_y;//角色血条显示高度
        this.operation_pos_y = ref.operation_pos_y;//角色头顶操作区域显示高度
        this.introduce = ref.introduce;//介绍
        this.icon = ref.icon;//图标
        this.move_type = ref.move_type;//是否空中单位
        this.actor_type = ref.actor_type;//角色类型
        this.actor_event_type = ref.actor_event_type;
        this.view_range = ref.view_range;//视野范围  厘米
        this.weight = ref.weight;
        this.radius = ref.radius;

//        this.element_type = ref.element_type;//角色属性分类
        this.property = NPPropertyModifier.readPropertyModifier(ref.property, "property");
        this.team_property = NPTeamPropertyModifier.readPropertyModifier(ref.team_property, "team_property");
        this.ignore_collide = ref.ignore_collide;
        this.always_no_select = ref.always_no_select;
        this.actor_unit_type = ref.actor_unit_type;
        for (int i = 0; i < ref.special_type.size(); i++)
        {
            this.special_type |= 1 << (int) ref.special_type.get(i).ordinal();
        }
        this.width = ref.width;//单位占据宽  格子数
        this.length = ref.length;//单位占据长  格子数

        //this.bullet_offset=ref.bullet_offset;//子弹发射偏移
        //this. hit_offset=ref.hit_offset;//子弹命中偏移

        this.attack_range = ref.attack_range;//攻击范围,浮点型 (单位:米,显示单位:米,取小数点后一位,向上取整)
        this.birth_delay_create_time_ms = ref.birth_delay_create_time_ms;//出生时延迟创建的时间（毫秒）
        this.seat_actor_id = ref.seat_actor_id;//出生时占位对象的Id和等级信息
        this.seat_actor_level = ref.seat_actor_level;
        this.kill_notification = ref.kill_notification;
        this.actor_camera_height = ref.actor_camera_height;
        this.blood_length = ref.blood_length;// 血条长度比例
        this.blood_widgth = ref.blood_widgth;// 血条宽度比例
        this.patrol_radius = ref.patrol_radius;
        this.is_ground_building = ref.is_ground_building;
        this.working_buf = ref.working_buf;
        this.def_rang_show = ref.def_rang_show;

        this.level_info = new ArrayList<NPActorLevel>();
        for (RefActorLevel refLevel : RefActorLevel.getMgr().getList())
        {
            if (refLevel.id == this.id)
            {
                NPActorLevel actorLvlObj = new NPActorLevel();
                actorLvlObj.adapt(refLevel);
                this.level_info.add(actorLvlObj);
            }
        }
        
        //计算总升级需要碎片数量
        // _m_LevelFullNeedChipNum =calcLevelupNeedChipNum(getMaxCardLevel());

        // _m_qualityRef = WCGRefCoreMgr.getInstance().actorQualityMap.getRef((long)this.quality.ordinal());;
    }


    public RefActor ref()
    {
        return _m_ref;
    }
}

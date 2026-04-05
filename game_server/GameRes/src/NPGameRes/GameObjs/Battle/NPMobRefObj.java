package NPGameRes.GameObjs.Battle;

import NPEnum.EQuality;
import NPGameRes.GameObjs.NPActorProperty.NPPropertyContainer;
import NPGameRes.GameObjs.NPActorProperty.NPPropertyModifier;
import NPGameRes.NPResFunc;
import NPGameRes.Refs.Battle.RefMapMobRef;

import java.util.ArrayList;
import java.util.List;

public class NPMobRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return id;
    }

    public long id;
    public long pet_id;                //卡牌Id
    public long army_id;                //军队Id
    public long actor_id;
    public short star;
    public int add_level;               //相对关卡基本等级增加的对应单位等级信息 

    public int lvl(int _basicLvl)
    {
        return _basicLvl + add_level;
    }

    public EQuality quality;
    public long skin_property_id;
    public List<Long> death_effect_id;//死亡触发效果

    public List<WCGAddBuffInfo> birth_buf_list;//出生附带的buf信息
    public List<Long> birth_effect_id;//出生触发效果
    public NPPropertyModifier addition_property;//属性附加值
    public NPTeamPropertyModifier team_property;//队伍属性加成

    public int new_ai_id;//替换原有的AI的Id
    public int new_defence_ai_id;//替换原有的AI的Id

    //本对象对应的卡牌数据对象，如果无卡牌数据对象则为null
//    private RefPet _m_crCardRef;

    public ArrayList<WCGActorSkill> skill_info_list = new ArrayList<>();// 其他技能等级列表
    public ArrayList<WCGActorSkill> active_skill_info = new ArrayList<>();// 主动技能等级列表

    public void adapt(RefMapMobRef ref)
    {
        this.id = ref.id;
        this.pet_id = ref.pet_id;
        this.actor_id = ref.actor_id;
        this.star = ref.star;
        this.add_level = ref.add_level;
        this.quality = ref.quality;
        this.skin_property_id = ref.skin_id;
        this.death_effect_id = ref.death_effect_id;

        this.birth_effect_id = ref.birth_effect_id;
        this.addition_property = NPPropertyModifier.readPropertyModifier(ref.addition_property, "");
        this.team_property = NPTeamPropertyModifier.readPropertyModifier(ref.team_property, "");
        this.new_ai_id = ref.new_ai_id;
        this.new_defence_ai_id = ref.new_defence_ai_id;
        this.birth_buf_list = WCGAddBuffInfo.readBuffInfoList(ref.birth_buf_list);
        this.skill_info_list.addAll(ref.skill_info_list);
        this.active_skill_info.addAll(ref.active_skill_info);

//        _m_crCardRef = RefPet.getMgr().get(this.pet_id);
//        if (0 != this.pet_id && null == _m_crCardRef)
//        {
//            //报警，查询不到有效数据
//            ALServerLog.Error("Can not find card for mob ref!!  Card id: " + this.pet_id);
//        }

    }

    public long getActorId()
    {
//        //判断卡牌类型是否有效
//        if (null != _m_crCardRef)
//        {
//            //获取卡牌数据
//            return _m_crCardRef.actor_id;
//            return 0;
//        } else
//        {
//            return actor_id;
//        }
        return 1;
    }

    public void getPropertyBonus(int _basicLvl, NPPropertyContainer _recPropertyBonus)
    {
        NPResFunc.calcMobLvlProperty(pet_id, lvl(_basicLvl), _recPropertyBonus);
    }
}
package NPGameRes.Refs.Hero;

import CommonEnum.ESpecAttrType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.EQuality;
import NPGameRes.Refs.Consort.RefConsort;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "hero")
public class RefHero extends RefBase
{
    private static RefTableContainer<RefHero> _g_mgr = new RefTableContainer<RefHero>();

    public static RefTableContainer<RefHero> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHero> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHero>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHero newRef = (RefHero) _newRef;
        id = newRef.id;
        spec_attr_type = newRef.spec_attr_type;
        unlock_gain_item_list = newRef.unlock_gain_item_list;
        default_skin_id = newRef.default_skin_id;
        default_talent_skill_id_list = newRef.default_talent_skill_id_list;
        extra_talent_skill_id_list = newRef.extra_talent_skill_id_list;
        auto_upgrade_talent_skill_id_list = newRef.auto_upgrade_talent_skill_id_list;
        star_skill_id_list = newRef.star_skill_id_list;
        business_skill_id = newRef.business_skill_id;
        extra_business_skill_id_list = newRef.extra_business_skill_id_list;
        halo_id = newRef.halo_id;
        suit_id = newRef.suit_id;
        can_star_upgrade = newRef.can_star_upgrade;
        hero_star_group_id = newRef.hero_star_group_id;
        settled_building_attr_type_list = newRef.settled_building_attr_type_list;
        quality = newRef.quality;
        name = newRef.name;
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

    public long id; // 唯一id
    public ESpecAttrType spec_attr_type = ESpecAttrType.NONE;//特长属性类型
    public List<NPCommonCostItem> unlock_gain_item_list = new ArrayList<>();//获得时的奖励
    public long default_skin_id;//默认皮肤
    public List<Long> default_talent_skill_id_list = new ArrayList<>();//默认资质技能id列表
    public List<WCGPairLong> extra_talent_skill_id_list = new ArrayList<>();//额外解锁资质技能id列表(星级:技能)
    public List<Long> auto_upgrade_talent_skill_id_list = new ArrayList<>();//自动升阶的资质技能id列表
    public List<Long> star_skill_id_list = new ArrayList<>();//觉醒技能列表(跟随星级提升等级的技能)
    public long business_skill_id;//初始经营技能id
    public List<WCGPairLong> extra_business_skill_id_list = new ArrayList<>();//额外解锁经营技能id列表(大臣等级:技能)
    public long halo_id;//光环id列表
    public long suit_id;//套系id列表
    public boolean can_star_upgrade;//能否升星
    public int hero_star_group_id;//升星组ID
    public List<ESpecAttrType> settled_building_attr_type_list;//可入驻的建筑相性列表
    public EQuality quality;//品质
    public String name;//名称

    public boolean isDefaultTalentSkill(long _talentSkillId)
    {
        return default_talent_skill_id_list.contains(_talentSkillId);
    }

    /**************
     * 下面是所有数据读取完成之后的初始化处理数据
     */

    // 大臣配套的皮肤数据
    @RefField(isIgnore =  true)
    private ArrayList<RefHeroSkin> _m_lSkinList = new ArrayList<>();
    public ArrayList<RefHeroSkin> getSkinList() { return _m_lSkinList; }
    public void setSkinList(ArrayList<RefHeroSkin> _list) { _m_lSkinList = _list; }

    // 大臣关联妃子
    @RefField(isIgnore =  true)
    private ArrayList<RefConsort> _m_alRelationConsortList = new ArrayList<>();
    public ArrayList<RefConsort> getRelationConsortList() { return _m_alRelationConsortList; }
    public void setRelationConsortList(ArrayList<RefConsort> _list) { _m_alRelationConsortList = _list; }
}

package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.BuildingCondition.BuildingConditionGroupObj;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;

@RefTable(tableName = "hero_business_skill")
public class RefHeroBusinessSkill extends RefBase
{
    private static RefTableContainer<RefHeroBusinessSkill> _g_mgr = new RefTableContainer<RefHeroBusinessSkill>();

    public static RefTableContainer<RefHeroBusinessSkill> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroBusinessSkill> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroBusinessSkill>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroBusinessSkill newRef = (RefHeroBusinessSkill) _newRef;
        id = newRef.id;
        can_upgrade = newRef.can_upgrade;
        limit_range = newRef.limit_range;
        business_skill_lvl_max = newRef.business_skill_lvl_max;
        bonus_prop_modifier = newRef.bonus_prop_modifier;
        bonus_prop_modifier_per_level = newRef.bonus_prop_modifier_per_level;
        upgrade_cost_group = newRef.upgrade_cost_group;
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

    public long id;//技能id
    public boolean can_upgrade;//是否可以升级
    public BuildingConditionGroupObj limit_range;//限制范围
    public int business_skill_lvl_max;//技能等级上限
    public PlayerBonusPropertyModifier bonus_prop_modifier;//初始加成
    public PlayerBonusPropertyModifier bonus_prop_modifier_per_level;//每级加成增长值
    public int upgrade_cost_group;//消耗组

    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefHeroBusinessSkillUpgrade> _m_levelMapMgr = new _TLevelMapMgr<>();
    public _TLevelMapMgr<RefHeroBusinessSkillUpgrade> getLevelMapMgr()
    {
        return _m_levelMapMgr;
    }
}

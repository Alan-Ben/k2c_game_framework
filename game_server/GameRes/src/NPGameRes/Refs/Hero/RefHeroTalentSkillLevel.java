package NPGameRes.Refs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;

@RefTable(tableName = "hero_talent_skill_level")
public class RefHeroTalentSkillLevel extends RefBase implements _ILevelBasicObj
{
    private static RefTableContainer<RefHeroTalentSkillLevel> _g_mgr = new RefTableContainer<RefHeroTalentSkillLevel>();

    public static RefTableContainer<RefHeroTalentSkillLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroTalentSkillLevel> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroTalentSkillLevel>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroTalentSkillLevel newRef = (RefHeroTalentSkillLevel) _newRef;
        id = newRef.id;
        talent_skill_id = newRef.talent_skill_id;
        level = newRef.level;
        cost = newRef.cost;
        cost_per_level = newRef.cost_per_level;
        self_attr_prop_modifier = newRef.self_attr_prop_modifier;
        self_attr_prop_modifier_per_level = newRef.self_attr_prop_modifier_per_level;
        union_bonus = newRef.union_bonus;
        union_bonus_per_level = newRef.union_bonus_per_level;
    }

    /**
     * 获取等级
     * @return
     */
    public int getLevel(){return level;}

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

    public long id;//技能星级
    public long talent_skill_id;//技能id
    public int level;//等级
    public NPCommonCostItem cost;//升级到下一级消耗
    public NPCommonCostItem cost_per_level;//每级消耗增长
    public PlayerAttrPropertyModifier self_attr_prop_modifier;//自身加成
    public PlayerAttrPropertyModifier self_attr_prop_modifier_per_level;//每级自身加成
    public UnionBonus union_bonus;//全局加成
    public UnionBonus union_bonus_per_level;//每级全局加成
}

package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyModifier;

@RefTable(tableName = "hero_halo_suit_skill_level")
public class RefHeroSuitSkillLevel extends RefBase implements _ILevelBasicObj
{
    private static RefTableContainer<RefHeroSuitSkillLevel> _g_mgr = new RefTableContainer<RefHeroSuitSkillLevel>();

    public static RefTableContainer<RefHeroSuitSkillLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroSuitSkillLevel> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroSuitSkillLevel>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroSuitSkillLevel newRef = (RefHeroSuitSkillLevel) _newRef;
        id = newRef.id;
        halo_suit_skill_id = newRef.halo_suit_skill_id;
        level = newRef.level;
        attr_prop_modifier = newRef.attr_prop_modifier;
        attr_prop_modifier_per_lvl = newRef.attr_prop_modifier_per_lvl;
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

    public long id;//唯一id
    public long halo_suit_skill_id;//技能id
    public int level;//技能等级

    public PlayerAttrPropertyModifier attr_prop_modifier;//按照技能等级起始属性加成
    public PlayerAttrPropertyModifier attr_prop_modifier_per_lvl;//基于起始等级后的每级加成
}

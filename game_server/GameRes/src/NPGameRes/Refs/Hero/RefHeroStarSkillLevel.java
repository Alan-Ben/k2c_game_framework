package NPGameRes.Refs.Hero;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;

@RefTable(tableName = "hero_star_skill_level")
public class RefHeroStarSkillLevel extends RefBase implements _ILevelBasicObj
{
    private static RefTableContainer<RefHeroStarSkillLevel> _g_mgr = new RefTableContainer<RefHeroStarSkillLevel>();

    public static RefTableContainer<RefHeroStarSkillLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroStarSkillLevel> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroStarSkillLevel>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroStarSkillLevel newRef = (RefHeroStarSkillLevel) _newRef;
        id = newRef.id;
        skill_id = newRef.skill_id;
        skill_level = newRef.skill_level;
        union_bonus = newRef.union_bonus;
        add_player = newRef.add_player;
    }

    /**
     * 获取等级
     * @return
     */
    public int getLevel(){return skill_level;}

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
    public long skill_id;//技能id
    public int skill_level;//技能等级
    public UnionBonus union_bonus;//全局加成
    public NPPlayerPropertyModifier add_player; //玩家属性加成
}

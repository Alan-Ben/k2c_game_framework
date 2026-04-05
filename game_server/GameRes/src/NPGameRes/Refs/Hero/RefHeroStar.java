package NPGameRes.Refs.Hero;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.HeroCondition.HeroConditionGroupObj;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyModifier;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.List;

@RefTable(tableName = "hero_star")
public class RefHeroStar extends RefBase
{
    private static RefTableContainer<RefHeroStar> _g_mgr = new RefTableContainer<RefHeroStar>();

    public static RefTableContainer<RefHeroStar> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefHeroStar> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefHeroStar>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHeroStar newRef = (RefHeroStar) _newRef;
        id = newRef.id;
        hero_star_group_id = newRef.hero_star_group_id;
        star = newRef.star;
        upgrade_cost = newRef.upgrade_cost;
        upgrade_player_condition = newRef.upgrade_player_condition;
        upgrade_condition = newRef.upgrade_condition;
        self_attr_prop_modifier = newRef.self_attr_prop_modifier;
        mars_team_player_property = newRef.mars_team_player_property;
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

    public long id;//唯一ID
    public int hero_star_group_id;//升星组ID
    public int star;//星级
    public List<NPCommonCostItem> upgrade_cost;//升到下一星级消耗
    public NPPlayerConditionGroupObj upgrade_player_condition;//升到当前星级玩家条件
    public HeroConditionGroupObj upgrade_condition;//升到当前星级条件
    public PlayerAttrPropertyModifier self_attr_prop_modifier;//自身加成
    public NPPlayerPropertyModifier mars_team_player_property;//对火星队伍的加成

    /**
     * 根据组ID和星级查找配置
     *
     * @param groupId 升星组ID
     * @param star 星级
     * @return 对应的配置，如果不存在返回null
     */
    public static RefHeroStar getByGroupAndStar(int groupId, int star)
    {
        for (RefHeroStar refHeroStar : getMgr().values())
        {
            if (refHeroStar.hero_star_group_id == groupId && refHeroStar.star == star)
            {
                return refHeroStar;
            }
        }
        return null;
    }
}

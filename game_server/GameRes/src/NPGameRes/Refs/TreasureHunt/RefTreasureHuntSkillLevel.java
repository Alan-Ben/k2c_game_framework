package NPGameRes.Refs.TreasureHunt;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;

/**
 * id	技能id	技能等级	升级需要的消耗数量	玩家属性加成	全局加成
 * id	skill_id	level	upgrade_cost_num	add_player	union_bonus
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_skill_level")
public class RefTreasureHuntSkillLevel extends RefBase
{
    private static RefTreasureHuntSkillLevelMgr _g_mgr = new RefTreasureHuntSkillLevelMgr();

    public static RefTreasureHuntSkillLevelMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntSkillLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntSkillLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntSkillLevel newRef = (RefTreasureHuntSkillLevel) _newRef;
        id = newRef.id;
        skill_id = newRef.skill_id;
        level = newRef.level;
        upgrade_cost_num = newRef.upgrade_cost_num;
        add_player = newRef.add_player;
        union_bonus = newRef.union_bonus;
    }

    public static class RefTreasureHuntSkillLevelMgr extends RefTableContainer<RefTreasureHuntSkillLevel>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//id
    public long skill_id;//技能id
    public int level;//技能等级
    public long upgrade_cost_num;//升级需要的消耗技能点数量
    public NPPlayerPropertyModifier add_player;//玩家属性加成
    public UnionBonus union_bonus;//全局加成
}

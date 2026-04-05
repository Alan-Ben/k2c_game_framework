package NPGameRes.Refs.GuildDungeon;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.GuildDungeon.GuildDungeonMonsterHpList;

@RefTable(tableName = "guild_dungeon_lvl")
public class RefGuildDungeonLvl extends RefBase implements _ILevelBasicObj
{
    private static RefGuildDungeonLvlMgr _g_mgr = new RefGuildDungeonLvlMgr();

    public static RefGuildDungeonLvlMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildDungeonLvlMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildDungeonLvlMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildDungeonLvl newRef = (RefGuildDungeonLvl) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        lvl = newRef.lvl;
        monster_hp = newRef.monster_hp;
        star_cost_item = newRef.star_cost_item;
        star_cost_guild_wealth = newRef.star_cost_guild_wealth;
        upgrade_cost_guild_wealth = newRef.upgrade_cost_guild_wealth;
        finish_guild_exp = newRef.finish_guild_exp;
    }

    public static class RefGuildDungeonLvlMgr extends RefTableContainer<RefGuildDungeonLvl>
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

	@Override
	public int getLevel() 
	{
		return lvl;
	}

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public long id;
    public int group_id;//分组ID
    public int lvl;//等级
    public GuildDungeonMonsterHpList monster_hp = new GuildDungeonMonsterHpList();//怪物血量（怪物类型:血量）
    public NPCommonCostItem star_cost_item = new NPCommonCostItem();//开启消耗(物品消耗comcositem)
    public int star_cost_guild_wealth;//开启消耗(联盟财富)
    public int upgrade_cost_guild_wealth;//升级消耗(联盟财富)
    public int finish_guild_exp;//完成后获得的联盟经验
}
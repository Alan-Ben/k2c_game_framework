package NPGameRes.Refs.GuildDungeon;

import Common.GuildDungeonEnum.EGuildDungeon_MonsterType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

@RefTable(tableName = "guild_dungeon_monster")
public class RefGuildDungeonMonster extends RefBase
{
    private static RefGuildDungeonMonsterMgr _g_mgr = new RefGuildDungeonMonsterMgr();

    public static RefGuildDungeonMonsterMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildDungeonMonsterMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildDungeonMonsterMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildDungeonMonster newRef = (RefGuildDungeonMonster) _newRef;
        id = newRef.id;
        monster_type = newRef.monster_type;
        pre_monster_id_list = newRef.pre_monster_id_list;
    }

    public static class RefGuildDungeonMonsterMgr extends RefTableContainer<RefGuildDungeonMonster>
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
    
    public long id;
    public EGuildDungeon_MonsterType monster_type = EGuildDungeon_MonsterType.NONE;//怪物类型
    public ArrayList<Long> pre_monster_id_list = new ArrayList<>();//上一个怪物ID列表
}
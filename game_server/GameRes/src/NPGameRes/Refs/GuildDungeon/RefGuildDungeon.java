package NPGameRes.Refs.GuildDungeon;

import Common.GuildDungeonEnum.EGuildDungeon_MonsterType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;
import NPGameRes.GameObjs.GuildDungeon.GuildDungeonMonsterKillReward;
import NPGameRes.GameObjs.GuildDungeon.GuildDungeonMonsterKillRewardList;

import java.util.ArrayList;
import java.util.HashSet;

@RefTable(tableName = "guild_dungeon")
public class RefGuildDungeon extends RefBase
{
    private static RefGuildDungeonMgr _g_mgr = new RefGuildDungeonMgr();

    public static RefGuildDungeonMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildDungeonMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildDungeonMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildDungeon newRef = (RefGuildDungeon) _newRef;
        id = newRef.id;
        unlock_need_guild_lvl = newRef.unlock_need_guild_lvl;
        upgrade_group = newRef.upgrade_group;
        boss_id = newRef.boss_id;
        kill_reward = newRef.kill_reward;
        kill_boss_reward = newRef.kill_boss_reward;
    }

    public static class RefGuildDungeonMgr extends RefTableContainer<RefGuildDungeon>
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
    public int unlock_need_guild_lvl;//解锁需要公会等级
    public int upgrade_group;//升级Group
    public long boss_id;//最终怪物
    public GuildDungeonMonsterKillRewardList kill_reward = new GuildDungeonMonsterKillRewardList();//击杀奖励（怪物类型:有奖励的数量:CostItem）
    public NPCommonCostItem kill_boss_reward = new NPCommonCostItem();//击杀boss奖励
    
    /**
     * 等级配表列表
     */
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefGuildDungeonLvl> _m_lmLevelMapMgr = new _TLevelMapMgr<RefGuildDungeonLvl>();
    public void setLevelMapMgr(_TLevelMapMgr<RefGuildDungeonLvl> _mgr) {_m_lmLevelMapMgr = _mgr;} 
    public _TLevelMapMgr<RefGuildDungeonLvl> getLevelMapMgr() {return _m_lmLevelMapMgr;} 

    /**
     * 怪物配置列表
     */
    @RefField(isIgnore = true)
    public ArrayList<RefGuildDungeonMonster> _m_alDungeonMonsterRefList = new ArrayList<>();
    public void setDungeonMonsterRefList(ArrayList<RefGuildDungeonMonster> _list) {_m_alDungeonMonsterRefList = _list;}
    public ArrayList<RefGuildDungeonMonster> getDungeonMonsterRefList() {return _m_alDungeonMonsterRefList;}
    
    /**
     * 创建可以获得奖励的怪物列表
     * @return
     */
    public HashSet<Long> buildRewardMonsterList()
    {
    	HashSet<Long> monsterIdSet = new HashSet<>();
    	
    	GuildDungeonMonsterKillRewardList killReward = this.kill_reward;
    	for(int i = 0; i < killReward.getKillRewardList().size(); i++)
    	{
    		GuildDungeonMonsterKillReward obj = killReward.getKillRewardList().get(i);
    		if(null == obj)
    			continue;
    		
    		ArrayList<RefGuildDungeonMonster> monsterRefList = randMonosterListByType(obj.getType(), obj.getValue());
    		if(null == monsterRefList || monsterRefList.isEmpty())
    			continue;
    		
    		for(int j = 0; j < monsterRefList.size(); j++)
    		{
    			monsterIdSet.add(monsterRefList.get(j).id);
    		}
    	}
    	
    	return monsterIdSet;
    }
    
    /**
     * 从选定的类型monster中随机选择指定数量怪物
     * @param _type
     * @param _count
     * @return
     */
    public ArrayList<RefGuildDungeonMonster> randMonosterListByType(EGuildDungeon_MonsterType _type, int _count)
    {
    	if(null == _type || EGuildDungeon_MonsterType.NONE == _type || _count <= 0)
    		return null;
    	
    	ArrayList<RefGuildDungeonMonster> typeMonsterRefList = new ArrayList<>();
    	
    	ArrayList<RefGuildDungeonMonster> monsterRefList = this._m_alDungeonMonsterRefList;
    	for(int i = 0; i < monsterRefList.size(); i++)
    	{
    		RefGuildDungeonMonster ref = monsterRefList.get(i);
    		if(null == ref)
    			continue;
    		
    		if(ref.monster_type == _type)
    			typeMonsterRefList.add(ref);
    	}
    	
    	if(typeMonsterRefList.isEmpty())
    		return null;
    	
    	ArrayList<RefGuildDungeonMonster> refList = new ArrayList<>();
    	for(int i = 0; i < _count; i++)
    	{
    		if(typeMonsterRefList.isEmpty())
    			break;
    		
    		int idx = CommonFunc.randomInt(typeMonsterRefList.size() - 1);
    		refList.add(typeMonsterRefList.remove(idx));
    	}
    	
    	return refList;
    }
}
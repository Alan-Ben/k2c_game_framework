package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeon;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonLvl;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonMonster;

import java.util.*;

public class GuildDungeonInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        // 预整理：按 group_id 分组副本等级数据
        Map<Integer, _TLevelMapMgr<RefGuildDungeonLvl>> levelMapByGroup = new HashMap<>();
        List<RefGuildDungeonLvl> guildDungeonLvlRefList = RefGuildDungeonLvl.getMgr().getList();
        for (int i = 0; i < guildDungeonLvlRefList.size(); i++)
        {
            RefGuildDungeonLvl ref = guildDungeonLvlRefList.get(i);
            if (null == ref)
                continue;
            levelMapByGroup.computeIfAbsent(ref.group_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
        }

        List<RefGuildDungeon> guildDungeonRefList = RefGuildDungeon.getMgr().getList();
        for (int i = 0; i < guildDungeonRefList.size(); i++)
        {
            RefGuildDungeon ref = guildDungeonRefList.get(i);
            if (null == ref)
                continue;

            // 副本等级直接从预分组 Map 中取
            _TLevelMapMgr<RefGuildDungeonLvl> levelMapMgr = levelMapByGroup.get(ref.upgrade_group);
            ref.setLevelMapMgr(levelMapMgr != null ? levelMapMgr : new _TLevelMapMgr<>());

            // 公会副本怪物列表
            ArrayList<RefGuildDungeonMonster> monsterRefList = new ArrayList<>();
            RefGuildDungeonMonster bossRef = RefGuildDungeonMonster.getMgr().get(ref.boss_id);
            if (null == bossRef)
            {
                CommLog.error("GuildDungeonInitDealer.dealInit - guild dungeon:{} get boss-monster:{} ref fail.", ref.id, ref.boss_id);
            }
            else
            {
                monsterRefList.add(bossRef);
                // 加载前置的所有怪物配置数据
                HashSet<Long> monsterIdSet = new HashSet<>();
                ArrayList<Long> preMonsterIdList = new ArrayList<>();
                preMonsterIdList.addAll(bossRef.pre_monster_id_list);
                while (preMonsterIdList.size() > 0)
                {
                    long monsterId = preMonsterIdList.remove(0);
                    if (!monsterIdSet.add(monsterId))
                        continue;

                    RefGuildDungeonMonster monsterRef = RefGuildDungeonMonster.getMgr().get(monsterId);
                    if (null == monsterRef)
                    {
                        CommLog.error("GuildDungeonInitDealer.dealInit - guild dungeon:{} get pre-monster:{} ref fail.", ref.id, monsterId);
                        continue;
                    }
                    monsterRefList.add(monsterRef);
                    preMonsterIdList.addAll(monsterRef.pre_monster_id_list);
                }
            }
            ref.setDungeonMonsterRefList(monsterRefList);
        }
    }
}

using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 联盟PVE副本
	/// </summary>
	[Serializable]
	public class GuildDungeonRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		
		public long id; //主键ID
		public long unlock_need_guild_lvl; //解锁需要公会等级
		public long upgrade_group; //升级Group
		public long boss_id; //最终怪物
		public string name; //名字
		public NPGTextureIndex icon; //图标
		public NPGTextureIndex map_bg_img; //地图背景
		public NPGTextureIndex battle_bg_img; //战斗背景
		public List<GuildDungeonMonsterReward> kill_reward; //击杀奖励
		public NPCommonCostItem kill_boss_reward; // 怪物血量
#if NP_GAME
	
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		private List<GuildDungeonLvlRefObj> _m_lvl_list = null;// 等级列表
		public List<GuildDungeonLvlRefObj> lvl_list 
		{
			get
			{
				if (_m_lvl_list == null)
				{
					_initLevelList();
				}
				return _m_lvl_list;
			}
		}
		
		private void _initLevelList()
		{
			_m_lvl_list = new List<GuildDungeonLvlRefObj>();
			foreach (GuildDungeonLvlRefObj refObj in GRefdataCoreMgr.instance.guildDungeonLvlRefCore.refList)
			{
				if (refObj != null && refObj.group_id == upgrade_group)
				{
					_m_lvl_list.Add(refObj);
				}
			}

			// 按照level从小到大排序
			_m_lvl_list.Sort((a, b) => a.lvl.CompareTo(b.lvl));
		}

		public GuildDungeonLvlRefObj getLvlRef(long _lvl)
		{
			foreach (GuildDungeonLvlRefObj lvlRefObj in lvl_list)
			{
				if (lvlRefObj.lvl == _lvl)
				{
					return lvlRefObj;
				}
			}

			return null;
		}

		
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		private List<GuildDungeonMonsterRefObj> _m_monoster_list = null;// 怪物列表		
		public List<GuildDungeonMonsterRefObj> monoster_list 
		{
			get
			{
				if (_m_monoster_list == null)
				{
					if (_m_bossRefObj == null)
					{
						_m_bossRefObj = GRefdataCoreMgr.instance.guildDungeonMonsterRefCore.getRef(boss_id);
					}					
					_m_monoster_list = new List<GuildDungeonMonsterRefObj>();
					if (_m_bossRefObj != null)
					{
						// 使用递归方式收集所有相关怪物
						HashSet<long> visitedMonsters = new HashSet<long>();
						_collectAllMonsters(_m_bossRefObj, _m_monoster_list, visitedMonsters);
					}
				}
				return _m_monoster_list;
			}
		}
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		private GuildDungeonMonsterRefObj _m_bossRefObj = null;// Boss
		public GuildDungeonMonsterRefObj bossRefObj 
		{
			get
			{
				if (_m_bossRefObj == null)
				{
					_m_bossRefObj = GRefdataCoreMgr.instance.guildDungeonMonsterRefCore.getRef(boss_id);
				}
				return _m_bossRefObj;
			}
		}

		public long getTotalMonsterHp(long _lvl)
		{
			long totalHp = 0;
			GuildDungeonLvlRefObj lvlRefObj = getLvlRef(_lvl); // 获取第一级别的参考对象
			if(lvlRefObj == null ||monoster_list == null)
				return totalHp;
			foreach (var monster in monoster_list)
			{
				if (monster != null) totalHp += lvlRefObj.getMonsterHp(monster.monster_type);
			}
			return totalHp;
			
		}
		/// <summary>
		/// 递归收集所有相关怪物（包括前置怪物）
		/// </summary>
		/// <param name="currentMonster">当前怪物</param>
		/// <param name="monsterList">怪物列表</param>
		/// <param name="visitedMonsters">已访问的怪物ID集合，防止循环引用</param>
		private void _collectAllMonsters(GuildDungeonMonsterRefObj currentMonster, List<GuildDungeonMonsterRefObj> monsterList, HashSet<long> visitedMonsters)
		{
			if (currentMonster == null || visitedMonsters.Contains(currentMonster.id))
				return;
				
			// 标记当前怪物为已访问
			visitedMonsters.Add(currentMonster.id);
			
			// 添加当前怪物到列表
			monsterList.Add(currentMonster);
			
			// 递归处理前置怪物
			if (currentMonster.pre_monster_id_list != null)
			{
				foreach (long preMonsterID in currentMonster.pre_monster_id_list)
				{
					GuildDungeonMonsterRefObj preMonster = GRefdataCoreMgr.instance.guildDungeonMonsterRefCore.getRef(preMonsterID);
					if (preMonster != null)
					{
						_collectAllMonsters(preMonster, monsterList, visitedMonsters);
					}
				}
			}
		}
		
		
		/// 获取指定ID的怪物
		public GuildDungeonMonsterRefObj getMonsterRefObj(long _monsterId)
		{
			foreach (var monsterRef in monoster_list)
			{
				if (monsterRef.id == _monsterId)
				{
					return monsterRef;
				}
			}

			return null;
		}

		public string getFullTransName(long _lvl)
		{
			return TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_monster_level_name,
				TextTranslate.instance.getLanguage(name), _lvl);
		}

		public GuildDungeonMonsterReward getMonsterReward(Common.GuildDungeonEnum.EGuildDungeon_MonsterType _monsterType)
		{
			foreach (var reward in kill_reward)
			{
				if (reward != null && reward.monsterType == _monsterType)
					return reward;
			}

			return null;
		}
		
		
#endif
		
	}

	public class GSOGuildDungeonRefSet : _TALSOBasicRefSet<GuildDungeonRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_dungeon_refdata.unity3d"; } }
		public static string objName { get { return "guild_dungeon"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;
using Common.GuildDungeonEnum;

namespace GOE
{
	/// <summary>
	/// 联盟PVE副本等级表
	/// </summary>
	[Serializable]
	public class GuildDungeonLvlRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		
		public long id; //主键ID
		public long group_id; //分组ID
		public long lvl; //等级
		public NPCommonCostItem star_cost_item; //开启消耗(物品消耗comcositem)
		public long star_cost_guild_wealth; //开启消耗(联盟财富)
		public long upgrade_cost_guild_wealth;//升级消耗(联盟财富)
		public List<GuildDungeonMonsterHP> monster_hp; //怪物血量（怪物类型:血量）
		public long finish_guild_exp; //完成后获得的联盟经验
		
		public long getMonsterHp(EGuildDungeon_MonsterType monsterType)
		{
			foreach (var hp in monster_hp)
			{
				if (hp.monsterType == monsterType)
				{
					return hp.monsterHp;
				}
			}
			return 0; // 如果没有找到对应的怪物类型，返回0
		}
	}

	public class GSOGuildDungeonLvlRefSet : _TALSOBasicRefSet<GuildDungeonLvlRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_dungeon_refdata.unity3d"; } }
		public static string objName { get { return "guild_dungeon_lvl"; } }
	}
}
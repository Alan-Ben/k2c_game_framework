using ALPackage;
using System;
using System.Collections.Generic;
using Common.GuildDungeonEnum;

namespace GOE
{
	/// <summary>
	/// 联盟PVE副本怪物表
	/// </summary>
	[Serializable]
	public class GuildDungeonMonsterRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		
		public long id; //主键ID
		public EGuildDungeon_MonsterType monster_type; //怪物类型
		public List<long> pre_monster_id_list; //上一个怪物ID列表
		public long monster_show_id; //怪味表现ID
	}

	public class GSOGuildDungeonMonsterRefSet : _TALSOBasicRefSet<GuildDungeonMonsterRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_dungeon_refdata.unity3d"; } }
		public static string objName { get { return "guild_dungeon_monster"; } }
	}
}
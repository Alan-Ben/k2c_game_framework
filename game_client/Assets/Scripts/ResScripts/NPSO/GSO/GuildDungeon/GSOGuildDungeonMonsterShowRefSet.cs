using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 联盟PVE副本怪物展示
	/// </summary>
	[Serializable]
	public class GuildDungeonMonsterShowRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		
		public long id; //怪物展示id
		public string name; //名字
		public NPGTextureIndex icon; //图标
		public NPGGoIndex boss_go_index; //boss形象
	}

	public class GSOGuildDungeonMonsterShowRefSet : _TALSOBasicRefSet<GuildDungeonMonsterShowRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_dungeon_refdata.unity3d"; } }
		public static string objName { get { return "guild_dungeon_monster_show"; } }
	}
}
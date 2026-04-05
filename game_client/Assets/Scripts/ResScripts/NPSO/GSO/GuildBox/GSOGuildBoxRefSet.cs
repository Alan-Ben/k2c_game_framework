using ALPackage;
using System;
using System.Collections.Generic;
using Common.GuildEnum;
using UnityEngine.Serialization;

namespace GOE
{
	/// <summary>
	/// 联盟宝箱
	/// </summary>
	[Serializable]
	public class GuildBoxRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id; //唯一ID
		public EGuildBoxType type; //宝箱类型（枚举：EGuildBoxType）
		public long reward_id; //奖励
		public string box_source_desc;
		public NPGTextureIndex box_icon;//图标
		public int gain_guild_active_point; //获得的联盟活跃点
	}

	public class GSOGuildBoxRefSet : _TALSOBasicRefSet<GuildBoxRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_box_refdata.unity3d"; } }
		public static string objName { get { return "guild_box"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 联盟捐赠进度奖励表
	/// </summary>
	[Serializable]
	public class GuildConstructRewardRefObj : _IALBasicRefObj
	{
		public long _refId { get { return num; } }
		public long num;

		public List<NPCommonCostItem> reward_item_list;//奖励列表
	}

	public class GSOGuildConstructRewardRefSet : _TALSOBasicRefSet<GuildConstructRewardRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		
		public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
		public static string objName { get { return "guild_construct_reward"; } }
	}
}
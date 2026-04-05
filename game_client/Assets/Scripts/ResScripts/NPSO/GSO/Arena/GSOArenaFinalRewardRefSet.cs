using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 竞技场最终奖励表
	/// </summary>
	[Serializable]
	public class ArenaFinalRewardRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public WCGIntRange defeat_num_range;//击败数量范围
        public List<NPCommonCostItem> reward_item_list;//奖励道具列表
	}

	public class GSOArenaFinalRewardRefSet : _TALSOBasicRefSet<ArenaFinalRewardRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/arena_refdata.unity3d"; } }
		public static string objName { get { return "arena_final_reward"; } }
	}
}
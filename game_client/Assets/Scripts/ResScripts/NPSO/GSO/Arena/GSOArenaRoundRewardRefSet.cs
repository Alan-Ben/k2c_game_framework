using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 竞技场轮次奖励表
	/// </summary>
	[Serializable]
	public class ArenaRoundRewardRefObj : _IALBasicRefObj
	{
		public long _refId { get { return round; } }
		public long round;//轮次
        public long reward_id;//奖励
    }

	public class GSOArenaRoundRewardRefSet : _TALSOBasicRefSet<ArenaRoundRewardRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/arena_refdata.unity3d"; } }
		public static string objName { get { return "arena_round_reward"; } }
	}
}
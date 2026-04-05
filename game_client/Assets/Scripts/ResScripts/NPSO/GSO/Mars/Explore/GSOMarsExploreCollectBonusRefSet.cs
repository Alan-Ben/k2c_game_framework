using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星探索收集奖励表
	/// </summary>
	[Serializable]
	public class MarsExploreCollectBonusRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long need_team_power;
		public long add_collect_speed;
	}

	public class GSOMarsExploreCollectBonusRefSet : _TALSOBasicRefSet<MarsExploreCollectBonusRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_explore_collect_bonus"; } }
	}
}

using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星居民奖励求助表
	/// </summary>
	[Serializable]
	public class MarsPeopleRewardHelpRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public int add_satisfaction_degree;//增加满意度
		public List<NPCommonCostItem> reward_list;//获取奖励列表
	}

	public class GSOMarsPeopleRewardHelpRefSet : _TALSOBasicRefSet<MarsPeopleRewardHelpRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_people_reward_help"; } }
	}
}

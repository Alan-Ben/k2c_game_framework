using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 千万目标奖励
	/// </summary>
	[Serializable]
	public class EarningGoalRewardRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long earning_goal;// 赚速目标
		public List<NPCommonCostItem> first_gain_item_list;//首达奖励
		public List<NPCommonCostItem> all_gain_item_list;//全民奖励
	}

	public class GSOEarningGoalRewardRefSet : _TALSOBasicRefSet<EarningGoalRewardRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/earning_goal_refdata.unity3d"; } }
		public static string objName { get { return "earning_goal_reward"; } }
	}
}
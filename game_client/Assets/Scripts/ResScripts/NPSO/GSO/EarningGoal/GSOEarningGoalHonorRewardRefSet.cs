using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 千万目标荣耀奖励表
	/// </summary>
	[Serializable]
	public class EarningGoalHonorRewardRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long earning_goal;// 赚速目标
		public List<NPCommonCostItem> first_gain_item_list;//首达奖励
		public long system_log_id;// 系统消息id
	}

	public class GSOEarningGoalHonorRewardRefSet : _TALSOBasicRefSet<EarningGoalHonorRewardRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/earning_goal_refdata.unity3d"; } }
		public static string objName { get { return "earning_goal_honor_reward"; } }
	}
}
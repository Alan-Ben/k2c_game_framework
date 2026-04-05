using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 基金阶段表
	/// </summary>
	[Serializable]
	public class ActivityFundStepRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long activity_fund_id; // 活动ID
		public long step; // 阶段
		public long need_count; // 领取所需的计数
		public List<NPCommonCostItem> free_reward_item_list; // 免费档位的物品奖励
		public List<NPCommonCostItem> pay_reward_item_list; // 付费档位的物品奖励
		public bool is_special_step; // 是否为大奖阶段
		
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public long prev_step_need_count; // 上一阶段所需的计数，用于进度计算
	}

	public class GSOActivityFundStepRefSet : _TALSOBasicRefSet<ActivityFundStepRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_fund_refdata.unity3d"; } }
		public static string objName { get { return "activity_fund_step"; } }
	}
}

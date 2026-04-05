using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 充值返利阶段表
	/// </summary>
	[Serializable]
	public class RechargeRebateStepRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public long group_id;//组id
		public long step;//阶段
        public long target_count;//达成当前档位所需计数
        public List<NPCommonCostItem> reward_list;//奖励列表
        public string name;//名称
        public string name_args;//名称参数
    }

	public class GSORechargeRebateStepRefSet : _TALSOBasicRefSet<RechargeRebateStepRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/recharge_rebate_refdata.unity3d"; } }
		public static string objName { get { return "recharge_rebate_step"; } }
	}
}
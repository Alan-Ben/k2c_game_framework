using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 基金等级表
	/// </summary>
	[Serializable]
	public class ActivityFundLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long activity_fund_id; // 基金ID
		public long level; // 等级
		public long last_step; // 这个等级的最后一个阶段
		public NPGTextureIndex banner; // 横幅图片
		public string activity_fund_name; // 基金名称
		public List<string> activity_fund_name_args; // 基金名称参数
		public string activate_tip; // 激活提示
		public List<string> activate_tip_args; // 激活提示参数
		public string activated_tip; // 已激活提示
		public List<string> activated_tip_args; // 已激活提示参数
		public long gift_pack_id; // 付费礼包 id
		public NPCommonItem distinguish_item; // 用于识别的物品凭证
		public long activate_exp_count; // 激活后获得的经验值
		[ALAutoExportVariableAttr(true, false)]
		public NPCommonCostItem special_item; // 投放的大奖
		public long profit_per; // 性价比
		[ALAutoExportVariableAttr(true, false)]
		public NPCommonCostItem profit_item; // 性价比展示物品

		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public List<ActivityFundStepRefObj> step_ref_list;
	}

	public class GSOActivityFundLevelRefSet : _TALSOBasicRefSet<ActivityFundLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_fund_refdata.unity3d"; } }
		public static string objName { get { return "activity_fund_level"; } }
	}
}

using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 基金主表
	/// </summary>
	[Serializable]
	public class ActivityFundRefObj : _IALBasicRefObj
	{
		public long _refId { get { return activity_fund_id; } }
		public long activity_fund_id; // 基金ID
		public long activity_id; // 活动ID
		public long task_group_id; // 任务组ID
		public long task_refresh_time; // 任务刷新时间
		public long page_ui_res_id;
		public long activate_ui_res_id;
		public long fund_simple_unlock_id;
		public string exp_value_name;
		public _NPPlayerEffectSerializeInfo go_to; // 跳转效果
		public int sort_order; // 排序值

		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public List<ActivityFundLevelRefObj> level_ref_list;
	}

	public class GSOActivityFundRefSet : _TALSOBasicRefSet<ActivityFundRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_fund_refdata.unity3d"; } }
		public static string objName { get { return "activity_fund"; } }
	}
}

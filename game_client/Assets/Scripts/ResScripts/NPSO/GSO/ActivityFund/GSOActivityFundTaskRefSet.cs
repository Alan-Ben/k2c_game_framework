using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 基金任务表
	/// </summary>
	[Serializable]
	public class ActivityFundTaskRefObj : _IALBasicRefObj
	{
		public long _refId { get { return task_id; } }
		public long task_id;
		public long task_group_id; // 活动ID
		public string desc;//描述
		public List<string> desc_args;//描述参数
		public long task_finish_limit; // 任务可完成次数上限
		public EValueFormatType process_num_format;//进度值格式化显示方式
		public long done_task_count; // 任务完成需要的计数
		public _NPPlayerEffectSerializeInfo go_to;//跳转效果
		public long gain_score;
	}

	public class GSOActivityFundTaskRefSet : _TALSOBasicRefSet<ActivityFundTaskRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_fund_refdata.unity3d"; } }
		public static string objName { get { return "activity_fund_task"; } }
	}
}

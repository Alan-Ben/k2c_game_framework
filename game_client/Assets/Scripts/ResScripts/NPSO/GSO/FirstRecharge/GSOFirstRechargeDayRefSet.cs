using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 首充天数表
	/// </summary>
	[Serializable]
	public class FirstRechargeDayRefObj : _IALBasicRefObj
	{
		public long _refId { get { return day; } }
		public long day;
        public NPCommonCostItem special_item;//特殊奖励
        public List<NPCommonCostItem> item_list;//奖励列表
        public string special_item_gain_tip;//特殊奖励领取提示
        public string day_reward_desc;//每日奖励描述
        public List<string> day_reward_desc_args;//每日奖励描述参数
        public long buff_id;//关联BuffId
        public _NPPlayerConditionSerializeInfo condition;//条件
    }

	public class GSOFirstRechargeDayRefSet : _TALSOBasicRefSet<FirstRechargeDayRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/first_recharge_refdata.unity3d"; } }
		public static string objName { get { return "first_recharge_day"; } }
	}
}
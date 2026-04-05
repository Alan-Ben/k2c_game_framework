using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 七日登录
	/// </summary>
	[Serializable]
	public class SevenDayLoginRefObj : _IALBasicRefObj
	{
		public long _refId { get { return login_count; } }
		public long login_count; //登录天数	
		public List<NPCommonCostItem> reward_item_list;//奖励
		public NPCommonCostItem show_reward_item; //显示奖励
	}

	public class GSOSevenDayLoginRefSet : _TALSOBasicRefSet<SevenDayLoginRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/seven_day_login_refdata.unity3d"; } }
		public static string objName { get { return "seven_day_login"; } }
	}
}
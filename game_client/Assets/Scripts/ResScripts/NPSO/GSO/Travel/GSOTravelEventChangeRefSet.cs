using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 游历交换事件表
	/// </summary>
	[Serializable]
	public class TravelEventChangeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;
		
		public NPCommonCostItem event_cost;//事件消耗
		public List<NPCommonCostItem> exchange_item_list;//兑换的物品列表
		public bool default_change;//默认是否需要交换
		public long change_dialog_option_id;//需要交换对应的对话选项id
		public long not_change_dialog_option_id;//不需要交换对应的对话选项id
	}

	public class GSOTravelEventChangeRefSet : _TALSOBasicRefSet<TravelEventChangeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_change"; } }
	}
}
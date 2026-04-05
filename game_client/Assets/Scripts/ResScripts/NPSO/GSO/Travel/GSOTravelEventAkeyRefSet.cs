using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 游历事件一键表
	/// </summary>
	[Serializable]
	public class TravelEventAkeyRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;

		public List<NPCommonCostItem> event_item_list;//事件奖励列表
	}

	public class GSOTravelEventAkeyRefSet : _TALSOBasicRefSet<TravelEventAkeyRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_akey"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 游历大臣实力事件表
	/// </summary>
	[Serializable]
	public class TravelEventAddPowerRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;

		public int add_power;//增加的实力
		public string desc;//效果描述
		public List<string> desc_args;//效果描述参数
	}

	public class GSOTravelEventAddPowerRefSet : _TALSOBasicRefSet<TravelEventAddPowerRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_add_power"; } }
	}
}
using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 游历单次事件表
	/// </summary>
	[Serializable]
	public class TravelEventOnceRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;//事件id
	}

	public class GSOTravelEventOnceRefSet : _TALSOBasicRefSet<TravelEventOnceRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_once"; } }
	}
}
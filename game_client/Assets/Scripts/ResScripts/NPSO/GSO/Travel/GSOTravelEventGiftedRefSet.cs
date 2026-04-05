using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 游历卷王事件表
	/// </summary>
	[Serializable]
	public class TravelEventGiftedRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;
	}

	public class GSOTravelEventGiftedRefSet : _TALSOBasicRefSet<TravelEventGiftedRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_giftde"; } }
	}
}
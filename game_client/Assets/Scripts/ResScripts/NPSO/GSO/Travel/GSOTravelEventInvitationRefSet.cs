using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 游历邀约事件表
	/// </summary>
	[Serializable]
	public class TravelEventInvitationRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;
	}

	public class GSOTravelEventInvitationRefSet : _TALSOBasicRefSet<TravelEventInvitationRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_invitation"; } }
	}
}
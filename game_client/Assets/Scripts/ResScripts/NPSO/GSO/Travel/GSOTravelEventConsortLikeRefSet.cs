using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 游历妃子好感度事件表
	/// </summary>
	[Serializable]
	public class TravelEventConsortLikeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;//事件id

		public long consort_id;//妃子id
		public int add_like;//增加的好感度
	}

	public class GSOTravelEventConsortLikeRefSet : _TALSOBasicRefSet<TravelEventConsortLikeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_consort_like"; } }
	}
}
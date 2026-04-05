using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 游历妃子亲密度事件表
	/// </summary>
	[Serializable]
	public class TravelEventConsortIntimacyRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;
		
		public long consort_id;//妃子id
		public int add_intimacy;//增加的亲密度
	}

	public class GSOTravelEventConsortIntimacyRefSet : _TALSOBasicRefSet<TravelEventConsortIntimacyRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_consort_intimacy"; } }
	}
}
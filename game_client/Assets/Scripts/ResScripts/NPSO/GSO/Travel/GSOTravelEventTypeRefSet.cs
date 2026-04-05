using ALPackage;
using System;
using Common.TravelEnum;

namespace GOE
{
	/// <summary>
	/// 游历事件类型表
	/// </summary>
	[Serializable]
	public class TravelEventTypeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return (long)event_type; } }
		public ETravelEventType event_type;

		public string event_name;//事件名称
		public long event_center_tip_id;//事件center_tip_id
		public NPGTextureIndex event_banner;// 事件banner图
	}

	public class GSOTravelEventTypeRefSet : _TALSOBasicRefSet<TravelEventTypeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_type"; } }
	}
}
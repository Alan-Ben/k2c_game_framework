using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 游历博彩事件表
	/// </summary>
	[Serializable]
	public class TravelEventGambleRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;

		public int min_ante;//最少下注钻石
		public int max_ante;//最多下注钻石
	}

	public class GSOTravelEventGambleRefSet : _TALSOBasicRefSet<TravelEventGambleRefObj>
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_gamble"; } }
	}
}
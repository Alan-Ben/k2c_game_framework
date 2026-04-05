using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 游历妃子酒馆事件表
	/// </summary>
	[Serializable]
	public class TravelEventConsortBarRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;//事件id

		public List<long> trigger_consort_id_list;//触发的妃子id列表
		// public long ui_path_res_id;//ui预制路径id
	}

	public class GSOTravelEventConsortBarRefSet : _TALSOBasicRefSet<TravelEventConsortBarRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_consort_bar"; } }
	}
}
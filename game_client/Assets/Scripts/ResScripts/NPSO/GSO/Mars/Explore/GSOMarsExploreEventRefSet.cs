using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 火星探索事件表
	/// </summary>
	[Serializable]
	public class MarsExploreEventRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;
		public NPGGoIndex scene_go_index;
	}

	public class GSOMarsExploreEventRefSet : _TALSOBasicRefSet<MarsExploreEventRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_explore_event"; } }
	}
}

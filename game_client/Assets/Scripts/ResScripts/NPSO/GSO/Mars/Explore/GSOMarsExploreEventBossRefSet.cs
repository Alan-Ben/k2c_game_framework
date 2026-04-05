using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星探索PVE事件表
	/// </summary>
	[Serializable]
	public class MarsExploreEventBossRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;
		public string name;
		public List<string> name_parms;
		public string desc;
		public string type_desc;
		public NPGTextureIndex icon;
		public NPGTextureIndex banner;
		public List<NPCommonCostItem> reward_item_list;
        public long solider_power;
        public long solider_num;
    }

	public class GSOMarsExploreEventBossRefSet : _TALSOBasicRefSet<MarsExploreEventBossRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_explore_event_boss"; } }
	}
}

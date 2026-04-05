using ALPackage;
using System;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 火星探索战斗事件表
	/// </summary>
	[Serializable]
	public class MarsExploreEventBattleRefObj : _IALBasicRefObj
	{
		public long _refId { get { return event_id; } }
		public long event_id;
		public string name;
		public string desc;
		public string type_desc;
		public NPGTextureIndex icon;
		public NPGTextureIndex banner;
		public EQuality quality;
		public int refresh_wei;
	}

	public class GSOMarsExploreEventBattleRefSet : _TALSOBasicRefSet<MarsExploreEventBattleRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_explore_event_battle"; } }
	}
}

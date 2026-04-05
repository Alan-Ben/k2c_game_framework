using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 竞技场贸易站等级表
	/// </summary>
	[Serializable]
	public class ArenaStationLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return level; } }
		public long level;
        public NPCommonCostItem upgrade_cost;//升级到下一等级道具消耗
        public long harvest_ratio;//收成比例（万分比）
        public long storage_limit_sec;//储存上限时间/秒
	}

	public class GSOArenaStationLevelRefSet : _TALSOBasicRefSet<ArenaStationLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/arena_refdata.unity3d"; } }
		public static string objName { get { return "arena_station_level"; } }
	}
}
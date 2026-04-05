using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 太空舱等级表
	/// </summary>
	[Serializable]
	public class TreasureHuntStationLvlRefObj : _IALBasicRefObj
	{
		public long _refId { get { return level; } }
		public int level;

		public long each_pickup_exp;//每次拾取经验值
		public long level_up_need_exp;//升级所需经验值
		public int auto_fly_distance;//自动飞行距离
		public int max_fly_distance;//最大飞行距离
		public int fly_protect_times;//飞行保护次数
		public long unlock_area_id;//解锁区域ID(客户端展示使用)
	}

	public class GSOTreasureHuntStationLvlRefSet : _TALSOBasicRefSet<TreasureHuntStationLvlRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_station_lvl"; } }
	}
}
using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 寻宝区域距离表
	/// </summary>
	[Serializable]
	public class TreasureHuntAreaDistanceRefObj : _IALBasicRefObj
	{
		public long _refId { get { return distance; } }
		public long distance;
		public NPGGoIndex reward_box_go_index;
		public float player_speed_up_add;
		public float obstacle_spawn_rate_second;
		public int capture_reward_num_add;
	}

	public class GSOTreasureHuntAreaDistanceRefSet : _TALSOBasicRefSet<TreasureHuntAreaDistanceRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_area_distance"; } }
	}
}

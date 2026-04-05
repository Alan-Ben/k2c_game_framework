using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 太空区域表
	/// </summary>
	[Serializable]
	public class TreasureHuntAreaRefObj : _IALBasicRefObj
	{
		public long _refId { get { return area_id; } }
		public long area_id;

		public List<long> ore_list;//矿石列表
		public List<long> treasure_list;//奇物列表
		public _NPPlayerConditionSerializeInfo unlock_condition; //解锁条件
		public string unlock_condition_desc; //解锁条件描述
		public List<string> unlock_condition_desc_args_list;//解锁条件描述参数列表
		public string name;//区域名称
		public List<NPGGoIndex> obstacle_res_list;//障碍物资源列表
		public NPGGoIndex bg_res_index;
		public NPGTextureIndex thumbnail_image;//缩略图
		public long unlock_red_tip_id;//解锁红点id

		/// <summary>
		/// 是否已解锁
		/// </summary>
		/// <returns></returns>
		public bool isUnlock()
		{
			return unlock_condition == null || unlock_condition.isNoConditionOrEnable(null);
		}
	}

	public class GSOTreasureHuntAreaRefSet : _TALSOBasicRefSet<TreasureHuntAreaRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_area"; } }
	}
}
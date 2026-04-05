using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星部件食物等级
	/// </summary>
	[Serializable]
	public class MarsEquipmentFoodLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long group_id;
		public int level;
		public long energy_consume_per_min;
		public long satiety_yield;
	}

	public class GSOMarsEquipmentFoodLevelRefSet : _TALSOBasicRefSet<MarsEquipmentFoodLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_equipment_food_level"; } }
	}
}

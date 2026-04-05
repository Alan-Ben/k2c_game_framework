using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星部件生活等级
	/// </summary>
	[Serializable]
	public class MarsEquipmentLivingLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long group_id;
		public int level;
		public long comfort_yield;
		public long mood_yield;
		public long sleep_yield;
		public long people_num_limit;
	}

	public class GSOMarsEquipmentLivingLevelRefSet : _TALSOBasicRefSet<MarsEquipmentLivingLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_equipment_living_level"; } }
	}
}

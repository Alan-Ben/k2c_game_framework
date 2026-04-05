using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星部件能源等级
	/// </summary>
	[Serializable]
	public class MarsEquipmentEnergyLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long group_id;
		public int level;
		public long output_value_per_min;
		public long people_output_value_per_min;
		public long max_storage;
	}

	public class GSOMarsEquipmentEnergyLevelRefSet : _TALSOBasicRefSet<MarsEquipmentEnergyLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_equipment_energy_level"; } }
	}
}

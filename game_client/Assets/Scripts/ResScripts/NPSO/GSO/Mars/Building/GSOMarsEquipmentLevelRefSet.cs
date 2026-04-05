using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星部件等级
	/// </summary>
	[Serializable]
	public class MarsEquipmentLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long group_id;
		public int level;
		public NPCommonCostItem upgrade_cost;
	}

	public class GSOMarsEquipmentLevelRefSet : _TALSOBasicRefSet<MarsEquipmentLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_equipment_level"; } }
	}
}

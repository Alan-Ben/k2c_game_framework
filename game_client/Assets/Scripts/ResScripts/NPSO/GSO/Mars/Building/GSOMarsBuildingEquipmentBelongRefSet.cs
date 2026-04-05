using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星建筑部件归属
	/// </summary>
	[Serializable]
	public class MarsBuildingEquipmentBelongRefObj : _IALBasicRefObj
	{
		public long _refId { get { return group_id; } }
		public long group_id;
		public List<long> main_equipment_id_list;
		public List<long> other_equipment_id_list;
	}

	public class GSOMarsBuildingEquipmentBelongRefSet : _TALSOBasicRefSet<MarsBuildingEquipmentBelongRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_building_equipment_belong"; } }
	}
}

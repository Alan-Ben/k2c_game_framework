using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星建筑派遣等级
	/// </summary>
	[Serializable]
	public class MarsBuildingSettleLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return group_id; } }
		public long group_id;
		public int level;
		public int slot_num;
	}

	public class GSOMarsBuildingSettleLevelRefSet : _TALSOBasicRefSet<MarsBuildingSettleLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_building_settle_level"; } }
	}
}

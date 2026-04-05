using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 博物馆表
	/// </summary>
	[Serializable]
	public class MuseumItemUpgradeCostRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long group_id;
		public int level;
		public NPCommonCostItem upgrade_cost_item;
	}

	public class GSOMuseumItemUpgradeCostRefSet : _TALSOBasicRefSet<MuseumItemUpgradeCostRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/museum_refdata.unity3d"; } }
		public static string objName { get { return "museum_item_upgrade_cost"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 组合图鉴表
	/// </summary>
	[Serializable]
	public class TreasureHuntCompositeCatalogRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public List<long> ore_list;//矿石ID列表
		public long normal_skill_id;//普通技能id
		public long advanced_skill_id;//高级技能id
		public EQuality quality;//组合品质
		public string name;//组合名称
		public string desc;//组合描述
	}

	public class GSOTreasureHuntCompositeCatalogRefSet : _TALSOBasicRefSet<TreasureHuntCompositeCatalogRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_composite_catalog"; } }
	}
}
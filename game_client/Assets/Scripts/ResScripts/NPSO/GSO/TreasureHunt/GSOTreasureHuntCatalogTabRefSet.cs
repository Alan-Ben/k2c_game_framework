using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 图鉴页签表
	/// </summary>
	[Serializable]
	public class TreasureHuntCatalogTabRefObj : _IALBasicRefObj
	{
		public long _refId { get { return tab_id; } }
		public long tab_id;//图鉴页签id
		
		public string tab_name;//图鉴页签名称
		public NPGTextureIndex tab_banner;//图鉴页签banner图
	}

	public class GSOTreasureHuntCatalogTabRefSet : _TALSOBasicRefSet<TreasureHuntCatalogTabRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_catalog_tab"; } }
	}
}
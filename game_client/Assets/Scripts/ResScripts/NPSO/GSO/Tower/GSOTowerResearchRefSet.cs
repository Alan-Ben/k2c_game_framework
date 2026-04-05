using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 爬塔研究表
	/// </summary>
	[Serializable]
	public class TowerResearchRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long chapter_id; //章节id
		public int level;// 章节层数
		public long building_profit_add_per;//加成
	}

	public class GSOTowerResearchRefSet : _TALSOBasicRefSet<TowerResearchRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/tower_research_refdata.unity3d"; } }
		public static string objName { get { return "tower_research"; } }
	}
}
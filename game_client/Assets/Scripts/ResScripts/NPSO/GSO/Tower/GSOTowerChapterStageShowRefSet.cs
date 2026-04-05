using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 爬塔调整展示阶梯表
	/// </summary>
	[Serializable]
	public class TowerChapterStageShowRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public int show_lvl;// 展示阶梯
	}

	public class GSOTowerChapterStageShowRefSet : _TALSOBasicRefSet<TowerChapterStageShowRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/tower_refdata.unity3d"; } }
		public static string objName { get { return "tower_chapter_stage_show"; } }
	}
}
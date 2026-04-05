using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡 节-Stage 配表数据
	/// </summary>
	[Serializable]
	public class ChapterStageRefObj : _IALBasicRefObj
	{
		public long _refId { get { return stage_id; } }
		public long stage_id;// 节id
		public long story_id;// 所处故事id
		public string stage_name;// 节名称
		public string stage_name_args_list;//节名称参数列表
		public string stage_desc;// 节描述
		public string stage_desc_args_list;// 节描述参数列表
		public NPCommonAssetPathInfo prefab_asset_path;// 节预制体资源路径
		public long start_chapter_id;//起始章id
		public long end_chapter_id;// 结束章id
		public List<long> plot_id_list;// 剧情id列表
	}

	public class GSOChapterStageRefSet : _TALSOBasicRefSet<ChapterStageRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_stage"; } }
	}
}
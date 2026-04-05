using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡建筑解锁子表
	/// </summary>
	[Serializable]
	public class ChapterBuildUnlockRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;// id
		public NPGTextureIndex preview_tex_index;
		public string condition_desc;
		public List<string> condition_desc_params;
	}

	public class GSOChapterBuildUnlockRefSet : _TALSOBasicRefSet<ChapterBuildUnlockRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_build_unlock"; } }
	}
}
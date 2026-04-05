using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡故事表
	/// </summary>
	[Serializable]
	public class ChapterStoryRefObj : _IALBasicRefObj
	{
		public long _refId { get { return story_id; } }
		public long story_id;// 故事id
		public string name;// 故事名称
		public string name_args_list;// 故事名参数列表
		public NPGTextureIndex banner_img;//banner图
		public NPGTextureIndex bg_img;//背景图
	}

	public class GSOChapterStoryRefSet : _TALSOBasicRefSet<ChapterStoryRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_story"; } }
	}
}
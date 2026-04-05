using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡派遣事件条件表
	/// </summary>
	[Serializable]
	public class ChapterEventDispatchCondRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;// 唯一id
		public HeroConditionSerializeInfo condition;// 条件
		public string desc;// 描述
		public List<string> desc_args;// 描述参数
	}

	public class GSOChapterEventDispatchCondRefSet : _TALSOBasicRefSet<ChapterEventDispatchCondRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_event_dispatch_cond"; } }
	}
}
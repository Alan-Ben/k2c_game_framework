using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡派遣事件展示表
	/// </summary>
	[Serializable]
	public class ChapterEventDispatchShowRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;// 派遣事件展示id
		public string event_title;// 事件标题
		public string event_desc;// 事件描述
		public string result_desc;// 事件完成描述
		public string result_fail_desc;// 事件失败描述
	}

	public class GSOChapterEventDispatchShowRefSet : _TALSOBasicRefSet<ChapterEventDispatchShowRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_event_dispatch_show"; } }
	}
}
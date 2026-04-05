using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡派遣事件表
	/// </summary>
	[Serializable]
	public class ChapterEventDispatchRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;// 派遣事件唯一id
		public List<long> condition_id_list;// 条件列表
		// public List<long> event_reward_list;// 事件奖励id列表
		public int hero_num;// 可派遣骑士数量
		public long dispatch_show_id;// 展示id

	}

	public class GSOChapterEventDispatchRefSet : _TALSOBasicRefSet<ChapterEventDispatchRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_event_dispatch"; } }
	}
}
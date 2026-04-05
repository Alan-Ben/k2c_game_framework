using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡事件奖励表
	/// </summary>
	[Serializable]
	public class ChapterEventRewardRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		// public List<NPCommonCostItem> reward_item;//奖励
		public string event_title;// 事件标题
		public string event_desc;// 事件描述
	}

	public class GSOChapterEventRewardRefSet : _TALSOBasicRefSet<ChapterEventRewardRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_event_reward"; } }
	}
}
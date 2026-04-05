using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡选择事件选项表
	/// </summary>
	[Serializable]
	public class ChapterEventChoiceOptionRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;// id
		public string option_name;// 选项名字
		// public List<NPCommonCostItem> reward_item_list;// 选项的奖励
		public string option_result_desc;// 选择这个选项的结果描述
	}

	public class GSOChapterEventChoiceOptionRefSet : _TALSOBasicRefSet<ChapterEventChoiceOptionRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_event_choice_option"; } }
	}
}
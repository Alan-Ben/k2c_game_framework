using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 关卡选择事件表
	/// </summary>
	[Serializable]
	public class ChapterEventChoiceRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;// id
		public List<long> option_id_list;// 选项列表
		public string event_title;// 事件标题
		public string event_desc;// 事件描述

		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		private List<ChapterEventChoiceOptionRefObj> _m_option_ref_list;// 选项列表
#if NP_GAME
		public List<ChapterEventChoiceOptionRefObj> option_ref_list
		{
			get
			{
				if (_m_option_ref_list != null) return _m_option_ref_list;
				
				_m_option_ref_list = new List<ChapterEventChoiceOptionRefObj>(option_id_list.Count);
				foreach (long optionId in option_id_list)
				{
					ChapterEventChoiceOptionRefObj optionRef = GRefdataCoreMgr.instance.chapterEventChoiceOptionRefCore.getRef(optionId);
					if (optionRef != null)
						_m_option_ref_list.Add(optionRef);
				}
				return _m_option_ref_list;
			}
		}
#endif
		
	}

	public class GSOChapterEventChoiceRefSet : _TALSOBasicRefSet<ChapterEventChoiceRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_event_choice"; } }
	}
}
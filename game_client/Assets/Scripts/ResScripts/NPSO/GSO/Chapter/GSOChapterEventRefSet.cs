using ALPackage;
using System;
using Common.ChapterEnum;

namespace GOE
{
	/// <summary>
	/// 关卡事件表
	/// </summary>
	[Serializable]
	public class ChapterEventRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public EChapterEventType type;// 事件类型
		public long before_dialogue_id;// 事件前对话id
		public long after_dialogue_id;// 事件后对话id
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		private _IALBasicRefObj _m_event_type_ref;// 具体事件类型的数据
#if NP_GAME
		public _IALBasicRefObj event_type_ref
		{
			get
			{
				if (_m_event_type_ref != null) return _m_event_type_ref;

				switch (type)
				{
					case EChapterEventType.REWARD:
						_m_event_type_ref = GRefdataCoreMgr.instance.chapterEventRewardRefCore.getRef(id);
						break;
					case EChapterEventType.CHOICE:
						_m_event_type_ref = GRefdataCoreMgr.instance.chapterEventChoiceRefCore.getRef(id);
						break;
					case EChapterEventType.DISPATCH:
						_m_event_type_ref = GRefdataCoreMgr.instance.chapterEventDispatchRefCore.getRef(id);
						break;
				}
				return _m_event_type_ref;
			}
		}
#endif
	}

	public class GSOChapterEventRefSet : _TALSOBasicRefSet<ChapterEventRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_event"; } }
	}
}
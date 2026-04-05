using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 缺少物品触发推送礼包表
	/// </summary>
	[Serializable]
	public class PushGiftItemTriggerRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public List<NPCommonItem> item_list;//缺少的道具列表
		public long trigger_push_gift_group_id;//触发的推送礼包组id
#if NP_GAME	
		/// <summary>
		/// 触发的推送礼包组配表数据
		/// </summary>
		[NonSerialized]
		private PushGiftGroupRefObj _m_triggerPushGiftGroupRefObj = null;
		public PushGiftGroupRefObj triggerPushGiftGroupRefObj
		{
			get
			{
				if (_m_triggerPushGiftGroupRefObj == null && trigger_push_gift_group_id > 0)
					_m_triggerPushGiftGroupRefObj = GRefdataCoreMgr.instance.pushGiftGroupRefCore.getRef(trigger_push_gift_group_id);
				return _m_triggerPushGiftGroupRefObj;
			}
		}
#endif
	}

	public class GSOPushGiftItemTriggerRefSet : _TALSOBasicRefSet<PushGiftItemTriggerRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/push_gift_refdata.unity3d"; } }
		public static string objName { get { return "push_gift_item_trigger"; } }
	}
}

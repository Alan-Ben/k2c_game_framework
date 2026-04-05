using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 推送礼包表
	/// </summary>
	[Serializable]
	public class PushGiftPackRefObj : _IALBasicRefObj
	{
		public long _refId { get { return push_gift_id; } }
		public long push_gift_id;

		public long gift_pack_id;//对应礼包表id

		public long continue_time;//礼包持续时间(s)
		public long next_trigger_push_gift_id;//下一个触发的推送礼包id
		public bool after_buy_auto_trigger_next;//达到购买上限后自动触发下一个
		public long ui_res_path_id;//礼包预制资源路径id
		// public NPCommonItem special_show_item;//特殊展示道具(客户端展示使用)// 暂时去掉，因为现金礼包那边又默认规则, 奖励的第一个是特殊展示奖励, 这边就也延用现金礼包那边规则
		public NPGTextureIndex banner_img;//banner图
		public List<string> pop_show_node_list;// 触发弹窗展示窗口节点列表
		
#if NP_GAME
		
		/// <summary>
		/// 礼包表数据
		/// </summary>
		[NonSerialized]
		private GiftPackRefObj _m_giftPackRefObj = null;
		public GiftPackRefObj giftPackRefObj
		{
			get
			{
				if (_m_giftPackRefObj == null)
					_m_giftPackRefObj = GRefdataCoreMgr.instance.giftPackRefCore.getRef(gift_pack_id);
				return _m_giftPackRefObj;
			}
		}
	
		/// <summary>
		/// 下一个触发的推送礼包表数据
		/// </summary>
		[NonSerialized]
		private PushGiftPackRefObj _m_nextTriggerGiftPackRefObj = null;
		public PushGiftPackRefObj nextTriggerGiftPackRefObj
		{
			get
			{
				if (_m_nextTriggerGiftPackRefObj == null && next_trigger_push_gift_id > 0)
					_m_nextTriggerGiftPackRefObj = GRefdataCoreMgr.instance.pushGiftPackRefCore.getRef(next_trigger_push_gift_id);
				return _m_nextTriggerGiftPackRefObj;
			}
		}
#endif
		/// <summary>
		/// 是否在触发弹窗展示节点列表中
		/// </summary>
		/// <param name="_nodeTag"></param>
		/// <returns></returns>
		public bool isCanPopShowNode(string _nodeTag)
		{
			return pop_show_node_list?.Contains(_nodeTag) ?? false;
		}
	}

	public class GSOPushGiftPackRefSet : _TALSOBasicRefSet<PushGiftPackRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/push_gift_refdata.unity3d"; } }
		public static string objName { get { return "push_gift_pack"; } }
	}
}

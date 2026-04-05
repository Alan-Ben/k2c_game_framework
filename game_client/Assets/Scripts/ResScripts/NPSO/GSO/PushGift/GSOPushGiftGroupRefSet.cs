using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 推送礼包组表
	/// </summary>
	[Serializable]
	public class PushGiftGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return group_id; } }
		
		public long group_id;//推送礼包组id
		public EPushGiftPackType gift_pack_type;//推送礼包类型
		public bool need_reset_buy_count;//是否重置购买次数
		public _NPPlayerConditionSerializeInfo trigger_condition;//触发条件
		public long next_trigger_need_seconds;//下次触发所需时间（秒）
		public long first_trigger_gift_id;//首次触发礼包id

		/// <summary>
		/// 触发条件是否达成
		/// </summary>
		/// <returns></returns>
		public bool triggerConditionIsEnable(NPVarInfo _varVariableInfo)
		{
			return trigger_condition == null || trigger_condition.isNoConditionOrEnable(_varVariableInfo);
		}

#if NP_GAME
		
		/// <summary>
		/// 首次触发礼包配表数据
		/// </summary>
		[NonSerialized]
		private PushGiftPackRefObj _m_firstTriggerGiftPackRefObj = null;
		public PushGiftPackRefObj firstTriggerGiftPackRefObj
		{
			get
			{
				if (_m_firstTriggerGiftPackRefObj == null && first_trigger_gift_id > 0)
					_m_firstTriggerGiftPackRefObj = GRefdataCoreMgr.instance.pushGiftPackRefCore.getRef(first_trigger_gift_id);
				return _m_firstTriggerGiftPackRefObj;
			}
		}
		
#endif
		
	}

	public class GSOPushGiftGroupRefSet : _TALSOBasicRefSet<PushGiftGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/push_gift_refdata.unity3d"; } }
		public static string objName { get { return "push_gift_group"; } }
	}
}

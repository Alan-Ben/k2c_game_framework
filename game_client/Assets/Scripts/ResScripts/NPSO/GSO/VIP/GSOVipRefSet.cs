using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// VIP表
	/// </summary>
	[Serializable]
	public class VipRefObj : _IALBasicRefObj
	{
		public long _refId { get { return vip_lvl; } }
		public long vip_lvl;//VIP等级
        public long vip_exp;//升到该级所需经验
        public List<NPCommonItem> show_special_reward_list;//奖励大臣/妃子id列表(客户端展示)
        public List<NPCommonCostItem> gain_item_list;//奖励(要包含前一个字段的奖励)
        public List<NPCommonCostItem> recharge_reward_list;//充值奖励列表
        public long page_ui_res_id;//奖励页面信息预制路径
        public NPPlayerPropertyModifier player_property;//玩家属性列表
        public long trigger_push_gift_group_id;//触发推送礼包组id
	}

	public class GSOVipRefSet : _TALSOBasicRefSet<VipRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/vip_refdata.unity3d"; } }
		public static string objName { get { return "vip"; } }
	}
}
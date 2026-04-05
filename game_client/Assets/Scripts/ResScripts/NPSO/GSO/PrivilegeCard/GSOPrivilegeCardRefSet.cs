using ALPackage;
using System;
using System.Collections.Generic;
using Common.PrivilegeCardEnum;

namespace GOE
{
	/// <summary>
	/// 权益卡表
	/// </summary>
	[Serializable]
	public class PrivilegeCardRefObj : _IALBasicRefObj
	{
		public long _refId { get { return (long)privilege_card_type; } }
		public EPrivilegeCardType privilege_card_type;//权益卡类型
        public long effect_days;//有效天数
        public List<NPCommonCostItem> daily_gain_item_list;//每日可以领取的物品列表
		public NPPlayerPropertyModifier player_pro;//玩家属性
        public long gift_pack_id;//购买礼包ID
        public List<string> privilege_desc_title_list;//特权描述标题列表
        public List<DescWithParamPair> privilege_desc_content_list;//特权描述内容列表
        public string reward_desc;//获得奖励描述
        public List<string> reward_desc_args;//获得奖励描述参数
        public List<long> player_permission_list;//玩家权限列表
		public PlayerBonusPropertyModifier bonus_prop_modifier;//属性加成
    }

	public class GSOPrivilegeCardRefSet : _TALSOBasicRefSet<PrivilegeCardRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/privilege_card_refdata.unity3d"; } }
		public static string objName { get { return "privilege_card"; } }
	}
}
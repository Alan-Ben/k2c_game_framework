using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 活动商店道具表
	/// </summary>
	[Serializable]
	public class ActivityShopItemRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long activity_shop_id;//商店id
        public long sort_id;//排序id（从小到大）
        public NPCommonCostItem item;//获得物品
        public long discount;//折扣
        public long buy_num;//可购买次数
        public NPCommonCostItem cost_item;//固定单价
        public long times_price_type_id;//单价递增消耗(有递增消耗则固定单价无效)
        public bool is_recommend;//是否为推荐商品
    }

	public class GSOActivityShopItemRefSet : _TALSOBasicRefSet<ActivityShopItemRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
		public static string objName { get { return "activity_shop_item"; } }
	}
}
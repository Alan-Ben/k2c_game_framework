using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 钻石礼包表
	/// </summary>
	[Serializable]
	public class CrystalGiftPackRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public string name;//礼包名称
        public long crystal_gift_pack_group_id;//礼包组id
        public long sort_id;//排序id（从小到大）
        public List<NPCommonCostItem> item_list;//获得物品列表
        public long discount;//折扣
        public long buy_num;//购买次数
		public NPCommonCostItem cost_item;//固定单价
        public long times_price_type_id;//单价递增消耗(有递增消耗则固定单价无效)
    }

	public class GSOCrystalGiftPackRefSet : _TALSOBasicRefSet<CrystalGiftPackRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gift_pack_refdata.unity3d"; } }
		public static string objName { get { return "crystal_gift_pack"; } }
	}
}
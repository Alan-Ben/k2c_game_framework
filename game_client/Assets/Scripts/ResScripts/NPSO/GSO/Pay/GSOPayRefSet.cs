using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 支付档位表
	/// </summary>
	[Serializable]
	public class PayRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public string sdk_pay_id;//后台商品支付id（对应sdk参数app_product_id）
        public long vip_exp;//获得VIP经验
        public float show_price;//显示价格（不带sdk时客户端展示用）
        public NPCommonCostItem voucher_item;//档位对应代金券物品
    }

	public class GSOPayRefSet : _TALSOBasicRefSet<PayRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/pay_refdata.unity3d"; } }
		public static string objName { get { return "pay"; } }
	}
}
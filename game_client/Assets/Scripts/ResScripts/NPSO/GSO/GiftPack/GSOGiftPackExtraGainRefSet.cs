using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 礼包额外获得表
	/// </summary>
	[Serializable]
	public class GiftPackExtraGainRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long gift_pack_id;//礼包ID
        public WCGLongRange buy_times_range;//购买次数范围
		public List<NPCommonCostItem> extra_gain_list;//额外获得道具列表
    }

	public class GSOGiftPackExtraGainRefSet : _TALSOBasicRefSet<GiftPackExtraGainRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gift_pack_refdata.unity3d"; } }
		public static string objName { get { return "gift_pack_extra_gain"; } }
	}
}
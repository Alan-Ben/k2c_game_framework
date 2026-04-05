using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 商店总表
	/// </summary>
	[Serializable]
	public class ShopMainRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public List<long> shop_id_list;//子商店id列表
        public long ui_path_id; //加载的资源id
        public long no_tab_ui_path_id; //独立商店加载的资源id
        public _NPPlayerConditionSerializeInfo unlock_cond;//解锁条件
        public string unlock_cond_desc;//解锁条件描述
        public List<string> unlock_cond_desc_args;//解锁条件描述参数
    }

	public class GSOShopMainRefSet : _TALSOBasicRefSet<ShopMainRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/shop_refdata.unity3d"; } }
		public static string objName { get { return "shop_main"; } }
	}
}
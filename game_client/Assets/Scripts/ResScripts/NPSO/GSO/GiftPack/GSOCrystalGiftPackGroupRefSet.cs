using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 钻石礼包组表
	/// </summary>
	[Serializable]
	public class CrystalGiftPackGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public string tab_name;//礼包组页签名称
        public string refresh_desc;//限购次数刷新描述
        public long page_ui_res_id;//钻石礼包页面加载资源id
    }

	public class GSOCrystalGiftPackGroupRefSet : _TALSOBasicRefSet<CrystalGiftPackGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gift_pack_refdata.unity3d"; } }
		public static string objName { get { return "crystal_gift_pack_group"; } }
	}
}
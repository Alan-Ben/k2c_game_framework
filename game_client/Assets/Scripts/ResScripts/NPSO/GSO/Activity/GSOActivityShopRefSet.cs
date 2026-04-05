using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 活动商店表
	/// </summary>
	[Serializable]
	public class ActivityShopRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long activity_currency_id;//对应的活动货币id
    }

	public class GSOActivityShopRefSet : _TALSOBasicRefSet<ActivityShopRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
		public static string objName { get { return "activity_shop"; } }
	}
}
using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 活动兑换卷表
	/// </summary>
	[Serializable]
	public class ActivityCurrencyRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
	}

	public class GSOActivityCurrencyRefSet : _TALSOBasicRefSet<ActivityCurrencyRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
		public static string objName { get { return "activity_currency"; } }
	}
}
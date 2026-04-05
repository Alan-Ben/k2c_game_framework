using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 限时兑换组
	/// </summary>
	[Serializable]
	public class RushExchangeGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return group_id; } }
		public long group_id;
		//public condition;//刷新条件
	}

	public class GSORushExchangeGroupRefSet : _TALSOBasicRefSet<RushExchangeGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/rush_exchange_refdata.unity3d"; } }
		public static string objName { get { return "rush_exchange_group"; } }
	}
}
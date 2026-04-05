using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 限时兑换
	/// </summary>
	[Serializable]
	public class RushExchangeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long group_id;
		public List<NPCommonCostItem> cost_list;
		public List<NPCommonCostItem> reward_list;
	}

	public class GSORushExchangeRefSet : _TALSOBasicRefSet<RushExchangeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/rush_exchange_refdata.unity3d"; } }
		public static string objName { get { return "rush_exchange"; } }
	}
}
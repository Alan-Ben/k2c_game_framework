using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 限时兑换物品价值
	/// </summary>
	[Serializable]
	public class RushExchangeItemRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public NPCommonItem item; // 物品
		public int gem_count;// 物品钻石价格
	}

	public class GSORushExchangeItemRefSet : _TALSOBasicRefSet<RushExchangeItemRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/rush_exchange_refdata.unity3d"; } }
		public static string objName { get { return "rush_exchange_item"; } }
	}
}
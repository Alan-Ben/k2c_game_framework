using ALPackage;
using System;
using Common.BagItemUseEnum;
using Common.MarsEnum;

namespace GOE
{
	/// <summary>
	/// 火星背包物品时间减少
	/// </summary>
	[Serializable]
	public class MarsBagItemTimeReduceRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;//背包物品ID
		public EMarsBagItemUseTimeType time_type;
		public int reduce_sec;
	}

	public class GSOMasrBagItemTimeReduceRefSet : _TALSOBasicRefSet<MarsBagItemTimeReduceRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_bag_item_time_reduce"; } }
	}
}

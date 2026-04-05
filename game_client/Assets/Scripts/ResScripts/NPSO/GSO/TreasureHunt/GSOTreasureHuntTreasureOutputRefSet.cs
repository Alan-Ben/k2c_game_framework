using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 太空寻宝奇物产出表
	/// </summary>
	[Serializable]
	public class TreasureHuntTreasureOutputRefObj : _IALBasicRefObj
	{
		public long _refId { get { return treasure_id; } }
		public long treasure_id;//奇物id

		public long basic_value;//产出基础值
		public NPCommonItem output_item;//产出物品
	}

	public class GSOTreasureHuntTreasureOutputRefSet : _TALSOBasicRefSet<TreasureHuntTreasureOutputRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_treasure_output"; } }
	}
}

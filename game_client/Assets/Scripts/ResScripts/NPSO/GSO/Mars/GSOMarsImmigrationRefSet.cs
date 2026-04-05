using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星移民表
	/// </summary>
	[Serializable]
	public class MarsImmigrationRefObj : _IALBasicRefObj
	{
		public long _refId { get { return times; } }
		public long times;//移民次数
		public NPCommonCostItem cost_item;//消耗
		public long reward_id;//奖励id
		public long duration;//移民时长（秒）
		public bool need_cost_tip;//是否出现消耗过大提示
		public string cost_tip;//消耗过大提示
	}

	public class GSOMarsImmigrationRefSet : _TALSOBasicRefSet<MarsImmigrationRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_immigration"; } }
	}
}
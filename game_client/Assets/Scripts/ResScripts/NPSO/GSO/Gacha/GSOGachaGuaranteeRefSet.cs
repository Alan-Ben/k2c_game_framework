using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 抽卡保底表
	/// </summary>
	[Serializable]
	public class GachaGuaranteeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public int guarantee_times;//触发保底的次数
	}

	public class GSOGachaGuaranteeRefSet : _TALSOBasicRefSet<GachaGuaranteeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gacha_refdata.unity3d"; } }
		public static string objName { get { return "gacha_guarantee"; } }
	}
}
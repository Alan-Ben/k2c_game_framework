using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星满意度表
	/// </summary>
	[Serializable]
	public class MarsSatisfactionDegreeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return satisfaction_degree_per; } }
		public int satisfaction_degree_per;

		public List<NPCommonCostItem> reward_list;//奖励列表
		public string desc;//满意度描述
	}

	public class GSOMarsSatisfactionDegreeRefSet : _TALSOBasicRefSet<MarsSatisfactionDegreeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_satisfaction_degree"; } }
	}
}
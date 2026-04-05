using System.Collections.Generic;
using ALPackage;

namespace GOE
{
	/// <summary>
	/// 通用表现组主表
	/// </summary>
	[System.Serializable]
	public class PerformGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public List<long> item_ids;
	}

	public class GSOPerformGroupRefSet : _TALSOBasicRefSet<PerformGroupRefObj>
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/perform_group_refdata.unity3d"; } }
		public static string objName { get { return "perform_group"; } }
	}
}


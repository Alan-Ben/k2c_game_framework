using ALPackage;

namespace GOE
{
	/// <summary>
	/// 通用表现组子项表
	/// </summary>
	[System.Serializable]
	public class PerformGroupItemRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public _NPPlayerConditionSerializeInfo condition;
		public EPerformGroupType type;
	}

	public class GSOPerformGroupItemRefSet : _TALSOBasicRefSet<PerformGroupItemRefObj>
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/perform_group_refdata.unity3d"; } }
		public static string objName { get { return "perform_group_item"; } }
	}
}


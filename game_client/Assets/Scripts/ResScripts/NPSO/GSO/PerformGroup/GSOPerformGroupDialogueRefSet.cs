using ALPackage;

namespace GOE
{
	/// <summary>
	/// 通用表现组对话类型子表
	/// </summary>
	[System.Serializable]
	public class PerformGroupDialogueRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long dialogue_id;
	}

	public class GSOPerformGroupDialogueRefSet : _TALSOBasicRefSet<PerformGroupDialogueRefObj>
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/perform_group_refdata.unity3d"; } }
		public static string objName { get { return "perform_group_dialogue"; } }
	}
}


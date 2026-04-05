using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 弹幕组随机表
	/// </summary>
	[Serializable]
	public class CommentPrefabRandomRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public NPCommonAssetPathInfo asset_path_info; // 资源ab路径
	}

	public class GSOCommentPrefabRandomRefSet : _TALSOBasicRefSet<CommentPrefabRandomRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
		public static string objName { get { return "comment_prefab_random"; } }
	}
}
using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 弹幕名字随机表
	/// </summary>
	[Serializable]
	public class CommentNameRandomRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public string name; // 名字
	}

	public class GSOCommentNameRandomRefSet : _TALSOBasicRefSet<CommentNameRandomRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
		public static string objName { get { return "comment_name_random"; } }
	}
}
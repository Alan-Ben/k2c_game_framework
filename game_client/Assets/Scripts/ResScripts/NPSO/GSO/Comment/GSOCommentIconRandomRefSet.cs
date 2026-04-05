using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 弹幕头像随机表
	/// </summary>
	[Serializable]
	public class CommentIconRandomRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public NPGTextureIndex icon; // 头像
	}

	public class GSOCommentIconRandomRefSet : _TALSOBasicRefSet<CommentIconRandomRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
		public static string objName { get { return "comment_icon_random"; } }
	}
}
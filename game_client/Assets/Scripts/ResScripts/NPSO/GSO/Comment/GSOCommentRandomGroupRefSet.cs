using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 弹幕组随机表
	/// </summary>
	[Serializable]
	public class CommentRandomGroupRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public long group_id; // 组别id
    
		public string comment;//弹幕评论文本
		public List<string> comment_args;//弹幕评论文本参数
	}

	public class GSOCommentRandomGroupRefSet : _TALSOBasicRefSet<CommentRandomGroupRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
		public static string objName { get { return "comment_random_group"; } }
	}
}
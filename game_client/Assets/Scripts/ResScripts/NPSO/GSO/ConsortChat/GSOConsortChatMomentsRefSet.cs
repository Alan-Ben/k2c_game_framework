using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 妃子朋友圈表
	/// </summary>
	[Serializable]
	public class ConsortChatMomentsRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id; // Moments id
		public long consort_id; // 妃子id
		public List<long> consort_img_group_id_list; // 角色图片随机列表
		public List<long> bg_img_group_id_list; // 背景主题随机列表
	}

	public class GSOConsortChatMomentsRefSet : _TALSOBasicRefSet<ConsortChatMomentsRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_chat_moments"; } }
	}
}
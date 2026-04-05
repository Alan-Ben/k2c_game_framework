using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 妃子AI对话表
	/// </summary>
	[Serializable]
	public class ConsortChatAIRefObj : _IALBasicRefObj
	{
		public long _refId { get { return consort_id; } }
		public long consort_id; // 关联妃子id
		public int intimacy_count; // 亲密度
		public List<string> ai_first_list; // 第一句话消息key
	}

	public class GSOConsortChatAIRefSet : _TALSOBasicRefSet<ConsortChatAIRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_chat_ai"; } }
	}
}
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 妃子预设对话表
	/// </summary>
	[Serializable]
	public class ConsortChatDialogueRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id; // 唯一id
		public long start_sentence_id; // 起始句子id
		public long consort_id; // 关联妃子id
		// public _NPPlayerVariableSerializeInfo process_cur_count; // 计数器的高级公式
		// public long goal_count; // 目标计数
		// public List<string> event_list;//服务端需要监听的枚举
		public List<NPCommonCostItem> reward_item_list;//奖励列表
		public long reward_intimacy; // 亲密度奖励
	}

	public class GSOConsortChatDialogueRefSet : _TALSOBasicRefSet<ConsortChatDialogueRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_chat_dialogue"; } }
	}
}
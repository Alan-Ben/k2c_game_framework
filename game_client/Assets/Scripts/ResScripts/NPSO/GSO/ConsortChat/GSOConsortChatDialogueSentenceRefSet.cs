using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 妃子预设句子表
	/// </summary>
	[Serializable]
	public class ConsortChatDialogueSentenceRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id; // 句子唯一id
		public long next_id; // 下一句id(-1代表对话结束，并弹出奖励领取)
		public string content; // 对话内容KEY
		public string content_args; // 对话内容参数
		public List<long> response_sentence_list; // 回应选项句子id
		public long dialog_res_path_id; // 句子资源路径id（0表示不处理文本）
		public long image_show_id; // 图片Showid
		
		#if NP_GAME
		public string getContent()
		{
			return TextTranslate.instance.getLanguage(content, content_args);
		}
		#endif
	}

	public class GSOConsortChatDialogueSentenceRefSet : _TALSOBasicRefSet<ConsortChatDialogueSentenceRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/consort_chat_refdata.unity3d"; } }
		public static string objName { get { return "consort_chat_dialogue_sentence"; } }
	}
}
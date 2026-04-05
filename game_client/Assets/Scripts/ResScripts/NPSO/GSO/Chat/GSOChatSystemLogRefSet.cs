using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;
using UnityEngine.Serialization;

namespace GOE
{
	/// <summary>
	/// 聊天系统通知表
	/// </summary>
	[Serializable]
	public class ChatSystemLogRefObj : _IALBasicRefObj
	{
		public long _refId { get { return (long)system_log_id; } }
		public long system_log_id;//系统消息类型
		public string sender_name; // 发送者名字
		public string title; //	标题
		public string content; //	描述
		public bool content_has_sender_name;// 描述是否带上玩家名字
		public List<string> content_args;// 描述参数
		public  NPGTextureIndex banner; //	背景图
		public ENPFunctionType function_type; //功能类型
		public long simple_unlock_id;//
		public  _NPPlayerEffectSerializeInfo go_to; //	跳转效果
		public float ui_height;
		public NPCommonAssetPathInfo ui_path; //	ui加载路径配置（NPCommonAssetPathInfo）
		public NPCommonAssetPathInfo my_ui_path; // 自己发送ui加载路径配置（NPCommonAssetPathInfo）

	}

	public class GSOChatSystemLogRefSet : _TALSOBasicRefSet<ChatSystemLogRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chat_system_log_refdata.unity3d"; } }
		public static string objName { get { return "chat_system_log"; } }
	}
}
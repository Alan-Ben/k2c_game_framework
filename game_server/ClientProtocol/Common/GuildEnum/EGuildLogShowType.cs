using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟日志展示类型
/// </summary>
public enum EGuildLogShowType {
	NONE, //0 ==== 
	[InspectorName("CHAT - idx[1] - 聊天")]
	CHAT, //1 ==== 聊天
	[InspectorName("POPUP_WINDOW - idx[2] - 弹窗")]
	POPUP_WINDOW, //2 ==== 弹窗
}

public class EGuildLogShowTypeComparer : IEqualityComparer<EGuildLogShowType>{
	public bool Equals(EGuildLogShowType x, EGuildLogShowType y) { return x == y; }
	public int GetHashCode(EGuildLogShowType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


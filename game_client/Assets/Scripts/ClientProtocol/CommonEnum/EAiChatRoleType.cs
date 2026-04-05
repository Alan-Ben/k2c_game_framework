using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// AI聊天角色类型
/// </summary>
public enum EAiChatRoleType {
	[InspectorName("SYSTEM - idx[0] - 系统")]
	SYSTEM, //0 ==== 系统
	[InspectorName("USER - idx[1] - 用户")]
	USER, //1 ==== 用户
}

public class EAiChatRoleTypeComparer : IEqualityComparer<EAiChatRoleType>{
	public bool Equals(EAiChatRoleType x, EAiChatRoleType y) { return x == y; }
	public int GetHashCode(EAiChatRoleType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}


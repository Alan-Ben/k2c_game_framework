using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 聊天宝箱状态
/// </summary>
public enum ENPBoxChatStatus {
	NONE, //0 ==== 
	[InspectorName("IS_INVAILD - idx[1] - 已失效")]
	IS_INVAILD, //1 ==== 已失效
	[InspectorName("IS_LIMIT - idx[2] - 达到上限")]
	IS_LIMIT, //2 ==== 达到上限
	[InspectorName("IS_EMPTY - idx[3] - 已领完")]
	IS_EMPTY, //3 ==== 已领完
	[InspectorName("IS_GAINED - idx[4] - 已领取")]
	IS_GAINED, //4 ==== 已领取
}

public class ENPBoxChatStatusComparer : IEqualityComparer<ENPBoxChatStatus>{
	public bool Equals(ENPBoxChatStatus x, ENPBoxChatStatus y) { return x == y; }
	public int GetHashCode(ENPBoxChatStatus obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}


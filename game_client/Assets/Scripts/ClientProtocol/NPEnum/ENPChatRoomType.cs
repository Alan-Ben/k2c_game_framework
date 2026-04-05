using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 聊天房间类型
/// </summary>
public enum ENPChatRoomType {
	NONE, //0 ==== 
	[InspectorName("TOTAL_AREA - idx[1] - 全区玩家聊天")]
	TOTAL_AREA, //1 ==== 全区玩家聊天
	[InspectorName("US_SERVER - idx[2] - 单个玩家服聊天")]
	US_SERVER, //2 ==== 单个玩家服聊天
	[InspectorName("GUILD - idx[3] - 联盟聊天")]
	GUILD, //3 ==== 联盟聊天
	[InspectorName("ACTIVITY_TEAM - idx[4] - 活动队伍聊天")]
	ACTIVITY_TEAM, //4 ==== 活动队伍聊天
}

public class ENPChatRoomTypeComparer : IEqualityComparer<ENPChatRoomType>{
	public bool Equals(ENPChatRoomType x, ENPChatRoomType y) { return x == y; }
	public int GetHashCode(ENPChatRoomType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}


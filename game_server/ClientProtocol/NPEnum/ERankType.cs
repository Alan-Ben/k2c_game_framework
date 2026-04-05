using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 排行榜类型
/// </summary>
public enum ERankType {
	NONE, //0 ==== 
	[InspectorName("PLAYER - idx[1] - 玩家排行榜")]
	PLAYER, //1 ==== 玩家排行榜
	[InspectorName("GUILD - idx[2] - 联盟排行榜")]
	GUILD, //2 ==== 联盟排行榜
	[InspectorName("ACTIVITY_TEAM - idx[3] - 活动组队排行榜")]
	ACTIVITY_TEAM, //3 ==== 活动组队排行榜
}

public class ERankTypeComparer : IEqualityComparer<ERankType>{
	public bool Equals(ERankType x, ERankType y) { return x == y; }
	public int GetHashCode(ERankType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


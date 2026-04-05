using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟加入限制类型
/// </summary>
public enum EGuildJoinLimitType {
	NONE, //0 ==== 
	[InspectorName("NATION_POWER - idx[1] - 国力")]
	NATION_POWER, //1 ==== 国力
	[InspectorName("LEVEL - idx[2] - 等级")]
	LEVEL, //2 ==== 等级
}

public class EGuildJoinLimitTypeComparer : IEqualityComparer<EGuildJoinLimitType>{
	public bool Equals(EGuildJoinLimitType x, EGuildJoinLimitType y) { return x == y; }
	public int GetHashCode(EGuildJoinLimitType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


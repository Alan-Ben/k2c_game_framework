using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildDungeonEnum
{

/// <summary>
/// 联盟副本开启类型
/// </summary>
public enum EGuildDungeon_StartType {
	[InspectorName("COMMON_ITEM - idx[0] - 通用物品")]
	COMMON_ITEM, //0 ==== 通用物品
	[InspectorName("ALLIANCE_WEALTH - idx[1] - 联盟财富")]
	ALLIANCE_WEALTH, //1 ==== 联盟财富
}

public class EGuildDungeon_StartTypeComparer : IEqualityComparer<EGuildDungeon_StartType>{
	public bool Equals(EGuildDungeon_StartType x, EGuildDungeon_StartType y) { return x == y; }
	public int GetHashCode(EGuildDungeon_StartType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}


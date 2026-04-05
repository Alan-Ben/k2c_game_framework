using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟建设类型
/// </summary>
public enum EGuildConstructType {
	NONE, //0 ==== 
	[InspectorName("GOLD - idx[1] - 金币建设")]
	GOLD, //1 ==== 金币建设
	[InspectorName("ITEM - idx[2] - 道具建设")]
	ITEM, //2 ==== 道具建设
}

public class EGuildConstructTypeComparer : IEqualityComparer<EGuildConstructType>{
	public bool Equals(EGuildConstructType x, EGuildConstructType y) { return x == y; }
	public int GetHashCode(EGuildConstructType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


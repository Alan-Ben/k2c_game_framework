using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟集结类型
/// </summary>
public enum EGuildRallyType {
	NONE, //0 ==== 
	[InspectorName("DEFAULT - idx[1] - 默认集结")]
	DEFAULT, //1 ==== 默认集结
}

public class EGuildRallyTypeComparer : IEqualityComparer<EGuildRallyType>{
	public bool Equals(EGuildRallyType x, EGuildRallyType y) { return x == y; }
	public int GetHashCode(EGuildRallyType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}


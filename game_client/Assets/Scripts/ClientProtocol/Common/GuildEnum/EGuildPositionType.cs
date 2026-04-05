using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟职位类型
/// </summary>
public enum EGuildPositionType {
	NONE, //0 ==== 
	[InspectorName("MEMBER - idx[1] - 成员")]
	MEMBER, //1 ==== 成员
	[InspectorName("ELITE - idx[2] - 精英")]
	ELITE, //2 ==== 精英
	[InspectorName("DEPUTY_LEADER - idx[3] - 副盟主")]
	DEPUTY_LEADER, //3 ==== 副盟主
	[InspectorName("LEADER - idx[4] - 盟主")]
	LEADER, //4 ==== 盟主
}

public class EGuildPositionTypeComparer : IEqualityComparer<EGuildPositionType>{
	public bool Equals(EGuildPositionType x, EGuildPositionType y) { return x == y; }
	public int GetHashCode(EGuildPositionType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟火星互助类型
/// </summary>
public enum EGuildMarsHelpObjType {
	NONE, //0 ==== 
	[InspectorName("BUILDING_QUEUE - idx[1] - 火星建筑队列")]
	BUILDING_QUEUE, //1 ==== 火星建筑队列
	[InspectorName("TECH_UP - idx[2] - 火星科技升级")]
	TECH_UP, //2 ==== 火星科技升级
	[InspectorName("TEAM_REPAIR - idx[3] - 火星队伍修复")]
	TEAM_REPAIR, //3 ==== 火星队伍修复
}

public class EGuildMarsHelpObjTypeComparer : IEqualityComparer<EGuildMarsHelpObjType>{
	public bool Equals(EGuildMarsHelpObjType x, EGuildMarsHelpObjType y) { return x == y; }
	public int GetHashCode(EGuildMarsHelpObjType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


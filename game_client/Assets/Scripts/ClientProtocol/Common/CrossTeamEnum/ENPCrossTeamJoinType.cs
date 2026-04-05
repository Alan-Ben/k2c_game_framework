using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.CrossTeamEnum
{

/// <summary>
/// 队伍加入方式
/// </summary>
public enum ENPCrossTeamJoinType {
	NONE, //0 ==== 
	[InspectorName("FREE_JOIN - idx[1] - 自由加入")]
	FREE_JOIN, //1 ==== 自由加入
	[InspectorName("FORBID_JOIN - idx[2] - 禁止加入")]
	FORBID_JOIN, //2 ==== 禁止加入
	[InspectorName("COND_JOIN - idx[3] - 条件加入")]
	COND_JOIN, //3 ==== 条件加入
}

public class ENPCrossTeamJoinTypeComparer : IEqualityComparer<ENPCrossTeamJoinType>{
	public bool Equals(ENPCrossTeamJoinType x, ENPCrossTeamJoinType y) { return x == y; }
	public int GetHashCode(ENPCrossTeamJoinType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.CrossTeamEnum
{

/// <summary>
/// 队伍加入条件
/// </summary>
public enum ENPCrossTeamJoinCond {
	NONE, //0 ==== 
	[InspectorName("MIN_POWER - idx[1] - 最小战力")]
	MIN_POWER, //1 ==== 最小战力
	[InspectorName("MIN_MARS_POWER - idx[2] - 最小火星战力")]
	MIN_MARS_POWER, //2 ==== 最小火星战力
}

public class ENPCrossTeamJoinCondComparer : IEqualityComparer<ENPCrossTeamJoinCond>{
	public bool Equals(ENPCrossTeamJoinCond x, ENPCrossTeamJoinCond y) { return x == y; }
	public int GetHashCode(ENPCrossTeamJoinCond obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


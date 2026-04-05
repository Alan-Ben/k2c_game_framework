using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.CrossTeamEnum
{

/// <summary>
/// 队伍成员职位
/// </summary>
public enum ENPCrossTeamMemberPos {
	[InspectorName("NONE - idx[0] - 无职位")]
	NONE, //0 ==== 无职位
	[InspectorName("LEADER - idx[1] - 队长")]
	LEADER, //1 ==== 队长
}

public class ENPCrossTeamMemberPosComparer : IEqualityComparer<ENPCrossTeamMemberPos>{
	public bool Equals(ENPCrossTeamMemberPos x, ENPCrossTeamMemberPos y) { return x == y; }
	public int GetHashCode(ENPCrossTeamMemberPos obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}


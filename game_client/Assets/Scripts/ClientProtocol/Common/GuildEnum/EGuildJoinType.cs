using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟加入类型
/// </summary>
public enum EGuildJoinType {
	NONE, //0 ==== 
	[InspectorName("FREE_JOIN - idx[1] - 自由加入")]
	FREE_JOIN, //1 ==== 自由加入
	[InspectorName("APPROVAL_JOIN - idx[2] - 审批加入")]
	APPROVAL_JOIN, //2 ==== 审批加入
	[InspectorName("DECLINE_JOIN - idx[3] - 拒绝加入")]
	DECLINE_JOIN, //3 ==== 拒绝加入
}

public class EGuildJoinTypeComparer : IEqualityComparer<EGuildJoinType>{
	public bool Equals(EGuildJoinType x, EGuildJoinType y) { return x == y; }
	public int GetHashCode(EGuildJoinType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


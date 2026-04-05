using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MarsEnum
{

/// <summary>
/// 火星探索队伍状态
/// </summary>
public enum EMarsExploreTeamState {
	[InspectorName("NONE - idx[0] - 未解锁")]
	NONE, //0 ==== 未解锁
	[InspectorName("ERROR - idx[1] - 错误状态，需要手动处理")]
	ERROR, //1 ==== 错误状态，需要手动处理
	[InspectorName("IDLE - idx[2] - 空闲中")]
	IDLE, //2 ==== 空闲中
	[InspectorName("MARCH - idx[3] - 行军中")]
	MARCH, //3 ==== 行军中
	[InspectorName("BACK - idx[4] - 返程中")]
	BACK, //4 ==== 返程中
	[InspectorName("REPAIR - idx[5] - 修理中")]
	REPAIR, //5 ==== 修理中
	[InspectorName("BATTLE - idx[6] - Battle事件战斗中")]
	BATTLE, //6 ==== Battle事件战斗中
	[InspectorName("COLLECT - idx[7] - 采集中")]
	COLLECT, //7 ==== 采集中
	[InspectorName("BOSS_BATTLE - idx[8] - Boss事件战斗中")]
	BOSS_BATTLE, //8 ==== Boss事件战斗中
	[InspectorName("WAIT_RALLY - idx[9] - 等待集结出发")]
	WAIT_RALLY, //9 ==== 等待集结出发
}

public class EMarsExploreTeamStateComparer : IEqualityComparer<EMarsExploreTeamState>{
	public bool Equals(EMarsExploreTeamState x, EMarsExploreTeamState y) { return x == y; }
	public int GetHashCode(EMarsExploreTeamState obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 10;
}
}


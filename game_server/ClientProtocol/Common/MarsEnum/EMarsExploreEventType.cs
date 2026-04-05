using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MarsEnum
{

/// <summary>
/// 火星探索事件类型
/// </summary>
public enum EMarsExploreEventType {
	NONE, //0 ==== 
	[InspectorName("BATTLE - idx[1] - 战斗")]
	BATTLE, //1 ==== 战斗
	[InspectorName("BOSS - idx[2] - 固定点刷新战斗事件")]
	BOSS, //2 ==== 固定点刷新战斗事件
}

public class EMarsExploreEventTypeComparer : IEqualityComparer<EMarsExploreEventType>{
	public bool Equals(EMarsExploreEventType x, EMarsExploreEventType y) { return x == y; }
	public int GetHashCode(EMarsExploreEventType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


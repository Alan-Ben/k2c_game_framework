using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MarsEnum
{

/// <summary>
/// 火星探索-PVP日志
/// </summary>
public enum EMarsExplorePVPLogType {
	NONE, //0 ==== 
	[InspectorName("MINE_ATTACK - idx[1] - 矿挑战日志")]
	MINE_ATTACK, //1 ==== 矿挑战日志
	[InspectorName("MINE_DEFEND - idx[2] - 矿防守日志")]
	MINE_DEFEND, //2 ==== 矿防守日志
	[InspectorName("BATTLE_EVENT - idx[3] - 事件挑战日志")]
	BATTLE_EVENT, //3 ==== 事件挑战日志
	[InspectorName("BOSS_BATTLE_EVENT - idx[4] - BOSS事件挑战日志")]
	BOSS_BATTLE_EVENT, //4 ==== BOSS事件挑战日志
	[InspectorName("MINE_COLLECT_COMPLETE - idx[5] - 矿采集完成日志")]
	MINE_COLLECT_COMPLETE, //5 ==== 矿采集完成日志
}

public class EMarsExplorePVPLogTypeComparer : IEqualityComparer<EMarsExplorePVPLogType>{
	public bool Equals(EMarsExplorePVPLogType x, EMarsExplorePVPLogType y) { return x == y; }
	public int GetHashCode(EMarsExplorePVPLogType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 6;
}
}


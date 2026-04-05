using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildDungeonEnum
{

/// <summary>
/// 联盟副本日志类型
/// </summary>
public enum EGuildDungeon_LogType {
	NONE, //0 ==== 
	[InspectorName("START - idx[1] - 开启")]
	START, //1 ==== 开启
	[InspectorName("ATTACK - idx[2] - 攻击")]
	ATTACK, //2 ==== 攻击
	[InspectorName("KILL - idx[3] - 击杀")]
	KILL, //3 ==== 击杀
	[InspectorName("AUTO_START - idx[4] - 自动开启")]
	AUTO_START, //4 ==== 自动开启
}

public class EGuildDungeon_LogTypeComparer : IEqualityComparer<EGuildDungeon_LogType>{
	public bool Equals(EGuildDungeon_LogType x, EGuildDungeon_LogType y) { return x == y; }
	public int GetHashCode(EGuildDungeon_LogType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}


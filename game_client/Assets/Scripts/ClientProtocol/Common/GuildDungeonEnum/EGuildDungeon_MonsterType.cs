using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildDungeonEnum
{

/// <summary>
/// 联盟副本怪物类型
/// </summary>
public enum EGuildDungeon_MonsterType {
	NONE, //0 ==== 
	[InspectorName("COMMON - idx[1] - 普通怪物")]
	COMMON, //1 ==== 普通怪物
	[InspectorName("SENIOR - idx[2] - 高级怪物")]
	SENIOR, //2 ==== 高级怪物
	[InspectorName("BOSS - idx[3] - boss")]
	BOSS, //3 ==== boss
}

public class EGuildDungeon_MonsterTypeComparer : IEqualityComparer<EGuildDungeon_MonsterType>{
	public bool Equals(EGuildDungeon_MonsterType x, EGuildDungeon_MonsterType y) { return x == y; }
	public int GetHashCode(EGuildDungeon_MonsterType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


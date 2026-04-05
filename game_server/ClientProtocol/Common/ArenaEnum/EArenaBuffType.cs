using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ArenaEnum
{

/// <summary>
/// 竞技场buff类型
/// </summary>
public enum EArenaBuffType {
	NONE, //0 ==== 
	[InspectorName("CRYSTAL - idx[1] - 水晶增益")]
	CRYSTAL, //1 ==== 水晶增益
	[InspectorName("TWO_COIN - idx[2] - 2硬币增益")]
	TWO_COIN, //2 ==== 2硬币增益
	[InspectorName("ONE_COIN - idx[3] - 1硬币增益")]
	ONE_COIN, //3 ==== 1硬币增益
}

public class EArenaBuffTypeComparer : IEqualityComparer<EArenaBuffType>{
	public bool Equals(EArenaBuffType x, EArenaBuffType y) { return x == y; }
	public int GetHashCode(EArenaBuffType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


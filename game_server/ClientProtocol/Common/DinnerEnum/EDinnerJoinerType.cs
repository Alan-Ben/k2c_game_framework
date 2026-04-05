using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.DinnerEnum
{

/// <summary>
/// 赴宴对象类型
/// </summary>
public enum EDinnerJoinerType {
	NONE, //0 ==== 
	[InspectorName("PLAYER - idx[1] - 玩家")]
	PLAYER, //1 ==== 玩家
	[InspectorName("HERO - idx[2] - 大臣")]
	HERO, //2 ==== 大臣
}

public class EDinnerJoinerTypeComparer : IEqualityComparer<EDinnerJoinerType>{
	public bool Equals(EDinnerJoinerType x, EDinnerJoinerType y) { return x == y; }
	public int GetHashCode(EDinnerJoinerType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 战斗表现类型
/// </summary>
public enum ENPBattleShowType {
	[InspectorName("NORMAL - idx[0] - 普通")]
	NORMAL, //0 ==== 普通
	[InspectorName("MISSION_BOOS - idx[1] - 关卡boss")]
	MISSION_BOOS, //1 ==== 关卡boss
}

public class ENPBattleShowTypeComparer : IEqualityComparer<ENPBattleShowType>{
	public bool Equals(ENPBattleShowType x, ENPBattleShowType y) { return x == y; }
	public int GetHashCode(ENPBattleShowType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}


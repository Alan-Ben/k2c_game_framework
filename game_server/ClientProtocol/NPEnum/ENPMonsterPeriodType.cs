using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 怪物阶级枚举
/// </summary>
public enum ENPMonsterPeriodType {
	NONE, //0 ==== 
	[InspectorName("BOSS - idx[1] - boss")]
	BOSS, //1 ==== boss
	[InspectorName("CREAM - idx[2] - 精英")]
	CREAM, //2 ==== 精英
	[InspectorName("TRASH - idx[3] - 杂鱼")]
	TRASH, //3 ==== 杂鱼
}

public class ENPMonsterPeriodTypeComparer : IEqualityComparer<ENPMonsterPeriodType>{
	public bool Equals(ENPMonsterPeriodType x, ENPMonsterPeriodType y) { return x == y; }
	public int GetHashCode(ENPMonsterPeriodType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


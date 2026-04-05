using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 周卡NPC类型
/// </summary>
public enum EWeekCardNPCType {
	NONE, //0 ==== 
	[InspectorName("HERO - idx[1] - 大臣")]
	HERO, //1 ==== 大臣
	[InspectorName("CONSORT - idx[2] - 妃子")]
	CONSORT, //2 ==== 妃子
}

public class EWeekCardNPCTypeComparer : IEqualityComparer<EWeekCardNPCType>{
	public bool Equals(EWeekCardNPCType x, EWeekCardNPCType y) { return x == y; }
	public int GetHashCode(EWeekCardNPCType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


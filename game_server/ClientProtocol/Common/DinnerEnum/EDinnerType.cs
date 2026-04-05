using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.DinnerEnum
{

/// <summary>
/// 宴会标签
/// </summary>
public enum EDinnerType {
	NONE, //0 ==== 
	[InspectorName("CEREMONY - idx[1] - 典礼")]
	CEREMONY, //1 ==== 典礼
	[InspectorName("PARTY - idx[2] - 酒会")]
	PARTY, //2 ==== 酒会
}

public class EDinnerTypeComparer : IEqualityComparer<EDinnerType>{
	public bool Equals(EDinnerType x, EDinnerType y) { return x == y; }
	public int GetHashCode(EDinnerType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


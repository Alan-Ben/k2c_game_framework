using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.TravelEnum
{

/// <summary>
/// 博彩结果类型
/// </summary>
public enum ETravelGambleResult {
	NONE, //0 ==== 
	[InspectorName("WIN - idx[1] - 胜利")]
	WIN, //1 ==== 胜利
	[InspectorName("LOSE - idx[2] - 失败")]
	LOSE, //2 ==== 失败
	[InspectorName("JACKPOT - idx[3] - 特别大奖")]
	JACKPOT, //3 ==== 特别大奖
	[InspectorName("ABANDON - idx[4] - 放弃")]
	ABANDON, //4 ==== 放弃
}

public class ETravelGambleResultComparer : IEqualityComparer<ETravelGambleResult>{
	public bool Equals(ETravelGambleResult x, ETravelGambleResult y) { return x == y; }
	public int GetHashCode(ETravelGambleResult obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}


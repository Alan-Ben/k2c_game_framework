using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 通用品质枚举
/// </summary>
public enum EQuality {
	NONE, //0 ==== 
	[InspectorName("WHITE - idx[1] - 白")]
	WHITE, //1 ==== 白
	[InspectorName("GREEN - idx[2] - 绿")]
	GREEN, //2 ==== 绿
	[InspectorName("BLUE - idx[3] - 蓝")]
	BLUE, //3 ==== 蓝
	[InspectorName("PURPLE - idx[4] - 紫")]
	PURPLE, //4 ==== 紫
	[InspectorName("ORANGE - idx[5] - 橙")]
	ORANGE, //5 ==== 橙
	[InspectorName("ORANGE1 - idx[6] - 橙1")]
	ORANGE1, //6 ==== 橙1
	[InspectorName("RED - idx[7] - 红")]
	RED, //7 ==== 红
}

public class EQualityComparer : IEqualityComparer<EQuality>{
	public bool Equals(EQuality x, EQuality y) { return x == y; }
	public int GetHashCode(EQuality obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 8;
}
}


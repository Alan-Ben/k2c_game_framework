using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.LevyEnum
{

/// <summary>
/// 征收类型类型
/// </summary>
public enum ELevy_Type {
	NONE, //0 ==== 
	[InspectorName("SILVER - idx[1] - 银币 -> 金币")]
	SILVER, //1 ==== 银币 -> 金币
	[InspectorName("FOOD - idx[2] - 粮食 -> 面粉")]
	FOOD, //2 ==== 粮食 -> 面粉
	[InspectorName("SOLDIER - idx[3] - 士兵 -> 面包")]
	SOLDIER, //3 ==== 士兵 -> 面包
}

public class ELevy_TypeComparer : IEqualityComparer<ELevy_Type>{
	public bool Equals(ELevy_Type x, ELevy_Type y) { return x == y; }
	public int GetHashCode(ELevy_Type obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


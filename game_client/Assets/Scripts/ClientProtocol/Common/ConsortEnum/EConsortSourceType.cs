using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ConsortEnum
{

/// <summary>
/// 家人来源类型
/// </summary>
public enum EConsortSourceType {
	NONE, //0 ==== 
	[InspectorName("TRAVEL - idx[1] - 游历获取")]
	TRAVEL, //1 ==== 游历获取
	[InspectorName("CHAPTER - idx[2] - 关卡获取")]
	CHAPTER, //2 ==== 关卡获取
}

public class EConsortSourceTypeComparer : IEqualityComparer<EConsortSourceType>{
	public bool Equals(EConsortSourceType x, EConsortSourceType y) { return x == y; }
	public int GetHashCode(EConsortSourceType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


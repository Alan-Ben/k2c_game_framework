using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.NumMergeEnum
{

/// <summary>
/// 数字合并模式类型
/// </summary>
public enum ENumMerge_ModeType {
	NONE, //0 ==== 
	[InspectorName("NORMAL - idx[1] - 普通模式")]
	NORMAL, //1 ==== 普通模式
	[InspectorName("ADVANCED - idx[2] - 快速模式")]
	ADVANCED, //2 ==== 快速模式
	[InspectorName("ULTRA - idx[3] - 极速模式")]
	ULTRA, //3 ==== 极速模式
}

public class ENumMerge_ModeTypeComparer : IEqualityComparer<ENumMerge_ModeType>{
	public bool Equals(ENumMerge_ModeType x, ENumMerge_ModeType y) { return x == y; }
	public int GetHashCode(ENumMerge_ModeType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


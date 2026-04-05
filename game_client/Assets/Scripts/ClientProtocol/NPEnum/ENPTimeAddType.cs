using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 时间叠加方式枚举
/// </summary>
public enum ENPTimeAddType {
	[InspectorName("ADD - idx[0] - 默认叠加")]
	ADD, //0 ==== 默认叠加
	[InspectorName("SET - idx[1] - 设置")]
	SET, //1 ==== 设置
}

public class ENPTimeAddTypeComparer : IEqualityComparer<ENPTimeAddType>{
	public bool Equals(ENPTimeAddType x, ENPTimeAddType y) { return x == y; }
	public int GetHashCode(ENPTimeAddType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}


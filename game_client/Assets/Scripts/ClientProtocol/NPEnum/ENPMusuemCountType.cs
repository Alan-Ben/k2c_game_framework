using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 博物馆藏品数量,筛选类型
/// </summary>
public enum ENPMusuemCountType {
	NONE, //0 ==== 
	[InspectorName("QUALITY - idx[1] - 品质")]
	QUALITY, //1 ==== 品质
}

public class ENPMusuemCountTypeComparer : IEqualityComparer<ENPMusuemCountType>{
	public bool Equals(ENPMusuemCountType x, ENPMusuemCountType y) { return x == y; }
	public int GetHashCode(ENPMusuemCountType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}


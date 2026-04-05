using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.TileMatchEnum
{

/// <summary>
/// 三消模式类型
/// </summary>
public enum ETileMatch_ModeType {
	NONE, //0 ==== 
	[InspectorName("NORMAL - idx[1] - 普通")]
	NORMAL, //1 ==== 普通
	[InspectorName("ADVANCED - idx[2] - 高级")]
	ADVANCED, //2 ==== 高级
	[InspectorName("EXTREME - idx[3] - 极限")]
	EXTREME, //3 ==== 极限
}

public class ETileMatch_ModeTypeComparer : IEqualityComparer<ETileMatch_ModeType>{
	public bool Equals(ETileMatch_ModeType x, ETileMatch_ModeType y) { return x == y; }
	public int GetHashCode(ETileMatch_ModeType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


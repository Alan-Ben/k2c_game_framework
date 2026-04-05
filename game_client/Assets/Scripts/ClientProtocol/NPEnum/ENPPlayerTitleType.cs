using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家称号类型
/// </summary>
public enum ENPPlayerTitleType {
	NONE, //0 ==== 
	[InspectorName("COMMON - idx[1] - 普通称号")]
	COMMON, //1 ==== 普通称号
	[InspectorName("COMBO - idx[2] - 组合称号")]
	COMBO, //2 ==== 组合称号
}

public class ENPPlayerTitleTypeComparer : IEqualityComparer<ENPPlayerTitleType>{
	public bool Equals(ENPPlayerTitleType x, ENPPlayerTitleType y) { return x == y; }
	public int GetHashCode(ENPPlayerTitleType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


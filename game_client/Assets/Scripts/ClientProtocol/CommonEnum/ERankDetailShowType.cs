using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 排行榜详细信息展示类型
/// </summary>
public enum ERankDetailShowType {
	NONE, //0 ==== 
	[InspectorName("PLAYER - idx[1] - 玩家")]
	PLAYER, //1 ==== 玩家
	[InspectorName("HERO - idx[2] - 大臣")]
	HERO, //2 ==== 大臣
}

public class ERankDetailShowTypeComparer : IEqualityComparer<ERankDetailShowType>{
	public bool Equals(ERankDetailShowType x, ERankDetailShowType y) { return x == y; }
	public int GetHashCode(ERankDetailShowType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}


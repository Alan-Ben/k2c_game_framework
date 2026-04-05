using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ChildEnum
{

/// <summary>
/// 子嗣状态
/// </summary>
public enum EAdultStatus {
	NONE, //0 ==== 
	[InspectorName("APPLY_PLAYER - idx[1] - 指定联姻请求中")]
	APPLY_PLAYER, //1 ==== 指定联姻请求中
	[InspectorName("APPLY_SERVER - idx[2] - 全服联姻请求中")]
	APPLY_SERVER, //2 ==== 全服联姻请求中
	[InspectorName("MARRIED - idx[3] - 已婚")]
	MARRIED, //3 ==== 已婚
}

public class EAdultStatusComparer : IEqualityComparer<EAdultStatus>{
	public bool Equals(EAdultStatus x, EAdultStatus y) { return x == y; }
	public int GetHashCode(EAdultStatus obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家buff类型
/// </summary>
public enum ENPPlayerBuffFunctionType {
	NONE, //0 ==== 
	[InspectorName("MARS_EVENT - idx[1] - 火星事件")]
	MARS_EVENT, //1 ==== 火星事件
}

public class ENPPlayerBuffFunctionTypeComparer : IEqualityComparer<ENPPlayerBuffFunctionType>{
	public bool Equals(ENPPlayerBuffFunctionType x, ENPPlayerBuffFunctionType y) { return x == y; }
	public int GetHashCode(ENPPlayerBuffFunctionType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}


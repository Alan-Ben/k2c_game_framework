using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 红点类型
/// </summary>
public enum ERedDotType {
	[InspectorName("NONE - idx[0] - 无")]
	NONE, //0 ==== 无
}

public class ERedDotTypeComparer : IEqualityComparer<ERedDotType>{
	public bool Equals(ERedDotType x, ERedDotType y) { return x == y; }
	public int GetHashCode(ERedDotType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 1;
}
}


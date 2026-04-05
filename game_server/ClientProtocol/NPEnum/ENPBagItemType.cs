using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 背包物品类型
/// </summary>
public enum ENPBagItemType {
	NONE, //0 ==== 
	[InspectorName("RES - idx[1] - 资源")]
	RES, //1 ==== 资源
	[InspectorName("HERO - idx[2] - 骑士")]
	HERO, //2 ==== 骑士
	[InspectorName("FUNCTION - idx[3] - 功能")]
	FUNCTION, //3 ==== 功能
	[InspectorName("OTHER - idx[4] - 其他")]
	OTHER, //4 ==== 其他
}

public class ENPBagItemTypeComparer : IEqualityComparer<ENPBagItemType>{
	public bool Equals(ENPBagItemType x, ENPBagItemType y) { return x == y; }
	public int GetHashCode(ENPBagItemType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}


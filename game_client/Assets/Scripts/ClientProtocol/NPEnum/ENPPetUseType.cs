using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 宠物使用类型
/// </summary>
public enum ENPPetUseType {
	[InspectorName("SPACE_COMMON - idx[0] - 地图通用 适用人走物品消失的")]
	SPACE_COMMON, //0 ==== 地图通用 适用人走物品消失的
	[InspectorName("DIGGING - idx[1] - 挖矿")]
	DIGGING, //1 ==== 挖矿
}

public class ENPPetUseTypeComparer : IEqualityComparer<ENPPetUseType>{
	public bool Equals(ENPPetUseType x, ENPPetUseType y) { return x == y; }
	public int GetHashCode(ENPPetUseType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

